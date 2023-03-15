using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class IngredientSequence
{
    public string ingredientName;
    public List<string> cookingSequences;

    public IngredientSequence(string name, List<string> sequences)
    {
        ingredientName = name;
        cookingSequences = sequences;
    }
}

[System.Serializable]
public class Menu
{
    public int grade;
    public int gold;
    public float waitingTime;
    public List<string> properties;
    public string koreanName;
    public List<IngredientSequence> ingredientSequences;
}

[System.Serializable]
public class Order
{
    public string residentName;
    public float waitingTime;
    public bool isSpecial;
}

[System.Serializable]
public class NormalOrder : Order
{
    public List<string> menuList = new List<string>();
}


public class TycoonManager : MonoBehaviour
{
    public static TycoonManager instance;

    public GameObject angryUI;
    public int angrySeatID = -1;
    public GameObject goldMinusUI;

    public Dictionary<string, Menu> menus;

    #region read json
    
    void ReadRecipes()
    {
        menus = IOManager.instance.ReadJson<Dictionary<string, Menu>>("Tycoon/Menus");
    }

    #endregion

    #region decide order
    
    int specialResidentCount = 0;

    Dictionary<string, int> menuSalesVolumes = new Dictionary<string, int>();
    int menuWholeVolume = 0;

    [HideInInspector]
    public List<Order> orderList = new List<Order>();

    void DecideSpecialResidentCount()
    {
        int random = 100; // 테스트용 Random.Range(0, 100);

        if (random < 10)
            specialResidentCount = 0;
        else if (random < 55)
            specialResidentCount = 1;
        else if (random < 90)
            specialResidentCount = 2;
        else
            specialResidentCount = 3;
    }

    void DecideSalesVolume()
    {
        menuWholeVolume = 0;

        int range1;
        int range2;
        int range3;

        if (GameManager.instance.ownRecipes.hasThirdGradeRecipe)
        {
            range1 = Random.Range(7, 9);
            range2 = 6;
            range3 = Random.Range(1, 3);
        }
        else
        {
            range1 = Random.Range(8, 10);
            range2 = Random.Range(6, 8);
            range3 = 0;
        }

        menuWholeVolume = range1 + range2 + range3;
        
        
        var tempList = GameManager.instance.menuOfTheMonth.firstGradeRecipe;

        menuSalesVolumes[tempList[0]] = range1 / 2;
        menuSalesVolumes[tempList[0]] += range1 % 2;
            
        menuSalesVolumes[tempList[1]] = range1 / 2;
        
        if (GameManager.instance.ownRecipes.hasThirdGradeRecipe)
        {
            tempList = GameManager.instance.menuOfTheMonth.secondGradeRecipe;
            
            menuSalesVolumes[tempList[0]] = range2;
            
            tempList = GameManager.instance.menuOfTheMonth.thirdGradeRecipe;
            
            menuSalesVolumes[tempList[0]] = range3;
        }
        else
        {
            tempList = GameManager.instance.menuOfTheMonth.secondGradeRecipe;
            
            menuSalesVolumes[tempList[0]] = range2 / 2;
            menuSalesVolumes[tempList[0]] += range2 % 2;
            
            menuSalesVolumes[tempList[1]] = range2 / 2;
        }


        /*foreach (KeyValuePair<string, Menu> menu in menus)
        {
            menuSalesVolumes.Add(menu.Key, menuVolume);
        }*/

        /*  for (int i = 0; i < menuWholeVolume % menus.Count; i++)
          {
              int random = Random.Range(0, menus.Count);

              // 이번 주에 선택된 메뉴 리스트를 따로 만들어서 거기다 넣어야 할 듯
          }*/
    }

    // 특수주민 대기시간은 40초
    void SelectSpecialResidents()
    {
        List<string> residentNames = new List<string>();

        for (int i = 0; i < GameManager.instance.specialResidentInfo.Count; i++)
        {
            residentNames.Add(GameManager.instance.specialResidentInfo.ElementAt(i).Key);
        }

        for (int i = 0; i < specialResidentCount; i++)
        {
            int specialResidentID = Random.Range(0, residentNames.Count);
            Order tempOrder1 = new Order();
            tempOrder1.residentName = residentNames[specialResidentID];
            tempOrder1.isSpecial = true;
            tempOrder1.waitingTime = 40.0f;

            orderList.Add(tempOrder1);
            residentNames.RemoveAt(specialResidentID);
        }
    }

    void NormalResidentsTakeMenus()
    {
        while (menuWholeVolume > 2)
        {
            NormalOrder tempOrder1 = new NormalOrder();
            tempOrder1.residentName = "Normal";

            int menuCount = Random.Range(1, 3);

            // 여기를 수정해야해요
            for (int i = 0; i < menuCount; i++)
            {
                int menuID = Random.Range(0, menuSalesVolumes.Count);
                string menuName = menuSalesVolumes.ElementAt(menuID).Key;

                if (menuSalesVolumes[menuName] > 0)
                {
                    tempOrder1.menuList.Add(menuName);
                    tempOrder1.waitingTime += menus[menuName].waitingTime;
                    menuSalesVolumes[menuName]--;
                    menuWholeVolume--;
                }
            }

            if (tempOrder1.menuList.Count == 0)
            {
                foreach (KeyValuePair<string, int> menuSalesVolume in menuSalesVolumes)
                {
                    if (menuSalesVolume.Value > 0)
                    {
                        string mn = menuSalesVolume.Key;
                        tempOrder1.menuList.Add(mn);
                        tempOrder1.waitingTime += menus[mn].waitingTime;
                        menuSalesVolumes[mn]--;
                        menuWholeVolume--;

                        break;
                    }
                }
            }

            orderList.Add(tempOrder1);
        }

        if (menuWholeVolume > 0)
        {
            NormalOrder tempOrder2 = new NormalOrder();
            tempOrder2.residentName = "Normal";

            foreach (KeyValuePair<string, int> menu in menuSalesVolumes)
            {
                string menuName = menu.Key;

                for (int i = 0; i < menu.Value; i++)
                {
                    tempOrder2.menuList.Add(menuName);
                }
            }
        }
    }
    #endregion

    #region angry UI
    public void ActivateAngryUI(int angrySeatID)
    {
        this.angrySeatID = angrySeatID;
        angryUI.SetActive(true);

        StartCoroutine(UITimer());
    }

    IEnumerator UITimer()
    {
        int tempID = angrySeatID;

        yield return new WaitForSeconds(2f);

        if (tempID == angrySeatID)
        {
            angryUI.SetActive(false);
        }
    }
    #endregion

    #region show tycoon info and end tycoon
    
    [HideInInspector]
    public int earnedGold = 0;

    public void EarnGold(string foodName)
    {
        earnedGold += menus[foodName].gold;
    }


    public GameObject tycoonEndPanel;

    int endCount = 0;

    public List<string> specialResidentNames = new List<string>();
    public Dictionary<string, int> servedMenus = new Dictionary<string, int>();


    public void EndTycoon()
    {
        endCount++;

        if (endCount == 3)
        {
            StartCoroutine(WaitTycoonEnd());
        }
    }


    IEnumerator WaitTycoonEnd()
    {
        yield return new WaitForSeconds(1f);

        tycoonEndPanel.SetActive(true);

        //tycoonEndPanel.GetComponent<TycoonEndPanel>().UpdatePanel();
    }

    #endregion


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        ReadRecipes();
        DecideSpecialResidentCount();
        DecideSalesVolume();
        SelectSpecialResidents();
        NormalResidentsTakeMenus();
    }
}

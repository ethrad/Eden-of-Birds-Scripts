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
    public int gold;
    public List<int> serveRange;
    public float waitingTime;
    public List<IngredientSequence> ingredientSequences;
}

[System.Serializable]
public class SpecialResident
{
    public float waitingTime;
    public string favoriteFood;
}

[System.Serializable]
public class Order
{
    public string residentName;
    public float waitingTime;
    public List<string> menuList = new List<string>();

    public Order(string name)
    {
        residentName = name;
    }

    public Order(string name, SpecialResident sr)
    {
        residentName = name;
        menuList.Add(sr.favoriteFood);
    }
}

public class TycoonManager : MonoBehaviour
{
    public static TycoonManager instance;

    public GameObject angryUI;
    public int angrySeatID = -1;
    public GameObject goldMinusUI;

    public Dictionary<string, Menu> menus;
    public Dictionary<string, SpecialResident> specialResidents;

    #region read json
    void ReadRecipes()
    {
        menus = IOManager.instance.ReadJson<Dictionary<string, Menu>>("Tycoon/Menus");
    }

    void ReadspecialResidents()
    {
        specialResidents = IOManager.instance.ReadJson<Dictionary<string, SpecialResident>>("Tycoon/SpecialResidentList");
    }

    #endregion

    #region decide order
    int specialResidentCount = 0;

    Dictionary<string, int> menuSalesVolumes = new Dictionary<string, int>();
    int menuWholeVolume = 0;
    int savedWholeVolume;

    [HideInInspector]
    public List<Order> orderList = new List<Order>();

    void DecideSpecialResidentCount()
    {
        int random = Random.Range(0, 100);

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
        int random = 0;
        Menu currentMenu;

        foreach (KeyValuePair<string, Menu> menu in menus)
        {
            currentMenu = menu.Value;
            random = Random.Range(currentMenu.serveRange[0], currentMenu.serveRange[1] + 1);
            menuSalesVolumes.Add(menu.Key, random);
            menuWholeVolume += random;
        }

        savedWholeVolume += menuWholeVolume;
    }

    void SpecialResidentsTakeMenus()
    {
        for (int i = 0; i < specialResidentCount; i++)
        {
            int specialResidentID = Random.Range(0, specialResidents.Count);
            SpecialResident tempResident = specialResidents.ElementAt(specialResidentID).Value;
            Order tempOrder1 = new Order(specialResidents.ElementAt(specialResidentID).Key, tempResident);
            tempOrder1.waitingTime += menus[tempResident.favoriteFood].waitingTime;

            menuSalesVolumes[tempResident.favoriteFood]--;
            menuWholeVolume--;


            // 다른 메뉴 선택
            int random = Random.Range(0, 3);
            for (int j = 0; j < random; j++)
            {
                int menuID = Random.Range(0, menus.Count);
                string menuName = menus.ElementAt(menuID).Key;
                tempOrder1.menuList.Add(menuName);
                tempOrder1.waitingTime += menus[menuName].waitingTime;

                menuSalesVolumes[menuName]--;
                menuWholeVolume--;
            }

            orderList.Add(tempOrder1);
            specialResidents.Remove(specialResidents.ElementAt(specialResidentID).Key);
            //DialogueManager.instance.ReadSpecialResidentDialogues(temp.residentName);
        }
    }

    void NormalResidentsTakeMenus()
    {
        while (menuWholeVolume > 3)
        {
            Order tempOrder1 = new Order("normal");

            int menuCount = Random.Range(1, 4);

            for (int i = 0; i < menuCount; i++)
            {
                int menuID = Random.Range(0, menus.Count);
                string menuName = menus.ElementAt(menuID).Key;

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
            Order tempOrder2 = new Order("normal");
            foreach (KeyValuePair<string, Menu> menu in menus)
            {
                string menuName = menu.Key;

                while (menuSalesVolumes[menuName] > 0)
                {
                    tempOrder2.menuList.Add(menuName);
                    menuSalesVolumes[menuName]--;
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

    #region tycoon info and end tycoon
    
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
        ReadspecialResidents();
        DecideSpecialResidentCount();
        DecideSalesVolume();
        SpecialResidentsTakeMenus();
        NormalResidentsTakeMenus();
    }
}

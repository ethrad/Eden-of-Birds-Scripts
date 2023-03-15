using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int month;
    public int year;
    public int tycoonCount;

    public int maxStamina;
    public int stamina;
}

[System.Serializable]
public class MenuOfTheMonth
{
    public List<string> firstGradeRecipe;
    public List<string> secondGradeRecipe;
    public List<string> thirdGradeRecipe;

    public MenuOfTheMonth()
    {
        firstGradeRecipe = new List<string>();
        secondGradeRecipe = new List<string>();
        thirdGradeRecipe = new List<string>();
    }
}

[System.Serializable]
public class OwnRecipes
{
    public bool hasThirdGradeRecipe;

    public List<string> firstGradeRecipe;
    public List<string> secondGradeRecipe;
    public List<string> thirdGradeRecipe;

    public OwnRecipes()
    {
        hasThirdGradeRecipe = false;

        firstGradeRecipe = new List<string>();
        secondGradeRecipe = new List<string>();
        thirdGradeRecipe = new List<string>();
    }
}

[System.Serializable]
public class SpecialResidentInfo
{
    public string dialogue;
    public List<int> friendshipLevel;
    public List<string> properties;
}

public class ResidentFriendship
{
    public int friendship;
    public bool isInteractedInThisMonth;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameData gameData;

    // 레시피랑 이달의 메뉴
    public Dictionary<string, int> recipeGrade;

    public MenuOfTheMonth menuOfTheMonth;

    public OwnRecipes ownRecipes;

    public Dictionary<string, SpecialResidentInfo> specialResidentInfo;

    public Dictionary<string, ResidentFriendship> residentFriendship;


    #region Data IO

    void ReadGameData()
    {
        gameData = IOManager.instance.ReadLocalJson<GameData>("GameData");
        
        if (gameData.month == 0)
        {
            gameData.month = 4;
            gameData.year = 1;
        }
    }

    void WriteGameData()
    {
        IOManager.instance.WriteJson(gameData, "GameData.json");
    }

    void ReadRecipeGrade()
    {
        recipeGrade = IOManager.instance.ReadJson<Dictionary<string, int>>("RecipeGrade");
    }

    void ReadMenuOfTheMonth()
    {
        menuOfTheMonth = IOManager.instance.ReadLocalJson<MenuOfTheMonth>("MenuOfTheMonth");
    }

    void WriteMenuOfTheMonth()
    {
        IOManager.instance.WriteJson(menuOfTheMonth, "MenuOfTheMonth.json");
    }

    void ReadOwnRecipes()
    {
        ownRecipes = IOManager.instance.ReadLocalJson<OwnRecipes>("OwnRecipes");
    }

    void WriteOwnRecipes()
    {
        IOManager.instance.WriteJson(ownRecipes, "OwnRecipes.json");
    }

    void ReadSpecialResidentInfo()
    {
        specialResidentInfo = IOManager.instance.ReadLocalJson<Dictionary<string, SpecialResidentInfo>>("SpecialResidentInfo");
    }

    void ReadResidentFriendShip()
    {
        residentFriendship = IOManager.instance.ReadLocalJson<Dictionary<string, ResidentFriendship>>("ResidentFriendship");
    }

    void WriteResidentFriendShip()
    {
        IOManager.instance.WriteJson(residentFriendship, "ResidentFriendship.json");
    }

    #endregion

    #region Date System

    public void AddTycoonCount()
    {
        gameData.tycoonCount++;

        if (gameData.tycoonCount == 3)
        {
            AddMonth();
            gameData.tycoonCount = 0;
        }
    }

    public void AddMonth()
    {
        gameData.month++;

        if (gameData.month == 13)
        {
            AddYear();
            gameData.month = 1;
        }
    }

    public void AddYear()
    {
        gameData.year++;
    }
    
    #endregion
    
    #region Tycoon System
    
    // 플레이어가 무슨 레시피 얻었는지 기록

    public void GetRecipe(string recipeName)
    {
        List<string> selectedList;

        switch (recipeGrade[recipeName])
        {
            case 1:
                selectedList = ownRecipes.firstGradeRecipe;
                break;
            case 2:
                selectedList = ownRecipes.secondGradeRecipe;
                break;
            case 3:
                selectedList = ownRecipes.thirdGradeRecipe;
                break;
            default:
                selectedList = null;
                Debug.Log("오류");
                break;
        }

        if (selectedList != null && !selectedList.Contains(recipeName))
        {
            selectedList.Add(recipeName);
        }
    }


    // 이달의 메뉴 결정 => 저장

    public void SelectMenuOfTheMonth()
    {
        menuOfTheMonth = new MenuOfTheMonth();

        if (ownRecipes.hasThirdGradeRecipe == true)
        {
            for (int i = 0; i < 2; i++)
            {
                List<string> tempList = ownRecipes.firstGradeRecipe;

                int random = Random.Range(0, tempList.Count);

                menuOfTheMonth.firstGradeRecipe.Add(tempList[random]);
                tempList.RemoveAt(random);
            }

            int random1 = Random.Range(0, ownRecipes.secondGradeRecipe.Count);

            menuOfTheMonth.secondGradeRecipe.Add(ownRecipes.secondGradeRecipe[random1]);

            int random2 = Random.Range(0, ownRecipes.thirdGradeRecipe.Count);

            menuOfTheMonth.thirdGradeRecipe.Add(ownRecipes.thirdGradeRecipe[random2]);
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                List<string> tempList = ownRecipes.firstGradeRecipe;

                int random = Random.Range(0, tempList.Count);

                menuOfTheMonth.firstGradeRecipe.Add(tempList[random]);
                tempList.RemoveAt(random);
            }

            for (int i = 0; i < 2; i++)
            {
                List<string> tempList = ownRecipes.secondGradeRecipe;

                int random = Random.Range(0, tempList.Count);

                menuOfTheMonth.secondGradeRecipe.Add(tempList[random]);
                tempList.RemoveAt(random);
            }
        }

        WriteMenuOfTheMonth();
        ReadOwnRecipes();
    }

    #endregion


    // 튜토리얼 끝난 후에 할 것들 모음
    #region Initialize

    void Initialize()
    {
        // 기본으로 주는 레시피 추가
        GetRecipe("slimeMargarita");
        GetRecipe("slimePudding");
        GetRecipe("ratatouille");
        GetRecipe("rabbitSteak");
        WriteOwnRecipes();


        // 이 달의 메뉴 결정
        SelectMenuOfTheMonth();
        WriteMenuOfTheMonth();
    }

    #endregion


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }

    void Start()
    {
        ReadGameData();
        ReadRecipeGrade();
        ReadMenuOfTheMonth();
        ReadOwnRecipes();
        ReadSpecialResidentInfo();
        ReadResidentFriendShip();

        Initialize();
    }
}

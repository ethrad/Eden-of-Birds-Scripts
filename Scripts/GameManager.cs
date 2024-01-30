using HomeworkSpace;
using JetBrains.Annotations;
using PixelCrushers.DialogueSystem;
using SpecialShop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[Serializable]
public class GameData
{
    public int month;
    public int year;
    public int tycoonEnteredCount;
    public int dungeonRemainCount;
    public int reputation;

    public bool canEnterTycoon;
    public bool canEnterDungeon;
    public bool canEnterSparrowShop;
    public int dungeonEnterAdCount;
    public bool isIncaTernInTown;

    private int _gold; // RW

    public int gold
    {
        get { return _gold; } // _data 반환
        set { _gold = value; } // _data에 value 저장
    }

    private int _feather; // RW

    public int feather
    {
        get { return _feather; } // _data 반환
        set { _feather = value; } // _data에 value 저장
    }

    private bool _purchasedAdFree;

    public bool purchasedAdFree
    {
        get { return _purchasedAdFree; }
        set { _purchasedAdFree = value; }
    }

    public void Initialize()
    {
        month = 4;
        year = 1;
        tycoonEnteredCount = 0;
        dungeonRemainCount = 2;
        reputation = 0;
        canEnterTycoon = false;
        canEnterDungeon = false;
        canEnterSparrowShop = false;
        dungeonEnterAdCount = 0;
        isIncaTernInTown = false;
        _gold = 0;
        _feather = 0;
        purchasedAdFree = false;
    }

    public bool PurchaseGold(int price)
    {
        if (gold >= price)
        {
            _gold -= price;
            return true;
        }
        else
        {
            DialogueManager.ShowAlert("골드가 부족합니다.");
            return false;
        }
    }

    public void GetGold(int gold)
    {
        _gold += gold;
    }

    public bool PurchaseFeather(int price)
    {
        if (feather >= price)
        {
            _feather -= price;
            return true;
        }
        else
        {
            DialogueManager.ShowAlert("깃털이 부족합니다.");
            return false;
        }

    }

    public void GetFeather(int feather)
    {
        _feather += feather;
    }

    public void ResetCounts()
    {
        dungeonRemainCount = 2;
        tycoonEnteredCount = 0;
        dungeonEnterAdCount = 0;
    }
}

public class ReputationData
{
    public int level;
    public int star;
    public List<int> tycoonSalesVolume;
    public string title;
    public List<string> leftHeadline;
    public List<string> leftBody;
    public List<string> rightHeadline;
    public List<string> rightBody;
}

public class ResidentPreferFood
{
    public Dictionary<int, List<string>> preferFood;
}

#region Recipe and Menu Classes

[Serializable]
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

[Serializable]
public class Menu
{
    public int grade;
    public int gold;
    public float waitingTime;
    public List<string> properties;
    public string koreanName;
    public List<IngredientSequence> ingredientSequences;
}

#endregion

#region Resident

[Serializable]
public class SpecialResidentInfo
{
    public string tycoonDialogue;
    public List<int> friendshipLevel;
    public List<string> properties;
}

[Serializable]
public class ResidentFriendship
{
    public int friendship;
    public int level;
    public bool isInteractedInThisMonth;
    public bool gavePresentInThisMonth;
    public bool isAlerted;
    public List<bool> levelLimit;
    public List<bool> gotReward;
    public bool gotFurniture;

    // 가구 추가

    public ResidentFriendship()
    {

    }

    public ResidentFriendship(int f, int lv, bool i, bool p, bool n, List<bool> lm, List<bool> ir)
    {
        friendship = f;
        level = lv;
        isInteractedInThisMonth = i;
        gavePresentInThisMonth = p;
        isAlerted = n;
        levelLimit = lm;
        gotReward = ir;
    }
}

public class ResidentDiaryData : CSVData
{
    public string residentName;
    public List<int> friendshipLevel = new List<int>();

    public override void csvToClass(string[] csvArray)
    {
        residentName = csvArray[0];
        for (int i = 1; i < 6; i++)
        {
            friendshipLevel.Add(int.Parse(csvArray[i]));
        }
    }
}

public class FriendshipReward : CSVData
{
    public string residentName;
    public int gold;
    public int feather;
    public string furnitureName;
    public string appliedSystemName;
    public string skillType;
    public string skillName;
    public List<float> skillValues;

    public override void csvToClass(string[] csvArray)
    {
        residentName = csvArray[0];
        gold = int.Parse(csvArray[1]);
        feather = int.Parse(csvArray[2]);
        furnitureName = csvArray[3];
        appliedSystemName = csvArray[4];
        skillType = csvArray[5];
        skillName = csvArray[6];

        skillValues = new List<float>();
        for (int i = 7; i < 10; i++)
        {
            skillValues.Add(float.Parse(csvArray[i]));
        }
    }
}

public class Skill
{
    public string skillType; // 공격력, 방어력, 체력, 이동 속도 등
    public string skillName;
    public float skillValue;

    public Skill(string skillType, string skillName, float skillValue)
    {
        this.skillType = skillType;
        this.skillName = skillName;
        this.skillValue = skillValue;
    }
}

#endregion

public class ItemInfo
{
    public string name;
    public string attribute;
    public string description;
    public int price;
}

#region Quest Classes

public class PreCondition
{
    public int type;
    public string field1;
    public string field2;
    public string field3;

    public PreCondition(int type, string field1, string field2, string field3)
    {
        this.type = type;
        this.field1 = field1;
        this.field2 = field2;
        this.field3 = field3;
    }
}

public class QuestGoal
{
    public int type;
    public string name;
    public int count;

    public QuestGoal(int type, string name, int count)
    {
        this.type = type;
        this.name = name;
        this.count = count;
    }
}

public class QuestReward
{
    public int type;
    public string name;
    public int count;

    public QuestReward(int type, string name, int count)
    {
        this.type = type;
        this.name = name;
        this.count = count;
    }
}

public class SpecialCondition
{
    public int type1;
    public int type2;
    public string name;

    public SpecialCondition(int type1, int type2, string name)
    {
        this.type1 = type1;
        this.type2 = type2;
        this.name = name;
    }
}

public class Quest
{
    public string NPCName;
    public string EndNPCName;
    public int QID;
    public string QName;
    public string BSName;
    public string ASName;
    public List<PreCondition> preConditions;
    public List<QuestGoal> questGoals;
    public List<QuestReward> questRewards;
    public List<SpecialCondition> specialConditions;
}

[Serializable]
public class OngoingQuestGoal
{
    public int type;
    public string name;
    public int count;
    public int countGoal;

    public OngoingQuestGoal()
    {

    }

    public OngoingQuestGoal(QuestGoal questGoal)
    {
        type = questGoal.type;
        name = questGoal.name;
        count = 0;
        countGoal = questGoal.count;
    }
}

[Serializable]
public class OngoingQuest
{
    public string NPCName;
    public string EndNPCName;
    public int QID;
    public List<OngoingQuestGoal> ongoingQuestGoals;

    public OngoingQuest()
    {

    }

    public OngoingQuest(Quest quest)
    {
        NPCName = quest.NPCName;
        EndNPCName = quest.EndNPCName;
        QID = quest.QID;

        ongoingQuestGoals = new List<OngoingQuestGoal>();

        foreach (var qg in quest.questGoals)
        {
            ongoingQuestGoals.Add(new OngoingQuestGoal(qg));
        }
    }
}

  #endregion

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameData gameData; // RW
    public Dictionary<string, ReputationData> reputationData;
    public List<FriendshipReward> friendshipRewards;
    public Dictionary<string, List<Skill>> earnedSkills; // 적용 시스템, 스킬 정보


    #region Resources Data

    private void ReadReputationData()
    {
        reputationData = IOManager.instance.ReadJsonFromResources<Dictionary<string, ReputationData>>("ReputationData");
    }

    private void ReadRecipes()
    {
        menus = IOManager.instance.ReadJsonFromResources<Dictionary<string, Menu>>("Tycoon/Menus");
    }

    private void ReadItemList()
    {
        itemList = IOManager.instance.ReadJsonFromResources<Dictionary<string, ItemInfo>>("ItemList");
    }

    private void ReadQuests()
    {
        quests = IOManager.instance.ReadQuest("Quests");
    }

    public void ReadFriendshipRewards()
    {
        friendshipRewards = IOManager.instance.ReadCSV<FriendshipReward>("FriendshipRewards");
    }

    #endregion

    #region Server Data

    private void ReadGameData()
    {
        if (isTestMode)
        {
            gameData = IOManager.instance.ReadJsonFromLocal<GameData>("GameData");
        }
        else
        {
            gameData = IOManager.instance.ReadJsonFromServer<GameData>("GameData");
        }

    }

    public void WriteGameData()
    {
        if (isTestMode)
        {
            IOManager.instance.WriteJsonToLocal(gameData, "GameData.json");
        }
        else
        {
            IOManager.instance.WriteJsonToServer(gameData, "GameData");
        }
    }

    private void ReadMenuOfTheMonth()
    {
        if (isTestMode)
        {
            menuOfTheMonth = IOManager.instance.ReadJsonFromLocal<Dictionary<string, List<string>>>("MenuOfTheMonth");
        }
        else
        {
            menuOfTheMonth = IOManager.instance.ReadJsonFromServer<Dictionary<string, List<string>>>("MenuOfTheMonth");
        }
    }

    public void WriteMenuOfTheMonth()
    {
        if (isTestMode)
        {
            IOManager.instance.WriteJsonToLocal(menuOfTheMonth, "MenuOfTheMonth.json");
        }
        else
        {
            IOManager.instance.WriteJsonToServer(menuOfTheMonth, "MenuOfTheMonth");
        }
    }

    private void ReadOwnRecipes()
    {
        if (isTestMode)
        {
            ownRecipes = IOManager.instance.ReadJsonFromLocal<Dictionary<string, List<string>>>("OwnRecipes");
        }
        else
        {
            ownRecipes = IOManager.instance.ReadJsonFromServer<Dictionary<string, List<string>>>("OwnRecipes");
        }
    }

    public void WriteOwnRecipes()
    {
        if (isTestMode)
        {
            IOManager.instance.WriteJsonToLocal(ownRecipes, "OwnRecipes.json");
        }
        else
        {        
            IOManager.instance.WriteJsonToServer(ownRecipes, "OwnRecipes");
        }
    }

    private void ReadFriendshipData()
    {
        friendshipLevelUpValue = IOManager.instance.ReadCSV<ResidentDiaryData>("FriendshipLevelUpValue");
    }

    void ReadResidentFriendShip()
    {
        if (isTestMode)
        {
            residentFriendships = IOManager.instance.ReadJsonFromLocal<Dictionary<string, ResidentFriendship>>("ResidentFriendship");
        }
        else
        {
            residentFriendships = IOManager.instance.ReadJsonFromServer<Dictionary<string, ResidentFriendship>>("ResidentFriendship");
        }
    }

    public void WriteResidentFriendShip()
    {
        if (isTestMode)
        {
            IOManager.instance.WriteJsonToLocal(residentFriendships, "ResidentFriendship.json");
        }
        else
        {
            IOManager.instance.WriteJsonToServer(residentFriendships, "ResidentFriendship");
        }
    }

    private void ReadInventory()
    {
        if (isTestMode)
        {
            inventory = IOManager.instance.ReadJsonFromLocal<Dictionary<string, int>>("Inventory");
        }
        else
        {
            inventory = IOManager.instance.ReadJsonFromServer<Dictionary<string, int>>("Inventory");
        }
    }

    public void WriteInventory()
    {
        if (isTestMode)
        {
            IOManager.instance.WriteJsonToLocal(inventory, "Inventory.json");
        }
        else
        {
            IOManager.instance.WriteJsonToServer(inventory, "Inventory");
        }
    }

    private void ReadClearedQuests()
    {
        if (isTestMode)
        {
            clearedQuests = IOManager.instance.ReadJsonFromLocal<Dictionary<string, List<int>>>("ClearedQuests");
        }
        else
        {
            clearedQuests = IOManager.instance.ReadJsonFromServer<Dictionary<string, List<int>>>("ClearedQuests");
        }
    }

    private void ReadOngoingQuests()
    {
        if (isTestMode)
        {
            ongoingQuests = IOManager.instance.ReadJsonFromLocal<Dictionary<string, List<OngoingQuest>>>("OngoingQuests");
        }
        else
        {
            ongoingQuests = IOManager.instance.ReadJsonFromServer<Dictionary<string, List<OngoingQuest>>>("OngoingQuests");
        }
    }

    private void WriteClearedQuests()
    {
        if (isTestMode)
        {
            IOManager.instance.WriteJsonToLocal(clearedQuests, "ClearedQuests.json");
        }
        else
        {
            IOManager.instance.WriteJsonToServer(clearedQuests, "ClearedQuests");
        }
    }

    public void WriteOngoingQuests()
    {
        if (isTestMode)
        {
            IOManager.instance.WriteJsonToLocal(ongoingQuests, "OngoingQuests.json");
        }
        else
        {
            IOManager.instance.WriteJsonToServer(ongoingQuests, "OngoingQuests");
        }
    }

    private void ReadHomeworkList()
    {
        homeworkList = IOManager.instance.ReadCSV<Homework>("HomeworkList");
    }

    private void ReadHomeworkData()
    {
        if (isTestMode)
        {
            if (IOManager.instance.ReadJsonFromLocal<Dictionary<string, HomeworkData>>("HomeworkData").TryGetValue("HomeworkData", out var homeworkJsonData))
            {
                homeworkData = homeworkJsonData;

                if ((DateTime.Now - homeworkData.initTime).Days >= 1)
                {
                    homeworkData = new HomeworkData(homeworkList);
                }
            }
            else
            {
                homeworkData = new HomeworkData(homeworkList);
            }
        }
        else
        {
            if (IOManager.instance.ReadJsonFromServer<Dictionary<string, HomeworkData>>("HomeworkData").TryGetValue("HomeworkData", out var homeworkJsonData))
            {
                homeworkData = homeworkJsonData;

                if ((DateTime.Now - homeworkData.initTime).Days >= 1)
                {
                    homeworkData = new HomeworkData(homeworkList);
                }
            }
            else
            {
                homeworkData = new HomeworkData(homeworkList);
            }
        }
    }

    public void WriteHomeworkData()
    {
        var tempDictionary = new Dictionary<string, HomeworkData>();
        tempDictionary.Add("HomeworkData", homeworkData);

        if (isTestMode)
        {
            IOManager.instance.WriteJsonToLocal(tempDictionary, "HomeworkData.json");
        }
        else
        {
            IOManager.instance.WriteJsonToServer(tempDictionary, "HomeworkData");
        }
    }


    void ReadDiary()
    {
        Dictionary<string, List<string>> tempDictionary;
        
        if (isTestMode)
        {
            tempDictionary = IOManager.instance.ReadJsonFromLocal<Dictionary<string, List<string>>>("DungeonDiary");
        }
        else
        {
            tempDictionary = IOManager.instance.ReadJsonFromServer<Dictionary<string, List<string>>>("DungeonDiary");
        }
        
        if (!tempDictionary.TryGetValue("MonsterDiary", out var v))
        {
            monsterDiary = new List<string>();
            dungeonDiary = new List<string>();
        }
        else
        {
            monsterDiary = tempDictionary["MonsterDiary"];
            dungeonDiary = tempDictionary["DungeonDiary"];
        }
    }

    public void WriteDiary()
    {
        Dictionary<string, List<string>> tempDictionary = new Dictionary<string, List<string>>();
        tempDictionary.Add("MonsterDiary", monsterDiary);
        tempDictionary.Add("DungeonDiary", dungeonDiary);

        if (isTestMode)
        {
            IOManager.instance.WriteJsonToLocal(tempDictionary, "DungeonDiary.json");
        }
        else
        {
            IOManager.instance.WriteJsonToServer(tempDictionary, "DungeonDiary");
        }
    }

    public void ReadEarnedRewards()
    {
        if (isTestMode)
        {
            earnedSkills = IOManager.instance.ReadJsonFromLocal<Dictionary<string, List<Skill>>>("EarnedRewards");
        }
        else
        {
            earnedSkills = IOManager.instance.ReadJsonFromServer<Dictionary<string, List<Skill>>>("EarnedRewards");
        }
        
    }

    public void WriteEarnedRewards()
    {
        if (isTestMode)
        {
            IOManager.instance.WriteJsonToLocal(earnedSkills, "EarnedRewards.json");
        }
        else
        {
            IOManager.instance.WriteJsonToServer(earnedSkills, "EarnedRewards");
        }
    }

    #endregion

    #region Local Data

    public Settings settings;

    private void ReadSettings()
    {
        settings = IOManager.instance.ReadJsonFromLocal<Settings>("Settings");

        // 볼륨이랑 미니맵 홀딩 방식이랑 조이스틱 위치 세팅
    }

    public void WriteSettings()
    {
        IOManager.instance.WriteJsonToLocal(settings, "Settings.json");
    }

    #endregion

    #region Date System

    public void EndTycoon()
    {
        gameData.tycoonEnteredCount++;

        if (gameData.tycoonEnteredCount == 3)
        {
            AddMonth();
            gameData.tycoonEnteredCount = 0;
        }

        WriteOngoingQuests();
        WriteGameData();
        WriteInventory();

        if (isMonthChanged)
        {
            LoadScene("Town");
        }
        else
        {
            if (gameData.purchasedAdFree)
            {
                SceneManager.LoadScene("Town");
            }
            else
            {
                LoadMobileAD.Instance.LoadInterstitialAd("Town");
            }
        }
    }

    [HideInInspector]
    public bool isMonthChanged;

    private void AddMonth()
    {
        gameData.month++;
        gameData.ResetCounts();
        isMonthChanged = true;

        foreach (var item in friendshipLevelUpValue)
        {
            residentFriendships[item.residentName].gavePresentInThisMonth = false;
            residentFriendships[item.residentName].isInteractedInThisMonth = false;
        }

        if (gameData.month == 13)
        {
            AddYear();
            gameData.month = 1;
        }
    }

    void AddYear()
    {
        gameData.year++;
    }

    #endregion

    #region Recipe and Menu

    public Dictionary<string, Menu> menus;
    public Dictionary<string, List<string>> ownRecipes; // grade.ToString(), menuName
    public Dictionary<string, List<string>> menuOfTheMonth; // grade.ToString(), menuName

    // 플레이어가 무슨 레시피 얻었는지 기록

    public void GetRecipe(string recipeName)
    {
        if (!ownRecipes.ContainsKey(menus[recipeName].grade.ToString()))
        {
            ownRecipes.Add(menus[recipeName].grade.ToString(), new List<string>());
        }
        if (!ownRecipes[menus[recipeName].grade.ToString()].Contains(recipeName))
        {
            ownRecipes[menus[recipeName].grade.ToString()].Add(recipeName);
        }
    }

    [HideInInspector]
    public string invitedResidentName;

    #endregion

    #region ResidentFriendship

    public Dictionary<string, ResidentFriendship> residentFriendships; // RW

    public List<ResidentDiaryData> friendshipLevelUpValue;


    public void InitializedResidentFriendshipList(string residentName)
    {
        if (!residentFriendships.TryGetValue(residentName, out ResidentFriendship temp)) //없는 캐릭터면 추가해주기
        {
            List<bool> lvLimitTemp = new List<bool>();
            for (int i = 0; i < 5; i++)
            {
                lvLimitTemp.Add(false);
            }

            List<bool> isRewardTemp = new List<bool>();
            for (int i = 0; i < 5; i++)
            {
                isRewardTemp.Add(false);
            }

            ResidentFriendship initializeValue = new ResidentFriendship(100, 0, false, false, false, lvLimitTemp, isRewardTemp);
            residentFriendships.Add(residentName, initializeValue);
        }
        else return;
    }

    public void InitializeResidentNotice()
    {
        foreach (var item in residentFriendships)
        {
            ResidentDiaryData tempData =
                friendshipLevelUpValue.Find(friendshipLevelUpValue => friendshipLevelUpValue.residentName == item.Key);
            if (item.Value.isAlerted == false && item.Value.friendship >= tempData.friendshipLevel[item.Value.level])
                item.Value.isAlerted = true;
            else return;
        }
    }

    //이 달의 첫 번째 대화인지 체크
    public void CheckInteractedInThisMonth(string residentName)
    {
        ResidentDiaryData residentLevelUpValue =
            friendshipLevelUpValue.Find(friendshipLevelUpValue => friendshipLevelUpValue.residentName == residentName);

        if (residentFriendships[residentName].isInteractedInThisMonth == false) //이번 달 첫 번째 대화
        {
            TownManager.instance.gameObject.GetComponent<ResidentFriendshipFunction>().ResidentFriendshipAddOrSub(residentName, 8);
            residentFriendships[residentName].isInteractedInThisMonth = true;
        }

        ResidentLevelCheck(residentLevelUpValue, residentName);
        WriteResidentFriendShip();
    }


    public void SetFriendShip(string residentName, int value, bool isGivePresent)
    {
        ResidentDiaryData item =
            friendshipLevelUpValue.Find(friendshipLevelUpValue => friendshipLevelUpValue.residentName == residentName);

        int tempLevel = residentFriendships[residentName].level;

        //현재 호감도레벨 돌파 안 된 상태 
        if (residentFriendships[residentName].levelLimit[tempLevel] == false)
        {
            if (residentFriendships[residentName].friendship + value < item.friendshipLevel[tempLevel])
            {
                residentFriendships[residentName].friendship += value;
                if (isGivePresent) ShowFriendship(value);
            }
            else
            {
                //레벨 한계수치로 해주기
                value = item.friendshipLevel[tempLevel] - residentFriendships[residentName].friendship;
                if (isGivePresent) ShowFriendship(value);
                residentFriendships[residentName].friendship = item.friendshipLevel[tempLevel];
                DialogueManager.ShowAlert("애장품을 선물해 레벨 상한을 올려보세요!");
                residentFriendships[residentName].isAlerted = true;
                WriteResidentFriendShip();
            }
        }

        if (residentFriendships[residentName].friendship < 0) residentFriendships[residentName].friendship = 0;
        else if (residentFriendships[residentName].friendship >= item.friendshipLevel[item.friendshipLevel.Count - 1])
            residentFriendships[residentName].friendship = item.friendshipLevel[item.friendshipLevel.Count - 1];

        WriteResidentFriendShip();
    }


    //레벨 체크 함수
    public void ResidentLevelCheck(ResidentDiaryData residentInfo, string residentName)
    {
        for (int i = 0; i < residentInfo.friendshipLevel.Count; i++)
        {
            if (residentFriendships[residentName].friendship >= residentInfo.friendshipLevel[i])
            {
                if (residentFriendships[residentName].levelLimit[i])
                    residentFriendships[residentName].level = i + 1;
            }
        }
    }

    public void ShowFriendship(int wantValue)
    {
        if (SceneManager.GetActiveScene().name == "Town")
        {
            ResidentController temp = TownManager.instance.residentDic[TownManager.instance.residentName];

            temp.friendshipValuePanel.SetTrigger("FriendshipAnim");
            if (wantValue < 0)
                temp.valueText.text = wantValue.ToString();
            else
                temp.valueText.text = "+" + wantValue;
        }
    }

    public void EarnFriendshipReward(string residentName, int friendshipLevel)
    {
        var selectedFriendshipReward = friendshipRewards.Find(fr => fr.residentName == residentName);

        switch (friendshipLevel)
        {
            case 0: // 골드랑 깃털
                gameData.GetGold(selectedFriendshipReward.gold);
                gameData.GetFeather(selectedFriendshipReward.feather);

                break;

            case 1: // 가구
                residentFriendships[residentName].gotFurniture = true;
                TownManager.instance.residentDic[residentName].SetFurniture();
                WriteResidentFriendShip();
                break;

            case 2:
            case 3:
            case 4: // 스킬
                var earnedReward2 = new Skill(selectedFriendshipReward.skillType, selectedFriendshipReward.skillName, selectedFriendshipReward.skillValues[friendshipLevel - 2]);

                if (!earnedSkills.TryGetValue(selectedFriendshipReward.appliedSystemName, out var skillList))
                {
                    earnedSkills.Add(selectedFriendshipReward.appliedSystemName, new List<Skill>());
                }

                // 스킬이 중복으로 들어올 때 처리
                var matchedSkill = earnedSkills[selectedFriendshipReward.appliedSystemName].Find(skill => skill.skillName == earnedReward2.skillName);

                if (matchedSkill != null)
                {
                    matchedSkill.skillValue = earnedReward2.skillValue;
                }
                else
                {
                    earnedSkills[selectedFriendshipReward.appliedSystemName].Add(earnedReward2);
                }
                WriteEarnedRewards();
                break;
        }
    }

    #endregion

    public Dictionary<string, ItemInfo> itemList; // Read Only
    public Dictionary<string, int> inventory; // RW

    [HideInInspector]
    public List<string> monsterDiary;
    [HideInInspector]
    public List<string> dungeonDiary;

    #region Quest

    public Dictionary<string, List<Quest>> quests; // Read Only
    public Dictionary<string, List<int>> clearedQuests; // RW
    public Dictionary<string, List<OngoingQuest>> ongoingQuests; // RW

    public List<Quest> CheckPreCondition(string NPCName)
    {
        // 한 캐릭터에 대한 퀘스트만 검사
        // NPCName이 일치하는 퀘스트 중 ongoingQuests에 있거나 clearedQuests에 있는 퀘스트는 제외

        List<Quest> temp = new List<Quest>();


        for (int i = 0; i < quests[NPCName].Count; i++)
        {
            if (clearedQuests.TryGetValue(NPCName, out List<int> value) && value.Contains(i))
            {
                continue;
            }

            if (ongoingQuests.TryGetValue(NPCName, out var v2) && v2.Any(oq => oq.QID == i))
            {
                continue;
            }

            bool canAcceptQuest = true;

            foreach (var pc in quests[NPCName][i].preConditions)
            {
                switch (pc.type)
                {
                    case 0:
                        if (!clearedQuests.TryGetValue(pc.field1, out value) || !value.Contains(int.Parse(pc.field2)))
                        {
                            canAcceptQuest = false;
                            goto End;
                        }
                        break;

                    case 1:
                        if (!residentFriendships.TryGetValue(pc.field1, out ResidentFriendship v) || !(v.friendship >= int.Parse(pc.field2)))
                        {
                            canAcceptQuest = false;
                            goto End;
                        }
                        break;

                    case 2:
                        if (!(gameData.year * 12 + gameData.month >= int.Parse(pc.field1) * 12 + int.Parse(pc.field2)))
                        {
                            canAcceptQuest = false;
                            goto End;
                        }
                        break;
                }
            }

            if (canAcceptQuest)
            {
                temp.Add(quests[NPCName][i]);
                CheckSpecialCondition(quests[NPCName][i], 0);
            }

            End: ;
        }

        return temp;
    }

    public void AcceptQuest(Quest quest)
    {
        if (quest.questGoals[0].type == 0)
        {
            ClearQuest(quest);

            return;
        }

        OngoingQuest ongoingQuest = new OngoingQuest(quest);

        if (!ongoingQuests.TryGetValue(quest.NPCName, out var v))
        {
            ongoingQuests.Add(quest.NPCName, new List<OngoingQuest>());
        }

        ongoingQuests[quest.NPCName].Add(ongoingQuest);
        CheckSpecialCondition(quest, 1);

        WriteOngoingQuests();
    }

    public List<Quest> CheckGoalCondition(string NPCName)
    {
        // OngoingQuests에 있는 퀘스트 중 EndNPCName이 일치 + 퀘스트 목표를 모두 달성

        List<Quest> temp = new List<Quest>();

        foreach (var p in ongoingQuests)
        {
            foreach (var oq in p.Value)
            {
                if (clearedQuests.TryGetValue(oq.NPCName, out var result))
                {
                    if (result.Contains(oq.QID))
                    {
                        continue;
                    }
                }

                if (oq.EndNPCName == NPCName)
                {
                    foreach (var oqg in oq.ongoingQuestGoals)
                    {
                        switch (oqg.type)
                        {
                            case 0:
                            case 1:
                                break;

                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 7:
                            case 8:
                            case 10:
                            case 11:
                            case 12:
                            case 13:
                            case 14:
                                if (!(oqg.count >= oqg.countGoal))
                                {
                                    goto End;
                                }
                                break;

                            case 6:
                                if (!inventory.TryGetValue(oqg.name, out int item) || !(item >= oqg.countGoal))
                                {
                                    goto End;
                                }
                                break;

                            case 9:
                                if (!(gameData.gold >= oqg.countGoal))
                                {
                                    goto End;
                                }
                                break;
                        }
                    }
                    temp.Add(quests[oq.NPCName][oq.QID]);
                    End: ;
                }
            }
        }
        return temp;
    }

    public void ClearQuest(Quest quest)
    {
        if (!clearedQuests.TryGetValue(quest.NPCName, out var v))
        {
            clearedQuests.Add(quest.NPCName, new List<int>());
        }
        clearedQuests[quest.NPCName].Add(quest.QID);

        foreach (var item in quest.questRewards)
        {
            switch (item.type)
            {
                case 0:
                    gameData.GetFeather(item.count);
                    break;

                case 1:
                    gameData.GetGold(item.count);
                    break;

                case 3:
                    AddItemToInventory(item.name, item.count);
                    break;

                case 2:
                    if (residentFriendships.TryGetValue(item.name, out var residentFriendship))
                    {
                        SetFriendShip(item.name, item.count, false);
                    }
                    else
                    {
                        List<bool> lvLimitTemp = new List<bool>();
                        for (int i = 0; i < 5; i++)
                        {
                            lvLimitTemp.Add(false);
                        }

                        List<bool> isRewardTemp = new List<bool>();
                        for (int i = 0; i < 5; i++)
                        {
                            isRewardTemp.Add(false);
                        }
                        residentFriendships.Add(item.name, new ResidentFriendship(item.count, 0, false, false, false, lvLimitTemp, isRewardTemp));
                    }
                    break;

                case 4:
                    GetRecipe(item.name);
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
            }
        }

        if (ongoingQuests.TryGetValue(quest.NPCName, out var v2))
        {
            for (int i = 0; i < v2.Count; i++)
            {
                if (v2[i].QID == quest.QID)
                {
                    ongoingQuests[quest.NPCName].RemoveAt(i);
                    break;
                }
            }
        }

        WriteOngoingQuests();
        WriteClearedQuests();
        WriteOwnRecipes();
        WriteGameData();
        WriteInventory();

        CheckSpecialCondition(quest, 2);
    }


    void CheckSpecialCondition(Quest quest, int type1)
    {
        foreach (var sc in quest.specialConditions)
        {
            if (sc.type1 == type1)
            {
                switch (sc.type2)
                {
                    case 0:
                        if (sc.name == "on") // 던전 입장 제한을 켠다
                        {
                            gameData.canEnterDungeon = false;
                        }
                        else // 던전 입장 제한을 끈다
                        {
                            gameData.canEnterDungeon = true;
                        }
                        WriteGameData();
                        break;
                    case 1:
                        if (sc.name == "on")
                        {
                            gameData.canEnterTycoon = false;
                        }
                        else
                        {
                            gameData.canEnterTycoon = true;
                        }
                        WriteGameData();
                        break;
                    case 2:

                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        if (sc.name == "on")
                        {
                            gameData.canEnterSparrowShop = false;
                        }
                        else
                        {
                            gameData.canEnterSparrowShop = true;
                        }
                        WriteGameData();
                        break;
                }
            }
        }
    }

    #endregion

    #region Homework

    [HideInInspector]
    public HomeworkData homeworkData;
    [HideInInspector]
    public List<Homework> homeworkList;

    #endregion

    #region Loading

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(WaitLoadScene(sceneName));
    }

    IEnumerator WaitLoadScene(string sceneName)
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        //op.allowSceneActivation = false;
        float timer = 0.0f;

        while (!op.isDone && timer < 2.0f)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        if (!gameData.purchasedAdFree)
        {
            LoadMobileAD.Instance.LoadInterstitialAd();
        }
    }

    #endregion

    #region SpecialShop

    public List<ShopData> specialShopItemList;
    public Dictionary<string, ResidentTreasureShopData> residentTreasureShopList;

    void ReadSpecialShopData()
    {
        specialShopItemList = IOManager.instance.ReadCSV<ShopData>("SpecialShop");
    }

    void ReadResidentTreasureShop()
    {
        residentTreasureShopList = IOManager.instance.ReadJsonFromResources<Dictionary<string, ResidentTreasureShopData>>("ResidentsTreasureShop");
    }

    public void WriteResidentTreasureShop()
    {
        IOManager.instance.WriteJsonToServer(residentTreasureShopList, "ResidentTreasureShop");
    }

    #endregion

    #region SparrowShop

    public Dictionary<string, SparrowShop> sparrowShopList;

    void ReadSparrowShopData()
    {
        sparrowShopList = IOManager.instance.ReadJsonFromResources<Dictionary<string, SparrowShop>>("SparrowShop");
    }

    #endregion


    // 튜토리얼 끝난 후에 할 것들 모음
    #region Initialize

    private void Initialize()
    {
        gameData.Initialize();
        menuOfTheMonth.Add("1", new List<string>());
        menuOfTheMonth.Add("2", new List<string>());

        menuOfTheMonth["1"].Add("slimeMargarita");
        menuOfTheMonth["1"].Add("bigTea");
        menuOfTheMonth["2"].Add("ratatouille");
        menuOfTheMonth["2"].Add("rabbitSteak");

        WriteGameData();
        WriteMenuOfTheMonth();
    }

    #endregion

    #region Inventory

    public void AddItemToInventory(string itemName, int count)
    {
        if (!inventory.TryGetValue(itemName, out var i))
        {
            inventory.Add(itemName, 0);
        }

        inventory[itemName] += count;
    }

    public bool SubItemToInventory(string itemName, int count)
    {
        if (!inventory.TryGetValue(itemName, out var i))
        {
            if (i < count)
            {
                return false;
            }
            inventory[itemName] -= count;
            return true;
        }

        return false;
    }

    #endregion

    #region Fade Effect

    [SerializeField]
    private GameObject FadeEffectPanel;

    public void FadeInEffect() // 점점 밝아지는 효과
    {
        FadeEffectPanel.GetComponent<FadeEffectManager>().StandardPanelFadeIn();
    }

    public void FadeOutEffect() // 점점 어두워지는 효과
    {
        FadeEffectPanel.GetComponent<FadeEffectManager>().StandardPanelFadeOut();

    }

    public void FadeEffect() //인 아웃 합침
    {
        FadeEffectPanel.GetComponent<FadeEffectManager>().StandardPanelFadeEffect();
    }

    [HideInInspector]
    public bool isFadeInEnd;
    [HideInInspector]
    public bool isFadeOutEnd;
    [HideInInspector]
    public bool isFadeEffectEnd;

    public void ResetFadeFlags()
    {
        isFadeInEnd = false;
        isFadeOutEnd = false;
        isFadeEffectEnd = false;
    }

    #endregion


    #region Server

    public GameObject FailedToConnectServerPanel;
    private bool isErrorTypeRead;
    private string writeColumnName;

    [SerializeField]
    private bool isTestMode;
    public void ReadDataFromServer()
    {
        ReadGameData();
        ReadMenuOfTheMonth();
        ReadOwnRecipes();
        ReadResidentFriendShip();
        ReadClearedQuests();
        ReadOngoingQuests();
        ReadHomeworkData();
        ReadDiary();
        ReadInventory();
        ReadEarnedRewards();

        if (gameData.month == 0)
        {
            Initialize();
        }

        InitializeResidentNotice();
    }

    public void FailedToConnectServer(bool isErrorTypeRead)
    {
        this.isErrorTypeRead = isErrorTypeRead;
        FailedToConnectServerPanel.SetActive(true);
    }
    public void FailedToConnectServer(bool isErrorTypeRead, string columnName)
    {
        this.isErrorTypeRead = isErrorTypeRead;
        writeColumnName = columnName;
        FailedToConnectServerPanel.SetActive(true);
    }
    public void OnReconnectButtonClicked()
    {
        if (isErrorTypeRead)
        {
            FailedToConnectServerPanel.SetActive(false);
            ReadDataFromServer();
        }
        else if (!isErrorTypeRead)
        {
            FailedToConnectServerPanel.SetActive(false);
            switch (writeColumnName)
            {
                case "GameData":
                    WriteGameData();
                    break;
                case "OwnRecipes":
                    WriteOwnRecipes();
                    break;
                case "ResidentFriendship":
                    WriteResidentFriendShip();
                    break;
                case "ClearedQuests":
                    WriteClearedQuests();
                    break;
                case "OngoingQuests":
                    WriteOngoingQuests();
                    break;
                case "Inventory":
                    WriteInventory();
                    break;
                case "HomeworkData":
                    WriteHomeworkData();
                    break;
                case "DungeonDiary":
                    WriteDiary();
                    break;
                case "EarnedRewards":
                    WriteEarnedRewards();
                    break;
            }
        }
    }

    #endregion

    private void Awake()
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

    private void Start()
    {
        ReadReputationData();
        ReadRecipes();
        ReadItemList();
        ReadQuests();
        ReadHomeworkList();
        ReadSpecialShopData();
        ReadResidentTreasureShop();
        ReadFriendshipData();

        ReadSparrowShopData();
        ReadFriendshipRewards();

        ReadSettings();
    }

    public GameObject gameExitPanel;
}

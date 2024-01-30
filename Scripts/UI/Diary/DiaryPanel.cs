using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeon;


namespace Diary
{
    public class Resident
    {
        public string koreanName;
        public string sex;
        public string homeTown;
        public string height;
        public string personality;
        public Dictionary<int, List<string>> preferredFood;
    }

    public class Dungeon
    {
        public string koreanName;
        public List<string> monsterList = new List<string>();
        public string story;
    }

    public class Food
    {
        public string koreanName;
        public int grade;
        public int price;
        public Dictionary<string, int> ingredients;
        public string description;

    }
    public class Monster
    {
        public string koreanName;
        public int hp;
        public int atk;
        public string area;
        public List<string> dropItems = new List<string>();
        public string description;
    }

    public class DiaryPanel : MonoBehaviour
    {
        private bool isInitialized;

        #region FoodPage
        public Dictionary<string, Food> food;
        void ReadFoodList()
        {
            food = IOManager.instance.ReadJsonFromResources<Dictionary<string, Food>>("Foods");
        }
        #endregion

        #region ResidentPage
        public Dictionary<string, Resident> resident;

        void ReadResidentList()
        {
            resident = IOManager.instance.ReadJsonFromResources<Dictionary<string, Resident>>("Residents");
        }
        #endregion
        
        #region MonsterPage
        public  Dictionary<string, Monster> monster;

        void ReadMonsterList()
        {
            monster = IOManager.instance.ReadJsonFromResources<Dictionary<string, Monster>>("Monsters");
        }
        #endregion
    
        #region DungeonPage
        public Dictionary<string, Dungeon> dungeon;

        void ReadDungeonList()
        {
            dungeon = IOManager.instance.ReadJsonFromResources<Dictionary<string, Dungeon>>("Dungeons");
        }
        #endregion
        
        public void OnDiaryButtonClicked()
        {
            if (!isInitialized)
            {
                ReadFoodList();
                ReadResidentList();
                ReadMonsterList();
                ReadDungeonList();
                Initialize();
                PageInitialize();
            }

            foreach (var p in pages)
            {
                p.Value.GetComponent<Page>().Initialize();
            }

            //처음에 나올 페이지 해줘야 함
            pages[currentPanelName].SetActive(true);
            pages[currentPanelName].GetComponent<Page>().detailPanel.SetActive(true);
            gameObject.SetActive(true);
        }

        public void OnExitButtonClicked()
        {
            gameObject.SetActive(false);
            pages[currentPanelName].GetComponent<Page>().OffPage();
            pages[currentPanelName].SetActive(false);
        }


        private Dictionary<string, GameObject> pages = new Dictionary<string, GameObject>();

        public GameObject foodPage;
        public GameObject questPage;
        public GameObject residentPage;
        public GameObject dungeonPage;
        public GameObject monsterPage;

        private string currentPanelName;

        public void OnPageButtonClicked(Button button)
        {
            // 버튼 스프라이트를 selected로 변경할 수 있는지?

            if (currentPanelName != button.name)
            {
                pages[currentPanelName].GetComponent<Page>().OffPage();
                currentPanelName = button.name;
                pages[currentPanelName].GetComponent<Page>().OnPage();
            }
        }

        private void Initialize()
        {
            currentPanelName = "food"; //처음에 나올 페이지 뭘로?

            pages.Add("food", foodPage);
            pages.Add("quest", questPage);
            pages.Add("resident", residentPage);
            pages.Add("dungeon", dungeonPage);
            pages.Add("monster", monsterPage);

            isInitialized = true;
        }

        public void PageInitialize()
        {
            foodPage.GetComponent<FoodPage>().Initialize(food);
            residentPage.GetComponent<ResidentPage>().Initialize(resident);
            monsterPage.GetComponent<MonsterPage>().Initialize(monster);
            dungeonPage.GetComponent<DungeonPage>().Initialize(dungeon);
        }

    }
}
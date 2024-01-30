using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

namespace Tycoon
{
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

        
        public Dictionary<string, SpecialResidentInfo> specialResidentInfo; // Read Only
        
        void ReadSpecialResidentInfo()
        {
            specialResidentInfo = IOManager.instance.ReadJsonFromResources<Dictionary<string, SpecialResidentInfo>>("Tycoon/SpecialResidentInfo");
        }
        
        #region decide order

        int specialResidentCount = 0;

        Dictionary<string, int> menuSalesVolumes = new Dictionary<string, int>();
        int menuWholeVolume = 0;

        [HideInInspector]
        public List<Order> orderList = new List<Order>();
        
        private void DecideSpecialResidentCount()
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

        private void DecideSalesVolume()
        {
            menuWholeVolume = 0;

            int range1;
            int range2;
            int range3;
            bool hasThirdGradeRecipes = GameManager.instance.ownRecipes.TryGetValue("3", out var thirdGrandRecipes);
            if (hasThirdGradeRecipes)
            {
                ReputationData tempReputationData = null;

                foreach (var reputationData in GameManager.instance.reputationData)
                {
                    if (GameManager.instance.gameData.reputation > int.Parse(reputationData.Key)) continue;
                    tempReputationData = reputationData.Value;
                    break;
                }
                
                List<int> salesVolume = tempReputationData.tycoonSalesVolume;
                range1 = Random.Range(salesVolume[0], salesVolume[1]);
                range2 = Random.Range(salesVolume[2], salesVolume[3]);
                range3 = Random.Range(salesVolume[4], salesVolume[5]);
            }
            else
            {
                range1 = Random.Range(8, 10);
                range2 = Random.Range(6, 8);
                range3 = 0;
            }

            menuWholeVolume = range1 + range2 + range3;


            var tempList = GameManager.instance.menuOfTheMonth["1"];

            menuSalesVolumes[tempList[0]] = range1 / 2;
            menuSalesVolumes[tempList[0]] += range1 % 2;

            menuSalesVolumes[tempList[1]] = range1 / 2;

            if (hasThirdGradeRecipes)
            {
                tempList = GameManager.instance.menuOfTheMonth["2"];

                menuSalesVolumes[tempList[0]] = range2;

                tempList = GameManager.instance.menuOfTheMonth["3"];

                menuSalesVolumes[tempList[0]] = range3;
            }
            else
            {
                tempList = GameManager.instance.menuOfTheMonth["2"];

                menuSalesVolumes[tempList[0]] = range2 / 2;
                menuSalesVolumes[tempList[0]] += range2 % 2;

                menuSalesVolumes[tempList[1]] = range2 / 2;
            }
        }

        // 특수주민 대기시간은 40초
        private void SelectSpecialResidents()
        {
            List<string> residentNames = new List<string>();

            for (int i = 0; i < specialResidentInfo.Count; i++)
            {
                residentNames.Add(specialResidentInfo.ElementAt(i).Key);
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

        private void AddInvitedResident()
        {
            if (string.IsNullOrEmpty(GameManager.instance.invitedResidentName))
            {
                return;
            }
            
            foreach (var order in orderList)
            {
                if (order.residentName == GameManager.instance.invitedResidentName)
                {
                    return;
                }
            }
            
            Order tempOrder = new Order();
            tempOrder.residentName = GameManager.instance.invitedResidentName;
            tempOrder.isSpecial = true;
            tempOrder.waitingTime = 40.0f;
            
            orderList.Add(tempOrder);
            GameManager.instance.invitedResidentName = "";
        }

        private void NormalResidentsTakeMenus()
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
                        tempOrder1.waitingTime += GameManager.instance.menus[menuName].waitingTime;
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
                            tempOrder1.waitingTime += GameManager.instance.menus[mn].waitingTime;
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

        private void ApplySkills()
        {
            foreach (var skillList in GameManager.instance.earnedSkills)
            {
                if (skillList.Key == "tycoon")
                {
                    foreach(var skill in skillList.Value)
                    {
                        switch (skill.skillType)
                        {
                            case "waitingTime":
                                foreach (var order in orderList)
                                {
                                    order.waitingTime += skill.skillValue;
                                }
                                break;
                        }
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

        private int revenue = 0;
        [HideInInspector]
        public int loss = 0;

        public void EarnGold(string foodName)
        {
            revenue += GameManager.instance.menus[foodName].gold;
        }

        public void EarnGold(int gold)
        {
            revenue += gold;
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

            tycoonEndPanel.GetComponent<EndPanel>().UpdatePanel(revenue, loss);
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

            ReadSpecialResidentInfo();
            DecideSpecialResidentCount();
            DecideSalesVolume();
            SelectSpecialResidents();
            AddInvitedResident();
            NormalResidentsTakeMenus();
            ApplySkills();
        }
    }
}

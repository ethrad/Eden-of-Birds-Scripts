using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Diary
{
    public class ResidentPage : Page
    {
        public GameObject content;

        // page 2
        public Image image;
        [FormerlySerializedAs("name")] public Text residentNameText;
        public Text residentInfo;
        public Text hometown;
        public GameObject friendshipPanel;
        public GameObject heartIcon;
        public Text friendship;
        public Text personality;
        public Text prefferdFood;

        public GameObject residentSlotPre;
        public GameObject nonePanel;
        public GameObject upgradePanel;
        public GameObject upgradePanelButton;
        public Scrollbar scrollbar;

        public GameObject descPanel;
        
        [HideInInspector]
        public Dictionary<string, Resident> residents;
        List<ResidentSlotButton> resSlotPreList = new List<ResidentSlotButton>();
        [HideInInspector]
        public string curResName;

        public GameObject treasureTutorialPanel;

        //애장품 튜토리얼 보여줄지 말지 체크하는 함수
        //슬롯 터치할때 실행됨
        public void CheckTreasureTutorial()
        {
            if (treasureTutorialPanel.GetComponent<TreasureTutorial>().isTreasureTutorial == false && GameManager.instance.residentFriendships[curResName].isAlerted)
            {
                treasureTutorialPanel.GetComponent<TreasureTutorial>().StartTreasureTutorial();
            }
        }

        public void SetResidentSlotDetail(string residentName)
        {
            prefferdFood.text = "";
            
            if (GameManager.instance.residentFriendships.TryGetValue(residentName, out ResidentFriendship temp))
            {
                scrollbar.value = 1;
                curResName = residentName;
                                
                if(GameManager.instance.residentFriendships[curResName].isAlerted)
                {
                    upgradePanelButton.SetActive(true);
                }
                else upgradePanelButton.SetActive(false);
                
                image.sprite = Resources.Load<Sprite>("Diary/Illustrations/" + residentName);
                residentNameText.text = residents[residentName].koreanName;
                residentInfo.text = residents[residentName].sex + "/" + residents[residentName].height;
                hometown.text = residents[residentName].homeTown;

                if(friendshipPanel.transform.childCount > 0)
                {
                    for (int i = 0; i < friendshipPanel.transform.childCount; i++)
                    {
                        Destroy(friendshipPanel.transform.GetChild(i).gameObject);
                    }
                }
                
                //레벨에 따른 하트프리팹 생성
                int curLevel = GameManager.instance.residentFriendships[curResName].level;
                for (int i = 0; i < curLevel; i++)
                {
                    Instantiate(heartIcon, friendshipPanel.transform);
                }

                //호감도 레벨 클래스에 넣기
                ResidentDiaryData item =
                    GameManager.instance.friendshipLevelUpValue.Find(friendshipLevelUpValue =>
                        friendshipLevelUpValue.residentName == residentName);

                friendship.text = GameManager.instance.residentFriendships[curResName].friendship + "/" +
                                  item.friendshipLevel[curLevel];
                personality.text = residents[residentName].personality;

                
                if (residentName == "parrot") //앵무새는 선호음식 모름
                {
                    prefferdFood.text = residents[residentName].preferredFood[0][0];
                    return;
                }
                
                for (int i = 0; i < curLevel+1; i++)
                {
                    foreach (string food in residents[residentName].preferredFood[i])
                    {
                        prefferdFood.text += " #" + food;
                    }
                }
                ShowNonePanel(0);
            }
            else
            {
                nonePanel.SetActive(true);
                upgradePanelButton.SetActive(false);
            }
        }

        public void Initialize(Dictionary<string, Resident> resident)
        {
            if(resident != null)
            {
                this.residents = resident;

                if (content.transform.childCount > 0)
                {
                    resSlotPreList.Clear();
                    for (int i = 0; i < content.transform.childCount; i++)
                    {
                        Object.Destroy(content.transform.GetChild(i).gameObject);
                    }
                }

                foreach (KeyValuePair<string, Resident> item in residents)
                {
                    GameObject tempResSlot = Instantiate(residentSlotPre, content.transform);
                    resSlotPreList.Add(tempResSlot.GetComponent<ResidentSlotButton>());
                    tempResSlot.GetComponent<ResidentSlotButton>().UpdateResidentInfo(item.Key);
                    if (GameManager.instance.residentFriendships.TryGetValue(item.Key, out ResidentFriendship temp))
                    {
                        tempResSlot.GetComponent<ResidentSlotButton>().isDiscovery = true;
                        tempResSlot.GetComponent<ResidentSlotButton>().questionMark.SetActive(false);
                    }
                    else
                    {
                        tempResSlot.GetComponent<ResidentSlotButton>().questionMark.SetActive(true);
                    }
                }

                SetResidentSlotDetail(resSlotPreList[0].engName);
            }
        }

        // int FindCurrentNum()
        // {
        //     for (int i = 0; i < resSlotPreList.Count; i++)
        //     {
        //         if (curResName == resSlotPreList[i].engName) return i;
        //     }
        //     return 0;
        // }

        public void OnClickedUpgradeButton()
        {
            upgradePanel.GetComponent<UpgradeFriendship>().ShowUpgradePanel(curResName);
            
        }
        
        
        
        void ShowNonePanel(int idx)
        {
            if (resSlotPreList[idx].GetComponent<ResidentSlotButton>().isDiscovery == false)
            {
                nonePanel.SetActive(true);
            }
            else nonePanel.SetActive(false);
        }
        
        // public void OnClickedPagePlipButton(string dir)
        // {
        //     int temp = FindCurrentNum();
        //     if(dir == "Left")
        //     {
        //         if (temp == 0)
        //         {
        //             SetResidentSlotDetail(resSlotPreList[temp].engName);
        //         }
        //         else if(temp > 0)
        //         {
        //             temp--;
        //             SetResidentSlotDetail(resSlotPreList[temp].engName);
        //         }
        //     }
        //
        //     if (dir == "Right")
        //     {
        //         if (temp == resSlotPreList.Count - 1)
        //         {
        //             SetResidentSlotDetail(resSlotPreList[temp].engName);
        //         }
        //         else if (temp < resSlotPreList.Count)
        //         {
        //             temp++;
        //             SetResidentSlotDetail(resSlotPreList[temp].engName);
        //         }
        //     }
        //     ShowNonePanel(temp);
        // }
    }

}

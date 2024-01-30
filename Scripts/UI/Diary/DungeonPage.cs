using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Diary
{
    public class DungeonPage : Page
    {
        public GameObject content;


        // page 2
        public Image image;
        [FormerlySerializedAs("name")] public Text dungeonName;
        public Text story;
        public GameObject monsterIconPanel;
        public GameObject monstericonPre;
        public GameObject dungeonSlotPre;

        public GameObject nonePanel;
        public Scrollbar scrollbar;

        
        [HideInInspector]public Dictionary<string, Dungeon> dungeons;
        List<DungeonSlot> dungeonSlotPreList = new List<DungeonSlot>();
        string curDungeonName;

        public void SetDungeonSlotDetail(string dungeonName, bool isDiscovered)
        {
            if(isDiscovered == true)
            {
                scrollbar.value = 1;
                curDungeonName = dungeonName;
                image.sprite = Resources.Load<Sprite>("Diary/DungeonDots/" + dungeonName);
                ClearMonsterIcon();
                this.dungeonName.text = dungeons[dungeonName].koreanName;
                story.text = dungeons[dungeonName].story;
                foreach (var item in dungeons[dungeonName].monsterList)
                {
                    CreateMonsterSlot(item);
                }
            }
            else
            {
                nonePanel.SetActive(true);
            }
            
            ShowNonePanel(0);
        }

        void ClearMonsterIcon()//생성됐던 몬스터 리스트 없애주기
        {
            if (monsterIconPanel.transform.childCount > 0)
            {
                foreach (Transform child in monsterIconPanel.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        
        //해당 던전에 어떤 몬스터 나오는지 생성하는 함수
        void CreateMonsterSlot(string monsterName)
        {
            //이거 왜 안되지
            GameObject tempDropItemSlot = Instantiate(monstericonPre);
            tempDropItemSlot.GetComponent<ImageSlotChange>().UpdateDropItemPanel("Diary/MonsterDots/", monsterName);
            tempDropItemSlot.transform.SetParent(monsterIconPanel.transform);
        }


        public void Initialize(Dictionary<string, Dungeon> dungeons)
        {
            if(dungeons != null)
            {
                this.dungeons = dungeons;

                if (content.transform.childCount > 0)
                {
                    dungeonSlotPreList.Clear();
                    for (int i = 0; i < content.transform.childCount; i++)
                    {
                        Object.Destroy(content.transform.GetChild(i).gameObject);
                    }
                }

                foreach (KeyValuePair<string, Dungeon> item in dungeons)
                {
                    GameObject tempDungeonSlot = Instantiate(dungeonSlotPre, content.transform);
                    dungeonSlotPreList.Add(tempDungeonSlot.GetComponent<DungeonSlot>());
                    tempDungeonSlot.GetComponent<DungeonSlot>().UpdateDungeonInfo(item.Key);
                    tempDungeonSlot.GetComponent<DungeonSlot>().dungeonInfo = item.Value;
                    if (tempDungeonSlot.GetComponent<DungeonSlot>().checkDiary == true)
                    {
                        tempDungeonSlot.GetComponent<DungeonSlot>().questionMark.SetActive(false);
                    }
                    else
                    {
                        tempDungeonSlot.GetComponent<DungeonSlot>().questionMark.SetActive(true);
                    }
                }

                SetDungeonSlotDetail(dungeonSlotPreList[0].engName, dungeonSlotPreList[0].checkDiary);
            }
        }
        // int FindCurrentNum()
        // {
        //     for (int i = 0; i < dungeonSlotPreList.Count; i++)
        //     {
        //         if (curDungeonName == dungeonSlotPreList[i].engName) return i;
        //     }
        //     return 0;
        // }

        void ShowNonePanel(int idx)
        {
            if (dungeonSlotPreList[idx].GetComponent<DungeonSlot>().checkDiary == false)
            {
                nonePanel.SetActive(true);
            }
            else nonePanel.SetActive(false);
        }
        
        // public void OnClickedPagePlipButton(string dir)
        // {
        //     int temp = FindCurrentNum();
        //     if (dir == "Left")
        //     {
        //         if (temp == 0) SetDungeonSlotDetail(dungeonSlotPreList[temp].engName);
        //         else if (temp > 0)
        //         {
        //             temp--;
        //             SetDungeonSlotDetail(dungeonSlotPreList[temp].engName);
        //         }
        //     }
        //
        //     if (dir == "Right")
        //     {
        //         if (temp == dungeonSlotPreList.Count - 1) SetDungeonSlotDetail(dungeonSlotPreList[temp].engName);
        //         else if (temp < dungeonSlotPreList.Count)
        //         {
        //             temp++;
        //             SetDungeonSlotDetail(dungeonSlotPreList[temp].engName);
        //         }
        //     }
        //
        //     ShowNonePanel(temp);
        // }
    }

}

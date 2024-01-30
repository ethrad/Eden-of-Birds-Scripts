using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Diary
{
    public class MonsterPage : Page
    {
        public GameObject content;

        // page 2
        public Image image;
        public Text monsterNameText;
        public Text monsterInfo;
        public Text dungeon;
        public Text description;

        public GameObject monsterSlotButtonPre;
        public GameObject dropItemPre;
        public GameObject dropItemPanel;

        public GameObject nonePanel;
        
        public Scrollbar scrollbar;

        
        
        [HideInInspector]
        public Dictionary<string, Monster> monsters;
        List<MonsterSlotButton> monSlotPreList = new List<MonsterSlotButton>();
        string curMonName;

        public void SetMonsterSlotDetail(string monsterName, bool isDiscovered)
        {
            if (isDiscovered == true)
            {
                scrollbar.value = 1;
                curMonName = monsterName;
                image.sprite = Resources.Load<Sprite>("Diary/MonsterDots/" + monsterName);
                ClearDropItem();
                monsterNameText.text = monsters[monsterName].koreanName;
                monsterInfo.text = "HP " + monsters[monsterName].hp.ToString() + "/ ATK " +
                                   monsters[monsterName].atk.ToString();
                dungeon.text = monsters[monsterName].area;
                description.text = monsters[monsterName].description;
                foreach (var item in monsters[monsterName].dropItems)
                {
                    CreateDropItem(item);
                }
            }
            else
            {
                nonePanel.SetActive(true);
            }
        }

        void ClearDropItem()
        {
            if (dropItemPanel.transform.childCount > 0)
            {
                foreach (Transform child in dropItemPanel.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        void CreateDropItem(string dropItemName)
        {
            GameObject tempDropItemSlot = Instantiate(dropItemPre);
            tempDropItemSlot.GetComponent<ImageSlotChange>().UpdateDropItemPanel("Diary/ItemDots/" , dropItemName);
            tempDropItemSlot.transform.SetParent(dropItemPanel.transform);
        }

        public void Initialize(Dictionary<string, Monster> monster)
        {
            if(monster != null)
            {
                this.monsters = monster;

                if (content.transform.childCount > 0)
                {
                    monSlotPreList.Clear();
                    for (int i = 0; i < content.transform.childCount; i++)
                    {
                        Object.Destroy(content.transform.GetChild(i).gameObject);
                    }
                }

                foreach (KeyValuePair<string, Monster> item in monsters)
                {
                    GameObject tempMonSlot = Instantiate(monsterSlotButtonPre, content.transform);
                    monSlotPreList.Add(tempMonSlot.GetComponent<MonsterSlotButton>());
                    tempMonSlot.GetComponent<MonsterSlotButton>().UpdateMonsterInfo(item.Key);
                    tempMonSlot.GetComponent<MonsterSlotButton>().monsterInfo = item.Value;
                    if (tempMonSlot.GetComponent<MonsterSlotButton>().checkDiary)
                    {
                        tempMonSlot.GetComponent<MonsterSlotButton>().questionMark.SetActive(false);
                    }
                    else
                    {
                        tempMonSlot.GetComponent<MonsterSlotButton>().questionMark.SetActive(true);
                    }
                }
                SetMonsterSlotDetail(monSlotPreList[0].engName, monSlotPreList[0].checkDiary);
            }
        }

        // int FindCurrentNum()
        // {
        //     for (int i = 0; i < monSlotPreList.Count; i++)
        //     {
        //         if (curMonName == monSlotPreList[i].engName) return i;
        //     }
        //     return 0;
        // }
        
        // void ShowNonePanel(int idx)
        // {
        //     if (monSlotPreList[idx].GetComponent<MonsterSlotButton>().checkDiary == false)
        //     {
        //         nonePanel.SetActive(true);
        //     }
        //     else nonePanel.SetActive(false);
        // }

        // public void OnClickedPagePlipButton(string dir)
        // {
        //     int temp = FindCurrentNum();
        //     if(dir == "Left")
        //     {
        //         if (temp == 0)
        //         {
        //             SetMonsterSlotDetail(monSlotPreList[0].engName);
        //         }
        //         else if(temp > 0)
        //         {
        //             temp--;
        //             SetMonsterSlotDetail(monSlotPreList[temp].engName);
        //         }
        //     }
        //
        //     if (dir == "Right")
        //     {
        //         if (temp == monSlotPreList.Count - 1)
        //         {
        //             SetMonsterSlotDetail(monSlotPreList[temp].engName);
        //         }
        //         else if(temp < monSlotPreList.Count)
        //         {
        //             temp++;
        //             SetMonsterSlotDetail(monSlotPreList[temp].engName);
        //         }
        //     }
        //     ShowNonePanel(temp);
        // }
    }

}

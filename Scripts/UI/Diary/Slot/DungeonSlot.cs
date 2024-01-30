using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diary
{
    public class DungeonSlot : MonoBehaviour
    {
        public string engName;
        public GameObject questionMark;
        [HideInInspector] public Dungeon dungeonInfo;
        [HideInInspector] public bool checkDiary = false;


        public void OnClickedDungeonButton()
        {
            DungeonPage temp = this.transform.parent.parent.parent.parent.GetComponent<DungeonPage>();
            temp.SetDungeonSlotDetail(engName, checkDiary);
            if (checkDiary) temp.nonePanel.SetActive(false);
            else temp.nonePanel.SetActive(true);
        }

        public void UpdateDungeonInfo(string wantString)
        {
            engName = wantString;
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Diary/DungeonDots/" + engName);
            if(GameManager.instance.dungeonDiary.Count >0)
            {
                foreach (var item in GameManager.instance.dungeonDiary)
                {
                    if (item == wantString)
                    {
                        checkDiary = true;
                        break;
                    }
                }
            }
        }
    }
}
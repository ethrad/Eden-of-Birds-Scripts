using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diary;

public class MonsterSlotButton : MonoBehaviour
{
    public string engName;
    public GameObject questionMark;
    [HideInInspector] public Monster monsterInfo;
    [HideInInspector] public bool checkDiary = false;

    public void OnClickedMonsterButton()
    {
        MonsterPage temp = this.transform.parent.parent.parent.parent.GetComponent<MonsterPage>();
        temp.SetMonsterSlotDetail(engName, checkDiary);
        if (checkDiary)
        {
            temp.nonePanel.SetActive(false);
        }
        else
        {
            temp.nonePanel.SetActive(true);
        }
    }

    public void UpdateMonsterInfo(string wantString)
    {
        engName = wantString;
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Diary/MonsterDots/" + engName);
        if(GameManager.instance.monsterDiary.Count > 0)
        {
            foreach (var item in GameManager.instance.monsterDiary)
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
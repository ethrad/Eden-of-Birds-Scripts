using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTutorial : MonoBehaviour
{
    public List<GameObject> panelList;
    private int num = 0;

    public void StartQuestTutorial()
    {
        if (GameManager.instance.clearedQuests.TryGetValue("owl", out List<int> value))
        {
            foreach (var item in panelList)
            {
                Destroy(item);
            }
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(true);
            panelList[0].SetActive(true);
        }
    }
    
    public void ChangeTutorialPanel()
    {
        if(2 <= num || GameManager.instance.clearedQuests.TryGetValue("owl", out List<int> value))
        {
            foreach (var item in panelList)
            {
                Destroy(item);
            }
            Destroy(gameObject);
        }
        else if(num < 2)
        {
            panelList[num].SetActive(false);
            num++;
            panelList[num].SetActive(true);
        }
        else if(num <3)  panelList[num].SetActive(false);
    }
}

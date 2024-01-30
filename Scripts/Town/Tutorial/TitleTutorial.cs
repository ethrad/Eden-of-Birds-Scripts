using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTutorial : MonoBehaviour
{
    public GameObject cartoon;
    public List<GameObject> panelList;
    private int panelNum = 0;
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        
        if(PlayerPrefs.GetInt("isTutorial") != 1)
        {
            cartoon.SetActive(true);
            for (int i = 0; i < cartoon.transform.childCount - 1; i++)
            {
                panelList.Add(cartoon.transform.GetChild(i).gameObject);
            }
        }
        else gameObject.SetActive(false);
    }

    public void OnClickedPanel()
    {
        if(panelNum < panelList.Count - 1)
        {
            panelList[panelNum].SetActive(false);
            panelNum++;
            panelList[panelNum].SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("isTutorial", 1);
            gameObject.SetActive(false);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Diary;
using UnityEngine;

public class TreasureTutorial : MonoBehaviour
{
    public GameObject residentPage;
    public List<GameObject> treasurePanels;
    public int panelNum = 0;
    [HideInInspector]
    public bool isTreasureTutorial = false;

    public void StartTreasureTutorial()
    {
        if(PlayerPrefs.GetInt("isTreasureTutorial") != 1)
        {
            if (GameManager.instance.residentFriendships[residentPage.GetComponent<ResidentPage>().curResName].isAlerted)
            {
                gameObject.SetActive(true);
                treasurePanels[0].SetActive(true);
            }
        }
        else this.gameObject.SetActive(false);
    }
    
    public void OnClickedPanel()
    {
        if (panelNum < treasurePanels.Count - 1)
        {
            treasurePanels[panelNum].SetActive(false);
            panelNum++;
            treasurePanels[panelNum].SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("isTreasureTutorial", 1);
            isTreasureTutorial = true;
           // Destroy(gameObject.GetComponent<TreasureTutorial>());
            gameObject.SetActive(false);
        }
    }
}
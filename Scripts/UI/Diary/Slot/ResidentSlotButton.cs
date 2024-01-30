using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Diary
{
    public class ResidentSlotButton : MonoBehaviour
    {
        [HideInInspector] 
        public bool isDiscovery = false;
        public GameObject questionMark;
        public string engName;
        public GameObject checkMark;

        [HideInInspector]
        public Resident res;
        
        public void OnClickedResidentButton()
        {
            GameObject parentObj = transform.parent.parent.parent.parent.gameObject;
            ResidentPage temp = parentObj.GetComponent<ResidentPage>();
            temp.SetResidentSlotDetail(engName);
            temp.CheckTreasureTutorial();

            if (isDiscovery)
            {
                temp.nonePanel.SetActive(false);
            }
            else
            {
                temp.nonePanel.SetActive(true);
            }
        }
        
        public void UpdateResidentInfo(string wantString)
        {
            engName = wantString;
            if (GameManager.instance.residentFriendships.TryGetValue(wantString, out ResidentFriendship temp))
            {
                if (GameManager.instance.residentFriendships[wantString].isAlerted) checkMark.SetActive(true);
            }

            res = transform.parent.parent.parent.parent.GetComponent<ResidentPage>().residents[wantString];
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Diary/ResidentDots/" + engName);
            
        }
    }
}

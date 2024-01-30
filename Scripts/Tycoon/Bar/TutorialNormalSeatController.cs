using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Tycoon;

namespace Tutorial
{
    public class TutorialNormalSeatController : MonoBehaviour, IDropHandler
    {
        public int id;
        public GameObject orderBalloon;
        public GameObject heartImg;
        public string foodName;
        
        public GameObject kitchenButtonWorkingPanel;

        
        int tempPanelNum; 

        public void OnDrop(PointerEventData eventData)
        {
            if (foodName == eventData.pointerDrag.GetComponent<FoodController>().foodName)
            {
               if(id == 1) kitchenButtonWorkingPanel.SetActive(true);
                
                orderBalloon.SetActive(false);
                heartImg.SetActive(true);
                
                StartCoroutine(ResetSeat());

                Destroy(eventData.pointerDrag);

            }
        }
        
        IEnumerator ResetSeat()
        {
            if (id == 2)
            {
                TutorialTycoonManager.instance.kitchenButtonWorkingPanel.SetActive(true);
                TutorialTycoonManager.instance.panelNum = 18;
            }
            yield return new WaitForSeconds(1.8f);
            
            if (TutorialTycoonManager.instance.panelNum == 8)
            {
                TutorialTycoonManager.instance.panelNum++;
                TutorialTycoonManager.instance.SetActivePanel(TutorialTycoonManager.instance.panelNum);
            }

            if (TutorialTycoonManager.instance.panelNum >= 18)
            {
                TutorialTycoonManager.instance.panelList[18].SetActive(true);
            }
            
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
            if (this.id == 1)
            {
                id++;
                TutorialTycoonManager.instance.characters[id].SetActive(true);
                TutorialTycoonManager.instance.panelNum = 16;
                TutorialTycoonManager.instance.panelList[TutorialTycoonManager.instance.panelNum].SetActive(true);
                kitchenButtonWorkingPanel.SetActive(false);
            }
            Destroy(this.gameObject);

            yield return new WaitForSeconds(0.5f);

            
            yield break;
        }

        private void Start()
        {
            GetComponent<AudioSource>().volume = GameManager.instance.settings.soundEffectsVolume;
        }
    }
}
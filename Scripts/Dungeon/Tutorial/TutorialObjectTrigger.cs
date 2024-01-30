using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class TutorialObjectTrigger : MonoBehaviour
    {
        public GameObject highlightPanel;
        public GameObject speechBalloon;
        
        private bool isTriggered;
        
        protected void OnTriggerEnter2D(Collider2D col)
        {
            if (!isTriggered)
            {
                if (col.gameObject.CompareTag("player"))
                {
                    TutorialDungeonManager.instance.player.GetComponent<PlayerController>().StopMoving();
                    isTriggered = true;
                    highlightPanel.SetActive(true);
                    speechBalloon.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }
}

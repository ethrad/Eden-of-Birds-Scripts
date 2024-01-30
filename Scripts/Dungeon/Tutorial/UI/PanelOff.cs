using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class PanelOff : MonoBehaviour
    {
        public GameObject maskPanel;
        
        public void OnPanelClicked()
        {
            Time.timeScale = 1f;
            maskPanel.SetActive(false);
            gameObject.SetActive(false);
            TutorialDungeonManager.instance.player.GetComponent<PlayerController>().StartMoving();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dungeon
{
    public class CallEaglePanel : InteractPanel
    {
        public void OnCallEagleButtonClicked()
        {
            DungeonManager.instance.OffBasePanel();
            gameObject.SetActive(true);
        }
        
        public void OnEscapeButtonClicked(Button exitButton)
        {
            if (GameManager.instance.gameData.PurchaseGold(20))
            {
                exitButton.interactable = false;
                
                DungeonManager.instance.ClearDungeon();
            }
        }
        
        public void OnExitButtonClicked()
        {
            DungeonManager.instance.OnBasePanel();
            gameObject.SetActive(false);
        }
    }
}


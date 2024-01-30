using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class ByoPeddlerEnterPanel : InteractPanel
    {
        public GameObject byoPeddlerShopPanel;

        public void OnShopEnterButtonClicked()
        {
            gameObject.SetActive(false);
            byoPeddlerShopPanel.GetComponent<ByoPeddlerShopPanel>().UpdateGoldPanel();
            byoPeddlerShopPanel.SetActive(true);
            Time.timeScale = 0;
        }
        
        public void OnExitButtonClicked()
        {
            gameObject.SetActive(false);
            DungeonManager.instance.StartPlayerMoving(true);
        }
    }

}

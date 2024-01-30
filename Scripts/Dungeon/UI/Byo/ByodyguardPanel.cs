using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class ByodyguardPanel : InteractPanel
    {
        public void OnEnterButtonClicked()
        {
            DungeonManager.instance.OpenByoRoom();
            DungeonManager.instance.StartPlayerMoving(false);
            gameObject.SetActive(false);
        }
        
        public void OnExitButtonClicked()
        {
            DungeonManager.instance.StartPlayerMoving(true);
            gameObject.SetActive(false);
        }
    }
}

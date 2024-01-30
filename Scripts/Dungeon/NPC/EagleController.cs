using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class EagleController : NPCController
    {
        public GameObject eaglePanel;
        
        public override void Interact()
        {
            eaglePanel.GetComponent<EaglePanel>().Initialize();
            eaglePanel.SetActive(true);
        }
    }
}

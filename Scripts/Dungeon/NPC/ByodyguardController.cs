using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class ByodyguardController : NPCController
    {
        public override void Interact()
        {
            DungeonManager.instance.OnByodyguardPanel();
        }
    }
}

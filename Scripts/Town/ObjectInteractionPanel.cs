using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Town
{
    public class ObjectInteractionPanel : MonoBehaviour
    {
        protected virtual void Initialize()
        {
            
        }
    
        public virtual void Interact()
        {
            
        }

        public virtual void ExitPanel()
        {
            TownManager.instance.OnBasePanel();
        }
    }
}


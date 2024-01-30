using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace Town
{
    public class NormalResidentController : ObjectController
    {
        public string residentName;
        
        protected override void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                TownManager.instance.UpdateInteractedObjectState(gameObject, true);
            }
        }
        
        protected override void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                TownManager.instance.ResetInteractedObjectState(true);
            }
        }

        public override void Interact()
        {
            DialogueManager.StartConversation(residentName);
        }
    }
}

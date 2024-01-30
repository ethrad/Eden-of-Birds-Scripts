using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Town
{
    public class ObjectController : MonoBehaviour
    {
        public GameObject panel;
        
        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                TownManager.instance.UpdateInteractedObjectState(gameObject, false);
            }
        }
        
        protected virtual void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                TownManager.instance.ResetInteractedObjectState(false);
            }
        }

        public virtual void Interact()
        {
            panel.GetComponent<ObjectInteractionPanel>().Interact();
            panel.SetActive(true);
        }
    }
}

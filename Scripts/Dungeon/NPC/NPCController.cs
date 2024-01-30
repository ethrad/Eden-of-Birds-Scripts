using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class NPCController : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                DungeonManager.instance.OnInteractButton(gameObject);
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                DungeonManager.instance.OffInteractButton();
            }
        }
        
        public virtual void Interact()
        {
            /*interactPanel.GetComponent<InteractPanel>().Initialize();
            interactPanel.SetActive(true);*/
        }
    }

}

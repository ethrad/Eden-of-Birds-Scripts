using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Town
{
    public class PostboxController : ObjectController
    {
        public GameObject adFreePanel;

        public override void Interact()
        {
            if (GameManager.instance.gameData.purchasedAdFree)
            {
                adFreePanel.GetComponent<ObjectInteractionPanel>().Interact();
                adFreePanel.SetActive(true);
            }
            else
            {
                panel.GetComponent<ObjectInteractionPanel>().Interact();
                panel.SetActive(true);
            }
        }
    }
}

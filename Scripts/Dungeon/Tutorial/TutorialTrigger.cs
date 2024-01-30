using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class TutorialTrigger : MonoBehaviour
    {
        private bool isTriggered;
        protected void OnTriggerEnter2D(Collider2D col)
        {
            if (!isTriggered)
            {
                if (col.gameObject.CompareTag("player"))
                {
                    isTriggered = true;
                    TutorialDungeonManager.instance.OnPanel();
                }
            }
        }
    }

}

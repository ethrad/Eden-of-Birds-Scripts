using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentController : MonoBehaviour
{
    public string residentName;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            InteractionManager.instance.UpdateInteractionState(residentName);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            InteractionManager.instance.ResetInteractionState();
        }
    }
}

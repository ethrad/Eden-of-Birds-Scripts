using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonItemController : MonoBehaviour
{
    public string itemName;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            DungeonManager.instance.GetItem(itemName);

            Destroy(this.gameObject);
        }
    }
}

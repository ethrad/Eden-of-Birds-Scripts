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
            if (DungeonManager.instance.tempInventory.ContainsKey(itemName))
            {
                DungeonManager.instance.tempInventory[itemName]++;
            }
            else
            {
                DungeonManager.instance.tempInventory[itemName] = 1;
            }

            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public string itemName;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            ItemManager.instance.inventory[itemName]++;
            Destroy(this.gameObject);

            /* try
             {
                 DungeonManager.instance.tempItemBasket[itemName]++;
             }
             catch (System.Exception e)
             {

                 DungeonManager.instance.tempItemBasket[itemName] = 1;
             }
             finally
             {
                 Destroy(this.gameObject);
             }
 */
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    public string[] itemNames;
    public int[] itemProb;
    protected bool canHarvest = false;

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            DungeonManager.instance.OnHarvestButton(this.gameObject);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            DungeonManager.instance.OffHarvestButton();
        }
    }

    // 상태 변화 없이 본인만 없어짐
    public virtual void Harvest()
    {
        int random = Random.Range(0, 100);

        int itemIndex = -1;
        int tempProb = 0;

        for (int i = 0; i < itemProb.Length; i++)
        {
            if (random <= itemProb[i] + tempProb)
            {
                itemIndex = i;
            }
            else break;

            tempProb += itemProb[i];
        }

        DungeonManager.instance.GetItem(itemNames[itemIndex]);

        Destroy(this.gameObject);
    }

    void Start()
    {
        canHarvest = true;
    }
}

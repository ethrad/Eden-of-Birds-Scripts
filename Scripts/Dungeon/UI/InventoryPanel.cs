using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class InventoryPanel : MonoBehaviour
    {
        public GameObject itemPrefab;
        public GameObject itemPanel;
        public GameObject tempItemPanel;

        private bool isInitialized = false;
        
        public void Initialize()
        {
            if (!isInitialized)
            {
                foreach (var item in GameManager.instance.inventory)
                {
                    if (GameManager.instance.itemList[item.Key].attribute != "ingredient") continue;
                    
                    var tempItem = Instantiate(itemPrefab, itemPanel.transform);
                    tempItem.GetComponent<ItemSlotController>().UpdateItem(item.Key, item.Value);
                }
                
                isInitialized = true;
            }

            foreach (KeyValuePair<string, int> item in DungeonManager.instance.tempInventory)
            {
                GameObject tempItem = Instantiate(itemPrefab, tempItemPanel.transform);
                tempItem.GetComponent<ItemSlotController>().UpdateItem(item.Key, item.Value);
            }
        }

        public void Reset()
        {
            while (tempItemPanel.transform.childCount > 0)
            {
                DestroyImmediate(tempItemPanel.transform.GetChild(0).gameObject);
            }
            
            gameObject.SetActive(false);
        }
    }
}

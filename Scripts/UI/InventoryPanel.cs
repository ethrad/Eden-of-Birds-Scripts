using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public GameObject itemSlotPanel;
    public GameObject itemName;
    public GameObject description;
    
    public void OnInventoryButtonClicked()
    {
        foreach (KeyValuePair<string, int> item in ItemManager.instance.inventory)
        {
            GameObject tempItem = Instantiate(itemSlotPrefab);
            tempItem.GetComponent<ItemSlotController>().UpdateItem(item.Key, item.Value);
            tempItem.transform.SetParent(itemSlotPanel.transform);
        }

        gameObject.SetActive(true);
    }

    public void UpdateDescriptionPanel(string itemName)
    {
        this.itemName.GetComponent<Text>().text = ItemManager.instance.itemList[itemName].name;
        description.GetComponent<Text>().text = ItemManager.instance.itemList[itemName].description;
    }
}

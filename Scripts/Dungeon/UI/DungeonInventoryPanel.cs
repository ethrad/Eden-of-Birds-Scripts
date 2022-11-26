using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInventoryPanel : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject itemPanel;
    public GameObject tempItemPanel;

    public void OnInventoryButtonClicked()
    {
        foreach (KeyValuePair<string, int> item in ItemManager.instance.inventory)
        {
            GameObject tempItem = Instantiate(itemPrefab);
            tempItem.GetComponent<ItemSlotController>().UpdateItem(item.Key, item.Value);
            tempItem.transform.SetParent(itemPanel.transform);
        }

        foreach (KeyValuePair<string, int> item in DungeonManager.instance.tempInventory)
        {
            GameObject tempItem = Instantiate(itemPrefab);
            tempItem.GetComponent<ItemSlotController>().UpdateItem(item.Key, item.Value);
            tempItem.transform.SetParent(tempItemPanel.transform);
        }

        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void OnInventoryExitButtonClicked()
    {
        Time.timeScale = 1f;

        gameObject.SetActive(false);

        while (itemPanel.transform.childCount > 0)
        {
            DestroyImmediate(itemPanel.transform.GetChild(0).gameObject);
        }

        while (tempItemPanel.transform.childCount > 0)
        {
            DestroyImmediate(tempItemPanel.transform.GetChild(0).gameObject);
        }
    }
}

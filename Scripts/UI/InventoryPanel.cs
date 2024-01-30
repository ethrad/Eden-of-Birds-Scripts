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
    public GameObject goldText;
    
    public void OnInventoryButtonClicked()
    {
        foreach (KeyValuePair<string, int> item in GameManager.instance.inventory)
        {
            if (item.Value > 0)
            {
                GameObject tempItem = Instantiate(itemSlotPrefab, itemSlotPanel.transform);
                tempItem.GetComponent<ItemSlotController>().UpdateItem(item.Key, item.Value);
            }
        }
        goldText.GetComponent<Text>().text = GameManager.instance.gameData.gold.ToString();
        gameObject.SetActive(true);
    }

    public void UpdateDescriptionPanel(string itemName)
    {
        this.itemName.GetComponent<Text>().text = GameManager.instance.itemList[itemName].name;
        description.GetComponent<Text>().text = GameManager.instance.itemList[itemName].description;
    }

    public void OnExitButtonClicked()
    {
        while (itemSlotPanel.transform.childCount > 0)
        {
            DestroyImmediate(itemSlotPanel.transform.GetChild(0).gameObject);
        }

        gameObject.SetActive(false);
    }
}

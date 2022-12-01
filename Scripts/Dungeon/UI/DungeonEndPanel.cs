using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonEndPanel : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public GameObject itemSlotGroupPanel;
    public GameObject goldText;

    public void UpdatePanel()
    {
        foreach (KeyValuePair<string, int> item in DungeonManager.instance.tempInventory)
        {
            GameObject tempSlot = Instantiate(itemSlotPrefab);
            tempSlot.transform.SetParent(itemSlotGroupPanel.transform);
            tempSlot.GetComponent<ItemSlotController>().UpdateItem(item.Key, item.Value);
        }

        goldText.GetComponent<Text>().text = DungeonManager.instance.adjustGold.ToString();
    }

    public void OnDungeonExitButtonClicked()
    {
        foreach (KeyValuePair<string, int> item in DungeonManager.instance.tempInventory)
        {
            if (ItemManager.instance.inventory.ContainsKey(item.Key))
            {
                ItemManager.instance.inventory[item.Key] += item.Value;
            }
            else
            {
                ItemManager.instance.inventory[item.Key] = item.Value;
            }
        }

        ItemManager.instance.WriteInventory();
        Time.timeScale = 1f;

        SceneManager.LoadScene("Town");
    }
}

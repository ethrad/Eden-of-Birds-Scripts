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

    public void UpdatePanel(bool isCleared)
    {
        Time.timeScale = 0f;
        foreach (KeyValuePair<string, int> item in DungeonManager.instance.tempInventory)
        {
            GameObject tempSlot = Instantiate(itemSlotPrefab, itemSlotGroupPanel.transform);
            tempSlot.GetComponent<ItemSlotController>().UpdateItem(item.Key, item.Value);
        }

        if (!isCleared || DungeonManager.instance.isAllCleared)
        {
            goldText.GetComponent<Text>().text = DungeonManager.instance.adjustGold.ToString();
        }
    }

    public void WriteDiscoveryMonster()
    {
        GameManager.instance.WriteDiary();
    }
    
    public void OnDungeonExitButtonClicked(Button exitButton)
    {
        exitButton.interactable = false;
        
        foreach (KeyValuePair<string, int> item in DungeonManager.instance.tempInventory)
        {
            GameManager.instance.AddItemToInventory(item.Key, item.Value);
        }
        foreach (var t in DungeonManager.instance.gameObject.GetComponents<QuestTrigger>())
        {
            t.OnQuestTrigger();
        }

        gameObject.GetComponent<HomeworkTrigger>().OnHomeworkTrigger();
        GameManager.instance.gameData.GetGold(DungeonManager.instance.adjustGold);
        GameManager.instance.gameData.dungeonRemainCount--;
        GameManager.instance.WriteGameData();
        GameManager.instance.WriteInventory();
        GameManager.instance.WriteOngoingQuests();
        GameManager.instance.WriteOwnRecipes();
        
        Time.timeScale = 1f;

        if (GameManager.instance.gameData.purchasedAdFree)
        {
            SceneManager.LoadScene("Town");
        }
        else
        {
            LoadMobileAD.Instance.LoadInterstitialAd("Town");
        }
    }
}

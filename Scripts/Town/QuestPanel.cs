using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    public GameObject questList;
    public GameObject progressQuestSlotPrefab;
    public GameObject clearQuestSlotPrefab;

    public void UpdateQuestPanel()
    {
        foreach (var quest in TownManager.instance.selectedNPC.GetComponent<ResidentController>().progressQuests)
        {
            GameObject questSlot = Instantiate(progressQuestSlotPrefab, questList.transform);
            questSlot.GetComponent<QuestSlot>().UpdateQuestSlot(quest, true);
        }
        
        
        foreach (var quest in TownManager.instance.selectedNPC.GetComponent<ResidentController>().clearQuests)
        {
            GameObject questSlot = Instantiate(clearQuestSlotPrefab, questList.transform);
            questSlot.GetComponent<QuestSlot>().UpdateQuestSlot(quest, false);
        }
    }

    public void ResetQuestPanel()
    {
        for (int i = 0; i < questList.transform.childCount; i++)
        {
            Destroy(questList.transform.GetChild(i).gameObject);
        }
    }

    public void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
        TownManager.instance.OnBasePanel();
        
        for(int i = 0; i < questList.transform.childCount; i++)
        {
            Destroy(questList.transform.GetChild(i).gameObject);
        }
    }
}

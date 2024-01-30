using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestSlot : MonoBehaviour
{
    public Text questName;
    private Quest quest;
    private bool isProgressQuest;

    public void UpdateQuestSlot(Quest quest, bool isProgressQuest)
    {
        this.quest = quest;
        this.isProgressQuest = isProgressQuest;
        questName.GetComponent<Text>().text = quest.QName;
    }

    public void OnQuestSlotClicked()
    {
        TownManager.instance.selectedQuest = quest;
        TownManager.instance.isProgressQuest = isProgressQuest;
        
        if (isProgressQuest)
        {
            DialogueManager.StartConversation(quest.BSName); // 대화 불러오기
        }
        else
        {
            DialogueManager.StartConversation(quest.ASName); // 대화 불러오기
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diary
{
    public class QuestSlot : MonoBehaviour
    {
        public Quest quest;

        public Text questTitle;
        
        public void UpdateSlot(Quest quest)
        {
            this.quest = quest;
            questTitle.text = quest.QName;
        }

        public void OnQuestSlotClicked()
        {
            transform.parent.parent.parent.parent.GetComponent<QuestPage>().OnQuestSlotClicked(quest);
        }
    }
}
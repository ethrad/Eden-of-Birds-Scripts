using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public int questType;
    public string goalName;

    public void OnQuestTrigger()
    {
        foreach (var p in GameManager.instance.ongoingQuests)
        {
            foreach (var oq in p.Value)
            {
                var results = from ongoingQuestGoal in oq.ongoingQuestGoals
                    where ongoingQuestGoal.type == questType && ongoingQuestGoal.name == goalName
                    select ongoingQuestGoal;

                foreach (var result in results)
                {
                    result.count++;
                }
            }
        }
    }
}

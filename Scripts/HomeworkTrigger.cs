using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeworkTrigger : MonoBehaviour
{
    public int homeworkType;

    public void OnHomeworkTrigger()
    {
        foreach (var oh in GameManager.instance.homeworkData.ongoingHomeworkList)
        {
            if (homeworkType == oh.type)
            {
                oh.count++;
                break;
            }
        }
        
        GameManager.instance.WriteHomeworkData();
    }
}

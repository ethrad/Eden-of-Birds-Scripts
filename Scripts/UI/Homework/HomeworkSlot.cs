using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HomeworkSpace
{
    public class HomeworkSlot : MonoBehaviour
    {
        private Homework homework;
        public Text homeworkName;
        public Button completeButton;
        public Button gotRewardButton;

        public void SetSlot(Homework homework)
        {
            this.homework = homework;
            homeworkName.text = homework.title;
            completeButton.interactable = false;
        }

        public void CheckHomeworkState()
        {
            foreach (var oh1 in GameManager.instance.homeworkData.ongoingHomeworkList)
            {
                if (homework.type != oh1.type) continue;
                
                if (oh1.gotReward)
                {
                    completeButton.gameObject.SetActive(false);
                    gotRewardButton.gameObject.SetActive(true);
                    return;
                }
                
                switch (homework.type)
                {
                    case 0:
                        completeButton.interactable = true;
                        return;
                    case 4:
                        bool canClear = true;

                        foreach (var oh2 in GameManager.instance.homeworkData.ongoingHomeworkList)
                        {
                            if (oh2.type == 4) break;

                            if (oh2.gotReward) continue;
                            
                            canClear = false;
                            break;
                        }

                        if (canClear)
                        {
                            completeButton.interactable = true;
                        }

                        return;
                }
                
                if (oh1.count >= oh1.countGoal)
                {
                    completeButton.interactable = true;
                }
                break;
            }
        }

        public void OnCompleteButtonClicked()
        {
            GameManager.instance.homeworkData.ClearHomework(homework);
            transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<HomeworkSlot>().CheckHomeworkState();
            completeButton.gameObject.SetActive(false);
            gotRewardButton.gameObject.SetActive(true);
        }
    }
}

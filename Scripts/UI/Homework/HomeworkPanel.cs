using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeworkSpace
{
    public class Homework : CSVData
    {
        public int type;
        public string title;
        public string name;
        public int count;
        
        public override void csvToClass(string[] csvArray)
        {
            type = int.Parse(csvArray[0]);
            title = csvArray[1];
            name = csvArray[2];
            count = int.TryParse(csvArray[3], out count) ? count : 0;
        }
    }

    public class OngoingHomework
    {
        public int type;
        public string name;
        public int count;
        public int countGoal;
        public bool gotReward;

        public OngoingHomework()
        {
        
        }
    
        public OngoingHomework(Homework homework)
        {
            type = homework.type;
            name = homework.name;
            count = 0;
            countGoal = homework.count;
            gotReward = false;
        }
    }
    
    [System.Serializable]
    public class HomeworkData
    {
        public DateTime initTime;
        
        public List<OngoingHomework> ongoingHomeworkList;
        
        public HomeworkData()
        {
            initTime = DateTime.Now;
            
            ongoingHomeworkList = new List<OngoingHomework>();
        }
        
        public HomeworkData(List<Homework> homeworkList)
        {
            initTime = DateTime.Now;
            
            ongoingHomeworkList = new List<OngoingHomework>();
            
            foreach (var homework in homeworkList)
            {
                ongoingHomeworkList.Add(new OngoingHomework(homework));
            }
        }

        public void ClearHomework(Homework homework)
        {
            foreach (var oh in ongoingHomeworkList)
            {
                if (oh.type == homework.type)
                {
                    oh.gotReward = true;
                    break;
                }
            }
            
            GameManager.instance.WriteHomeworkData();
            GameManager.instance.gameData.GetFeather(5);
            GameManager.instance.WriteGameData();
        }
    }
    
    public class HomeworkPanel : MonoBehaviour
    {
        public GameObject homeworkSlotContent;
        public GameObject homeworkSlotPrefab;
        private bool isInitialized;
        
        public void OnHomeworkButtonClicked()
        {
            if (!isInitialized)
            {
                Initialize();
            }

            CheckHomeworkState();
            gameObject.SetActive(true);
        }

        private void CheckHomeworkState()
        {
            for (int i = 0; i < homeworkSlotContent.transform.childCount; i++)
            {
                homeworkSlotContent.transform.GetChild(i).GetComponent<HomeworkSlot>().CheckHomeworkState();
            }
        }

        private void Initialize()
        {
            isInitialized = true;
            
            foreach (var homework in GameManager.instance.homeworkList)
            {
                GameObject homeworkSlot = Instantiate(homeworkSlotPrefab, homeworkSlotContent.transform);
                homeworkSlot.GetComponent<HomeworkSlot>().SetSlot(homework);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Diary
{
    public class QuestPage : Page
    {
        private void ReadWordDictionary()
        {
            wordDictionary = IOManager.instance.ReadJsonFromResources<Dictionary<string, string>>("WordDictionary");
        }
        
        
        
        public override void Initialize()
        {
            if (wordDictionary == null)
            {
                ReadWordDictionary();
            }
            
            currentQuestID = -1;
            for (int i = 0; i < ongoingQuestContents.transform.childCount; i++)
            {
                Destroy(ongoingQuestContents.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < clearedQuestContents.transform.childCount; i++)
            {
                Destroy(clearedQuestContents.transform.GetChild(i).gameObject);
            }

            questTitle.text = "";
            NPCName.text = "";
            EndNPCName.text = "";
            questGoal.text = "";
            questReward.text = "";

            foreach (var p in GameManager.instance.ongoingQuests)
            {
                foreach (var oq in p.Value)
                {
                    GameObject tempSlot = Instantiate(questSlotPrefab, ongoingQuestContents.transform);
                    tempSlot.GetComponent<QuestSlot>().UpdateSlot(GameManager.instance.quests[oq.NPCName][oq.QID]);
                }
            }
            
            foreach (var p in GameManager.instance.clearedQuests)
            {
                foreach (var cq in p.Value)
                {
                    GameObject tempSlot = Instantiate(questSlotPrefab, clearedQuestContents.transform);
                    tempSlot.GetComponent<QuestSlot>().UpdateSlot(GameManager.instance.quests[p.Key][cq]);
                }
            }

            ongoingQuestButton.enabled = false;
            gameObject.SetActive(false);
        }

        public override void OnPage()
        {
            gameObject.SetActive(true);
        }

        public override void OffPage()
        {
            gameObject.SetActive(false);
        }
    
        [Header("Page 1")]
        public GameObject questSlotPrefab;
        public GameObject ongoingQuestContents;
        public GameObject clearedQuestContents;
        
        public Button ongoingQuestButton;
        public Button clearedQuestButton;
        
        public GameObject ongoingQuestPage;
        public GameObject clearedQuestPage;
        
        public void OnOngoingQuestButtonClicked()
        {
            ongoingQuestButton.enabled = false;
            clearedQuestButton.enabled = true;
            
            clearedQuestPage.SetActive(false);
            ongoingQuestPage.SetActive(true);
            
        }
        
        public void OnClearedQuestButtonClicked()
        {
            clearedQuestButton.enabled = false;
            ongoingQuestButton.enabled = true;
            
            ongoingQuestPage.SetActive(false);
            clearedQuestPage.SetActive(true);
        }
        
        [Header("Page 2")]
        public Text questTitle;
        public Text NPCName;
        public Text EndNPCName;
        public Text questGoal;
        public Text questReward;

        private Dictionary<string, string> wordDictionary;

        private string currentQuestNPCName = "";
        private int currentQuestID = -1;

        public void OnQuestSlotClicked(Quest quest)
        {
            if (currentQuestNPCName != quest.NPCName || currentQuestID != quest.QID)
            {
                currentQuestNPCName = quest.NPCName;
                currentQuestID = quest.QID;
                questGoal.text = "";
                questReward.text = "";

                questTitle.text = quest.QName;
                NPCName.text = "의뢰한 주민 : " + wordDictionary[quest.NPCName];
                EndNPCName.text = "완수할 주민: " + wordDictionary[quest.EndNPCName];

                foreach (var qg in quest.questGoals)
                {
                    switch (qg.type)
                    {
                        case 0:
                        case 1:
                            questGoal.text += wordDictionary[quest.EndNPCName] + "에게 찾아가 의뢰 완수하기\n";
                            break;
                        case 2:
                            questGoal.text += "둥지 영업 " + qg.count + "회 하기\n";
                            break;
                        case 3:
                            questGoal.text += "던전에 " + qg.count + "회 다녀오기\n";
                            break;
                        case 4:
                            questGoal.text += "음식 " + qg.count + "회 서빙하기\n";
                            break;
                        case 5:
                            questGoal.text += wordDictionary[qg.name] + " " + qg.count + "회 서빙하기\n";
                            break;
                        case 6:
                            questGoal.text += wordDictionary[qg.name] + " " + qg.count + "개 전달하기\n";
                            break;
                        case 7:
                            questGoal.text += "특수 주민에게" + qg.count + "회 서빙하기\n";
                            break;
                        case 8:
                            questGoal.text += wordDictionary[qg.name] + "에게 음식 " + qg.count + "회 서빙하기\n";
                            break;
                        case 9:
                            questGoal.text += qg.count + "골드 전달하기\n";
                            break;
                        case 10:
                            questGoal.text += "던전에서 아무 몬스터나 " + qg.count + "마리 처치하기\n";
                            break;
                        case 11:
                            questGoal.text += "던전에서 " + wordDictionary[qg.name] + " " + qg.count + "마리 처치하기\n";
                            break;
                        case 12:
                            questGoal.text += wordDictionary[qg.name] + "에서 " + qg.count + "번 죽지 않고 돌아오기\n";
                            break;
                        case 13:
                            questGoal.text += "던전에서 아무 재료나 " + qg.count + "회 채집하기";
                            break;
                        case 14:
                            questGoal.text += "던전에서 " + wordDictionary[qg.name] + " " + qg.count + "회 채집하기";
                            break;
                    }
                }

                foreach (var qr in quest.questRewards)
                {
                    switch (qr.type)
                    {
                        case 0:
                            questReward.text = qr.count + " 깃털\n";
                            break;
                        case 1:
                            questReward.text += qr.count + " 골드\n";
                            break;
                        case 2:
                            questReward.text += wordDictionary[qr.name] + "의 호감도 " + qr.count + " 상승\n";
                            break;
                        case 3:
                            questReward.text += wordDictionary[qr.name] + " " + qr.count + "개\n";
                            break;
                        case 4:
                            questReward.text += wordDictionary[qr.name] + " 레시피\n";
                            break;
                        case 5:
                            questReward.text += wordDictionary[qr.name] + " 접근 가능\n";
                            break;
                        case 6:
                            questReward.text += wordDictionary[qr.name] + " 애장품 " + qr.count + "개\n";
                            break;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Tycoon
{
    public class EndPanel : MonoBehaviour
    {
        public Text revenueText;
        public Text lossText;
        public Text netProfitText;
        
        private int revenue;
        private int loss;
        
        public void UpdatePanel(int revenue, int loss)
        {
            foreach (var skillList in GameManager.instance.earnedSkills)
            {
                if (skillList.Key == "tycoon")
                {
                    foreach(var skill in skillList.Value)
                    {
                        switch (skill.skillType)
                        {
                            case "gold":
                                revenue += (int)Math.Round(revenue * skill.skillValue, 0, MidpointRounding.AwayFromZero);
                                break;
                        }
                    }
                }
            }
            
            this.revenue = revenue;
            this.loss = loss;
            
            revenueText.text = revenue.ToString();
            lossText.text = loss.ToString();
            netProfitText.text = (revenue - loss).ToString();
        }
        
        public void OnExitButtonClicked()
        {
            GetComponent<HomeworkTrigger>().OnHomeworkTrigger();
            TycoonManager.instance.gameObject.GetComponent<QuestTrigger>().OnQuestTrigger();
            GameManager.instance.gameData.GetGold(revenue);
            GameManager.instance.EndTycoon();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace IncaTernTycoon
{
    public class EndPanel : MonoBehaviour
    {
        public Text revenueText;
        public Text reputationText;
        
        private int revenue;
        private int reputation;
        
        public void UpdatePanel(int revenue, int reputation)
        {
            this.revenue = revenue;
            this.reputation = reputation;
            
            revenueText.text = revenue.ToString();
            reputationText.text = reputation.ToString();
        }
        
        public void OnExitButtonClicked()
        {
            GetComponent<HomeworkTrigger>().OnHomeworkTrigger();
            GameManager.instance.gameData.GetGold(revenue);
            GameManager.instance.gameData.reputation += reputation;
            GameManager.instance.gameData.isIncaTernInTown = false;

            GameManager.instance.EndTycoon();
        }
    }
}
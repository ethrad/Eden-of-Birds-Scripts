using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using Tycoon;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

namespace Tutorial
{
    public class TutorialTycoonManager : MonoBehaviour
    {
        public static TutorialTycoonManager instance;

        public List<GameObject> panelList;
        public List<GameObject> rabbitTeakePanelList;
        
        public List<GameObject> characters;
        
        public GameObject kitchenButtonWorkingPanel;
        public GameObject barButtonWorkingPanel;
        
        public int panelNum = 0;
        public int rabbitPanelNum = 0;
        
        public AudioSource bgmSource;
        
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                    Destroy(this.gameObject);
            }

        }

        public void UpdatePanel()
        {
            if (panelNum == 14)
            {
                characters[1].SetActive(true);
            }

            if (panelNum == 16 && characters[1])
            {
                rabbitTeakePanelList[rabbitPanelNum].SetActive(true);
                return;
            }
            panelNum++;
            SetActivePanel(panelNum);
        }

        public void ExitRabbitTeakeRecipeButton()
        {
            if (panelNum == 16 && rabbitPanelNum == 0)
            {
                rabbitTeakePanelList[rabbitPanelNum].SetActive(false);
                rabbitPanelNum++;
                rabbitTeakePanelList[rabbitPanelNum].SetActive(true);
                
            }
        }
        
        public void TutotialRecipeButton()
        {
            if (panelNum == 3)
            {
                panelNum++;
                SetActivePanel(panelNum);
            }
        }
        public GameObject panelObj;
        public GameObject panelObj1;

        public void SetActiveRabbitPanel(int num)
        {
            rabbitTeakePanelList[num-1].SetActive(false);
            rabbitTeakePanelList[num].SetActive(true);
        }
        public void SetActivePanel(int wantNum)
        {

            if (wantNum == 10)
            {
                panelObj.SetActive(true);
                panelObj1.SetActive(true);
            }

            if (wantNum == 16)
            {
                if(characters[1])
                {
                    panelList[wantNum-1].SetActive(false);
                }
                else 
                {
                    panelList[wantNum].SetActive(false);
                    panelNum++;
                    panelList[panelNum].SetActive(true);
                }
                
                return;
            }

            if (wantNum == 18)
            {
                panelList[wantNum-1].SetActive(false);
                return;
            }

            if (wantNum > 18) return;
            panelList[wantNum-1].SetActive(false);
            panelList[wantNum].SetActive(true);
        }

        public void CallingCustomer(int wantId)
        {
            characters[wantId].SetActive(true);
        }
    
        private Dictionary<string, int> savedInventory;

        public void FinishedTutorial()
        {
            GameManager.instance.inventory = savedInventory;
            PlayerPrefs.SetInt("isTycoonTutorialCleared", 1);
            SceneManager.LoadScene("Tycoon");
        }
        
        public GameObject[] plates;
        public GameObject foodPrefab;
        public string[] foodNames = new string[5];
        
        public bool GetServed(string recipeName)
        {
            bool fullFlag = false;

            for (int i = 0; i < 5; i++)
            {
                if (foodNames[i] == "")
                {
                    fullFlag = false;
                    foodNames[i] = recipeName;
                    StartCoroutine(Plating(i));
                    break;
                }

                fullFlag = true;
            }

            if (fullFlag == true)
            {
                return false;
            }

            return true;
        }

        public void BarButtonClick()
        {
            if(rabbitPanelNum == 9)
            {
                rabbitTeakePanelList[9].SetActive(false);
                rabbitPanelNum = -4;
            }
        }
        
        IEnumerator Plating(int plateIndex)
        {
            yield return new WaitForSeconds(2.5f);

            GameObject tempFood = Instantiate(foodPrefab, plates[plateIndex].transform);
            tempFood.GetComponent<FoodController>().UpdateFood(plateIndex, foodNames[plateIndex]);
            tempFood.transform.SetParent(plates[plateIndex].transform);
        }

        
        private void Start()
        {
            characters[0].SetActive(true);
            panelList[0].SetActive(true);

            Dictionary<string, int> tempInventory = new Dictionary<string, int>();
            tempInventory.Add("slimeMucus", 10);
            tempInventory.Add("bigLeaf", 10);
            tempInventory.Add("stem", 10);
            tempInventory.Add("rabbitMeat", 10);

            savedInventory = GameManager.instance.inventory;
            GameManager.instance.inventory = tempInventory;
            
            bgmSource.volume = GameManager.instance.settings.backgroundMusicVolume;
        }
    }
}

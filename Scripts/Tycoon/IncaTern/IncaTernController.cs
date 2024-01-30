using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace IncaTernTycoon
{
    public class ServingUnit : CSVData
    {
        public string ownRecipe;
        public string dialogue;
        public int grade;
        public List<string> properties; // 최대 4개
        public List<int> prices; // 3개
        public List<int> reputations; //3개
        public float waitingTime;

        public ServingUnit()
        {
            
        }
        
        public override void csvToClass(string[] csvArray)
        {
            dialogue = csvArray[0];
            ownRecipe = csvArray[1];
            grade = int.Parse(csvArray[2]);

            properties = new List<string>();
            prices = new List<int>();
            reputations = new List<int>();
            
            for (int i = 3; i < 7; i++)
            {
                if (csvArray[i] != "")
                {
                    properties.Add(csvArray[i]);
                }
            }
            
            for (int i = 7; i < 10; i++)
            {
                if (csvArray[i] != "")
                {
                    prices.Add(int.Parse(csvArray[i]));
                }
            }
            
            for (int i = 10; i < 13; i++)
            {
                if (csvArray[i] != "")
                {
                    reputations.Add(int.Parse(csvArray[i]));
                }
            }
            
            waitingTime = float.Parse(csvArray[13]);
        }
    }
    
    public class IncaTernController : MonoBehaviour, IDropHandler
    {
        public static IncaTernController instance;
        [HideInInspector]
        public bool isIncaTern = true;
        
        [Header("Inca Tern")]
        public Image characterImage;
        public Text dialogue;
        public Image timeBar;
        private float timeLimit;
        private float currentTime;

        private Animator animator;
        
        [Header("Sounds")]
        private AudioSource audioSource;
        public AudioClip audioPerfect;
        public AudioClip audioGood;
        public AudioClip audioBad;
        
        [Space(10f)]
        public AudioSource bgmSource;
        
        private List<ServingUnit> servingUnitList;
        private int servingCount;
        private int randomIndex;
        private ServingUnit currentUnit;

        private bool canServe;
        
        private void UpdateSeat()
        {
            if (servingCount < 10)
            {
                if (servingCount != 0)
                {
                    servingUnitList.RemoveAt(randomIndex);
                }
                
                randomIndex = Random.Range(0, servingUnitList.Count);
                currentUnit = servingUnitList[randomIndex];
                UpdateBalloon();
                
                servingCount++;
            }
            else
            {
                EndTycoon();
            }
        }
        
        private IEnumerator timeBarCoroutine;
        
        private void UpdateBalloon()
        {
            dialogue.text = currentUnit.dialogue;
            timeLimit = currentUnit.waitingTime;
            currentTime = timeLimit;
            canServe = true;
            
            timeBarCoroutine = UpdateTimeBar();
            StartCoroutine(timeBarCoroutine);
        }
    
        private IEnumerator UpdateTimeBar()
        {
            while (currentTime > 0)
            {
                currentTime -= 0.1f;
                timeBar.fillAmount = currentTime / timeLimit;

                yield return new WaitForSeconds(0.1f);
            }

            ServeLate();
            yield break;
        }

        #region Manage Plates

        public GameObject foodPrefab;
        public GameObject[] plates;
        [HideInInspector]
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

        IEnumerator Plating(int plateIndex)
        {
            yield return new WaitForSeconds(2.5f);

            GameObject tempFood = Instantiate(foodPrefab, plates[plateIndex].transform);
            tempFood.GetComponent<FoodController>().UpdateFood(plateIndex, foodNames[plateIndex]);
            tempFood.transform.SetParent(plates[plateIndex].transform);
        }

        public void ResetPlate(int plateIndex)
        {
            Destroy(plates[plateIndex].transform.GetChild(0).gameObject);
            foodNames[plateIndex] = "";
        }

        #endregion

        #region Serving

        public void OnDrop(PointerEventData eventData)
        {
            if (!canServe) return;
            
            string foodName = eventData.pointerDrag.GetComponent<FoodController>().foodName;
            
            if (currentUnit.grade == 0)
            {
                int intersectCount = GameManager.instance.menus[foodName].properties
                    .Intersect(currentUnit.properties).Count();

                if (intersectCount == currentUnit.properties.Count)
                {
                    ServePerfect();
                }
                else if (intersectCount == 0)
                {
                    ServeBad();
                }
                else
                {
                    ServeGood();
                }
            }
            else
            {
                if (currentUnit.grade == GameManager.instance.menus[foodName].grade)
                {
                    ServePerfect();
                }
                else
                {
                    ServeBad();
                }
            }

            canServe = false;
            eventData.pointerDrag.GetComponent<FoodController>().ResetPlate();
            StopCoroutine(timeBarCoroutine);
            StartCoroutine(ServingDelay());
        }

        private IEnumerator ServingDelay()
        {
            yield return new WaitForSeconds(3f);
            UpdateSeat();
        }

        private void ServePerfect()
        {
            audioSource.clip = audioPerfect;
            audioSource.Play();
            
            animator.SetTrigger("Perfect");
            dialogue.text = "...? 제법이군요!";
            
            UpdateResult(currentUnit.prices[0], currentUnit.reputations[0]);
        }
        
        private void ServeGood()
        {
            audioSource.clip = audioGood;
            audioSource.Play();
            
            dialogue.text = "먹을만하네요.";
            UpdateResult(currentUnit.prices[1], currentUnit.reputations[1]);
        }
        
        private void ServeBad()
        {
            audioSource.clip = audioBad;
            audioSource.Play();
            
            animator.SetTrigger("Bad");
            dialogue.text = "이 음식은 당장 치워주세요. 끔찍하네요.";
            
            UpdateResult(currentUnit.prices[2], currentUnit.reputations[2]);
        }

        private void ServeLate()
        {
            canServe = false;
            StopCoroutine(timeBarCoroutine);
            StartCoroutine(ServingDelay());
            
            audioSource.clip = audioBad;
            audioSource.Play();
            
            animator.SetTrigger("Bad");
            dialogue.text = "제시간에 내오지도 못하다니 심각하네요.";
        }
        
        private int revenue = 0;
        private int reputation = 0;

        private void UpdateResult(int price, int reputation)
        {
            revenue += price;
            this.reputation += reputation;
            
        }

        #endregion
        
        public GameObject endPanel;
        
        private void EndTycoon()
        {
            endPanel.GetComponent<EndPanel>().UpdatePanel(revenue, reputation);
            endPanel.SetActive(true);
        }
        
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
        
        private void Start()
        {
            animator = characterImage.GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            
            bgmSource.volume = GameManager.instance.settings.backgroundMusicVolume;
            audioSource.volume = GameManager.instance.settings.soundEffectsVolume;
            
            List<ServingUnit> servingUnits = IOManager.instance.ReadCSV<ServingUnit>("Tycoon/IncaTernServingUnits");
            List<string> ownRecipeList = GameManager.instance.ownRecipes.Values.SelectMany(x => x).ToList(); 
            
            servingUnitList = servingUnits.Where(x => ownRecipeList.Contains(x.ownRecipe) || x.ownRecipe == "").ToList();

            UpdateSeat();
        }
    }
}



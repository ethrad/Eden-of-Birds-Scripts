using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialDungeonManager : MonoBehaviour
    {
        public static TutorialDungeonManager instance;

        public Dictionary<string, int> tempInventory = new Dictionary<string, int>();
        public GameObject dropItemPrefab;

        public GameObject player;
        public AudioSource bgmSource;
        private AudioSource audioSource;

        #region Object Pooling

        [SerializeField] private GameObject[] ObjectPoolingPrefabs;
        private Dictionary<string, GameObject> poolingObjectPrefabs = new Dictionary<string, GameObject>();

        private Dictionary<string, Queue<GameObject>> poolingObjectQueues = new Dictionary<string, Queue<GameObject>>();

        private void InitializeObject(int initCount)
        {
            for (int i = 0; i < ObjectPoolingPrefabs.Length; i++)
            {
                poolingObjectPrefabs.Add(ObjectPoolingPrefabs[i].name, ObjectPoolingPrefabs[i]);
                poolingObjectQueues.Add(ObjectPoolingPrefabs[i].name, new Queue<GameObject>());

                for (int j = 0; j < initCount; j++)
                {
                    poolingObjectQueues[ObjectPoolingPrefabs[i].name]
                        .Enqueue(CreateNewObject(ObjectPoolingPrefabs[i].name));
                }
            }
        }

        private GameObject CreateNewObject(string objectName)
        {
            var newObj = Instantiate(poolingObjectPrefabs[objectName], transform, true);
            newObj.gameObject.SetActive(false);
            return newObj;
        }

        public GameObject GetObject(string objectName)
        {
            if (instance.poolingObjectQueues[objectName].Count > 0)
            {
                var obj = instance.poolingObjectQueues[objectName].Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = instance.CreateNewObject(objectName);
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        public void ReturnObject(string objectName, GameObject obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(instance.transform);
            instance.poolingObjectQueues[objectName].Enqueue(obj);
        }

        #endregion

        [Header("JoyStick and Interact Button")]
        public GameObject joyStick;

        public GameObject shootingJoyStick;

        public GameObject interactButton;
        public GameObject harvestButton;
        public GameObject chestButton;


        #region Interact

        private GameObject currentNPC;

        public void OnInteractButton(GameObject NPC)
        {
            currentNPC = NPC;
            interactButton.SetActive(true);
            shootingJoyStick.SetActive(false);
        }

        public void OnInteractButtonClicked()
        {
            StopPlayerMoving();
            currentNPC.GetComponent<NPCController>().Interact();
            interactButton.SetActive(false);
        }


        public void OffInteractButton()
        {
            interactButton.SetActive(false);
            shootingJoyStick.SetActive(true);
        }

        GameObject currentCrop;

        public void OnHarvestButton(GameObject crop)
        {
            currentCrop = crop;
            harvestButton.SetActive(true);
            shootingJoyStick.SetActive(false);
        }

        public void OnHarvestButtonClicked()
        {
            player.GetComponent<PlayerController>().Harvest();
            currentCrop.GetComponent<CropController>().Harvest();

            OffHarvestButton();
        }

        public void OffHarvestButton()
        {
            harvestButton.SetActive(false);
            shootingJoyStick.SetActive(true);
        }

        public void StopPlayerMoving()
        {
            player.GetComponent<PlayerController>().StopMoving();
            joyStick.SetActive(false);
            interactButton.SetActive(false);
            shootingJoyStick.SetActive(false);
        }

        public void StartPlayerMoving(bool canInteractAgain)
        {
            player.GetComponent<PlayerController>().StartMoving();
            joyStick.SetActive(true);

            if (canInteractAgain)
            {
                interactButton.SetActive(true);
            }
            else
            {
                shootingJoyStick.SetActive(true);
            }
        }

        #endregion

        #region Equipment

        public List<Equipment> equipmentList;
        public Dictionary<string, Equipment> equipments = new Dictionary<string, Equipment>();

        GameObject currentChest;
        [Header("Panels")] 
        public GameObject chestPanel;

        void ReadEquipmentList()
        {
            equipmentList = IOManager.instance.ReadCSV<Equipment>("Dungeon/EquipmentList");
            equipments.Add("head", null);
            equipments.Add("body", null);
            equipments.Add("weapon", null);
        }

        public void OnChestButton(GameObject chest)
        {
            currentChest = chest;
            chestButton.SetActive(true);
            shootingJoyStick.SetActive(false);
        }

        public void OnChestButtonClicked()
        {
            currentChest.GetComponent<ChestController>().OpenChest();
            OffChestButton();
        }

        public void OnChestPanel(Equipment equipment)
        {
            chestPanel.GetComponent<ChestPanel>().Initialize(equipment);
            chestPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ChangeEquipment(Equipment equipment)
        {
            int HP = equipment.HP;
            int ATK = equipment.ATK;
            int DEF = equipment.DEF;

            if (equipments[equipment.type] != null)
            {
                HP = equipment.HP - equipments[equipment.type].HP;
                ATK = equipment.ATK - equipments[equipment.type].ATK;
                DEF = equipment.DEF - equipments[equipment.type].DEF;
            }

            player.GetComponent<PlayerController>().ChangeEquipment(HP, ATK, DEF);

            equipments[equipment.type] = equipment;
        }

        public void OffChestButton()
        {
            chestButton.SetActive(false);
            shootingJoyStick.SetActive(true);
        }

        #endregion

        #region Get Item

        [Header("Get Item UI")] public GameObject getItemPanel;
        public GameObject itemImage;
        public GameObject itemNumText;
        public AudioClip audioItem;

        bool isUIActive = false;

        public void GetItem(string itemName)
        {
            if (tempInventory.ContainsKey(itemName))
            {
                tempInventory[itemName]++;
            }
            else
            {
                tempInventory[itemName] = 1;
            }

            audioSource.clip = audioItem;
            audioSource.Play();

            StartCoroutine(CountGetItemUI(itemName));
        }

        IEnumerator CountGetItemUI(string itemName)
        {
            if (isUIActive)
            {
                getItemPanel.SetActive(false);
                itemImage.SetActive(false);
                itemNumText.SetActive(false);

                yield return new WaitForSeconds(0.2f);
            }

            itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Items/" + itemName);
            itemImage.SetActive(true);
            itemNumText.SetActive(true);
            getItemPanel.SetActive(true);

            isUIActive = true;

            yield return new WaitForSeconds(2f);

            itemImage.SetActive(false);
            itemNumText.SetActive(false);
            getItemPanel.SetActive(false);
            isUIActive = false;
        }

        #endregion


        public GameObject byodyguard;
        public GameObject byoPeddler;

        public List<GameObject> tutorialPanelList;

        public GameObject moveToNextPanelButton;
        private int panelIndex = 0;

        public void MoveToNextPanel()
        {
            var currentPanel = tutorialPanelList[panelIndex].GetComponent<PanelTrigger>();

            switch (currentPanel.panelType)
            {
                case 0:
                    tutorialPanelList[panelIndex].gameObject.SetActive(false);
                    panelIndex++;
                    tutorialPanelList[panelIndex].gameObject.SetActive(true);
                    break;
                case 1:
                    tutorialPanelList[panelIndex].gameObject.SetActive(false);
                    moveToNextPanelButton.SetActive(false);
                    panelIndex++;
                    Time.timeScale = 1f;
                    player.GetComponent<PlayerController>().StartMoving();
                    break;
                case 2:
                    moveToNextPanelButton.SetActive(false);
                    break;
                case 3: 
                    currentPanel.OnSlime();
                    tutorialPanelList[panelIndex].gameObject.SetActive(false);
                    moveToNextPanelButton.SetActive(false);
                    panelIndex++;
                    Time.timeScale = 1f;
                    player.GetComponent<PlayerController>().StartMoving();
                    break;
                case 4:
                    currentPanel.OffEagle();
                    break;
                case 5:
                    moveToNextPanelButton.SetActive(false);
                    gameObject.GetComponent<QuestTrigger>().OnQuestTrigger();
                    gameObject.GetComponent<HomeworkTrigger>().OnHomeworkTrigger();
                    GameManager.instance.gameData.dungeonRemainCount--;
                    GameManager.instance.WriteGameData();
                    GameManager.instance.WriteOngoingQuests();
                    GameManager.instance.WriteInventory();
                    
                    Time.timeScale = 1f;
                    if (GameManager.instance.gameData.purchasedAdFree)
                    {
                        SceneManager.LoadScene("Town");
                    }
                    else
                    {
                        LoadMobileAD.Instance.LoadInterstitialAd("Town");
                    }
                    break;
            }

            if (currentPanel.isFade)
            {
                StartCoroutine(FadeEffect());
            }
        }

        IEnumerator FadeEffect()
        {
            Debug.Log(panelIndex);
            
            GameManager.instance.FadeOutEffect();

            while (!GameManager.instance.isFadeOutEnd)
            {
                yield return null;
            }

            switch (panelIndex)
            {
                case 9:
                    byodyguard.SetActive(false);
                    break;
                case 12:
                    byoPeddler.SetActive(false);
                    break;
            }

            GameManager.instance.FadeInEffect();
            GameManager.instance.ResetFadeFlags();
        }

        public void OnPanel()
        {
            player.GetComponent<PlayerController>().StopMoving();
            moveToNextPanelButton.SetActive(!tutorialPanelList[panelIndex].GetComponent<PanelTrigger>().hasButton);
            
            tutorialPanelList[panelIndex].gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        public void OnReloadButtonClicked()
        {
            tutorialPanelList[panelIndex].gameObject.SetActive(false);
            panelIndex++;
            tutorialPanelList[panelIndex].gameObject.SetActive(true);
            moveToNextPanelButton.SetActive(true);
            player.GetComponent<PlayerController>().StartMoving();
        }
        
        [HideInInspector]
        public bool isRockHarvested = false;
        [HideInInspector]
        public bool isChestOpened = false;

        public GameObject secondRoomEagle;
        
        public void OnRockHarvested()
        {
            if (isRockHarvested)
            {
                return;
            }
            
            isRockHarvested = true;
            
            if (isChestOpened)
            {
                secondRoomEagle.SetActive(false);
            }
        }
        
        public void OnChestOpened()
        
        {
            if (isChestOpened)
            {
                return;
            }
            
            isChestOpened = true;
            
            if (isRockHarvested)
            {
                secondRoomEagle.SetActive(false);
            }
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

            InitializeObject(20);

            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            bgmSource.volume = GameManager.instance.settings.backgroundMusicVolume;
            audioSource.volume = GameManager.instance.settings.soundEffectsVolume;
            
            ReadEquipmentList();

            Time.timeScale = 0;
        }
    }
}

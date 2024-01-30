
using Dungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon
{
    public class Equipment : CSVData
    {
        public string type;
        public int grade;
        public string name;

        public int HP;
        public int ATK;
        public int DEF;

        public List<int> probs;
        
        public override void csvToClass(string[] csvArray)
        {
            type = csvArray[0];
            grade = int.Parse(csvArray[1]);
            
            name = csvArray[2];
            HP = int.Parse(csvArray[3]);
            ATK = int.Parse(csvArray[4]);
            DEF = int.Parse(csvArray[5]);
            
            probs = new List<int>();
            for (int i = 6; i < csvArray.Length; i++)
            {
                probs.Add(int.Parse(csvArray[i]));
            }
        }
    }
    
    public class ShopItem : CSVData
    {
        public string type;
        public string name;
        public string koreanName;
        public string description;
        public int count;
        public int price;

        public override void csvToClass(string[] csvArray)
        {
            type = csvArray[0];
            name = csvArray[1];
            koreanName = csvArray[2];
            description = csvArray[3];
            count = int.Parse(csvArray[4]);
            price = int.Parse(csvArray[5]);
        }
    }
    
}

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public Dictionary<string, int> tempInventory = new Dictionary<string, int>();
    public GameObject dropItemPrefab;
    public GameObject bossDropItemPrefab;
    
    public new Camera camera;
    public GameObject player;
    public GameObject miniMap;
    public AudioSource bgmSource;
    public AudioClip audioBoss;
    private AudioSource audioSource;

    #region Object Pooling

    [SerializeField]
    private GameObject[] ObjectPoolingPrefabs;
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
                poolingObjectQueues[ObjectPoolingPrefabs[i].name].Enqueue(CreateNewObject(ObjectPoolingPrefabs[i].name));
            }
        }
    }

    private GameObject CreateNewObject(string objectName) { 
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

    #region Manage Dungeon Rooms
    
    public GameObject virtualCamera;
    public GameObject eagle;

    [Header("[Dungeon Rooms]")]
    public GameObject[] huntingRoomPrefabs;
    public GameObject[] harvestRoomsPrefabs;
    public GameObject[] emptyRoomsPrefabs;
    public GameObject bossRoomPrefab;

    // 층 별 방 개수
    public List<int> huntingRoomCount;
    public List<int> harvestRoomCount;
    public List<int> emptyRoomCount;

    [Header("[Room Parents]")]
    public GameObject tempRooms;
    
    private GameObject firstRoom;
    private GameObject lastRoom;
    public GameObject placedRooms;
    public GameObject sideRooms;
    
    public int floor = 0; // 몇 층인지

    [Header("[Byo Room]")]
    public GameObject byoRoomPrefab;
    private GameObject byoRoom;
    public List<int> byoProbs;

    private void MakeFloor()
    {
        List<int> huntingRoomIndex = new List<int>();
        List<int> harvestRoomIndex = new List<int>();
        List<int> emptyRoomIndex = new List<int>();
        
        for (int i = 0; i < huntingRoomPrefabs.Length; i++)
        {
            huntingRoomIndex.Add(i);
        }
        
        for (int i = 0; i < harvestRoomsPrefabs.Length; i++)
        {
            harvestRoomIndex.Add(i);
        }
        
        for (int i = 0; i < emptyRoomsPrefabs.Length; i++)
        {
            emptyRoomIndex.Add(i);
        }
        
        // 층에 사용할 방들을 미리 만들어둠
        for (int i = 0; i < huntingRoomCount[floor]; i++)
        {
            int roomIndex = Random.Range(0, huntingRoomIndex.Count);
            GameObject tempRoom = Instantiate(huntingRoomPrefabs[huntingRoomIndex[roomIndex]], tempRooms.transform);
            huntingRoomIndex.RemoveAt(roomIndex);
        }
        
        for (int i = 0; i < harvestRoomCount[floor]; i++)
        {
            int roomIndex = Random.Range(0, harvestRoomIndex.Count);
            GameObject tempRoom = Instantiate(harvestRoomsPrefabs[harvestRoomIndex[roomIndex]], tempRooms.transform);
            harvestRoomIndex.RemoveAt(roomIndex);
        }
        
        for (int i = 0; i < emptyRoomCount[floor]; i++)
        {
            int roomIndex = Random.Range(0, emptyRoomIndex.Count);
            GameObject tempRoom = Instantiate(emptyRoomsPrefabs[emptyRoomIndex[roomIndex]], tempRooms.transform);
            emptyRoomIndex.RemoveAt(roomIndex);
        }

        // 첫 방
        var roomCount = tempRooms.transform.childCount;
        int r = Random.Range(0, roomCount);
        tempRooms.transform.GetChild(0).GetComponent<RoomController>().SetRoom(0, RoomController.RoomType.Start);
        firstRoom = tempRooms.transform.GetChild(0).gameObject;
        firstRoom.transform.SetParent(placedRooms.transform);

        // 중간 방
        for (int i = 1; i < roomCount - 1; i++)
        {
            r = Random.Range(0, tempRooms.transform.childCount);
            
            switch (i % 3)
            {
                case 0:
                    tempRooms.transform.GetChild(r).GetComponent<RoomController>().SetRoom(i / 3, RoomController.RoomType.Top);
                    tempRooms.transform.GetChild(r).SetParent(placedRooms.transform);
                    break;
                case 1:
                    tempRooms.transform.GetChild(r).GetComponent<RoomController>().SetRoom(i / 3, RoomController.RoomType.Left);
                    tempRooms.transform.GetChild(r).transform.SetParent(sideRooms.transform);
                    break;
                case 2:
                    tempRooms.transform.GetChild(r).GetComponent<RoomController>().SetRoom(i / 3, RoomController.RoomType.Right);
                    tempRooms.transform.GetChild(r).transform.SetParent(sideRooms.transform);
                    break;
            }
        }
        
        // 끝 방
        tempRooms.transform.GetChild(0).GetComponent<RoomController>().SetRoom(roomCount / 3, RoomController.RoomType.End);
        lastRoom = tempRooms.transform.GetChild(0).gameObject;
        lastRoom.transform.SetParent(placedRooms.transform);
        
        // 첫번째 방이랑 끝 방에 순무, 독수리 배치
        player.transform.position = firstRoom.transform.position;
        eagle.transform.position = lastRoom.transform.position;
        
        // 카메라 위치 이동
        virtualCamera.transform.position = player.transform.position;
        camera.transform.position = player.transform.position;
        
        r = Random.Range(0, 100); // 뵤 등장 확률
        bool existByo = r < byoProbs[floor];
        
        if (existByo)
        {
            int byoRoomNum = Random.Range(0, sideRooms.transform.childCount);
            
            byoRoom = sideRooms.transform.GetChild(byoRoomNum).GetComponent<RoomController>().MakeByoRoom();
            byoPeddlerShopPanel.GetComponent<ByoPeddlerShopPanel>().Initialize();
        }
        
        //virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = cameraBounds.GetComponent<Collider2D>();
    }

    public void OpenByoRoom()
    {
        if (GameManager.instance.gameData.PurchaseGold(30))
        {
           byoRoom.GetComponent<ByoRoomController>().OpenRoom(); 
        }

        // 화면 꺼지는 연출
        
    }

    public void MoveToNextFloor()
    {
        StartCoroutine(MoveToNextFloorWithFadeEffect());
    }

    IEnumerator MoveToNextFloorWithFadeEffect()
    {
        GameManager.instance.FadeOutEffect();
        Time.timeScale = 0;
        
        while (!GameManager.instance.isFadeOutEnd)
        {
            yield return null;
        }
        
        foreach(var room in placedRooms.transform.GetComponentsInChildren<RoomController>())
        {
            Destroy(room.gameObject);
        }
        
        foreach(var room in sideRooms.transform.GetComponentsInChildren<RoomController>())
        {
            Destroy(room.gameObject);
        }
        
        Destroy(byoRoom);
        floor++;
        player.GetComponent<PlayerController>().Heal(1000);

        if (floor < 3)
        {
            MakeFloor();
        }
        else
        {
            bgmSource.Stop();
            bgmSource.clip = audioBoss;
            miniMap.SetActive(false);
            GameObject bossRoom = Instantiate(bossRoomPrefab);
            bossRoom.GetComponent<BossRoomController>().SetRoom();
            bgmSource.Play();
        }
        
        virtualCamera.transform.position = player.transform.position;
        camera.transform.position = player.transform.position;
        
        StartPlayerMoving(false);
        
        GameManager.instance.FadeInEffect();
        
        while(!GameManager.instance.isFadeInEnd)
        {
            yield return null;
        }
        
        Time.timeScale = 1;
        player.GetComponent<PlayerController>().Invincible();
    }
    
    #endregion
    
    #region Clear and Fail
    
    [Header("Dungeon End, Revive and Fail Panel")]
    public GameObject dungeonEndPanel;
    public GameObject dungeonAllClearPanel;
    public GameObject dungeonRevivePanel;
    public GameObject dungeonFailPanel;

    [HideInInspector]
    public bool isAllCleared = false;
    [HideInInspector]
    public int adjustGold = 0;
    
    public void ClearDungeon()
    {
        dungeonEndPanel.GetComponent<DungeonEndPanel>().UpdatePanel(true);
        dungeonEndPanel.SetActive(true);
    }
    
    public void ClearAllDungeon()
    {
        dungeonAllClearPanel.GetComponent<DungeonEndPanel>().UpdatePanel(true);
        dungeonAllClearPanel.SetActive(true);
    }

    public void OnRevivePanel()
    {
        dungeonRevivePanel.SetActive(true);
    }

    public void RevivePlayer()
    {
        player.GetComponent<PlayerController>().Revive();
    }

    public void FailDungeon()
    {
        // 아이템 그대로 얻는데 치료비 골드 차감
        adjustGold = Mathf.FloorToInt(GameManager.instance.gameData.gold * 0.05f);
        GameManager.instance.gameData.PurchaseGold(adjustGold);
        dungeonFailPanel.GetComponent<DungeonEndPanel>().UpdatePanel(false);
        dungeonFailPanel.SetActive(true);
    }

    #endregion

    [Header("JoyStick and Interact Button")]
    public GameObject basePanel;
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
        shootingJoyStick.GetComponent<ShootingJoystick>().Reset();
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
        shootingJoyStick.GetComponent<ShootingJoystick>().Reset();
        shootingJoyStick.SetActive(false);
    }

    public void OnHarvestButtonClicked()
    {
        player.GetComponent<PlayerController>().Harvest();
        currentCrop.GetComponent<CropController>().Harvest();

        harvestButton.SetActive(false);
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

    public void OnBasePanel()
    {
        player.GetComponent<PlayerController>().StartMoving();
        basePanel.SetActive(true);
    }

    public void OffBasePanel()
    {
        basePanel.SetActive(false);
        player.GetComponent<PlayerController>().StopMoving();
        joyStick.GetComponent<FixedJoystick>().Reset();
        shootingJoyStick.GetComponent<ShootingJoystick>().Reset();
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

    #region Byo

    [Header("Byo")]
    public GameObject byodyguardPanel;
    public GameObject byoPeddlerEnterPanel;
    public GameObject byoPeddlerShopPanel;
    
    public void OnByodyguardPanel()
    {
        byodyguardPanel.SetActive(true);
    }
    
    public void OnByoPeddlerPanel()
    {
        byoPeddlerEnterPanel.SetActive(true);
    }
    
    [HideInInspector]
    public GameObject byoPeddler;
    public Dictionary<string, List<ShopItem>> shopItems = new Dictionary<string, List<ShopItem>>();
    
    [HideInInspector]
    public bool isShopItemPurchased = false;
    [HideInInspector]
    public bool isRecipeInitialized = false;
    
    
    private void ReadShopItems()
    {
        List<ShopItem> tempShopItems = IOManager.instance.ReadCSV<ShopItem>("Dungeon/ByoShopItems");
        
        shopItems.Add("recipe", new List<ShopItem>());
        shopItems.Add("item", new List<ShopItem>());
        shopItems.Add("equipment", new List<ShopItem>());
        shopItems.Add("potion", new List<ShopItem>());

        foreach (var shopItem in tempShopItems)
        {
            shopItems[shopItem.type].Add(shopItem);
        }
    }

    #endregion
    
    #region Get Item

    [Header("Get Item UI")]
    public GameObject getItemPanel;
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
    
    public void GetItem(string itemName, int count)
    {
        if (tempInventory.ContainsKey(itemName))
        {
            tempInventory[itemName] += count;
        }
        else
        {
            tempInventory[itemName] = count;
        }
        
        audioSource.clip = audioItem;
        audioSource.Play();

        StartCoroutine(CountGetItemUI(itemName, count));
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

    IEnumerator CountGetItemUI(string itemName, int count)
    {
        if (isUIActive)
        {
            getItemPanel.SetActive(false);
            itemImage.SetActive(false);
            itemNumText.SetActive(false);

            yield return new WaitForSeconds(0.2f);
        }

        itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Items/" + itemName);
        itemNumText.GetComponent<Text>().text = "X " + count;
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

    private void SetJoysticks()
    {
        var joystickValues = GameManager.instance.settings.joystickValues;
        joyStick.GetComponent<Joystick>().SetJoystick(joystickValues);
        var tempJoystickValues = new JoystickValues(-joystickValues.XPosition, joystickValues.YPosition, joystickValues.joystickSize, joystickValues.handleSize);
        shootingJoyStick.GetComponent<Joystick>().SetJoystick(tempJoystickValues);
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

        bgmSource.volume = GameManager.instance.settings.backgroundMusicVolume;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.settings.soundEffectsVolume;
    }
    
    private void Start()
    {
        ReadEquipmentList();
        ReadShopItems();
        SetJoysticks();
        MakeFloor();
        player.GetComponent<PlayerController>().Invincible();
    }
}

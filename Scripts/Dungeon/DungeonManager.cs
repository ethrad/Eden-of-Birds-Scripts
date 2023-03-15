using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public Dictionary<string, int> tempInventory = new Dictionary<string, int>();

    public AudioSource audioSource;

    #region Object Pooling
    public GameObject player;

    [SerializeField]
    private GameObject[] prefabs;
    private Dictionary<string, GameObject> poolingObjectPrefabs = new Dictionary<string, GameObject>();

    private Dictionary<string, Queue<GameObject>> poolingObjectQueues = new Dictionary<string, Queue<GameObject>>();

    private void InitializeObject(int initCount)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            poolingObjectPrefabs.Add(prefabs[i].name, prefabs[i]);
            poolingObjectQueues.Add(prefabs[i].name, new Queue<GameObject>());

            for (int j = 0; j < initCount; j++)
            {
                poolingObjectQueues[prefabs[i].name].Enqueue(CreateNewObject(prefabs[i].name));
            }
        }
    }

    private GameObject CreateNewObject(string objectName) { 
        var newObj = Instantiate(poolingObjectPrefabs[objectName]);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
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

    public GameObject rooms;
    int roomIndex;

    public int monsterCount;
    public bool isRoomCleared;
    public bool isDungeonCleared;

    //몬스터 다 처치 플래그
    public static bool isClear = false;

    void SelectRoom()
    {
        monsterCount = 0;
        isRoomCleared = false;

        roomIndex = Random.Range(0, rooms.transform.childCount);

        GameObject tempRoom = rooms.transform.GetChild(roomIndex).gameObject;

        for (int i = 0; i < tempRoom.GetComponent<RoomController>().monsterNums.Length; i++)
        {
            monsterCount += tempRoom.GetComponent<RoomController>().monsterNums[i];
        }

        tempRoom.GetComponent<RoomController>().Initialize();
    }

    public void ClearRoom()
    {
        StartCoroutine(DestroyRoom(rooms.transform.GetChild(roomIndex).gameObject));
    }

    IEnumerator DestroyRoom(GameObject go)
    {
        Destroy(go);
        yield return new WaitForEndOfFrame();
        SelectRoom();
    }

    public void UpdateMonsterCount()
    {
        monsterCount--;

        if (monsterCount <= 0)
        {
            isClear = true;
            isRoomCleared = true;
            if (rooms.transform.childCount == 1)
            {
                isDungeonCleared = true;
                adjustGold = 30;
                ItemManager.instance.gold += 30;
            }

            rooms.transform.GetChild(roomIndex).gameObject.GetComponent<RoomController>().ClearRoom(isDungeonCleared);
        }
    }

    #endregion

    #region Clear and Fail

    public GameObject dungeonEndPanel;
    public GameObject dungeonFailPanel;
    public int adjustGold = 0;
    
    public void ClearDungeon()
    {
        dungeonEndPanel.GetComponent<DungeonEndPanel>().UpdatePanel();
        dungeonEndPanel.SetActive(true);
    }

    public void FailDungeon()
    {
        // 아이템 그대로 얻는데 치료비 골드 차감
        adjustGold = Mathf.FloorToInt(ItemManager.instance.gold * 0.05f);
        ItemManager.instance.gold -= adjustGold;
        Time.timeScale = 0f;
        dungeonFailPanel.GetComponent<DungeonEndPanel>().UpdatePanel();
        dungeonFailPanel.SetActive(true);
    }

    #endregion

    #region Harvest

    public GameObject harvestButton;
    GameObject currentCrop;

    public void OnHarvestButton(GameObject crop)
    {
        currentCrop = crop;
        harvestButton.SetActive(true);
    }

    public void OnHarvestButtonClicked()
    {
        isCrop = true;
        currentCrop.GetComponent<CropController>().Harvest();

        OffHarvestButton();
    }

    public void OffHarvestButton()
    {
        harvestButton.SetActive(false);
    }


    #endregion

    #region Get Item

    public GameObject itemImage;
    public GameObject itemNumText;
    public AudioClip audioItem;
    public AudioClip audioCrop;
    bool isCrop = false;

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

        StartCoroutine(CountGetItemUI(itemName));
    }

    IEnumerator CountGetItemUI(string itemName)
    {
        if (isUIActive == true)
        {
            itemImage.SetActive(false);
            itemNumText.SetActive(false);

            yield return new WaitForSeconds(0.2f);
        }

        itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Items/" + itemName);
        itemImage.SetActive(true);
        itemNumText.SetActive(true);

        if (isCrop == true)
        {
            audioSource.clip = audioCrop;
        }
        else
        {
            audioSource.clip = audioItem;
        }

        audioSource.Play();

        isUIActive = true;

        yield return new WaitForSeconds(2f);

        itemImage.SetActive(false);
        itemNumText.SetActive(false);
        isUIActive = false;
    }


    #endregion


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
        SelectRoom();

        this.audioSource = GetComponent<AudioSource>();
    }
}

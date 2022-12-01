using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public Dictionary<string, int> tempInventory = new Dictionary<string, int>();

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
    }
}

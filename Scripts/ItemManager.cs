using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
    public string name;
    public string description;
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    private int _gold;

    public int gold
    {
        get { return _gold; }    // _data 반환
        set { _gold = value; }   // value 키워드 사용
    }

    public Dictionary<string, ItemInfo> itemList = new Dictionary<string, ItemInfo>();
    public Dictionary<string, int> inventory;

    #region Data IO
    public void ReadInventory()
    {
        inventory = IOManager.instance.ReadLocalJson<Dictionary<string, int>>("Inventory");
        Debug.Log(inventory);
    }

    public void WriteInventory()
    {
        IOManager.instance.WriteJson(inventory, "Inventory.json");
        IOManager.instance.playerSettings.gold = gold;
        IOManager.instance.WritePlayerSettings();
    }

    public void ReadItemList()
    {
        itemList = IOManager.instance.ReadJson<Dictionary<string, ItemInfo>>("ItemList");
    }
    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }

    void Start()
    {
        ReadInventory();
        ReadItemList();
        gold = IOManager.instance.playerSettings.gold;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    public void ReadInventory()
    {
        inventory = IOManager.instance.ReadJson("Inventory.json").
            ToDictionary(pair => pair.Key, pair => (int)pair.Value);
    }

    public void WriteInventory()
    {
        IOManager.instance.WriteJson(inventory, "Inventory.json");
    }

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

/*        foreach (KeyValuePair<string, int> item in inventory)
        {
            Debug.Log("Key " + item.Key + "\nValue " + item.Value);
        }*/
    }
}

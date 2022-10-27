using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

   
    public GameObject player;

    [SerializeField]
    private GameObject[] prefabs;
    private Dictionary<string, GameObject> poolingObjectPrefabs = new Dictionary<string, GameObject>();

    private Dictionary<string, Queue<GameObject>> poolingObjectQueues = new Dictionary<string, Queue<GameObject>>();

    private void Initialize(int initCount)
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

        Initialize(20);
    }
}

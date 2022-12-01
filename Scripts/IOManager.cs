using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

public class PlayerSettings
{
    public int gold;
    public bool isTutorialCleared;

    public PlayerSettings(int gold, bool isTutorialCleared)
    {
        this.gold = gold;
        this.isTutorialCleared = isTutorialCleared;
    }
}

public class IOManager : MonoBehaviour
{
    public static IOManager instance;

    public PlayerSettings playerSettings;

    string path;
    string filePath;

    public T ReadJson<T>(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path + fileName);

        return JsonConvert.DeserializeObject<T>(textAsset.ToString());
    }

    public T ReadLocalJson<T>(string fileName) where T : new()
    {
        if (System.IO.File.Exists(filePath + "/" + fileName + ".json"))
        {
            using (StreamReader file = File.OpenText(filePath + "/" + fileName + ".json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject json = (JObject)JToken.ReadFrom(reader);

                return JsonConvert.DeserializeObject<T>(json.ToString());
            }
        }
        else
        {
            return new T();
        }
    }

    public void WriteJson<T>(T input, string fileName)
    {
        string json = JsonConvert.SerializeObject(input);
        JObject jobject = JObject.Parse(json);

        File.WriteAllText(filePath + "/" + fileName, jobject.ToString());
    }

    public void ReadPlayerSettings()
    {
        if (System.IO.File.Exists(filePath + "/playerSettings.json"))
        {
            using (StreamReader file = File.OpenText(filePath + "/playerSettings.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject json = (JObject)JToken.ReadFrom(reader);

                playerSettings = JsonConvert.DeserializeObject<PlayerSettings>(json.ToString());
            }
        }
        else
        {
            playerSettings = new PlayerSettings(0, false);
            WritePlayerSettings();
        }
    }

    public void WritePlayerSettings()
    {
        string json = JsonConvert.SerializeObject(playerSettings);
        JObject jobject = JObject.Parse(json);

        File.WriteAllText(filePath + "/playerSettings.json", jobject.ToString());
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

        filePath = Application.persistentDataPath;

        path = "Datas/";

        ReadPlayerSettings();
    }
}

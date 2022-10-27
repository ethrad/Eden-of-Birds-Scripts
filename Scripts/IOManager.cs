using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

public class IOManager : MonoBehaviour
{
    public static IOManager instance;

    string path;

    public Dictionary<string, dynamic> ReadJson(string fileName)
    {
        using (StreamReader file = File.OpenText(path + fileName))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            JObject json = (JObject)JToken.ReadFrom(reader);

            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json.ToString());
        }
    }

    public JObject ReadJson2(string fileName)
    {
        using (StreamReader file = File.OpenText(path + fileName))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            JObject json = (JObject)JToken.ReadFrom(reader);

            return json;
        }
    }

    public void WriteJson<T>(Dictionary<string, T> input, string fileName)
    {
        string json = JsonConvert.SerializeObject(input);
        JObject jobject = JObject.Parse(json);

        File.WriteAllText(path + fileName, jobject.ToString());
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

        // path = Application.persistentDataPath + "/Datas/";
        path = "Assets/Datas/";
    }
}

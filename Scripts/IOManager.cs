using Dungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Text;
using System.Text.RegularExpressions;

public class CSVData
{
    public virtual void csvToClass(string[] csvArray)
    {
            
    }
}

// 암호화 X

public class IOManager : MonoBehaviour
{
    public static IOManager instance;

    string path;
    string filePath;


    public Dictionary<string, List<Quest>> ReadQuest(string fileName)
    {
        string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

        Dictionary<string, List<Quest>> quests = new Dictionary<string, List<Quest>>();
        var data = Resources.Load(path + fileName) as TextAsset;

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] values = lines[i].Split(',');

            Quest tempQuest = new Quest();

            tempQuest.NPCName = values[0];
            tempQuest.EndNPCName = values[1];
            tempQuest.QID = Int32.Parse(values[2]);
            tempQuest.QName = values[3];
            tempQuest.BSName = values[4];
            tempQuest.ASName = values[5];

            tempQuest.preConditions = new List<PreCondition>();
            tempQuest.questGoals = new List<QuestGoal>();
            tempQuest.questRewards = new List<QuestReward>();
            tempQuest.specialConditions = new List<SpecialCondition>();
            
            
            for (int j = 0; j < 3; j++)
            {
                if (values[j * 4 + 6].Length == 0 || values[j * 4 + 6] == "")
                {
                    break;
                }
                
                tempQuest.preConditions.Add(new PreCondition(Int32.Parse(values[j * 4 + 6]), values[j * 4 + 7], values[j * 4 + 8], values[j * 4 + 9]));
            }
            
            for (int j = 0; j < 3; j++)
            {
                if (values[j * 3 + 18].Length == 0 || values[j * 3 + 18] == "")
                {
                    break;
                }

                Int32.TryParse(values[j * 3 + 20], out int t);
                    
                tempQuest.questGoals.Add(new QuestGoal(Int32.Parse(values[j * 3 + 18]), values[j * 3 + 19], t));
            }
            
            for (int j = 0; j < 8; j++)
            {
                if (values[j * 3 + 27].Length == 0 || values[j * 3 + 27] == "")
                {
                    break;
                }
                
                Int32.TryParse(values[j * 3 + 29], out int t);
                
                tempQuest.questRewards.Add(new QuestReward(Int32.Parse(values[j * 3 + 27]), values[j * 3 + 28], t));
            }
            
            for (int j = 0; j < 2; j++)
            {
                if (values[j * 3 + 51].Length == 0 || values[j * 3 + 51] == "")
                {
                    break;
                }

                Int32.TryParse(values[j * 3 + 52], out int t);
                tempQuest.specialConditions.Add(new SpecialCondition(Int32.Parse(values[j * 3 + 51]), t, values[j * 3 + 53]));
            }
            

            if (!quests.ContainsKey(values[0]))
            {
                quests[values[0]] = new List<Quest>();
            }
            quests[values[0]].Add(tempQuest);

        }
        return quests;
    }

    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    public List<T> ReadCSV<T>(string fileName) where T: CSVData, new()
    {
        List<T> list = new List<T>();
        var data = Resources.Load(path + fileName) as TextAsset;

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);
        
        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] values = lines[i].Split(',');
            for (int j = 0; j < values.Length; j++)
            {
                string value = values[j];

                value = Regex.Replace(value,"`", ",");
                values[j] = value;
            }


            T t = new T();
            t.csvToClass(values);
            list.Add(t);
        }
        
        return list;
    }
    
    public List<string> ReadCSV(string fileName)
    {
        List<string> list = new List<string>();
        var data = Resources.Load(path + fileName) as TextAsset;

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] values = lines[i].Split(',');
            for (int j = 0; j < values.Length; j++)
            {
                string value = values[j];

                value = Regex.Replace(value,"`", ",");
                values[j] = value;
            }

            list.Add(values[0]);
        }
        
        return list;
    }

    public Dictionary<string, Dictionary<string, int>> ReadPresent(string fileName)
    {
        Dictionary<string, Dictionary<string, int>> dic = new Dictionary<string, Dictionary<string, int>>();
        var data = Resources.Load(path + fileName) as TextAsset;

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);
        string[] v = lines[0].Split(',');

        List<string> itemNames = new List<string>();
        
        for (int i = 0; i < v.Length; i++)
        {
               itemNames.Add(v[i]);
        }


        for (int i = 1; i < lines.Length - 1; i++)
        {
            Dictionary<string, int> itemValues = new Dictionary<string, int>();
            string[] values = lines[i].Split(',');
            for (int j = 1; j < values.Length ; j++)
            {
                itemValues.Add(itemNames[j], int.Parse(values[j]));
            }
            dic.Add(values[0], itemValues);
        }

        return dic;
    }
    
    public T ReadJsonFromResources<T>(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path + fileName);

        return JsonConvert.DeserializeObject<T>(textAsset.ToString());
    }

    public T ReadJsonFromServer<T>(string columnName) where T : new() //수정
    {
        //JObject json = BackendManager.Instance.ReadPlayerData<T>("Player_Info", columnName);
        var result = BackendManager.Instance.ReadPlayerData<T>("Player_Info", columnName);
        return result == null ? new T() : result;
    }
    
    public T ReadJsonFromLocal<T>(string fileName) where T : new()
    {
        if (File.Exists(filePath + "/" + fileName + ".json"))
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
    
    public void WriteJsonToServer<T>(T updateData, string columnName) //수정
    {
        BackendManager.Instance.UpdatePlayerData("Player_Info", updateData, columnName);
    }
    
    public void WriteJsonToLocal<T>(T input, string fileName)
    {
        string json = JsonConvert.SerializeObject(input);
        JObject jobject = JObject.Parse(json);

        File.WriteAllText(filePath + "/" + fileName, jobject.ToString());
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

        path = "Data/";
    }
}



// 암호화 O

/*public class IOManager : MonoBehaviour
{
    public static string key = "7ZWY66OoIOyiheydvCDsnpDqs6Dsi7bs"; //32byte ��&��ȣŰ

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
            {
                string dncText = DecryptAES(file.ReadToEnd(), key);
                JObject json = JObject.Parse(dncText);
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

        string encJobject = EncryptAES(jobject.ToString(), key);
        File.WriteAllText(filePath + "/" + fileName, encJobject);
    }

    public void ReadPlayerSettings()
    {
        if (System.IO.File.Exists(filePath + "/playerSettings.json"))
        {
            using (StreamReader file = File.OpenText(filePath + "/playerSettings.json"))
            {
                string dncText = DecryptAES(file.ReadToEnd(), key);
                JObject json = JObject.Parse(dncText);
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

        string encJobject = EncryptAES(jobject.ToString(), key);
        File.WriteAllText(filePath + "/playerSettings.json", encJobject);
    }

    //��ȣȭ EncryptAES(��ȣȭ �� �ؽ�Ʈ, Ű ��)
    public static string EncryptAES(string jsonText, String key)
    {
        RijndaelManaged aes = new RijndaelManaged();
        aes.Mode = CipherMode.CBC;
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Padding = PaddingMode.PKCS7;
        aes.IV = Encoding.UTF8.GetBytes(key.Substring(0, 16)); //IV�� key�� �� 16byte�� �缳��

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[32];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);
        aes.Key = keyBytes;

        ICryptoTransform transform = aes.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(jsonText);
        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }

    //��ȣȭ DecryptAES(��ȣȭ �� �ؽ�Ʈ, Ű ��)
    public static string DecryptAES(string encText, String key)
    {
        RijndaelManaged aes = new RijndaelManaged();
        aes.Mode = CipherMode.CBC;
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Padding = PaddingMode.PKCS7;
        aes.IV = Encoding.UTF8.GetBytes(key.Substring(0, 16));

        byte[] encryptedData = Convert.FromBase64String(encText);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[32];

        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);

        aes.Key = keyBytes;

        byte[] plainText = aes.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(plainText);
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
}*/
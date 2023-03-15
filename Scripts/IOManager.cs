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


public class PlayerSettings
{
    public int gold;
    public bool isTutorialCleared;
}

// 암호화 X

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
            playerSettings = new PlayerSettings();
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

        path = "Data/";

        ReadPlayerSettings();
    }
}



// 암호화 O

/*public class IOManager : MonoBehaviour
{
    public static readonly string privateKey = "123";
    public static string key = "7ZWY66OoIOyiheydvCDsnpDqs6Dsi7bs"; //32byte 암&복호키

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

    //암호화 EncryptAES(암호화 할 텍스트, 키 값)
    public static string EncryptAES(string jsonText, String key)
    {
        RijndaelManaged aes = new RijndaelManaged();
        aes.Mode = CipherMode.CBC;
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Padding = PaddingMode.PKCS7;
        aes.IV = Encoding.UTF8.GetBytes(key.Substring(0, 16)); //IV는 key의 앞 16byte로 재설정

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

    //복호화 DecryptAES(복호화 할 텍스트, 키 값)
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
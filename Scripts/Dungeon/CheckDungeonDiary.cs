using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CheckDungeonDiary : MonoBehaviour
{
    public string dungeonName;
    void Start()
    {
        string tempStr = GameManager.instance.dungeonDiary.Find(item => item.Equals(dungeonName));

        if (tempStr != null) return;
        else GameManager.instance.dungeonDiary.Add(dungeonName);
        
        GameManager.instance.WriteDiary();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDiscoverTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other) // 트리거 됐을 때
    {
        if (other.tag == "monster")
        {
            string temp = other.name.Substring(1, other.name.Length - 1);
            temp = char.ToLower(other.name[0]) + temp;

            string[] monsterName = temp.Split(' '); 
            
            string tempStr = GameManager.instance.monsterDiary.Find(item => item.Equals(monsterName[0]));

            if (tempStr != null) return;
            else
            {
                GameManager.instance.monsterDiary.Add(monsterName[0]);
                Destroy(this.gameObject.GetComponent<MonsterDiscoverTrigger>());
            }
            
            GameManager.instance.WriteDiary();
        }
    }
}
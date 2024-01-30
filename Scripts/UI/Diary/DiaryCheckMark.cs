using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DiaryCheckMark : MonoBehaviour
{
    public GameObject diaryCheckMark;
    public GameObject resButtonCheckMark;

    public void UpdateCheckMark()
    {
        if(GameManager.instance.residentFriendships != null)
        {
            foreach (var item in GameManager.instance.residentFriendships)
            {
                if (!item.Value.isAlerted)
                {
                    diaryCheckMark.SetActive(false);
                    resButtonCheckMark.SetActive(false);
                }
                else
                {
                    diaryCheckMark.SetActive(true);
                    resButtonCheckMark.SetActive(true);
                    return;
                }
            }
        }
    }
    
    void Start()
    {
        UpdateCheckMark();
    }
}

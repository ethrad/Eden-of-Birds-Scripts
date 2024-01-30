using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByoRoomController : MonoBehaviour
{
    public enum RoomType { NorthWest, SouthWest, NorthEast, SouthEast}
    
    // 북서, 남서, 북동, 남동 순서
    public GameObject roofs; // 나무로 막힌 길
    public GameObject path; // 진짜 길
    public GameObject path2; // 겹치는 길
    
    public GameObject cameraBound;
    public GameObject byodyguardOnCenter;
    public GameObject[] byodyguardOnPath;
    
    public void SetRoom(Vector3 pos, RoomType t)
    {
        transform.position = pos;
        
        switch (t)
        {
            case RoomType.NorthWest:
                roofs.transform.GetChild(0).gameObject.SetActive(true);
                roofs.transform.GetChild(1).gameObject.SetActive(true);
                roofs.transform.GetChild(2).gameObject.SetActive(true);
                
                path.transform.GetChild(3).gameObject.SetActive(true);
                path2.transform.GetChild(3).gameObject.SetActive(true);
                break;
            
            case RoomType.SouthWest:
                roofs.transform.GetChild(0).gameObject.SetActive(true);
                roofs.transform.GetChild(1).gameObject.SetActive(true);
                roofs.transform.GetChild(3).gameObject.SetActive(true);
                
                path.transform.GetChild(2).gameObject.SetActive(true);
                path2.transform.GetChild(2).gameObject.SetActive(true);
                break;
            
            case RoomType.NorthEast:
                roofs.transform.GetChild(0).gameObject.SetActive(true);
                roofs.transform.GetChild(2).gameObject.SetActive(true);
                roofs.transform.GetChild(3).gameObject.SetActive(true);
                
                path.transform.GetChild(1).gameObject.SetActive(true);
                path2.transform.GetChild(1).gameObject.SetActive(true);
                break;
            
            case RoomType.SouthEast:
                
                roofs.transform.GetChild(1).gameObject.SetActive(true);
                roofs.transform.GetChild(2).gameObject.SetActive(true);
                roofs.transform.GetChild(3).gameObject.SetActive(true);
                
                path.transform.GetChild(0).gameObject.SetActive(true);
                path2.transform.GetChild(0).gameObject.SetActive(true);
                break;
        }
    }
    
    
    public void OpenRoom()
    {
        foreach(var byodyguard in byodyguardOnPath)
        {
            byodyguard.SetActive(false);
        }
        
        byodyguardOnCenter.SetActive(true);
    }

}

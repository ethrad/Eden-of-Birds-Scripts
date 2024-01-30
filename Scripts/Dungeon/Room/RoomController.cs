using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using Random = UnityEngine.Random;

public class RoomController : MonoBehaviour
{
    public enum RoomType { Start, Top, Left, Right, End }
    
    private RoomType roomType;
    private int height;
    
    // 북서, 남서, 북동, 남동 순서
    public GameObject roofs; // 나무로 막힌 길
    public GameObject path; // 진짜 길
    public GameObject path2; // 겹치는 길

    public GameObject lastRoomRoofs;
    public GameObject bossRoomRoofs;

    public GameObject cameraBound;
    
    public void SetRoom(int height, RoomType t)
    {
        roomType = t;
        this.height = height;
        
        switch (t)
        {
            case RoomType.Top:
                transform.position = new Vector3(0, height * 7.04f, 0);
                path2.transform.GetChild(1).gameObject.SetActive(true);
                path2.transform.GetChild(3).gameObject.SetActive(true);
                
                path.transform.GetChild(0).gameObject.SetActive(true);
                path.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case RoomType.Left:
                transform.position = new Vector3(-3.52f, height * 7.04f + 3.52f, 0);
                roofs.transform.GetChild(0).gameObject.SetActive(true);
                roofs.transform.GetChild(1).gameObject.SetActive(true);
                
                path.transform.GetChild(2).gameObject.SetActive(true);
                
                path2.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case RoomType.Right:
                transform.position = new Vector3(3.52f, height * 7.04f + 3.52f, 0);
                roofs.transform.GetChild(2).gameObject.SetActive(true);
                roofs.transform.GetChild(3).gameObject.SetActive(true);
                
                path.transform.GetChild(0).gameObject.SetActive(true);
                
                path2.transform.GetChild(1).gameObject.SetActive(true);
                break;
            
            case RoomType.Start:
                transform.position = new Vector3(0, 0, 0);
                roofs.transform.GetChild(1).gameObject.SetActive(true);
                roofs.transform.GetChild(3).gameObject.SetActive(true);
                
                path.transform.GetChild(0).gameObject.SetActive(true);
                path.transform.GetChild(2).gameObject.SetActive(true);
                break;
            
            case RoomType.End:
                transform.position = new Vector3(0, height * 7.04f, 0);
                
                if (DungeonManager.instance.floor == 2)
                {
                    bossRoomRoofs.SetActive(true);
                }
                else
                {
                    lastRoomRoofs.SetActive(true);
                }
                
                path2.transform.GetChild(1).gameObject.SetActive(true);
                path2.transform.GetChild(3).gameObject.SetActive(true);
                break;
        }
    }

    public GameObject MakeByoRoom()
    {
        // 0은 위, 1은 아래 방 생성
        int random = Random.Range(0, 2);
        GameObject tempByoRoom = Instantiate(DungeonManager.instance.byoRoomPrefab);

        Vector3 tempPos = default(Vector3);
        ByoRoomController.RoomType tempRoomType = ByoRoomController.RoomType.NorthWest;
        int pathNum = 0;
        
        switch (roomType)
        {
            case RoomType.Left:
                if (random == 0)
                {
                    pathNum = 0;
                    tempPos = transform.position + new Vector3(-3.52f, 3.52f, 0);
                    tempRoomType = ByoRoomController.RoomType.NorthWest;
                }
                else
                {
                    pathNum = 1;
                    tempPos = transform.position + new Vector3(-3.52f, -3.52f, 0);
                    tempRoomType = ByoRoomController.RoomType.SouthWest;
                }
                break;
            
            case RoomType.Right:
                if (random == 0)
                {
                    pathNum = 2;
                    tempPos = transform.position + new Vector3(3.52f, 3.52f, 0);
                    tempRoomType = ByoRoomController.RoomType.NorthEast;
                    
                }
                else
                {
                    pathNum = 3;
                    tempPos = transform.position + new Vector3(3.52f, -3.52f, 0);
                    tempRoomType = ByoRoomController.RoomType.SouthEast;
                }
                break;
        }
        
        roofs.transform.GetChild(pathNum).gameObject.SetActive(false);
        path.transform.GetChild(pathNum).gameObject.SetActive(true);
        tempByoRoom.GetComponent<ByoRoomController>().SetRoom(tempPos, tempRoomType);

        return tempByoRoom;
    }
}

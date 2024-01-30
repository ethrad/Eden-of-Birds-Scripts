using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class TownFloorPortal : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> point;
    public GameObject sunmuRoom;
    public GameObject firstPos;

    public GameObject minimapRawImageTexture;
    public List<RenderTexture> textureList;
    
    public GameObject fullScreenMiniMap;

    public void Start()
    {
        if (!GameManager.instance.clearedQuests.TryGetValue("owl", out List<int> value))
        {//순무 올빼미 앞
            TownManager.instance.player.transform.position = firstPos.transform.position;
            MinimapTextureSetting(1);
        }
        else
        {//순무 방 앞
            TownManager.instance.player.transform.position = sunmuRoom.transform.position;
            MinimapTextureSetting(2);
        }
        
        player.transform.position = point[1].transform.position;
        SetCameraBound(1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            if(other.gameObject.name == "Stair1")
            {
                MinimapTextureSetting(1);
                return;
            }
            if (other.gameObject.name == "Stair2")
            {
                MinimapTextureSetting(2);
                return;
            }
            StartCoroutine(Teleport(other));
        }
    }

    void MinimapTextureSetting(int textureNum)
    {
        minimapRawImageTexture.GetComponent<RawImage>().texture = textureList[textureNum];
    }

    void TeleportPlayer(Collider2D other)
    {
        switch (other.gameObject.name)
        {
            case "0": //플레이어 2층으로 이동
                TownManager.instance.player.transform.position = TownManager.instance. 
                    portals[1].transform.position;
                //미니맵
                player.transform.position = point[1].transform.position;
                MinimapTextureSetting(1);
                //버추얼 카메라
                SetCameraBound(1);
                break;
            case "1": //플레이어 1층으로 이동
                TownManager.instance.player.transform.position = TownManager.instance. 
                    portals[0].transform.position;
                //미니맵
                player.transform.position = point[0].transform.position;
                MinimapTextureSetting(0);
                //버추얼 카메라
                SetCameraBound(0);
                break;
            case "2": //플레이어 3층으로 이동
                TownManager.instance.player.transform.position = TownManager.instance. 
                    portals[3].transform.position;
                //미니맵
                player.transform.position = point[2].transform.position;
                MinimapTextureSetting(3);
                //버추얼 카메라
                SetCameraBound(2);
                break; 
            case "3": //플레이어 2층(2층에 2층)으로 이동
                TownManager.instance.player.transform.position = TownManager.instance. 
                    portals[2].transform.position;
                //미니맵
                player.transform.position = point[1].transform.position;
                MinimapTextureSetting(2);
                //버추얼 카메라
                SetCameraBound(1);
                break;
            default: return;
        }
    }
    
    IEnumerator Teleport(Collider2D other)
    {
        TownManager.instance.OffBasePanel();
        GameManager.instance.FadeOutEffect();
        while (!GameManager.instance.isFadeOutEnd)
        {
            yield return null;
        }
        GameManager.instance.FadeInEffect();
        GameManager.instance.ResetFadeFlags();
        TownManager.instance.OnBasePanel();
        TeleportPlayer(other);
        fullScreenMiniMap.SetActive(false);
    }
    
    public void SetCameraBound(int floorNum)
    {
        TownManager.instance.virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D 
            = TownManager.instance.floorBounds[floorNum].GetComponent<Collider2D>();
    }
}

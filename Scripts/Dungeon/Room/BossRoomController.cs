using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomController : MonoBehaviour
{
    public GameObject playerSpawnPoint;
    public GameObject eagleSpawnPoint;
    public GameObject cameraBound;
    public AudioClip audioBossRoomBGM;
    public GameObject kingSlimeAnimationSprite;
    
    public void SetRoom()
    {
        DungeonManager.instance.player.transform.position = playerSpawnPoint.transform.position;
        DungeonManager.instance.eagle.transform.position = eagleSpawnPoint.transform.position;
        DungeonManager.instance.eagle.SetActive(false);
        DungeonManager.instance.virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = cameraBound.GetComponent<Collider2D>();
        DungeonManager.instance.virtualCamera.transform.position = DungeonManager.instance.player.transform.position;
        DungeonManager.instance.camera.transform.position = DungeonManager.instance.player.transform.position;
        
        // bgmµµ πŸ≤„¡‡æﬂ«‘
    }

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void DestroyDirectionAnimation()
    {
        Time.timeScale = 1;
        Destroy(gameObject.GetComponent<Animator>());
        Destroy(kingSlimeAnimationSprite);
    }
}

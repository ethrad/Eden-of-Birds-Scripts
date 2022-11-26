using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomController : MonoBehaviour
{
    #region Initialize
    public GameObject[] monsterPrefabs;
    public int[] monsterNums;

    public GameObject monsterSpawnAreas;
    public GameObject playerSpawner;

    public GameObject bound;

    Vector3 GetRandomPosition()
    {
        int areaNum = 0;

        float randomX;
        float randomY;

        areaNum = Random.Range(0, monsterSpawnAreas.transform.childCount);
        GameObject selectedSpawnArea = monsterSpawnAreas.transform.GetChild(areaNum).gameObject;

        float dungeonCenterX = selectedSpawnArea.transform.position.x;
        float dungeonCenterY = selectedSpawnArea.transform.position.y;

        float dungeonRangeX = selectedSpawnArea.transform.localScale.x / 2;
        float dungeonRangeY = selectedSpawnArea.transform.localScale.y / 2;

        randomX = Random.Range(dungeonCenterX - dungeonRangeX, dungeonCenterX + dungeonRangeX);
        randomY = Random.Range(dungeonCenterY - dungeonRangeY, dungeonCenterY + dungeonRangeY);


        return new Vector3(randomX, randomY, 0);
    }

    void SpawnMonster()
    {
        for (int i = 0; i < monsterPrefabs.Length; i++)
        {
            for (int j = 0; j < monsterNums[i]; j++)
            {
                GameObject tempMonster = Instantiate(monsterPrefabs[i], GetRandomPosition(), Quaternion.Euler(0, 0, 0));
                //tempMonster.GetComponent<MonsterController>().monsterIndex = i;
            }
        }

        monsterSpawnAreas.SetActive(false);
    }

    void SpawnPlayer()
    {
        DungeonManager.instance.player.transform.position = playerSpawner.transform.position;
    }

    #endregion

    public GameObject eagle;
    public GameObject nextRoomPortal;

    public void ClearRoom(bool isDungeonCleared)
    {
        eagle.SetActive(true);

        if (isDungeonCleared == false)
        {
            nextRoomPortal.SetActive(true);
        }
    }

    public void Initialize()
    {
        SpawnMonster();
        SpawnPlayer();

        DungeonManager.instance.virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = bound.GetComponent<CompositeCollider2D>();
    }
}

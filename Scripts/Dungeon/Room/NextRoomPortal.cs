using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoomPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            DungeonManager.instance.StopPlayerMoving();
            DungeonManager.instance.MoveToNextFloor();
        }
    }
}

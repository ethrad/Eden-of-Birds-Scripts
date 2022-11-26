using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoomPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DungeonManager.instance.ClearRoom();
    }
}

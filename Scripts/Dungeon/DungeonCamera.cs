using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DungeonCamera : MonoBehaviour
{
    GameObject player;
    public float offsetX;
    public float offsetY;

    /*    public BoxCollider2D bound;

        private Vector3 minBound;
        private Vector3 maxBound;

        private float halfWidth;
        private float halfHeight;

        private Camera theCamera;*/


    Vector3 cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = DungeonManager.instance.player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cameraPosition.x = player.transform.position.x + offsetX;
        cameraPosition.y = player.transform.position.y + offsetY;
        cameraPosition.z = player.transform.position.z - 10;

        transform.position = Vector3.Lerp(transform.position, cameraPosition, 5f * Time.deltaTime);
    }
}

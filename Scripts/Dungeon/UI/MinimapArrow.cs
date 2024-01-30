using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapArrow : MonoBehaviour
{
    public GameObject player;
    public Transform eagle;
    public Transform arrow;
    public float arrowRadius;

    Vector3 dir;
    
    void Update()
    {
        Transform playerTransform = player.transform;
        dir = (eagle.position - playerTransform.position).normalized * arrowRadius;

        float angle = Mathf.Atan2(eagle.position.y - playerTransform.position.y,
            (eagle.position.x - playerTransform.position.x)*-1) * Mathf.Rad2Deg;

        arrow.transform.rotation = Quaternion.Euler(0, 0, -angle);
        arrow.transform.position = dir + player.transform.position;
    }
}

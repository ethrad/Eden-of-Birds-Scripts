using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PubTrigger : MonoBehaviour
{
    public GameObject pub;
    public GameObject eagle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            pub.SetActive(false);
            eagle.SetActive(true);
        }
    }
}

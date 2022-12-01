using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageHallTrigger : MonoBehaviour
{

    public GameObject villageHall;
    public GameObject pub;

    public void StartCrowTitAndOwlConversation()
    {
        villageHall.GetComponent<BoxCollider2D>().enabled = false;
        pub.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") StartCrowTitAndOwlConversation();
    }

}

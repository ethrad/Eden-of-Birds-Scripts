using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitRecognitionController : MonoBehaviour
{
    private GameObject Monster;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            Monster.GetComponent<MonsterController>().state = MonsterController.State.Follow;
            Monster.GetComponent<RabbitController>().canCopy = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            Monster.GetComponent<MonsterController>().state = MonsterController.State.Idle;
            Monster.GetComponent<RabbitController>().canCopy = false;
        }
    }

    void Awake()
    {
        Monster = gameObject.transform.parent.transform.gameObject;
        transform.position = Monster.transform.position;
    }
}

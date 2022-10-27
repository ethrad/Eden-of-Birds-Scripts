using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognitionController : MonoBehaviour
{
    private GameObject Monster;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            Monster.GetComponent<MonsterController>().state = MonsterController.State.Follow;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            Monster.GetComponent<MonsterController>().state = MonsterController.State.Idle;
        }
    }

    void Awake()
    {
        Monster = gameObject.transform.parent.transform.gameObject;
    }
}

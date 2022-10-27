using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonsterController
{
    public GameObject rabbitPrefab;
    public bool isOriginal = true;
    public bool canCopy = true;
    float copyDelay = 3f;
    float copyTimer = 0f;
    
    protected override void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            state = State.Follow;
        }

    }


    void Copy()
    {
        if (copyTimer > copyDelay)
        {
            if (canCopy == true)
            {
                int randX = Random.Range(0, 2);
                int randY = Random.Range(0, 2);

                if (randX == 0)
                    randX = -1;
                else randX = 1;

                if (randY == 0)
                    randY = -1;
                else randY = 1;

                Vector3 pos = transform.position + new Vector3(Random.Range(0.6f, 1f) * randX, Random.Range(0.6f, 1f) * randY, 0);

                GameObject copiedRabbit = Instantiate(rabbitPrefab, pos, Quaternion.Euler(0, 0, 0));
                copiedRabbit.GetComponent<RabbitController>().isOriginal = false;

                copyTimer = 0;
            }
        }

        copyTimer += Time.deltaTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                break;

            case State.Walk:
                Walk();
                break;

            case State.Follow:
                Follow();
                break;

            case State.Attack:
                Attack();
                Copy();
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonsterController
{
    public GameObject rabbitPrefab;
    public bool isOriginal = true;
    public bool canCopy;
    public float copyDelay;
    float copyTimer = 0f;

    //±¸ ¹Ý°æ
    public float maxRadiusScale;

    RaycastHit hit;

    void Copy()
    {
        if (isOriginal == true && canCopy == true)
        {
            if (copyTimer > copyDelay)
            {
                bool isHit = true;
                Vector3 pos = new Vector3(0, 0, 0);
                float radiusScale = 0;

                while (isHit)
                {
                    pos = Random.insideUnitSphere.normalized;
                    radiusScale = Random.Range(0.2f, maxRadiusScale);

                    isHit = Physics.SphereCast(transform.position, radiusScale, pos, out hit);
                }

                GameObject copiedRabbit = Instantiate(rabbitPrefab, transform.position + pos * radiusScale, Quaternion.Euler(0, 0, 0));
                copiedRabbit.GetComponent<RabbitController>().isOriginal = false;

                DungeonManager.instance.monsterCount++;
                copyTimer = 0;
            }
        }

        copyTimer += Time.deltaTime;
    }

    protected override void Dead()
    {
        if (isOriginal == true)
        {
            int random = Random.Range(0, 100);

            int itemIndex = -1;
            int tempProb = 0;

            for (int i = 0; i < itemProb.Length; i++)
            {
                if (random <= itemProb[i] + tempProb)
                {
                    itemIndex = i;
                }
                else break;

                tempProb += itemProb[i];
            }

            if (itemIndex != -1)
            {
                Instantiate(itemArray[itemIndex], transform.position, Quaternion.Euler(0, 0, 0));
            }
        }

        DungeonManager.instance.UpdateMonsterCount();
        Destroy(gameObject);
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

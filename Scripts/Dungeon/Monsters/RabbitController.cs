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

    //�� �ݰ�
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

                GameObject copiedRabbit = Instantiate(rabbitPrefab, gameObject.transform.parent.transform);
                copiedRabbit.transform.position = transform.position + pos * radiusScale;
                copiedRabbit.GetComponent<RabbitController>().isOriginal = false;
                
                copyTimer = 0;
            }
        }

        copyTimer += Time.deltaTime;
    }

    protected override void Dead()
    {
        if (isOriginal == true)
        {
            base.Dead();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
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

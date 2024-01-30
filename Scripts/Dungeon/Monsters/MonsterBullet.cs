using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : Bullet
{
    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Object"))
        {
            DungeonManager.instance.ReturnObject(poolingObjectName, gameObject);
        }

        if (col.gameObject.CompareTag(targetTag))
        {
            col.gameObject.GetComponent<PlayerController>().Damaged(damage);
            DungeonManager.instance.ReturnObject(poolingObjectName, gameObject);
        }
    }

    // 위치 지정, 데미지 지정, 플레이어 방향으로 향하도록 설정
    public void Initialize(Vector3 pos, int damage)
    {
        this.damage = damage;
        transform.position = pos;
        dir = (DungeonManager.instance.player.transform.position - transform.position).normalized;
        StartCoroutine(DestroySelf());
    }
}

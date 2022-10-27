using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : Bullet
{
    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Object")
        {
            Destroy(this.gameObject);
        }

        if (col.gameObject.tag == targetTag)
        {
            col.gameObject.GetComponent<PlayerController>().Damaged(damage);
        }
    }

    public override void Initialize(Vector3 pos)
    {
        transform.position = pos;
        dir = DungeonManager.instance.player.transform.position - pos;
        dir = dir.normalized;
        StartCoroutine(DestroySelf());
    }
}

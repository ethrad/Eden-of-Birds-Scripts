using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Object")
        {
            Destroy(this.gameObject);
        }

        if (col.gameObject.tag == targetTag)
        {
            col.gameObject.GetComponent<MonsterController>().Damaged(damage);
        }
    }

    public override void Initialize(Vector3 dir)
    {
        transform.position = DungeonManager.instance.player.transform.position;
        this.dir = dir.normalized;
        StartCoroutine(DestroySelf());
    }
}

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

        if (col.gameObject.tag == targetTag && !col.isTrigger)
        {
            col.gameObject.GetComponent<MonsterController>().Damaged(damage);
            Destroy(this.gameObject);
        }
    }

    public override void Initialize(Vector3 dir)
    {
        Vector3 tempVector = DungeonManager.instance.player.transform.position;
        transform.position = new Vector3(tempVector.x, tempVector.y, tempVector.z);
        this.dir = dir.normalized;
        StartCoroutine(DestroySelf());
    }
}

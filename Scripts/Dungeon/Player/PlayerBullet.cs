using BossMonster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class PlayerBullet : Bullet
    {
        public override void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Object"))
            {
                Destroy(gameObject);
            }

            if (col.gameObject.CompareTag(targetTag) && !col.isTrigger)
            {
                col.gameObject.GetComponent<MonsterController>().Damaged(gameObject);
                Destroy(gameObject);
            }

            if (col.gameObject.CompareTag("BossMonster"))
            {
                col.gameObject.GetComponent<BossMonsterController>().Damaged(gameObject);
                Destroy(gameObject);
            }
        }

        public override void Initialize(Vector3 dir)
        {
            damage = DungeonManager.instance.player.GetComponent<PlayerController>().ATK;

            transform.position = DungeonManager.instance.player.transform.position;
            this.dir = dir.normalized;
            StartCoroutine(DestroySelf());
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMonster
{
    public class BossBullet : MonsterBullet
    {
        public override void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Wall"))
            {
                BossMonsterController.instance.ReturnObject(poolingObjectName, gameObject);
            }

            if (col.gameObject.CompareTag(targetTag))
            {
                col.gameObject.GetComponent<PlayerController>().Damaged(damage);
                BossMonsterController.instance.ReturnObject(poolingObjectName, gameObject);
            }
        }

        protected override IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(destroyTime);
            BossMonsterController.instance.ReturnObject(poolingObjectName, gameObject);

            yield break;
        }
    }
}
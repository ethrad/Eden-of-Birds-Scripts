using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
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
        }
        
        protected override IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(destroyTime);
            TutorialDungeonManager.instance.ReturnObject(poolingObjectName, gameObject);
        
            yield break;
        }

        public override void Initialize(Vector3 dir)
        {
            damage = TutorialDungeonManager.instance.player.GetComponent<PlayerController>().ATK;

            transform.position = TutorialDungeonManager.instance.player.transform.position;
            this.dir = dir.normalized;
            StartCoroutine(DestroySelf());
        }
    }
}

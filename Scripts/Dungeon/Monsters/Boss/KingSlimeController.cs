using Diary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BossMonster;

namespace KingSlime
{
    public class KingSlimeController : BossMonsterController
    {
        // Å« À¯µµÅº
        protected override IEnumerator ShootingPattern1(AttackPatternNode node)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1.3f);
            var bullet = GetObject("GuidedBigBullet");
            bullet.GetComponent<Bullet>().Initialize((target.transform.position - transform.position).normalized * 0.45f);

            anim.SetTrigger("Idle");
            yield return new WaitForSeconds(0.3f);
            bullet.GetComponent<GuidedBigBullet>().canShoot = true;
            atttackAudioSource.Play();
            
            yield return new WaitForSeconds(5f + node.attackDelay);
            
            DecideNextNode(node);
            
            yield break;
        }

        // ºÎÃ¤²Ã Åº¸·
        protected override IEnumerator ShootingPattern2(AttackPatternNode node)
        {
            for (int i = 0; i < 16; i++)
            {
                var bullet = GetObject("StraightBullet");
                var tempDir = (Quaternion.AngleAxis(360 / 16 * i, Vector3.forward) * Vector3.up).normalized;
                bullet.GetComponent<StraightBullet>().Initialize(transform.position + tempDir * 0.43f, tempDir);
            }
            
            atttackAudioSource.Play();
            yield return new WaitForSeconds(node.attackDelay);
            
            DecideNextNode(node);
            
            yield break;
        }

        // À¯µµÅº 8°³ ¿¬»ç
        protected override IEnumerator ShootingPattern3(AttackPatternNode node)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(0.3f);
            
            for (int i = 0; i < 4; i++)
            {
                var playerDir = target.transform.position - transform.position;
                var bulletDir1 = (Quaternion.AngleAxis(-20, Vector3.forward) * playerDir).normalized;
                var bulletDir2 = (Quaternion.AngleAxis(20, Vector3.forward) * playerDir).normalized;
                
                var bullet1 = GetObject("GuidedSmallBullet");
                bullet1.GetComponent<GuidedSmallBullet>().Initialize(bulletDir1 * 0.4f);
                
                var bullet2 = GetObject("GuidedSmallBullet");
                bullet2.GetComponent<GuidedSmallBullet>().Initialize(bulletDir2 * 0.4f);
                
                atttackAudioSource.Play();
                
                yield return new WaitForSeconds(0.5f);
            }
            
            anim.SetTrigger("Idle");

            yield return new WaitForSeconds(node.attackDelay);
            
            DecideNextNode(node);
            
            yield break;
        }
    }
}

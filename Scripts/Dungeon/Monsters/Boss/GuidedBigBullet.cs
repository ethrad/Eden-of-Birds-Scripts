using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BossMonster;

namespace KingSlime
{
    public class GuidedBigBullet : BossBullet
    {
        // ��ġ�� ����
        
        protected override void FixedUpdate()
        {
            if (canShoot)
            {
                transform.position = Vector3.MoveTowards(transform.position, DungeonManager.instance.player.transform.position, moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

}

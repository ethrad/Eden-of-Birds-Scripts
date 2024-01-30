using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BossMonster;

namespace KingSlime
{
    public class GuidedSmallBullet : BossBullet
    {
        // ��ġ ����, �÷��̾� �������� ���ϵ��� ����
        public override void Initialize(Vector3 pos)
        {
            transform.position = pos;
            dir = (DungeonManager.instance.player.transform.position - transform.position).normalized;
            StartCoroutine(DestroySelf());
        }
    }

}

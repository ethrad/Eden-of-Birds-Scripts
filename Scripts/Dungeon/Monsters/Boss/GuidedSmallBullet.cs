using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BossMonster;

namespace KingSlime
{
    public class GuidedSmallBullet : BossBullet
    {
        // 위치 지정, 플레이어 방향으로 향하도록 설정
        public override void Initialize(Vector3 pos)
        {
            transform.position = pos;
            dir = (DungeonManager.instance.player.transform.position - transform.position).normalized;
            StartCoroutine(DestroySelf());
        }
    }

}

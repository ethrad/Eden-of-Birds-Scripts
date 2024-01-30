using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BossMonster;

namespace KingSlime
{
    public class StraightBullet : BossBullet
    {
        // ��ġ, ���� ����
        public void Initialize(Vector3 pos, Vector3 dir)
        {
            canShoot = false;
            transform.position = pos;
            this.dir = dir;
            StartCoroutine(DestroySelf());
        }
    }
}
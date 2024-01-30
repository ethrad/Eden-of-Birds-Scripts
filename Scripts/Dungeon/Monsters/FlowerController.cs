using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonsterController
{
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            state = State.Attack;
        }
    }

    protected override void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            state = State.Idle;
        }

    }

    protected override void Idle()
    {
        anim.SetBool("Idle", true);
    }
   
    public GameObject Bullet;


    // 애니메이션 트리거로 바꿔서 공격할 때만 모션 나오게
    protected override void Attack()
    {
        anim.SetBool("Idle", false);
        if (canAttack == true)
        {
            GameObject bullet = DungeonManager.instance.GetObject("MonsterBullet");
            bullet.transform.SetParent(transform);
            bullet.GetComponent<MonsterBullet>().Initialize(transform.position, ATK);
            canAttack = false;
            StartCoroutine(AttackDelay());
            anim.SetTrigger("Attack");
        }
    }


    Vector3 targetDir;


    protected override void Start()
    {
        isFixed = true;
        damagedState = DamagedState.Idle;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.settings.soundEffectsVolume;
        target = DungeonManager.instance.player;
        HP = maxHP = maxHPArr[DungeonManager.instance.floor];
        ATK = ATKArr[DungeonManager.instance.floor];
        canAttack = true;
    }
    
    protected override void FixedUpdate()
    {
        targetDir = target.transform.position - transform.position;
        anim.SetFloat("TargetX", targetDir.x);

        switch (state)
        {
            case State.Idle:
                Idle();
                break;

            case State.Walk:
                Walk();
                break;

            case State.Follow:
                Follow();
                break;

            case State.Attack:
                Attack();
                break;
        }

    }
}

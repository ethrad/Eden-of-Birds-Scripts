using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class SlimeController : MonsterController
    {
        protected override void Attack()
        {
            canWalk = false;
            if (canAttack)
            {
                if (isPlayerInAttackArea)
                {
                    target.GetComponent<PlayerController>().Damaged(ATK);
                }
                audioSource.clip = audioAttack;
                audioSource.Play();
                anim.SetTrigger("Attack");
                canAttack = false;
                canWalk = false;
            
                StartCoroutine(AttackDelay());
            }
        }
        
        
        protected override void Dead()
        {
            audioSource.clip = audioDead;
            audioSource.Play();
        
            int random = Random.Range(0, 100);

            int itemIndex = -1;
            int tempProb = 0;

            for (int i = 0; i < itemProb.Length; i++)
            {
                if (random < itemProb[i] + tempProb)
                {
                    itemIndex = i;
                    break;
                }

                tempProb += itemProb[i];
            }

            if (itemIndex != -1)
            {
                GameObject tempItem = Instantiate(TutorialDungeonManager.instance.dropItemPrefab, transform.position + Vector3.up * 0.06f, Quaternion.Euler(0, 0, 0));
                tempItem.GetComponent<ItemController>().Initialize(itemNames[itemIndex]);
            }

            Destroy(gameObject);

        }
        
        protected override void Start()
        {
            target = TutorialDungeonManager.instance.player;
            state = State.Idle;
            damagedState = DamagedState.Idle;

            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = GameManager.instance.settings.soundEffectsVolume;

            canAttack = true;

            HP = maxHP = maxHPArr[0];
            ATK = ATKArr[0];

            isFixed = false;
        }
    }
}
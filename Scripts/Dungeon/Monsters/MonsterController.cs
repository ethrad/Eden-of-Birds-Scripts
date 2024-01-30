using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    // Monster State 관리
    public enum State { Idle, Walk, Follow, Attack, Dead };
    protected enum DamagedState { Idle, Damaged };

    public State state;
    protected DamagedState damagedState;

    protected GameObject target; // 플레이어

    public int[] maxHPArr; // 최대 체력
    protected int maxHP;
    protected int HP; // 현재 체력

    public int[] ATKArr;
    protected int ATK; // 공격력
    public float attackDelay; // 공격속도

    Vector3 dir; // 이동 방향
    public float moveSpeed; // 이동 속도
    public bool isFixed; // 고정된 몬스터인지

    // 애니메이션
    protected Animator anim;
    
    protected AudioSource audioSource;
    public AudioClip audioDamaged;
    public AudioClip audioAttack;
    public AudioClip audioDead;

    protected bool canWalk = true;


    #region collider
    // 안쪽 콜라이더

    protected bool isPlayerInAttackArea = false;

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            isPlayerInAttackArea = true;
            state = State.Attack;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            isPlayerInAttackArea = false;
            state = State.Follow;
        }

    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            state = State.Idle;
        }
        if (col.gameObject.CompareTag("player"))
        {
            canWalk = false;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            canWalk = true;
        }
    }

    #endregion

    #region Idle Walk Follow
    protected virtual void Idle()
    {
        StartCoroutine(TurnWalk());
        anim.SetBool("Idle", true);
    }
    
    protected IEnumerator TurnWalk()
    {
        yield return new WaitForSeconds(2f);

        if (state == State.Idle)
        {
            dir = Random.insideUnitCircle.normalized;
            dir = dir.normalized;
            state = State.Walk;
            StartCoroutine(TurnIdle());
            yield break;
        }
        else
        {
            yield break;
        }
    }

    protected void Walk()
    {
        if (canWalk)
        {
            transform.position += dir * (Time.fixedDeltaTime * moveSpeed);

            if (transform.GetChild(0) != null)
            {
                transform.GetChild(0).localPosition = Vector3.zero;
            }
            anim.SetFloat("MoveX", dir.x);
            anim.SetBool("Idle", false);
        }
    }

    protected IEnumerator TurnIdle()
    {
        yield return new WaitForSeconds(3f);

        if (state == State.Walk)
        {
            state = State.Idle;
            yield break;
        }
        else
        {
            yield break;
        }
    }

    protected void Follow()
    {
        dir = target.transform.position - transform.position;
        dir = dir.normalized;
        Walk();
    }

    #endregion


    #region Attack

    protected bool canAttack;

    protected virtual void Attack()
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


    protected IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);

        canAttack = true;
        canWalk = true;
        yield break;
    }

    #endregion

    #region Damaged

    protected SpriteRenderer spriteRenderer;
    public Image HPBar;
    public float damageDelay;

    public void Damaged(GameObject bullet)
    {
        if (damagedState == DamagedState.Idle)
        {
            HP -= bullet.GetComponent<Bullet>().damage;
            HPBar.fillAmount = (float)HP / maxHP;

            if (HP <= 0)
            {
                Dead();
            }

            audioSource.clip = audioDamaged;
            audioSource.Play();
            
            damagedState = DamagedState.Damaged;

            if (!isFixed)
            {
                StartCoroutine(Knockback(bullet));
            }
            StartCoroutine(DamageDelay());
        }

    }

    IEnumerator DamageDelay()
    {
        spriteRenderer.color = new Color32(116, 116, 116, 225);
        yield return new WaitForSeconds(damageDelay);

        spriteRenderer.color = new Color32(255, 255, 255, 255);
        damagedState = DamagedState.Idle;

        yield break;
    }
    
    public float knockbackSpeed;
    private Vector3 knockbackDir;
    public float knockbackTime;

    IEnumerator Knockback(GameObject bullet)
    {
        anim.speed = 0f;
        
        knockbackDir = (-dir.normalized + bullet.GetComponent<Bullet>().dir.normalized).normalized;
        float countTime = 0;
        
        while (countTime < knockbackTime)
        {
            transform.position = Vector2.Lerp(transform.position, transform.position + knockbackDir, knockbackSpeed * Time.fixedDeltaTime);
            countTime += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        
        anim.speed = 1f;
        damagedState = DamagedState.Idle;
        
        yield break;
    }
    #endregion


    #region dead and drop item
    public string[] itemNames;
    public int[] itemProb;

    protected virtual void Dead()
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
            GameObject tempItem = Instantiate(DungeonManager.instance.dropItemPrefab, transform.position + Vector3.up * 0.06f, Quaternion.Euler(0, 0, 0));
            tempItem.GetComponent<DungeonItemController>().Initialize(itemNames[itemIndex]);
        }

        foreach (var t in gameObject.GetComponents<QuestTrigger>())
        {
            t.OnQuestTrigger();
        }
        
        Destroy(gameObject);

    }

    #endregion

    protected virtual void Start()
    {
        target = DungeonManager.instance.player; // 타겟을 플레이어로 지정
        state = State.Idle;
        damagedState = DamagedState.Idle;

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.settings.soundEffectsVolume;

        canAttack = true;

        HP = maxHP = maxHPArr[DungeonManager.instance.floor];
        ATK = ATKArr[DungeonManager.instance.floor];

        isFixed = false;
    }

    protected virtual void FixedUpdate()
    {
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

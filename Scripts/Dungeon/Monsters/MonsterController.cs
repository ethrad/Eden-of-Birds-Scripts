using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // Monster State 관리
    public enum State { Idle, Walk, Follow, Attack, Dead };
    protected enum DamagedState { Idle, Damaged };

    public State state;
    protected DamagedState damagedState;

    protected GameObject target; // 플레이어

    public int maxHP; // 최대 체력
    protected int HP; // 현재 체력
    public int ATK; // 공격력
    public float attackDelay; // 공격속도

    public float moveSpeed; // 이동 속도

    // 애니메이션
    protected Animator anim;

    bool canWalk = true;


    #region collider
    // 안쪽 콜라이더

    bool isPlayerInAttackArea = false;

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            isPlayerInAttackArea = true;
            state = State.Attack;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            isPlayerInAttackArea = false;
            state = State.Follow;
        }

    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            state = State.Idle;
        }
        if (col.gameObject.tag == "player")
        {
            canWalk = false;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "player")
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

    Vector3 dir;

    protected IEnumerator TurnWalk()
    {
        yield return new WaitForSeconds(2f);

        if (state == State.Idle)
        {
            dir = Random.insideUnitCircle.normalized;
            dir = dir.normalized;
            state = State.Walk;
            StartCoroutine(TurnIdle());
            yield return null;
        }
        else
        {
            yield return null;
        }
    }

    protected void Walk()
    {
        if (canWalk == true)
        {
            transform.position += dir * Time.deltaTime * moveSpeed;

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
            yield return null;
        }
        else
        {
            yield return null;
        }
    }


    protected IEnumerator Think()
    {
        yield return new WaitForSeconds(2f);

        if (state == State.Idle)
        {
            dir = Random.insideUnitCircle;
            dir = dir.normalized;
            state = State.Walk;
            yield return null;
        }
        else
        {
            yield return null;
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
        if (canAttack == true)
        {
            StartCoroutine(AttackDelay());
        }

    }


    protected IEnumerator AttackDelay()
    {
        if (isPlayerInAttackArea == true)
        {
            target.GetComponent<PlayerController>().Damaged(ATK);
        }
        anim.SetTrigger("Attack");
        canAttack = false;
        canWalk = false;
        yield return new WaitForSeconds(attackDelay);

        canAttack = true;
        canWalk = true;
        yield return null;

    }

    #endregion

    #region Damaged

    public float damageDelay;

    public void Damaged(int damage)
    {
        if (damagedState == DamagedState.Idle)
        {
            HP -= damage;

            if (HP <= 0)
            {
                Dead();
            }

            damagedState = DamagedState.Damaged;

            StartCoroutine(DamageDelay());
            //StartCoroutine(ColorChange());
        }

    }

    IEnumerator DamageDelay()
    {
        spriteRenderer.color = new Color32(216, 81, 255, 225);
        yield return new WaitForSeconds(damageDelay);

        spriteRenderer.color = new Color32(255, 255, 255, 255);
        damagedState = DamagedState.Idle;

        yield return null;
    }

    //int countTime = 0;
    public SpriteRenderer spriteRenderer;

    IEnumerator ColorChange()
    {
        spriteRenderer.color = new Color32(216, 81, 255, 225);

        yield return new WaitForSeconds(damageDelay);

        /*while (countTime < 10)
        {
            if (countTime % 2 == 0)
                spriteRenderer.color = new Color32(216, 81, 255, 225);
            else
                spriteRenderer.color = new Color32(236, 170, 255, 225);

            yield return new WaitForSeconds(0.2f);

            countTime++;
        }*/

        spriteRenderer.color = new Color32(255, 255, 255, 255);
        //countTime = 0;
        yield return null;
    }
    #endregion


    #region dead and drop item
    public GameObject[] itemArray;
    public int[] itemProb;

    protected virtual void Dead()
    {
        int random = Random.Range(0, 100);

        int itemIndex = -1;
        int tempProb = 0;

        for (int i = 0; i < itemProb.Length; i++)
        {
            if (random <= itemProb[i] + tempProb)
            {
                itemIndex = i;
            }
            else break;

            tempProb += itemProb[i];
        }

        if (itemIndex != -1)
        {
            Instantiate(itemArray[itemIndex], transform.position, Quaternion.Euler(0, 0, 0));
        }

        DungeonManager.instance.UpdateMonsterCount();
        Destroy(gameObject);

    }

    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        target = DungeonManager.instance.player; // 타겟을 플레이어로 지정
        state = State.Idle;
        damagedState = DamagedState.Idle;

        anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        canAttack = true;

        HP = maxHP;
    }

    // Update is called once per frame
    protected virtual void Update()
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

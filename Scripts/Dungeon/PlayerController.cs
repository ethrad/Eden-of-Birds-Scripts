using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 애니메이션
    private Animator anim;

    #region Move
    bool canMove;
    public float moveSpeed;

    float inputX;
    float inputY;
    Vector3 dir;
    bool isMoving;

    public GameObject Joystick;

    void Move()
    {
        inputX = Joystick.GetComponent<FixedJoystick>().Horizontal;
        inputY = Joystick.GetComponent<FixedJoystick>().Vertical;

        if (inputX == 0 && inputY == 0)
        {
            isMoving = false;
        }
        else isMoving = true;

        dir = new Vector3(inputX, inputY, 0).normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;

        anim.SetFloat("moveX", inputX);
        anim.SetBool("isMoving", isMoving);
    }

    #endregion

    #region Attack


    /*    public int ATK;
        public float ATKSpeed;


        bool canAttack;
        bool isAttacking;

        public void Attack()
        {
            anim.SetTrigger("Attack");
            StartCoroutine(Attacking());
        }

        IEnumerator Attacking()
        {
            isAttacking = true;
            canAttack = false;
            yield return new WaitForSeconds(0.16f);
            isAttacking = false;
            StartCoroutine(AttackDelay());
        }

        IEnumerator AttackDelay()
        {
            yield return new WaitForSeconds(ATKSpeed);
            canAttack = true;
            yield return null;
        }
    */
    #endregion

    #region Damaged

    public int maxHP;
    public int HP;
    bool canDamaged;

    public void Damaged(int damage)
    {
        if (canDamaged == true)
        {
            canMove = false;
            canDamaged = false;
            anim.SetTrigger("isDamaged");

            HP -= damage;

            if (HP <= 0)
            {
                Dead();
            }

            StartCoroutine(DamageDelay());
        }
    }

    IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(0.07f);

        canMove = true;
        canDamaged = true;
        
        yield return null;
    }


    void Dead()
    {
        canMove = false;
        // 죽는 애니메이션 재생
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        HP = maxHP;
        canDamaged = true;
        canMove = true;
        /*        canAttack = true;
                isAttacking = false;*/
        //DungeonManager.instance.player = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove == true)
        {
            Move();
        }
    }
}

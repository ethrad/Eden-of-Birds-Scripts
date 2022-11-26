using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownPlayerController : MonoBehaviour
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

    void Start()
    {
        anim = GetComponent<Animator>();
        canMove = true;
    }

    void FixedUpdate()
    {
        if (canMove == true)
        {
            Move();
        }
    }
}

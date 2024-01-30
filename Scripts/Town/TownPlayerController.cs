using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownPlayerController : MonoBehaviour
{
    private Animator anim;

    #region Move
    public bool canMove;
    public float moveSpeed;

    float inputX;
    float inputY;
    Vector3 dir;
    bool isMoving;

    public GameObject Joystick;
    private AudioSource audioWalkSource;
    private bool isAudioWalkPlaying;

    void Move()
    {
        inputX = Joystick.GetComponent<FixedJoystick>().Horizontal;
        inputY = Joystick.GetComponent<FixedJoystick>().Vertical;

        if (inputX == 0 && inputY == 0)
        {
            isMoving = false;
            audioWalkSource.Stop();
            isAudioWalkPlaying = false;
        }
        else
        {
            isMoving = true;

            if (!isAudioWalkPlaying)
            {
                audioWalkSource.Play();
                isAudioWalkPlaying = true;
            }
        }

        dir = new Vector3(inputX, inputY, 0).normalized;

        transform.position += dir * (moveSpeed * Time.deltaTime);

        anim.SetFloat("moveX", inputX);
        anim.SetBool("isMoving", isMoving);
    }

    public void Brake()
    {
        Joystick.GetComponent<Joystick>().Reset();
        dir = Vector3.zero;
    }

    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        canMove = true;
        audioWalkSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }
}

using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    public Animator anim;

    private int defaultATK;
    public int ATK;
    private int defaultDEF;
    private int DEF;
    private int defaultHP;
    private int maxHP;
    private int HP;
    
    #region Move
    
    bool canMove;
    public float moveSpeed;

    float inputX;
    float inputY;
    Vector3 dir;
    bool isMoving;

    public GameObject Joystick;
    public GameObject ShootingJoystick;
    
    private AudioSource audioSource;
    private AudioSource audioWalkSource;
    public AudioClip audioHarvest;
    public AudioClip audioDamaged;
    public AudioClip audioDead;

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

        transform.position += dir * (moveSpeed * Time.fixedDeltaTime);

        anim.SetFloat("moveX", inputX);
        anim.SetBool("isMoving", isMoving);

    }

    public void StopMoving()
    {
        canMove = false;
        dir = Vector3.zero;
        Joystick.GetComponent<Joystick>().Reset();
        ShootingJoystick.GetComponent<Joystick>().Reset();
        audioWalkSource.Stop();
    }

    public void StartMoving()
    {
        canMove = true;
    }

    #endregion
    
    #region Damaged and Dead

    public Image HPBar;
    private bool canDamaged;
    private bool isRevived;

    public void Damaged(int damage)
    {
        if (canDamaged)
        {
            canMove = false;
            canDamaged = false;
            anim.SetTrigger("isDamaged");
            audioSource.clip = audioDamaged;
            audioSource.Play();
            
            int calDamage = damage - (int)Math.Round(DEF * 0.2, 1);
            if (calDamage > 0)
            {
                HP -= calDamage;
            }
           
            HPBar.fillAmount = (float)HP / maxHP;

            if (HP <= 0)
            {
                Dead();
                return;
            }

            StartCoroutine(DamageDelay());
        }
    }

    IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(0.07f);

        canMove = true;
        canDamaged = true;
        
        yield break;
    }


    void Dead()
    {
        canMove = false;

        gameObject.tag = "object";
        anim.SetTrigger("isDead");
        audioWalkSource.Stop();
        audioSource.clip = audioDead;
        audioSource.Play();
        
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        Joystick.SetActive(false);
        ShootingJoystick.SetActive(false);
        StartCoroutine(DeadDelay());
    }

    IEnumerator DeadDelay()
    {
        yield return new WaitForSeconds(2.5f);

        Time.timeScale = 0f;

        if (isRevived)
        {
            DungeonManager.instance.FailDungeon();
        }
        else
        {
            DungeonManager.instance.OnRevivePanel();
        }
       
    }

    #endregion

    #region Equipment
    
    

    public void ChangeEquipment(int HP, int ATK, int DEF)
    {
        if (maxHP == this.HP)
        {
            this.HP += HP;
        }
        
        maxHP = defaultHP + HP;
        this.ATK = defaultATK + ATK;
        this.DEF = defaultDEF + DEF;
    }

    #endregion
    
    public void Harvest()
    {
        anim.SetTrigger("harvest");
        StartCoroutine(HarvestDelay());
    }
    
    IEnumerator HarvestDelay()
    {
        yield return new WaitForSeconds(1.6f);
        
        audioSource.clip = audioHarvest;
        audioSource.Play();
        
        ShootingJoystick.SetActive(true);
    }

    #region Heal and Revive

    public void Heal(int healHP)
    {
        if (HP + healHP > maxHP)
        {
            HP = maxHP;
        }
        else
        {
            HP += healHP;
        }
        
        HPBar.fillAmount = (float)HP / maxHP;
    }

    public void Revive()
    {
        isRevived = true;
        HP = maxHP;
        HPBar.fillAmount = (float)HP / maxHP;
        canMove = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        Joystick.SetActive(true);
        ShootingJoystick.SetActive(true);
        gameObject.tag = "player";
        anim.SetTrigger("endShooting");
        StartCoroutine(ReviveDelay());
    }

    IEnumerator ReviveDelay()
    {
        canDamaged = false;
        yield return new WaitForSeconds(2f);
        canDamaged = true;
        yield break;
    }
    
    #endregion
    
    public void Invincible()
    {
        StartCoroutine(InvincibleDelay());
    }
    
    IEnumerator InvincibleDelay()
    {
        canDamaged = false;
        yield return new WaitForSeconds(3f);
        
        canDamaged = true;
        yield break;
    }
    
    private void ApplySkills()
    {
        foreach (var skillList in GameManager.instance.earnedSkills)
        {
            if (skillList.Key == "dungeon" || skillList.Key == "all")
            {
                foreach(var skill in skillList.Value)
                {
                    switch (skill.skillType)
                    {
                        case "hp":
                            defaultHP = maxHP = HP += (int)skill.skillValue;
                            break;
                        case "atk":
                            defaultATK = ATK += (int)skill.skillValue;
                            break;
                        case "def":
                            defaultDEF = DEF += (int)skill.skillValue;
                            break;
                        case "speed":
                            moveSpeed += moveSpeed * skill.skillValue;
                            break;
                    }
                }
            }
        }
    }
    
    void Start()
    {
        defaultATK = ATK = 3;
        defaultDEF = DEF = 0;
        defaultHP = maxHP = HP = 30;

        anim = GetComponent<Animator>();
        canDamaged = true;
        canMove = true;
        audioSource = GetComponent<AudioSource>();
        audioWalkSource = GetComponents<AudioSource>()[1];

        audioSource.volume = GameManager.instance.settings.soundEffectsVolume;
        audioWalkSource.volume = GameManager.instance.settings.soundEffectsVolume;
        
        ApplySkills();
        //DungeonManager.instance.player = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }
}

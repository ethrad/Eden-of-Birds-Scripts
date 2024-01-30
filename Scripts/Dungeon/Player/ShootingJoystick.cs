using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Dungeon
{
    public class ShootingJoystick : Joystick
    {
        public float dragDistance;
        public GameObject arrow;
        public float arrowRadius;

        public Image bulletCountBar;
        [HideInInspector]
        public int bulletCount;

        private bool canShoot;

        [SerializeField]
        private float shootingDelay;
        private float shotTimer;

        Vector3 dir;

        private AudioSource audioSource;
        private Animator playerAnim;
        
        public bool isReloading;

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            if (Vector2.Distance(new Vector2(0, 0), Direction) < dragDistance)
            {
                return;
            }

            if (bulletCount <= 0)
            {
                playerAnim.SetTrigger("endShooting");
                return;
            }
            
            if (isReloading) return;

            canShoot = true;

            if (arrow.activeSelf == false)
            {
                arrow.SetActive(true);
                playerAnim.SetTrigger("startShooting");
            }

            dir = Direction.normalized * arrowRadius;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            arrow.transform.localEulerAngles = new Vector3(0, 0, angle);
            arrow.transform.position = -dir + DungeonManager.instance.player.transform.position;
            playerAnim.SetFloat("shootX", -Horizontal);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;

            shotTimer = 1;
            canShoot = false;
            arrow.SetActive(false);
            playerAnim.SetTrigger("endShooting");
        }

        public new void Reset()
        {
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;

            shotTimer = 1;
            canShoot = false;
            arrow.SetActive(false);
            playerAnim.SetTrigger("endShooting");
        }
        
        private void Shoot()
        {
            GameObject bullet = DungeonManager.instance.GetObject("PlayerBullet");
            bullet.GetComponent<PlayerBullet>().Initialize(-dir);
            bulletCount--;
            shotTimer = 0;
            audioSource.Play();

            if (bulletCount <= 0)
            {
                canShoot = false;
            }

            bulletCountBar.fillAmount = (float)bulletCount / 10;
        }

        protected override void Start()
        {
            base.Start();
            bulletCount = 10;
            //reloadDelay = 0.2f;
            shootingDelay = 0.3f;
            shotTimer = 1;
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = GameManager.instance.settings.soundEffectsVolume;
            playerAnim = DungeonManager.instance.player.GetComponent<Animator>();
        }

        private void Update()
        {
            if (canShoot)
            {
                if (shotTimer >= shootingDelay)
                {
                    Shoot();
                }

                shotTimer += Time.deltaTime;
            }
        }
    }
}

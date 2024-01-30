using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tycoon
{
    public class SeatController : MonoBehaviour, IDropHandler
    {
        public GameObject characterImage;

        public GameObject orderBalloon;

        public GameObject angryImg;

        protected AudioSource audioSource;
        public AudioClip audioEnter;

        [HideInInspector]
        public int seatID;

        protected bool canServe;

        public virtual void OnDrop(PointerEventData eventData)
        {

        }

        public virtual void UpdateSeat(Order order)
        {

        }

        protected IEnumerator ResetSeat()
        {
            yield return new WaitForSeconds(BarManager.instance.resetDelay);

            gameObject.SetActive(false);
            BarManager.instance.UpdateSeat(seatID);
            Destroy(this.gameObject);

            yield break;
        }


        #region Order Balloon

        public Image timeBar;

        protected float timeLimit;
        protected float currentTime;

        protected IEnumerator timeBarCoroutine;
        
        public virtual void UpdateBalloon()
        {
            //timeLimit = order.waitingTime;
            currentTime = timeLimit;
            
            timeBarCoroutine = UpdateTimeBar();
            StartCoroutine(timeBarCoroutine);
        }

        protected IEnumerator UpdateTimeBar()
        {
            while (currentTime > 0)
            {
                currentTime -= 0.1f;
                timeBar.fillAmount = currentTime / timeLimit;

                yield return new WaitForSeconds(0.1f);
            }

            FailedOrder();

            yield break;
        }

        private void FailedOrder()
        {
            canServe = false;
            StopCoroutine(timeBarCoroutine);
            orderBalloon.SetActive(false);
            angryImg.SetActive(true);
            TycoonManager.instance.ActivateAngryUI(seatID);
            StartCoroutine(ResetSeat());
        }

        #endregion


        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = GameManager.instance.settings.soundEffectsVolume;
        }
    }
}
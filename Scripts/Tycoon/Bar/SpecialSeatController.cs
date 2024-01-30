using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace Tycoon
{
    public class SpecialSeatController : SeatController
    {
        [HideInInspector] public Order order;

        public Text dialogue;
        public GameObject heartImg;
        public Text friendShipText;
        public string residentName;

        public override void OnDrop(PointerEventData eventData)
        {
            if (!canServe) return;
            
            string foodName = eventData.pointerDrag.GetComponent<FoodController>().foodName;

            int intersectCount = GameManager.instance.menus[foodName].properties
                .Intersect(TycoonManager.instance.specialResidentInfo[residentName].properties).Count();

            StopCoroutine(timeBarCoroutine);
            orderBalloon.SetActive(false);
            
            GameManager.instance.SetFriendShip(residentName, intersectCount, false);
            
            orderBalloon.SetActive(false);
            heartImg.SetActive(true);
            friendShipText.text = intersectCount.ToString();

            // 팁 추가
            TycoonManager.instance.EarnGold(Mathf.RoundToInt(intersectCount * 0.2f * GameManager.instance.menus[foodName].gold));
            eventData.pointerDrag.GetComponent<FoodController>().ResetPlate();
            
            foreach (var t in gameObject.GetComponents<QuestTrigger>())
            {
                if (t.questType == 8)
                {
                    t.goalName = residentName;
                }

                t.OnQuestTrigger();
            }
            
            canServe = false;
            StartCoroutine(ResetSeat());

            Destroy(eventData.pointerDrag);
        }

        public override void UpdateSeat(Order order)
        {
            this.order = order;

            residentName = order.residentName;

            characterImage.GetComponent<Animator>().runtimeAnimatorController =
                Resources.Load<RuntimeAnimatorController>("Animations/" + order.residentName);

            audioSource.clip = audioEnter;
            audioSource.Play();

            gameObject.SetActive(true);

            UpdateBalloon();
            orderBalloon.SetActive(true);
            canServe = true;
        }

        #region Order Balloon

        public override void UpdateBalloon()
        {
            dialogue.text = TycoonManager.instance.specialResidentInfo[residentName].tycoonDialogue;
            timeLimit = order.waitingTime;
            currentTime = timeLimit;
            timeBarCoroutine = UpdateTimeBar();
            StartCoroutine(timeBarCoroutine);
        }

        #endregion
    }
}
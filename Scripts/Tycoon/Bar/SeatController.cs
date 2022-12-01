using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SeatController : MonoBehaviour, IDropHandler
{
    public GameObject characterImage;
    public GameObject orderBalloon;
    public GameObject heart;
    public GameObject angry;

/*    AudioSource audioSource;
    public AudioClip audioEnter;
*/
    [HideInInspector]
    public Order order;

    [HideInInspector]
    public int seatID;
    int servedFoodCount = 0;

    public void OnDrop(PointerEventData eventData)
    {
        bool isOrdered = false;
        for (int i = 0; i < order.menuList.Count; i++)
        {
            if (order.menuList[i] == eventData.pointerDrag.GetComponent<FoodController>().foodName)
            {
                isOrdered = true;
                order.menuList[i] = "";
                orderBalloon.GetComponent<OrderBalloonController>().MarkServedFood(i);
                servedFoodCount++;
                TycoonManager.instance.EarnGold(eventData.pointerDrag.GetComponent<FoodController>().foodName);
                eventData.pointerDrag.GetComponent<FoodController>().ResetPlate();

                if (servedFoodCount == order.menuList.Count)
                {
                    // 모든 음식을 다 서빙했음
                    orderBalloon.GetComponent<OrderBalloonController>().StopBalloon();
                    orderBalloon.SetActive(false);
                    heart.SetActive(true);

                    StartCoroutine(ResetSeat());
                }
                Destroy(eventData.pointerDrag);
                break;
            }
        }
        
        if (isOrdered == false)
        {
            // 틀린 음식을 서빙했다는 대사 출력?
        }
    }

    public void FailedOrder()
    {
        orderBalloon.GetComponent<OrderBalloonController>().StopBalloon();
        orderBalloon.SetActive(false);
        angry.SetActive(true);
        TycoonManager.instance.ActivateAngryUI(seatID);
        StartCoroutine(ResetSeat());
    }


    IEnumerator ResetSeat()
    {
        yield return new WaitForSeconds(BarManager.instance.resetDelay);

        servedFoodCount = 0;
        gameObject.SetActive(false);
        heart.SetActive(false);
        angry.SetActive(false);
        orderBalloon.GetComponent<OrderBalloonController>().ResetBalloon();
        
        BarManager.instance.UpdateSeat(seatID);

        yield return null;
    }

    public void UpdateSeat(Order order)
    {
        this.order = order;

        if (order.residentName == "normal")
        {
            int random = Random.Range(1, 4);
            order.residentName += random.ToString();
        }
        characterImage.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + order.residentName);
/*
        audioSource.clip = audioEnter;
        audioSource.Play();*/
        gameObject.SetActive(true);
        orderBalloon.SetActive(true);
        orderBalloon.GetComponent<OrderBalloonController>().UpdateBalloon(order);
    }

/*    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
    }*/
}

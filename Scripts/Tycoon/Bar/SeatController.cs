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

                if (servedFoodCount == order.menuList.Count)
                {
                    // ��� ������ �� ��������
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
            // Ʋ�� ������ �����ߴٴ� ��� ���?
        }
    }

    public void FailedOrder()
    {
        orderBalloon.GetComponent<OrderBalloonController>().StopBalloon();
        orderBalloon.SetActive(false);
        angry.SetActive(true);

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
        gameObject.SetActive(true);
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

        orderBalloon.SetActive(true);
        orderBalloon.GetComponent<OrderBalloonController>().UpdateBalloon(order);
    }
}
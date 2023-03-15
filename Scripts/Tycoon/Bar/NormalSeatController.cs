using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NormalSeatController : SeatController
{
    public GameObject heartImg;
    
    [HideInInspector]
    public NormalOrder order;

    int servedFoodCount = 0;

    public override void OnDrop(PointerEventData eventData)
    {
        bool isOrdered = false;
        for (int i = 0; i < order.menuList.Count; i++)
        {
            if (order.menuList[i] == eventData.pointerDrag.GetComponent<FoodController>().foodName)
            {
                isOrdered = true;
                order.menuList[i] = "";
                MarkServedFood(i);
                servedFoodCount++;
                TycoonManager.instance.EarnGold(eventData.pointerDrag.GetComponent<FoodController>().foodName);
                eventData.pointerDrag.GetComponent<FoodController>().ResetPlate();

                if (servedFoodCount == order.menuList.Count)
                {
                    // 모든 음식을 다 서빙했음
                    StopCoroutine(UpdateTimeBar());
                    orderBalloon.SetActive(false);
                    heartImg.SetActive(true);

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

    public override void UpdateSeat(Order order)
    {
        this.order = (NormalOrder)order;

        int random = Random.Range(1, 4);
        order.residentName += random.ToString();

        characterImage.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + order.residentName);

        audioSource.clip = audioEnter;
        audioSource.Play();
        
        gameObject.SetActive(true);
        
        UpdateBalloon();
        orderBalloon.SetActive(true);
    }

    
    #region Order Balloon
    
    public string residentName;
    
    public GameObject[] foodImages; // 자식에 x 표시 된 것 만들기

    public override void UpdateBalloon()
    {
        for (int i = 0; i < order.menuList.Count; i++)
        {
            foodImages[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/" + order.menuList[i]);
        }

        timeLimit = order.waitingTime;
        currentTime = timeLimit;
        StartCoroutine(UpdateTimeBar());
    }

    public void MarkServedFood(int foodIndex)
    {
        foodImages[foodIndex].transform.GetChild(0).gameObject.SetActive(true);
    }
    
    #endregion
}

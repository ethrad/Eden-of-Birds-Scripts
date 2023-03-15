using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class SpecialSeatController : SeatController
{
    [HideInInspector]
    public Order order;

    public Text dialogue;

    public string residentName;

    public override void OnDrop(PointerEventData eventData)
    {
        string foodName = eventData.pointerDrag.GetComponent<FoodController>().foodName;

        int intersectCount = TycoonManager.instance.menus[foodName].properties
            .Intersect(GameManager.instance.specialResidentInfo[residentName].properties).Count();
        
        StopCoroutine(UpdateTimeBar());
        orderBalloon.SetActive(false);

        // 호감도 얼만큼 올랐는지 표시하는 UI 넣기
        // 겹치는 속성 * 1 만큼 호감도 증가


        // 팁 추가
        TycoonManager.instance.earnedGold += Mathf.RoundToInt(intersectCount * 0.2f * TycoonManager.instance.menus[foodName].gold);

        StartCoroutine(ResetSeat());

        Destroy(eventData.pointerDrag);
    }
    
    public override void UpdateSeat(Order order)
    {
        this.order = order;

        residentName = order.residentName;

        characterImage.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + order.residentName);

        audioSource.clip = audioEnter;
        audioSource.Play();
        
        gameObject.SetActive(true);
        
        UpdateBalloon();
        orderBalloon.SetActive(true);
    }

    #region Order Balloon

    public override void UpdateBalloon()
    {
        dialogue.text = GameManager.instance.specialResidentInfo[residentName].dialogue;
        timeLimit = order.waitingTime;
        currentTime = timeLimit;
        StartCoroutine(UpdateTimeBar());
    }
    
    #endregion
}

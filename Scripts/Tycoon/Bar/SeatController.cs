using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SeatController : MonoBehaviour, IDropHandler
{
    public GameObject characterImage;

    public GameObject orderBalloon;

    public GameObject angryImg;
    
    protected AudioSource audioSource;
    public AudioClip audioEnter;
    
    [HideInInspector]
    public int seatID;
    
    public virtual void OnDrop(PointerEventData eventData)
    {
        
    }

    public void FailedOrder()
    {
        StopCoroutine(UpdateTimeBar());
        orderBalloon.SetActive(false);
        angryImg.SetActive(true);
        TycoonManager.instance.ActivateAngryUI(seatID);
        StartCoroutine(ResetSeat());
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

        yield return null;
    }
    
    
    #region Order Balloon

    public Image timeBar;

    protected float timeLimit;
    protected float currentTime;
    
    public virtual void UpdateBalloon()
    {
        //timeLimit = order.waitingTime;
        currentTime = timeLimit;
        StartCoroutine(UpdateTimeBar());
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

        yield return null;
    }
    
    #endregion

    
    void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
    }
}

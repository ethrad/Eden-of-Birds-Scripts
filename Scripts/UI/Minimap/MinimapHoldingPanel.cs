using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MinimapHoldingPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject fullScreenPanel;
    public bool isHoldSetting = false;

    //public void ChangeMinimapWorking()
    public void ChangeMinimapPanelWorking()
    {
        isHoldSetting = GameManager.instance.settings.minimapHolding;
        SetPanelRaycast(isHoldSetting);
    }

    private void Start()
    {
        ChangeMinimapPanelWorking();
    }

    //누르기
    public void OnPointerDown(PointerEventData eventData)
    {
        fullScreenPanel.SetActive(true);

    }
    //떼기
    public void OnPointerUp(PointerEventData eventData)
    {
        fullScreenPanel.SetActive(false);
    }

    void SetPanelRaycast(bool isHold)
    {
        if (isHold)
            GetComponent<Image>().raycastTarget = true;
        else
            GetComponent<Image>().raycastTarget = false;
    }
}

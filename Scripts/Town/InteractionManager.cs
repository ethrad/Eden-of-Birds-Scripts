using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour, IPointerClickHandler
{
    public static InteractionManager instance;

    bool canClick = false;
    [HideInInspector]
    public string residentName;

    public GameObject speechBalloon;

    public void UpdateInteractionState(string residentName)
    {
        canClick = true;
        this.residentName = residentName;

        speechBalloon.SetActive(true);
    }

    public void ResetInteractionState()
    {
        canClick = false;
        residentName = "";

        speechBalloon.SetActive(false);
    }

    public GameObject interactionPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick == true)
        {
            interactionPanel.GetComponent<InteractionPanel>().UpdatePanel();
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Vector3 DefaultPos;
    public string foodName;

    public void UpdateFood(string recipeName)
    {
        foodName = recipeName;
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/" + foodName);
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        DefaultPos = this.transform.position;
        GetComponent<Image>().raycastTarget = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector3 currentPos = Camera.main.ScreenToWorldPoint(eventData.position);
        currentPos.z = 90f;
        this.transform.position = currentPos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        this.transform.position = DefaultPos;
        GetComponent<Image>().raycastTarget = true;
    }
}

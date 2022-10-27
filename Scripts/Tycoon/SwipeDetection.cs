using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject mainCamera;

    public float swipeLengthX;
    public float swipeLengthY;

    Vector2 clickedPosition;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        clickedPosition = pointerEventData.position;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (Mathf.Abs((pointerEventData.position - clickedPosition).y) < swipeLengthY)
        {
            if ((pointerEventData.position - clickedPosition).x > swipeLengthX)
            {
                mainCamera.transform.position = new Vector3(0, 0, -10f);
            }
            else if ((pointerEventData.position - clickedPosition).x < -swipeLengthX)
            {
                mainCamera.transform.position = new Vector3(2960f, 0, -10f);
            }
        }
    }
}

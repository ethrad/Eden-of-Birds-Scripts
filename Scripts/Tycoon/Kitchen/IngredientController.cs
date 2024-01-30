using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class IngredientController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Vector3 DefaultPos;

    public enum IngredientState { Idle, Dropped, Plated, Dumped };
    public IngredientState ingredientState = IngredientState.Idle;

    [HideInInspector]
    public bool isStarted = false;
    [HideInInspector]
    public GameObject beforeBowl;
    [HideInInspector]
    public List<string> cookingSequences = new List<string>();

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (ingredientState != IngredientState.Idle) return;

        DefaultPos = this.transform.position;
        GetComponent<Image>().raycastTarget = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (ingredientState != IngredientState.Idle)
        {
            return;
        }

        Vector3 currentPos = Camera.main.ScreenToWorldPoint(eventData.position);
        currentPos.z = 90f;
        this.transform.position = currentPos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (ingredientState == IngredientState.Idle)
        {
            transform.position = DefaultPos;
        }
        else if (ingredientState == IngredientState.Plated)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -32f);
            ingredientState = IngredientState.Idle;
        }
        else
        {
            if (isStarted == false)
            {
                try
                {
                    var temp = IncaTernTycoon.IncaTernController.instance.isIncaTern;
                    beforeBowl.GetComponent<IncaTernTycoon.BeforeBowlController>().UpdateBowl();
                }
                catch (System.Exception)
                {
                    GameManager.instance.inventory[name]--;
                    beforeBowl.GetComponent<Tycoon.BeforeBowlController>().UpdateBowl();
                }
                
                isStarted = true;
            }

            if (ingredientState == IngredientState.Dropped)
            {
                ingredientState = IngredientState.Idle;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        GetComponent<Image>().raycastTarget = true;
    }

    public GameObject[] cookingColors;
    int currentCCID = 0;
    public bool canCook = true;

    public void Cooking(string cookerName)
    {
        float c = GetComponent<Image>().color.r;
        GetComponent<Image>().color = new Color(c, c, c, 0f);

        for (int i = 0; i < 3; i++)
        {
            cookingColors[i].SetActive(false);
        }

        cookingSequences.Add(cookerName);
    }

    public void UpdateCookingColor(Color c)
    {
        cookingColors[currentCCID].transform.GetChild(0).gameObject.GetComponent<Image>().color = c;

        for (int i = 0; i <= currentCCID; i++)
        {
            cookingColors[i].SetActive(true);
        }

        currentCCID++;

        if (currentCCID >= 3)
        {
            canCook = false;
        }
    }


    public void Initialize(string ingredientName)
    {
        name = ingredientName;
        beforeBowl = transform.parent.gameObject;
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Items/" + ingredientName);
        GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 90f);
    }
}

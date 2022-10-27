using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CookerController : MonoBehaviour, IDropHandler
{
    public GameObject effectObject;
    public float cookingTime;
    public bool isShaker;
    GameObject ingredient;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 1)
        {
            if (eventData.pointerDrag.GetComponent<IngredientController>().ingredientState == IngredientController.IngredientState.Idle)
            {
                ingredient = eventData.pointerDrag;
                ingredient.GetComponent<IngredientController>().ingredientState = IngredientController.IngredientState.Dropped;
                ingredient.transform.SetParent(this.transform);
                ingredient.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

                StartCoroutine(Cooking());
            }
        }
    }

    IEnumerator Cooking()
    {
        if (isShaker == true)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 0f);
        }

        effectObject.SetActive(true);
        effectObject.transform.SetAsLastSibling();

        ingredient.GetComponent<Image>().raycastTarget = false;
        float c = ingredient.GetComponent<Image>().color.r;
        ingredient.GetComponent<Image>().color = new Color(c, c, c, 0f);
        ingredient.GetComponent<IngredientController>().cookingSequences.Add(this.name);

        yield return new WaitForSeconds(cookingTime);
        ingredient.GetComponent<Image>().color = new Color(c - 0.3f, c - 0.3f, c - 0.3f);
        ingredient.GetComponent<Image>().raycastTarget = true;
        effectObject.SetActive(false);

        if (isShaker == true)
        {
            GetComponent<Image>().color = new Color(1, 1, 1);
        }

        yield return null;
    }
}

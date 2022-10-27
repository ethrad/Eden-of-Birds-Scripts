using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AfterBowlController : MonoBehaviour, IDropHandler
{
	public void OnDrop(PointerEventData eventData)
	{
		if (transform.childCount == 0)
		{
			if (eventData.pointerDrag.GetComponent<IngredientController>().ingredientState == IngredientController.IngredientState.Idle)
            {
				GameObject ingredient = eventData.pointerDrag;
				ingredient.GetComponent<IngredientController>().ingredientState = IngredientController.IngredientState.Dropped;
				ingredient.transform.SetParent(this.transform);
				ingredient.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
			}
		}
	}
}

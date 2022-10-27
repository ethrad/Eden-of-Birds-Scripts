using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class KitchenPlateController : MonoBehaviour, IDropHandler
{
    public GameObject resultFoodImage;

    List<IngredientSequence> ingredients = new List<IngredientSequence>();
    bool canDrop = true;

    public void OnDrop(PointerEventData eventData)
    {
        if (canDrop == true)
        {
            if (eventData.pointerDrag.GetComponent<IngredientController>().isStarted == false)
            {
                ItemManager.instance.inventory[eventData.pointerDrag.name]--;
                eventData.pointerDrag.GetComponent<IngredientController>().beforeBowl.GetComponent<BeforeBowlController>().UpdateBowl();
            }

            ingredients.Add(new IngredientSequence(
                eventData.pointerDrag.name,
                eventData.pointerDrag.GetComponent<IngredientController>().cookingSequences));

            if (ingredients.Count > 5) canDrop = false;

            Destroy(eventData.pointerDrag);
        }
    }

    public void OnServiceBellClicked()
    {
        if (ingredients.Count > 0)
        {
            canDrop = false;
            string recipeName = "";

            foreach (KeyValuePair<string, Menu> recipe in TycoonManager.instance.menus)
            {
                recipeName = recipe.Key;

                if (recipe.Value.ingredientSequences.Count == ingredients.Count)
                {
                    for (int i = 0; i < recipe.Value.ingredientSequences.Count; i++)
                    {
                        if (!recipe.Value.ingredientSequences[i].ingredientName.Equals(ingredients[i].ingredientName))
                        {
                            recipeName = "";
                            break;
                        }
                        if (!recipe.Value.ingredientSequences[i].cookingSequences.SequenceEqual(ingredients[i].cookingSequences))
                        {
                            recipeName = "";
                            break;
                        }
                    }
                }
                else
                {
                    recipeName = "";
                }


                if (!recipeName.Equals(""))
                {
                    break;
                }
            }

            Debug.Log(recipeName);

            if (recipeName == "")
            {
                // 쓰레기 완성
                resultFoodImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/failedFood");
                resultFoodImage.SetActive(true);
                StartCoroutine(ThrowAway());
            }
            else
            {
                resultFoodImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/" + recipeName);
                resultFoodImage.SetActive(true);
                StartCoroutine(ServeToBar(recipeName));
            }

            ingredients.Clear();

        }



    }

    IEnumerator ServeToBar(string recipeName)
    {
        bool servingResult = BarPlateManager.instance.GetServed(recipeName);

        if (servingResult == false)
        {
            // 앵무새가 뭐라고 하는 팝업
            Debug.Log("??");
        }
        else
        {
            yield return new WaitForSeconds(2f);
            // 날아가는 애니메이션 추가
            resultFoodImage.SetActive(false);
            canDrop = true;
        }



        yield return null;
    }

    IEnumerator ThrowAway()
    {
        yield return new WaitForSeconds(2f);

        resultFoodImage.SetActive(false);
        canDrop = true;

        yield return null;
    }
}

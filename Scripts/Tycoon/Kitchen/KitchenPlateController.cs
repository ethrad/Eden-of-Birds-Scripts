using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class KitchenPlateController : MonoBehaviour, IDropHandler
{
    public GameObject resultFoodImage;
    public GameObject parrotHand;
    public GameObject parrotHaneAngry;

    List<IngredientSequence> ingredients = new List<IngredientSequence>();
    bool canDrop = true;
    bool hasFood = false;
    string recipeName = "";

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


    AudioSource audioSource;
    public AudioClip ringingSound;

    public void OnServiceBellClicked()
    {
        audioSource.clip = ringingSound;
        audioSource.Play();

        if (hasFood == true)
        {
            StartCoroutine(ServeToBar(recipeName));

            return;
        }

        if (ingredients.Count > 0)
        {
            canDrop = false;
            recipeName = "";

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

            if (recipeName == "")
            {
                // 쓰레기 완성
                resultFoodImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/failedFood");
                resultFoodImage.SetActive(true);

                /*                if (IOManager.instance.playerSettings.isTutorialCleared == false)
                                {
                                    KitchenTycoonTutorial.instance.foodName = "맛없는 쓰레기";
                                    KitchenTycoonTutorial.instance.CompleteFood();
                                }*/

                StartCoroutine(ThrowAway());
            }
            else
            {
                resultFoodImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/" + recipeName);
                resultFoodImage.SetActive(true);
                StartCoroutine(ServeToBar(recipeName));

               /* if (IOManager.instance.playerSettings.isTutorialCleared == false)
                {
                    KitchenTycoonTutorial.instance.foodName = "맛있는 " + TycoonManager.instance.menus[recipeName].koreanName;
                    Debug.Log(TycoonManager.instance.menus[recipeName].koreanName);
                    KitchenTycoonTutorial.instance.CompleteFood();
                }*/
            }

            ingredients.Clear();

        }
    }

    public float parrotHandSpeed;

    IEnumerator ServeToBar(string recipeName)
    {
        bool servingResult = BarManager.instance.GetServed(recipeName);

        if (servingResult == false)
        {
            hasFood = true;

            parrotHand.SetActive(true);
            parrotHaneAngry.SetActive(true);
            Vector3 tempV = parrotHand.GetComponent<RectTransform>().anchoredPosition;
            parrotHand.GetComponent<RectTransform>().anchoredPosition = new Vector3(tempV.x, -255, tempV.z);

            yield return new WaitForSeconds(1f);
            parrotHand.SetActive(false);
            parrotHaneAngry.SetActive(false);

        }
        else
        {
            parrotHand.SetActive(true);

            Vector3 tempV = parrotHand.GetComponent<RectTransform>().anchoredPosition;

            for (float y = -430; y <= 120; y += Time.deltaTime * parrotHandSpeed)
            {
                parrotHand.GetComponent<RectTransform>().anchoredPosition = new Vector3(tempV.x, y, tempV.z);
                yield return null;
            }

            resultFoodImage.SetActive(false);

            for (float y = 120; y >= -430; y -= Time.deltaTime * parrotHandSpeed)
            {
                parrotHand.GetComponent<RectTransform>().anchoredPosition = new Vector3(tempV.x, y, tempV.z);
                yield return null;
            }

            canDrop = true;
            hasFood = false;
        }

        yield return null;
    }

    IEnumerator ThrowAway()
    {
        parrotHand.SetActive(true);
        parrotHaneAngry.SetActive(true);
        Vector3 tempV = parrotHand.GetComponent<RectTransform>().anchoredPosition;
        parrotHand.GetComponent<RectTransform>().anchoredPosition = new Vector3(tempV.x, -255, tempV.z);

        yield return new WaitForSeconds(2f);
        parrotHand.SetActive(false);
        parrotHaneAngry.SetActive(false);

        resultFoodImage.SetActive(false);
        canDrop = true;

        yield return null;
    }

    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
    }
}

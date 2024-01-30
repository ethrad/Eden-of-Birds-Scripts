using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialCookerController : MonoBehaviour, IDropHandler
{
    public GameObject effectObject;
    public float cookingTime;
    public bool isShaker;
    GameObject ingredient;
    public Color color;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 1 && eventData.pointerDrag.GetComponent<IngredientController>().canCook == true)
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
        if (TutorialTycoonManager.instance.rabbitPanelNum == 1)
        {
            TutorialTycoonManager.instance.rabbitPanelNum++;
            TutorialTycoonManager.instance.SetActiveRabbitPanel(TutorialTycoonManager.instance.rabbitPanelNum);
        }
        else if (TutorialTycoonManager.instance.rabbitPanelNum == 2)
        {
            TutorialTycoonManager.instance.rabbitPanelNum++;
            TutorialTycoonManager.instance.SetActiveRabbitPanel(TutorialTycoonManager.instance.rabbitPanelNum);
        }
        else if (TutorialTycoonManager.instance.rabbitPanelNum == 4)
        {
            TutorialTycoonManager.instance.rabbitPanelNum++;
            TutorialTycoonManager.instance.SetActiveRabbitPanel(TutorialTycoonManager.instance.rabbitPanelNum);
        }
        
        audioSource.clip = cookingSound;
        audioSource.Play();

        if (isShaker == true)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 0f);
        }

        effectObject.SetActive(true);
        effectObject.transform.SetAsLastSibling();

        ingredient.GetComponent<Image>().raycastTarget = false;
        float c = ingredient.GetComponent<Image>().color.r;
        ingredient.GetComponent<IngredientController>().Cooking(this.name);

        yield return new WaitForSeconds(cookingTime);

        if (TutorialTycoonManager.instance.panelNum == 4)
        {
            TutorialTycoonManager.instance.panelNum++;
            TutorialTycoonManager.instance.SetActivePanel(TutorialTycoonManager.instance.panelNum);
        }

        
        
        ingredient.GetComponent<Image>().color = new Color(c - 0.2f, c - 0.2f, c - 0.2f);
        ingredient.GetComponent<IngredientController>().UpdateCookingColor(color);
        
        ingredient.GetComponent<Image>().raycastTarget = true;
        effectObject.SetActive(false);

        if (isShaker == true)
        {
            GetComponent<Image>().color = new Color(1, 1, 1);
        }

        
        
        yield break;
    }

    AudioSource audioSource;
    public AudioClip cookingSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.settings.soundEffectsVolume;
    }
}

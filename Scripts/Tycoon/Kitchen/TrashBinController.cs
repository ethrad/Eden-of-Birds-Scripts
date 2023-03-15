using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashBinController : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<IngredientController>().ingredientState == IngredientController.IngredientState.Idle)
        {
            audioSource.clip = throwingSound;
            audioSource.Play();

            GameObject ingredient = eventData.pointerDrag;
            ingredient.GetComponent<IngredientController>().ingredientState = IngredientController.IngredientState.Dumped;
            ingredient.transform.SetParent(this.transform);
            ingredient.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
    }

    AudioSource audioSource;
    public AudioClip throwingSound;

    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
    }
}

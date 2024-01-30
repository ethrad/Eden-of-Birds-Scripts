using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrashBinController : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite closedTrashBin;
    public Sprite openedTrashBin;
    private Image image;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = openedTrashBin;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = closedTrashBin;
    }
    
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
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.settings.soundEffectsVolume;
        
        image = GetComponent<Image>();
    }

}

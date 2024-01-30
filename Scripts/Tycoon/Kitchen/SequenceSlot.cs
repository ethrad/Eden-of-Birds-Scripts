using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceSlot : MonoBehaviour
{
    public GameObject ingredientImage;
    public Text ingredientName;
    public GameObject plusImage;
    public GameObject[] cookerImages;
    
    public void UpdateSlot(IngredientSequence s)
    {
        ingredientImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Items/" + s.ingredientName);
        ingredientName.text = GameManager.instance.itemList[s.ingredientName].name;

        if (s.cookingSequences.Count == 0)
        {
            plusImage.SetActive(false);
            return;
        }
        
        for (int i = 0; i < s.cookingSequences.Count; i++)
        {
            cookerImages[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/Cookers/" + s.cookingSequences[i]);
        }
    }
}

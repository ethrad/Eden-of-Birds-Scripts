using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipePage : MonoBehaviour
{
    public GameObject foodImage;
    public GameObject foodName;
    public GameObject gold;
    public GameObject sequenceSlotPrefab;
    public GameObject sequenceList;

    public void UpdatePage(string menuName, Menu menu)
    {
        foodImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/Foods/" + menuName);
        foodName.GetComponent<Text>().text = menu.koreanName;
        gold.GetComponent<Text>().text = menu.gold.ToString();

        foreach (var s in menu.ingredientSequences)
        {
            GameObject tempSlot = Instantiate(sequenceSlotPrefab, sequenceList.transform);
            tempSlot.GetComponent<SequenceSlot>().UpdateSlot(s);
        }

        gameObject.SetActive(false);
    }
}

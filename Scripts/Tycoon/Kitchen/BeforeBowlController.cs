using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BeforeBowlController : MonoBehaviour, IPointerClickHandler
{
    public string ingredientName;
    public int gold;
    public GameObject backgroundImage;

    GameObject ingredientPrefab;

    bool isEmpty = false;

    public void UpdateBowl()
    {
        int count;

        if (ItemManager.instance.inventory.ContainsKey(ingredientName))
        {
            count = ItemManager.instance.inventory[ingredientName];
        }
        else
        {
            count = 0;
        }

        if (count <= 1)
        {
            backgroundImage.GetComponent<Image>().color = new Color(0.65f, 0.65f, 0.65f, 0.5f);
        }
        if (count == 0)
        {
            isEmpty = true;
            return;
        }

        GameObject tempIngredient = Instantiate(ingredientPrefab, this.transform);
        tempIngredient.transform.SetParent(this.gameObject.transform);
        tempIngredient.GetComponent<IngredientController>().Initialize(ingredientName);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (isEmpty == true)
        {
            if (ItemManager.instance.inventory.ContainsKey(ingredientName))
            {
                ItemManager.instance.inventory[ingredientName]++;
            }
            else
            {
                ItemManager.instance.inventory[ingredientName] = 1;
            }

            GameObject tempText = Instantiate(TycoonManager.instance.goldMinusUI);
            tempText.GetComponent<Text>().text = "-" + gold + " G";
            ItemManager.instance.gold -= gold;
            tempText.transform.SetParent(transform);
            tempText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            tempText.transform.SetParent(transform.parent);

            isEmpty = false;
            UpdateBowl();
        }
    }

    void Initialize()
    {
        ingredientPrefab = Resources.Load("Prefabs/Tycoon/DefaultIngredient") as GameObject;
        backgroundImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/" + ingredientName);
    }

    void Start()
    {
        Initialize();
        UpdateBowl();
    }
}

using System.Collections;
using System.Collections.Generic;
using Tycoon;
using UnityEngine;
using UnityEngine.UI;

namespace Tycoon
{
    public class BeforeBowlController : MonoBehaviour
    {
        public string ingredientName;
        public int price;
        public GameObject backgroundImage;
        public GameObject goldMinusUI;

        GameObject ingredientPrefab;

        private bool isEmpty = false;

        public void UpdateBowl()
        {
            int count;

            if (GameManager.instance.inventory.ContainsKey(ingredientName))
            {
                count = GameManager.instance.inventory[ingredientName];
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

            GameObject tempIngredient = Instantiate(ingredientPrefab, transform);
            tempIngredient.GetComponent<IngredientController>().Initialize(ingredientName);
        }

        public void OnBowlClicked()
        {
            if (isEmpty && GameManager.instance.gameData.PurchaseGold(price))
            {
                GameManager.instance.AddItemToInventory(ingredientName, 1);

                isEmpty = false;
                UpdateBowl();

                TycoonManager.instance.loss += price;
                GameObject tempText = Instantiate(goldMinusUI, transform);
                tempText.GetComponent<Text>().text = "-" + price + " G";

                tempText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 16f);
            }
        }

        public void Initialize(GameObject ingredientPrefab, string ingredientName, int price)
        {
            this.ingredientPrefab = ingredientPrefab;
            this.ingredientName = ingredientName;
            this.price = price;
            backgroundImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Items/" + ingredientName);
        }

    }
}
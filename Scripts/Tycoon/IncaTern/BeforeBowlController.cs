using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IncaTernTycoon
{
    public class BeforeBowlController : MonoBehaviour
    {
        public string ingredientName;
        public GameObject backgroundImage;

        GameObject ingredientPrefab;

        public void UpdateBowl()
        {
            GameObject tempIngredient = Instantiate(ingredientPrefab, transform);
            tempIngredient.GetComponent<IngredientController>().Initialize(ingredientName);
        }

        public void Initialize(GameObject ingredientPrefab, string ingredientName)
        {
            this.ingredientPrefab = ingredientPrefab;
            this.ingredientName = ingredientName;
            backgroundImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Items/" + ingredientName);
        }

    }
}

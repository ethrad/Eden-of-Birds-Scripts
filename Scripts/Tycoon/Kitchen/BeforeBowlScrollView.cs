using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tycoon
{
    public class BeforeBowlScrollView : MonoBehaviour
    {
        public GameObject beforeBowlPrefab;
        public GameObject beforeBowls;

        public GameObject ingredientPrefab;

        private void Initialize()
        {
            Dictionary<string, List<string>> menuOfTheMonth = GameManager.instance.menuOfTheMonth;
            HashSet<string> ingredientNames = new HashSet<string>();
            
            int max = menuOfTheMonth.ContainsKey("3") ? 4 : 3;
        
            for (int i = 1; i < max; i++)
            {
                for (int j = 0; j < menuOfTheMonth[i.ToString()].Count; j++)
                {
                    List<IngredientSequence> sequences = GameManager.instance.menus[menuOfTheMonth[i.ToString()][j]].ingredientSequences;

                    for (int k = 0; k < GameManager.instance.menus[menuOfTheMonth[i.ToString()][j]].ingredientSequences.Count; k++)
                    {
                        ingredientNames.Add(sequences[k].ingredientName);
                    }
                }
            }

            foreach (string ingredientName in ingredientNames)
            {
                GameObject tempBeforeBowl = Instantiate(beforeBowlPrefab, beforeBowls.transform);
                tempBeforeBowl.GetComponent<BeforeBowlController>().Initialize(ingredientPrefab, ingredientName, GameManager.instance.itemList[ingredientName].price);
                tempBeforeBowl.GetComponent<BeforeBowlController>().UpdateBowl();
            }
        }

        private void Start()
        {
            Initialize();
        }
    }
}
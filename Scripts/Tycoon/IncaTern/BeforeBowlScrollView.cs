using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncaTernTycoon
{
    public class BeforeBowlScrollView : MonoBehaviour
    {
        public GameObject beforeBowlPrefab;
        public GameObject beforeBowls;

        public GameObject ingredientPrefab;

        private void Initialize()
        {
            Dictionary<string, List<string>> ownRecipes = GameManager.instance.ownRecipes;
            HashSet<string> ingredientNames = new HashSet<string>();

            int max = ownRecipes.ContainsKey("3") ? 4 : 3;
        
            for (int i = 1; i < max; i++)
            {
                for (int j = 0; j < ownRecipes[i.ToString()].Count; j++)
                {
                    List<IngredientSequence> sequences = GameManager.instance.menus[ownRecipes[i.ToString()][j]].ingredientSequences;

                    for (int k = 0; k < GameManager.instance.menus[ownRecipes[i.ToString()][j]].ingredientSequences.Count; k++)
                    {
                        ingredientNames.Add(sequences[k].ingredientName);
                    }
                }
            }

            foreach (string ingredientName in ingredientNames)
            {
                GameObject tempBeforeBowl = Instantiate(beforeBowlPrefab, beforeBowls.transform);
                tempBeforeBowl.GetComponent<BeforeBowlController>().Initialize(ingredientPrefab, ingredientName);
                tempBeforeBowl.GetComponent<BeforeBowlController>().UpdateBowl();
            }
        }

        private void Start()
        {
            Initialize();
        }
    }
}
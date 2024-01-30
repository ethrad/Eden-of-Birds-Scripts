using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;


namespace Dungeon
{
    public class ByoPeddlerShopPanel : MonoBehaviour
    {
        // 레시피 1개, 아이템 1개, 장비 2개, 포션 1개
        public GameObject[] shopSlots;
        public Text owlGoldText;

        private void DecideRecipe(IReadOnlyList<ShopItem> items)
        {
            // 있는 레시피 제외해야함
            List<ShopItem> tempItems = new List<ShopItem>();
            bool hasRecipe;
            
            foreach (var item in items)
            {
                hasRecipe = false;
                for (int i = 1; i <= 3; i++)
                {
                    if (GameManager.instance.ownRecipes.TryGetValue(i.ToString(), out var recipes))
                    {
                        if (recipes.Contains(item.name))
                        {
                            hasRecipe = true;
                            break;
                        }
                    }

                }

                if (!hasRecipe)
                {
                    tempItems.Add(item);
                }
            }

            if (tempItems.Count == 0)
            {
                shopSlots[0].SetActive(false);
                return;
            }
            
            int random = Random.Range(0, tempItems.Count);
            shopSlots[0].GetComponent<ShopSlot>().Initialize(tempItems[random]);
        }

        private void DecideItem(List<ShopItem> items)
        {
            int random = Random.Range(0, items.Count);
            shopSlots[1].GetComponent<ShopSlot>().Initialize(items[random]);
        }
        
        private void DecideEquipments(List<ShopItem> items)
        {
            int random = Random.Range(0, items.Count);
            shopSlots[2].GetComponent<ShopSlot>().Initialize(items[random]);
            items.RemoveAt(random);
            
            random = Random.Range(0, items.Count);
            shopSlots[3].GetComponent<ShopSlot>().Initialize(items[random]);
        }
        
        private void DecidePotion(IReadOnlyList<ShopItem> items)
        {
            shopSlots[4].GetComponent<ShopSlot>().Initialize(items[0]);
        }

        public void OnExitButtonClicked()
        {
            gameObject.SetActive(false);
            
            if (DungeonManager.instance.isShopItemPurchased)
            {
                DungeonManager.instance.byoPeddler.GetComponent<ByoPeddlerController>().OnVomitAnimation();
            }
            else
            {
                DungeonManager.instance.StartPlayerMoving(true);
            }
            Time.timeScale = 1;
        }

        public void UpdateGoldPanel()
        {
            owlGoldText.text = GameManager.instance.gameData.gold.ToString();
        }
        
        public void Initialize()
        {
            var shopItems = DungeonManager.instance.shopItems;
            DungeonManager.instance.isShopItemPurchased = false;

            if (!DungeonManager.instance.isRecipeInitialized)
            {
                DungeonManager.instance.isRecipeInitialized = true;
                DecideRecipe(shopItems["recipe"]);
            }

            DecideItem(shopItems["item"]);
            DecideEquipments(shopItems["equipment"].ToList());
            DecidePotion(shopItems["potion"]);
        }
    }
}


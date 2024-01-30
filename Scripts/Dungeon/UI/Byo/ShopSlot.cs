using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Dungeon
{
    public class ShopSlot : MonoBehaviour
    {
        public Image image;
        public Text nameText;
        public Text descriptionText;
        public Text priceText;

        public Button buyButton;

        protected ShopItem shopItem;

        public virtual void OnBuyButtonClicked()
        {
            if (GameManager.instance.gameData.PurchaseGold(shopItem.price))
            {
                buyButton.interactable = false;
                buyButton.transform.GetChild(0).GetComponent<Text>().text = "구매 완료";

                switch (shopItem.type)
                {
                    case "recipe":
                        GameManager.instance.GetRecipe(shopItem.name);
                        break;
                    case "item":
                        GameManager.instance.AddItemToInventory(shopItem.name, shopItem.count);
                        break;
                    case "potion":
                        DungeonManager.instance.player.GetComponent<PlayerController>().Heal(shopItem.count);
                        break;
                }

                DungeonManager.instance.byoPeddlerShopPanel.GetComponent<ByoPeddlerShopPanel>().UpdateGoldPanel();
                DungeonManager.instance.isShopItemPurchased = true;
            }
        }

        public void Initialize(ShopItem shopItem)
        {
            this.shopItem = shopItem;
            
            Sprite[] sprites = Resources.LoadAll<Sprite>("Dots/Dungeon/Shop/" + shopItem.type + "/" + shopItem.name);

            image.sprite = shopItem.type == "item" ? sprites[1] : sprites[0];
            nameText.text = shopItem.koreanName;
            descriptionText.text = shopItem.description;
            priceText.text = shopItem.price + " G";

            buyButton.interactable = true;
        }
    }
}

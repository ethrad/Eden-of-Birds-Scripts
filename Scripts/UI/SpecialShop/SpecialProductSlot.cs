using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SpecialShop
{
    public class SpecialProductSlot : MonoBehaviour
    {
        public Image icon;
        private ShopData data;
        public GameObject textGrid;
        public GameObject textPre;

        public int itemIdx;
        
        public string itemName;
        [HideInInspector]
        public GameObject korName;
        [HideInInspector]
        public GameObject fPrice;
        [HideInInspector]
        public GameObject countText;
        
        public GameObject cPrice;

        public void UpdateItemInfo(string wantName, int num)
        {
            itemName = wantName;
            itemIdx = num;
            icon.sprite = Resources.Load<Sprite>("Dots/SpecialShopItems/" + itemName);
            korName = Instantiate(textPre, textGrid.transform);
            korName.GetComponent<Text>().text = GameManager.instance.itemList[itemName].name;
            
            fPrice = Instantiate(textPre, textGrid.transform);
            fPrice.GetComponent<Text>().text = GameManager.instance.specialShopItemList[num].featherPrice.ToString() + "깃털";

            countText = Instantiate(textPre, textGrid.transform);
            countText.GetComponent<Text>().text = GameManager.instance.specialShopItemList[num].count.ToString() + "개";
            
            if (GameManager.instance.specialShopItemList[num].cashPrice > 0)
            {
                cPrice = Instantiate(textPre, textGrid.transform);
                cPrice.GetComponent<Text>().text = GameManager.instance.specialShopItemList[num].cashPrice.ToString() + "￦";
            }
        }

        public void OnClickedShowDetailButton()
        {
            this.transform.parent.parent.parent.parent.parent.GetComponent<SpecialShopItemInfoPanel>().SetDetailPanel(itemName, itemIdx);
        }
    }
}

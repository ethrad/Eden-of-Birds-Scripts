using System.Collections;
using System.Collections.Generic;
using SpecialShop;
using UnityEngine;
using UnityEngine.UI;

public class SparrowShopSlot : MonoBehaviour
{
    public Image icon;
    public string itemName;
    public Text productName;
    public Text price;
    
    public void UpdateSlot(string wantItemName)
    {
        itemName = wantItemName;
        icon.sprite = Resources.Load<Sprite>("Dots/SpecialShopItems/" + itemName);
        productName.text = GameManager.instance.itemList[itemName].name;
        price.text = GameManager.instance.sparrowShopList[itemName].price.ToString();
    }

    public void OnClickedPurchaseButton()
    {
        this.transform.parent.parent.parent.parent.parent.GetComponent<SparrowShopPanel>().ShowDetailPanel(itemName);

    }
    
}

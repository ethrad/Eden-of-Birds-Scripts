using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureSlot : MonoBehaviour
{
    public Image icon;
    public string  itemName;
    public Text productName;
    public Text price;
    public Text leftCount;

    public void UpdateTreasureInfo(string wantName)
    {
        itemName = wantName;
        icon.sprite = Resources.Load<Sprite>("Dots/SpecialShopItems/" + itemName);
        productName.text = GameManager.instance.itemList[wantName].name;
        price.text = GameManager.instance.residentTreasureShopList[wantName].featherPrice.ToString() + " 깃털";
        leftCount.text = "남은 개수 : " + GameManager.instance.residentTreasureShopList[wantName].count.ToString();
    }

    public void OnClickedPurchaseButton()
    {
        this.transform.parent.parent.parent.parent.parent.GetComponent<ResidentTreasureShopPanel>().ShowDetailPanel(itemName);
    }

    public void UpdateItemCount(int wantCount)
    {
        leftCount.text = "남은 개수 : " + wantCount.ToString();
    }
}

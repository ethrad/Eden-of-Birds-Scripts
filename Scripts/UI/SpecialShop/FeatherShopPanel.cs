using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class FeatherShopPanel : MonoBehaviour
{
    public GameObject purchasePanel;
    public Text currentFeatherCountText;

    public Text purchaseFeatherCountText;
    
    private void PurchaseEffect(string log)
    {
        purchaseFeatherCountText.text = log;
        purchasePanel.SetActive(true);
    }

    public void OnPurchaseComplete(Product product)
    {
        string log = null;
        switch (product.definition.id)
        {
            case "ad_free":
                log = "광고 제거권";
                GameManager.instance.gameData.purchasedAdFree = true;
                AdFreeSlot.SetActive(false);
                break;
            case "feather1":
                log = "깃털 100개";
                GameManager.instance.gameData.GetFeather(100);
                break;
            
            case "feather2":
                log = "깃털 500개";
                GameManager.instance.gameData.GetFeather(500);
                break;
            
            case "feather3":
                log = "깃털 1000개";
                GameManager.instance.gameData.GetFeather(1000);
                break;
        }
        
        currentFeatherCountText.text = GameManager.instance.gameData.feather.ToString();
        GameManager.instance.WriteGameData();
        GetComponent<ItemPurchaseButton>().PurchaseSuccess();
        PurchaseEffect(log + " 구매 완료!");
        
        BackendManager.Instance.PurchaseReceipt(product);
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription reason) 
    {
        Debug.Log(product.definition.id + " 구매 실패. 사유: " + reason);
        GetComponent<ItemPurchaseButton>().PurchaseFail();
        PurchaseEffect("구매를 실패하였습니다.\n사유: " + reason);
    }

    public GameObject AdFreeSlot;
    
    public void Initialize()
    {
        if (!GameManager.instance.gameData.purchasedAdFree)
        {
            AdFreeSlot.SetActive(true);
        }
    }
}

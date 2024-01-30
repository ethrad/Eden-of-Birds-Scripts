using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class AdFreeBanner : MonoBehaviour
{
    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == "ad_free")
        {
            GameManager.instance.gameData.purchasedAdFree = true;
            GameManager.instance.WriteGameData();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(product.definition.id + " 구매 실패.");
        }
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription reason) 
    {
        Debug.Log(product.definition.id + " 구매 실패. 사유: " + reason);
    }
    
    public void Initialize()
    {
        if (GameManager.instance.gameData.purchasedAdFree)
        {
            gameObject.SetActive(false);
        }
    }
}

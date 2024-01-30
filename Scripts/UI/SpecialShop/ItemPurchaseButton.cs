using System.Collections;
using System.Collections.Generic;
using MoreMountains.InventoryEngine;
using UnityEngine;

public class ItemPurchaseButton : MonoBehaviour
{
    //public GameObject descPanel;
    public GameObject purchasePanel;
    public GameObject purchaseSuccessPanel;
    public GameObject purchaseFailPanel;
    
    
    public void PurchaseSuccess()
    {
        //descPanel.SetActive(false);
        purchaseFailPanel.SetActive(false);
        purchaseSuccessPanel.SetActive(true);
        purchasePanel.GetComponent<Animator>().SetTrigger("Purchase");
    }
    
    public void PurchaseFail()
    {
        //descPanel.SetActive(false);
        purchaseFailPanel.SetActive(true);
        purchaseSuccessPanel.SetActive(false);
        purchasePanel.GetComponent<Animator>().SetTrigger("Purchase");
    }

    public void OnClickedCashButton()
    {
        //현금으로 아이템 구매하는건 없음
        // descPanel.SetActive(false);
        // purchasePanel.GetComponent<Animator>().SetTrigger("Purchase");
    }
}

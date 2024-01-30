using SpecialShop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject shopPanelWithAd;

    public void OnClickedShopButton()
    {
        if(GameManager.instance.gameData.purchasedAdFree)//광고 제거권 있으면
        {
            shopPanelWithAd.SetActive(false);
            shopPanel.SetActive(true);
            shopPanel.GetComponent<SpecialShopPanel>().OnClickedShopButton();

        }
        else
        {
            LoadMobileAD.Instance.LoadBannerTop();
            shopPanelWithAd.SetActive(true);
            shopPanel.SetActive(false);
            shopPanelWithAd.GetComponent<SpecialShopPanel>().OnClickedShopButton();
        }
    }

    public void DestroyAdMob()
    {
        LoadMobileAD.Instance.DestroyBannerView();
    }
}

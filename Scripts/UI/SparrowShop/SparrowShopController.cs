using SpecialShop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparrowShopController : MonoBehaviour
{
    public GameObject sparrowShopPanel;
    public GameObject sparrowShopPanelWithAd;
    public void OnClickedShopButton()
    {
        if (GameManager.instance.gameData.purchasedAdFree)//¿÷¿∏∏È
        {
            sparrowShopPanelWithAd.SetActive(false);
            sparrowShopPanel.SetActive(true);
            sparrowShopPanel.GetComponent<SparrowShopPanel>().OnClickedShopButton();

        }
        else
        {
            LoadMobileAD.Instance.LoadBannerTop();
            sparrowShopPanelWithAd.SetActive(true);
            sparrowShopPanel.SetActive(false);
            sparrowShopPanelWithAd.GetComponent<SparrowShopPanel>().OnClickedShopButton();

        }
    }

    public void ExitPanel()
    {
        LoadMobileAD.Instance.DestroyBannerView();
    }
}

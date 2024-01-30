using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using SpecialShop;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpecialShopItemInfoPanel : MonoBehaviour
{
    public RectTransform itemInfoPanel;
    public GameObject content;
    public GameObject slotPre;
    public GameObject cashButton;
    public GameObject featherButton;



    public GameObject descriptionPanel;
    public RectTransform descriptionPanelBackground;
    public Text itemNameText;
    public Text desc;

    public GameObject featherPanel;
    public RectTransform featherItemInfoPanel;
    
    public Text ConfirmationText;
    
    private string itemName;
    private int idx;
    

    public void Start()
    {        
        UpdatePanel();
        SetItemInfoPanelSize();
    }
    
    public void UpdatePanel()
    {
        content.GetComponent<GridLayoutGroup>().constraintCount = GameManager.instance.specialShopItemList.Count;
        featherPanel.SetActive(false);
        for (int i = 0; i < GameManager.instance.specialShopItemList.Count; i++)
        {
            GameObject tempSlot = Instantiate(slotPre, content.transform);
            tempSlot.GetComponent<SpecialProductSlot>().UpdateItemInfo(GameManager.instance.specialShopItemList[i].name, i);
        }
    }
    

    public void OnClickedPlusButton()
    {
        featherItemInfoPanel = itemInfoPanel;
        featherPanel.SetActive(true);
    }

    //광고 붙었을 때 사이즈 변경해주는거
    public void SetItemInfoPanelSize()
    {
        if (GameManager.instance.gameData.purchasedAdFree == false)//광고제거권 없으면 사이즈 변경해주기
        {
            itemInfoPanel.offsetMax = new Vector2(-130, -GameManager.instance.gameObject.GetComponent<LoadMobileAD>().DebugBannerViewHeight() - 30);
            itemInfoPanel.offsetMin = new Vector2(130, 0);
            descriptionPanelBackground.offsetMax = new Vector2(-130, -GameManager.instance.gameObject.GetComponent<LoadMobileAD>().DebugBannerViewHeight() - 30);
            descriptionPanelBackground.offsetMin = new Vector2(130, 0);
        }
    }

    public void SetDetailPanel(string itemName, int itemIdx)
    {

        this.itemName = itemName;
        idx = itemIdx;
        descriptionPanel.SetActive(true);
        itemNameText.text = GameManager.instance.itemList[itemName].name;
        desc.text = GameManager.instance.itemList[itemName].description;
        
        if (GameManager.instance.specialShopItemList[itemIdx].cashPrice == 0)
        {
            cashButton.SetActive(false);
            featherButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(1150, 388);
        }
        else
        {
            cashButton.SetActive(true);
            featherButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(577, 388);
        }
        ConfirmationText.text = itemNameText.text + " 구매완료!";
    }

    public void PurchaseItem()
    {
        //구매불가
        if (GameManager.instance.gameData.feather < GameManager.instance.specialShopItemList[idx].featherPrice)
        {//깃털 부족
            descriptionPanel.GetComponent<ItemPurchaseButton>().PurchaseFail();
        }
        else
        {
            GameManager.instance.gameData.PurchaseFeather(GameManager.instance.specialShopItemList[idx].featherPrice);
            
            //인벤토리추가
            GameManager.instance.AddItemToInventory(itemName, GameManager.instance.specialShopItemList[idx].count);
            
            GameManager.instance.WriteInventory();
            GameManager.instance.WriteGameData();
            descriptionPanel.GetComponent<ItemPurchaseButton>().PurchaseSuccess();
            transform.parent.GetComponent<SpecialShopPanel>().UpdateFeatherPanel();
        }
    }
}

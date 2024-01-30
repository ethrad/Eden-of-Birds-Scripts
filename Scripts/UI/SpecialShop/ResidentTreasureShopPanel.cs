using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpecialShop;
using UnityEngine.UI;

public class ResidentTreasureShopPanel : MonoBehaviour
{
    public RectTransform itemInfoPanel;
    public GameObject content;
    public GameObject slotPre;
    private List<GameObject> slotList = new List<GameObject>();
    public GameObject decsriptionPanel;
    public RectTransform descriptionPanelBackground;


    public Text itemName;
    public Text itemDesc;
    public Text ConfirmationText;

    public Text itemCountText;
    public Slider slider;

    private string itemResidentName;

    private int min;
    private int max;

    public void Start()
    {
        content.GetComponent<GridLayoutGroup>().constraintCount =
            GameManager.instance.residentTreasureShopList.Count;

        foreach (KeyValuePair<string, ResidentTreasureShopData> item in GameManager.instance
                     .residentTreasureShopList)
        {
            GameObject temp = Instantiate(slotPre, content.transform);
            temp.GetComponent<TreasureSlot>().UpdateTreasureInfo(item.Key);
            slotList.Add(temp);
        }

        SetTreasureShopInfoPanelSize();
    }


    //광고 붙었을 때 사이즈 변경해주는거
    public void SetTreasureShopInfoPanelSize()
    {
        if (GameManager.instance.gameData.purchasedAdFree == false)//광고제거권 없으면 사이즈 변경해주기
        {
            itemInfoPanel.offsetMax = new Vector2(-130, -GameManager.instance.gameObject.GetComponent<LoadMobileAD>().DebugBannerViewHeight() - 30);
            itemInfoPanel.offsetMin = new Vector2(130, 0);
            descriptionPanelBackground.offsetMax = new Vector2(-130, -GameManager.instance.gameObject.GetComponent<LoadMobileAD>().DebugBannerViewHeight() - 30);
            descriptionPanelBackground.offsetMin = new Vector2(130, 0);
        }
    }

    public void ShowDetailPanel(string wantName)
    {
        if (GameManager.instance.residentTreasureShopList[wantName].count > 0)
        {
            itemResidentName = wantName;
            
            decsriptionPanel.SetActive(true);
            itemName.text = GameManager.instance.itemList[wantName].name;
            itemDesc.text = GameManager.instance.itemList[wantName].description;
            max = GameManager.instance.gameData.feather / GameManager.instance.residentTreasureShopList[itemResidentName].featherPrice;
            if (max > GameManager.instance.residentTreasureShopList[itemResidentName].count)
                max = GameManager.instance.residentTreasureShopList[itemResidentName].count;
            itemCountText.text = "0";
            SetSlider();
        }
        ConfirmationText.text = itemName.text + " 구매완료!";
    }

    public void OnClickedPlusOrMinusButton(string wantBtn)
    {
        if (wantBtn == "Plus")
            slider.value++;
        if (wantBtn == "Minus")
            slider.value--;
    }

    public void OnClickedPurchaseButton()
    {
        //0일때 구매 안되도록
        int price = GameManager.instance.residentTreasureShopList[itemResidentName].featherPrice * itemCount;
        if (GameManager.instance.gameData.PurchaseFeather(price) && itemCount != 0)
        {
            GameManager.instance.residentTreasureShopList[itemResidentName].count -= itemCount;
            //아이템 몇 개 남았는지 업데이트 해줘야 됨
            GameManager.instance.WriteResidentTreasureShop();
            
            //인벤토리추가
            GameManager.instance.AddItemToInventory(itemResidentName, itemCount);
            
            GameManager.instance.WriteInventory();
            
            UpdatePanel();
            transform.parent.GetComponent<SpecialShopPanel>().UpdateFeatherPanel();
            decsriptionPanel.GetComponent<ItemPurchaseButton>().PurchaseSuccess();
        }

    }

    public void UpdatePanel()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            string tempName = slotList[i].GetComponent<TreasureSlot>().itemName;
            slotList[i].GetComponent<TreasureSlot>()
                .UpdateItemCount(GameManager.instance.residentTreasureShopList[tempName].count);
        }
    }

    private void SetSlider()
    {
        ResetSlider();
        slider.onValueChanged.AddListener(Function_Slider);
    }

    private int itemCount;

    private void Function_Slider(float _value)
    {
        itemCount = Convert.ToInt32(_value);
        itemCountText.text = _value.ToString();
    }

    private void ResetSlider()
    {
        slider.onValueChanged.RemoveAllListeners();
        slider.maxValue = max;
        slider.minValue = min;
        slider.wholeNumbers = true;
        slider.value = 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialShop
{
    public class SparrowShop : CSVData
    {
        public string itemName;
        public int price;
    }

    public class SparrowShopPanel : MonoBehaviour
    {
        public GameObject interactionList;
        public RectTransform itemInfoPanel;
        public GameObject countPanel;
        public GameObject content;
        public GameObject slotPre;
        public GameObject descriptionPanel;
        public RectTransform descriptionPanelBackground;
        public Slider slider;

        public Text itemName;
        public Text itemCountText;
        public Text gold;
        public Text noticeText;



        private string sparrowShopItemName;
        private int min;
        private int max;
        private int itemCount;

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

        public void OnClickedShopButton()
        {
            GetComponent<UIPanelEffect>().PanelFadeIn();
            UpdatePanel();
        }

        public void UpdatePanel()
        {
            if(content.transform.childCount > 0)
            {
                for (int i = 0; i < content.transform.childCount; i++)
                {
                    Destroy(content.transform.GetChild(i).gameObject);
                }
            }
            gold.text = GameManager.instance.gameData.gold.ToString();
            descriptionPanel.SetActive(false);
            content.GetComponent<GridLayoutGroup>().constraintCount = GameManager.instance.sparrowShopList.Count;
            foreach (KeyValuePair<string, SparrowShop> item in GameManager.instance.sparrowShopList)
            {
                GameObject temp = Instantiate(slotPre, content.transform);
                temp.GetComponent<SparrowShopSlot>().UpdateSlot(item.Key);
            }
        }

        public void ShowDetailPanel(string wantName)
        {
            sparrowShopItemName = wantName;
            itemName.text = GameManager.instance.itemList[wantName].name + " 구매하시겠습니까?";
            descriptionPanel.SetActive(true);
            max = 50;
            min = 0;
            itemCountText.text = "0";
            SetSlider();
        }

        public void OnClickedPurchaseButton()
        {
            if(itemCount>0)
            {
                int price = GameManager.instance.sparrowShopList[sparrowShopItemName].price * itemCount;
                if (GameManager.instance.gameData.PurchaseGold(price))
                {
                    descriptionPanel.SetActive(false);
                    GameManager.instance.AddItemToInventory(sparrowShopItemName, itemCount);
                    noticeText.text = GameManager.instance.itemList[sparrowShopItemName].name + "구매가 완료되었습니다.";
                    gold.text = GameManager.instance.gameData.gold.ToString();
                    descriptionPanel.GetComponent<ItemPurchaseButton>().PurchaseSuccess();
                }
                else
                {
                    descriptionPanel.GetComponent<ItemPurchaseButton>().PurchaseFail();
                }

                GameManager.instance.WriteInventory();
                GameManager.instance.WriteGameData();
            }
        }

        public void ExitButton()
        {
            interactionList.SetActive(true);
            countPanel.SetActive(false);
            TownManager.instance.OnBasePanel();
        }
        
        public void PlusOrMinus(string wantBtn)
        {
            if (wantBtn == "Plus")
                slider.value++;
            if (wantBtn == "Minus")
                slider.value--;
        }
        
        private void ResetSlider()
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.maxValue = max;
            slider.minValue = min;
            slider.wholeNumbers = true;
            slider.value = 0;
        }
        
        private void Function_Slider(float _value)
        {
            itemCount = Convert.ToInt32(_value);
            itemCountText.text = _value.ToString();
        }
        
        private void SetSlider()
        {
            ResetSlider();
            slider.onValueChanged.AddListener(Function_Slider);
        }
    }
}
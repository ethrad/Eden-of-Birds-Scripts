using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialShop
{
    public class ShopData : CSVData
    {
        public string name;
        public int count;
        public int featherPrice;
        public int cashPrice;

        public override void csvToClass(string[] csvArray)
        {
            name = csvArray[0];
            count = int.Parse(csvArray[1]);
            featherPrice = int.Parse(csvArray[2]);
            cashPrice = int.Parse(csvArray[3]);
        }
    }

    public class ResidentTreasureShopData
    {
        public int count;
        public int featherPrice;
        public List<int> requiredTreasureCount = new List<int>();
    }

    public class SpecialShopPanel : MonoBehaviour
    {
        public GameObject specialShopPanel;
        public Text itemFeatherText;
        public Text treasureFeatherText;
        public Text featherShopText;
        public void OnClickedShopButton()
        {
            specialShopPanel.SetActive(true);
            specialShopPanel.GetComponent<UIPanelEffect>().PanelFadeIn();
            specialShopPanel.GetComponent<SpecialShopItemInfoPanel>().SetItemInfoPanelSize();
            UpdateFeatherPanel();
        }

        public void UpdateFeatherPanel()
        {
            featherShopText.text = itemFeatherText.text = treasureFeatherText.text = GameManager.instance.gameData.feather.ToString();
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon
{
    public class ShopEquipmentSlot : ShopSlot
    {
        public GameObject equipmentPanel;
        
        public override void OnBuyButtonClicked()
        {
            var equipmentQuery = from equipment in DungeonManager.instance.equipmentList
                where equipment.name == shopItem.name
                select equipment;

            if (GameManager.instance.gameData.PurchaseGold(shopItem.price))
            {
                equipmentPanel.GetComponent<ShopEquipmentPanel>().Initialize(gameObject, equipmentQuery.First(), shopItem.price);
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon
{
    public class ShopEquipmentPanel : ChestPanel
    {
        private GameObject shopEquipmentSlot;
        private int price;
        
        public override void OnChangeButtonClicked()
        {
            DungeonManager.instance.ChangeEquipment(gottenEquipment);
            DungeonManager.instance.byoPeddlerShopPanel.GetComponent<ByoPeddlerShopPanel>().UpdateGoldPanel();
            shopEquipmentSlot.GetComponent<ShopEquipmentSlot>().buyButton.interactable = false;
            shopEquipmentSlot.GetComponent<ShopEquipmentSlot>().buyButton.transform.GetChild(0).GetComponent<Text>().text = "구매 완료";
            gameObject.SetActive(false);
        }

        public override void OnExitButtonClicked()
        {
            GameManager.instance.gameData.GetGold(price);
            gameObject.SetActive(false);
        }

        public void Initialize(GameObject slot, Equipment equipment, int price)
        {
            shopEquipmentSlot = slot;
            gottenEquipment = equipment;
            this.price = price;
            if (DungeonManager.instance.equipments[equipment.type] != null)
            {
                Equipment tempEquipment = DungeonManager.instance.equipments[equipment.type];
                currentEquipmentImage.sprite = Resources.Load<Sprite>("Dots/Dungeon/Equipments/" + tempEquipment.type + "/" + tempEquipment.grade);
                currentEquipmentText.text = "HP : " + tempEquipment.HP + "\nATK : " + tempEquipment.ATK + "\nDEF : " + tempEquipment.DEF;
            }

            gottenEquipmentImage.sprite = Resources.Load<Sprite>("Dots/Dungeon/Equipments/" + equipment.type + "/" + equipment.grade);
            gottenEquipmentText.text = "HP : " + equipment.HP + "\nATK : " + equipment.ATK + "\nDEF : " + equipment.DEF;
            
            gameObject.SetActive(true);
        }
    }

}

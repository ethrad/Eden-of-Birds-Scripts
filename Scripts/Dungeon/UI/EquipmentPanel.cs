using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon
{
    public class EquipmentPanel : MonoBehaviour
    {
        public GameObject[] equipmentSlots;

        public Image selectedEquipmentImage;
        public Text description;

        public void Initialize()
        {
            int i = 0;
            foreach (var p in DungeonManager.instance.equipments)
            {
                if (p.Value != null)
                {
                    equipmentSlots[i].GetComponent<EquipmentSlot>().UpdateSlot(p.Value);
                }

                i++;
            }

            if (equipmentSlots[0].GetComponent<EquipmentSlot>().equipment != null)
            {
                UpdateSelectedEquipment(equipmentSlots[0].GetComponent<EquipmentSlot>().equipment);
            }
        }

        public void Reset()
        {
            selectedEquipmentImage.sprite = Resources.Load<Sprite>("Dots/transparent");
            description.text = "체력 : \n공격력 : \n방어력 : ";
            
            gameObject.SetActive(false);
        }

        private void UpdateSelectedEquipment(Equipment equipment)
        {
            selectedEquipmentImage.sprite = Resources.Load<Sprite>("Dots/Dungeon/Equipments/" + equipment.type + "/" + equipment.grade);
            description.text = "체력 : " + equipment.HP + "\n공격력 : " + equipment.ATK + "\n방어력 : " + equipment.DEF;
        }

        public void OnSlotClicked(Button button)
        {
            Equipment tempEquipment = button.GetComponent<EquipmentSlot>().equipment;

            if (tempEquipment != null)
            {
                UpdateSelectedEquipment(tempEquipment);
            }
        }
    }
}

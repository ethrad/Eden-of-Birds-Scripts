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
            description.text = "ü�� : \n���ݷ� : \n���� : ";
            
            gameObject.SetActive(false);
        }

        private void UpdateSelectedEquipment(Equipment equipment)
        {
            selectedEquipmentImage.sprite = Resources.Load<Sprite>("Dots/Dungeon/Equipments/" + equipment.type + "/" + equipment.grade);
            description.text = "ü�� : " + equipment.HP + "\n���ݷ� : " + equipment.ATK + "\n���� : " + equipment.DEF;
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

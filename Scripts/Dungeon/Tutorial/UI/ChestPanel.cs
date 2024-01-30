using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeon;

namespace Tutorial
{
    public class ChestPanel : MonoBehaviour
    {
        public Image currentEquipmentImage;
        public Text currentEquipmentText;
        public Image gottenEquipmentImage;
        public Text gottenEquipmentText;
        protected Equipment gottenEquipment;

        public virtual void OnExitButtonClicked()
        {
            Time.timeScale = 1;
            TutorialDungeonManager.instance.StartPlayerMoving(false);
            gameObject.SetActive(false);
            
            currentEquipmentImage.sprite = Resources.Load<Sprite>("Dots/transparent");
            currentEquipmentText.text = "";
        }

        public virtual void OnChangeButtonClicked()
        {
            TutorialDungeonManager.instance.ChangeEquipment(gottenEquipment);
            Time.timeScale = 1;
            TutorialDungeonManager.instance.StartPlayerMoving(false);
            gameObject.SetActive(false);
        }


        public void Initialize(Equipment equipment)
        {
            TutorialDungeonManager.instance.StopPlayerMoving();
            
            gottenEquipment = equipment;

            if (TutorialDungeonManager.instance.equipments[equipment.type] != null)
            {
                Equipment tempEquipment = TutorialDungeonManager.instance.equipments[equipment.type];
                currentEquipmentImage.sprite = Resources.Load<Sprite>("Dots/Dungeon/Equipments/" + tempEquipment.type + "/" + tempEquipment.grade);
                currentEquipmentText.text = "HP : " + tempEquipment.HP + "\nATK : " + tempEquipment.ATK + "\nDEF : " + tempEquipment.DEF;
            }

            gottenEquipmentImage.sprite = Resources.Load<Sprite>("Dots/Dungeon/Equipments/" + equipment.type + "/" + equipment.grade);
            gottenEquipmentText.text = "HP : " + equipment.HP + "\nATK : " + equipment.ATK + "\nDEF : " + equipment.DEF;
        }
    }
}


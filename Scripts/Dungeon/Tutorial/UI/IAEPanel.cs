using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class IAEPanel : MonoBehaviour
    {
        public GameObject inventoryPanel;
        public GameObject equipmentPanel;

        public Button inventoryButton;
        public Button equipmentButton;

        public void OnIAEButtonClicked()
        {
            Time.timeScale = 0;
            //TutorialDungeonManager.instance.OffBasePanel();

            inventoryPanel.GetComponent<InventoryPanel>().Initialize();
            equipmentPanel.GetComponent<EquipmentPanel>().Initialize();
            
            inventoryButton.interactable = false;
            equipmentButton.interactable = true;
            inventoryPanel.SetActive(true);
            gameObject.SetActive(true);
        }

        public void OnInventoryButtonClicked()
        {
            inventoryButton.interactable = false;
            equipmentButton.interactable = true;

            equipmentPanel.SetActive(false);
            inventoryPanel.SetActive(true);
        }

        public void OnEquipmentButtonClicked()
        {
            inventoryButton.interactable = true;
            equipmentButton.interactable = false;

            inventoryPanel.SetActive(false);
            equipmentPanel.SetActive(true);
        }

        public void OnExitButtonClicked()
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            //DungeonManager.instance.OnBasePanel();

            inventoryPanel.GetComponent<InventoryPanel>().Reset();
            equipmentPanel.GetComponent<EquipmentPanel>().Reset();
        }
    }
}
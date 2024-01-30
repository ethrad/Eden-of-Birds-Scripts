using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;


public class PresentInventoryPanel : MonoBehaviour
{
    #region ResidentPresent

    public Dictionary<string, Dictionary<string, int>> residentPresent;

    void ReadPresentList()
    {
        residentPresent = IOManager.instance.ReadPresent("ResidentPresent");
    }

    #endregion
    
    public GameObject countPanel;
    public GameObject interactionList;
    
    public GameObject itemSlotPrefab;
    public GameObject itemSlotPanel;
    public GameObject itemName;
    public Image itemImage;
    private string itemEngName;

    public GameObject cancelPanel;
    public Text cancelText;

    public GameObject diary;
    
    public void OnPresentInventoryButtonClicked()
    {
        foreach (KeyValuePair<string, int> item in GameManager.instance.inventory)
        {
            if (item.Value > 0)
            {
                if(GameManager.instance.itemList[item.Key].attribute == "treasure") continue;
                GameObject tempItem = Instantiate(itemSlotPrefab, itemSlotPanel.transform);
                tempItem.GetComponent<PresentItemSlotController>().UpdateItem(item.Key, item.Value);
            }
        }
        gameObject.SetActive(true);
    }

    public void UpdateDescriptionPanel(string itemName)
    {
        itemEngName = itemName;
        itemImage.sprite = Resources.Load<Sprite>("Dots/Items/24x24/" + itemName);
        this.itemName.GetComponent<Text>().text = GameManager.instance.itemList[itemName].name;
    }

    public void OnGivingButtonClicked()
    {
        if(itemEngName != "invitationResident") //초대권이 아닐때
        {
            if(GameManager.instance.residentFriendships[TownManager.instance.residentName].gavePresentInThisMonth == false)
            {
                int value = residentPresent[TownManager.instance.residentName][itemEngName];

                GameManager.instance.inventory[itemEngName]--;
                GameManager.instance.residentFriendships[TownManager.instance.residentName].gavePresentInThisMonth = true;
                GameManager.instance.SetFriendShip(TownManager.instance.residentName, value, true);
                diary.GetComponent<DiaryCheckMark>().UpdateCheckMark();
                OnExitButtonClicked();
            }
            else
            {
                OnExitButtonClicked();
                diary.GetComponent<DiaryCheckMark>().UpdateCheckMark();
                DialogueManager.StartConversation(TownManager.instance.residentName+"_gotPresent");
            }

        }
        else //초대권일때 
        {
            
            //애장품을 줘버리는 경우
            
            if (TownManager.instance.residentName == "parrot")
            {
                OnExitButtonClicked();
                DialogueManager.StartConversation(TownManager.instance.residentName+"_"+itemEngName);
                return;
            }
            
            //초대된 주민 있고, 앵무새 아닐 때
            if(GameManager.instance.invitedResidentName != "" && TownManager.instance.residentName != "parrot" )
            {
                cancelText.text = "초대권을 이미 사용했습니다. 주점 영업이 끝날 때까지 사용할 수 없습니다.";
                cancelPanel.SetActive(true);
                return;
            }
            else if(GameManager.instance.invitedResidentName == "") //초대된 주민 없을 때
            {
                OnExitButtonClicked();
                GameManager.instance.inventory[itemEngName]--;
                GameManager.instance.invitedResidentName = TownManager.instance.residentName;
                DialogueManager.StartConversation(TownManager.instance.residentName+"_"+itemEngName);
            }
        }

        GameManager.instance.WriteResidentFriendShip();
    }
    
    public void OnExitButtonClicked()
    {
        while (itemSlotPanel.transform.childCount > 0)
        {
            DestroyImmediate(itemSlotPanel.transform.GetChild(0).gameObject);
        }

        gameObject.SetActive(false);
        interactionList.SetActive(true);
        countPanel.SetActive(false);
        TownManager.instance.OnBasePanel();
        
        itemImage.sprite = Resources.Load<Sprite>("Dots/Dungeon/Shop/item/None");
        this.itemName.GetComponent<Text>().text = "";
    }

    private void Start()
    {
        ReadPresentList();   
        cancelPanel.SetActive(false);
    }
}

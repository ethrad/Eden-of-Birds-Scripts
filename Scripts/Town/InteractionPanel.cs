using System;
using BackEnd.Game.GameInfo;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Wrappers;
using System.Collections;
using System.Collections.Generic;
using SpecialShop;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using ResidentFriendshipReward;

public class InteractionPanel : MonoBehaviour
{
    [Space(10f)]
    public GameObject residentImage;
    public GameObject interactionList;

    [Header("Buttons")]
    public GameObject questButton;
    public GameObject givingPresentButton;
    public GameObject friendshipCompensationButton;
    public GameObject tycoonButton;
    public GameObject dungeonEnterButton;
    public GameObject sparrowShopButton;

    [Header("Panels")]
    public GameObject countPanel;
    public GameObject failDungeonEnterPanel;
    public GameObject questPanel;
    public GameObject presentInventoryPanel;
    public GameObject friendshipCompensationPanel;

    public void UpdatePanel()
    {
        countPanel.SetActive(false);
        
        if (TownManager.instance.residentName == "parrot" && GameManager.instance.gameData.canEnterTycoon)
        {
            countPanel.transform.GetChild(0).GetComponent<Text>().text =
                "이번 달 영업 횟수 <" + GameManager.instance.gameData.tycoonEnteredCount + "/ 3>";
            countPanel.SetActive(true);
            tycoonButton.SetActive(true);
        }
        else
        {
            tycoonButton.SetActive(false);
        }

        if (TownManager.instance.residentName == "eagle" && GameManager.instance.gameData.canEnterDungeon)
        {
            countPanel.transform.GetChild(0).GetComponent<Text>().text =
                "이번 달 남은 탐사 횟수 <" + GameManager.instance.gameData.dungeonRemainCount + "/ 2 >";
            countPanel.SetActive(true);
            dungeonEnterButton.SetActive(true);
        }
        else
        {
            dungeonEnterButton.SetActive(false);
        }

        if (TownManager.instance.residentName == "sparrow" && GameManager.instance.gameData.canEnterSparrowShop)
        {
            sparrowShopButton.SetActive(true);
        }
        else
        {
            sparrowShopButton.SetActive(false);
        }

        if (TownManager.instance.selectedNPC.GetComponent<ResidentController>().hasQuest)
        {
            questButton.SetActive(true);
        }
        else
        {
            questButton.SetActive(false);
        }


        if (GameManager.instance.residentFriendships.TryGetValue(TownManager.instance.residentName, out ResidentFriendship val))
        {
            friendshipCompensationButton.SetActive(true);
        }
        else
        {
            friendshipCompensationButton.SetActive(false);
        }
        
        friendshipCompensationPanel.GetComponent<FriendshipRewardPanel>().ReadFriendshipRewardList();

        residentImage.GetComponent<Image>().sprite =
            Resources.Load<Sprite>("Illustrations/" + TownManager.instance.residentName);
        gameObject.SetActive(true);
    }

    public void OnConversationButtonClicked()
    {
        gameObject.SetActive(false);
        GameManager.instance.CheckInteractedInThisMonth(TownManager.instance.residentName);
        gameObject.GetComponent<HomeworkTrigger>().OnHomeworkTrigger();
        DialogueManager.StartConversation(TownManager.instance.residentName);
    }

    public void OnQuestButtonClicked()
    {
        gameObject.SetActive(false);
        questPanel.GetComponent<QuestPanel>().UpdateQuestPanel();
        questPanel.SetActive(true);
    }

    public void OnGivingPresentButtonClicked()
    {
        gameObject.SetActive(false);
        presentInventoryPanel.SetActive(true);
        presentInventoryPanel.GetComponent<PresentInventoryPanel>().OnPresentInventoryButtonClicked();
    }

    public void OnClickedFriendshipCompensationButton() //호감도보상 버튼 클릭
    {
        gameObject.SetActive(false);
        friendshipCompensationPanel.SetActive(true);
        friendshipCompensationPanel.GetComponent<FriendshipRewardPanel>().UpdateSlot();
    }

    public void OnDungeonEnterButtonClicked()
    {
        if (PlayerPrefs.GetInt("isDungeonTutorialCleared") == 1)
        {
            if (GameManager.instance.gameData.dungeonRemainCount > 0)
            {
                SceneManager.LoadScene("Field");
            }
            else
            {
                failDungeonEnterPanel.GetComponent<FailDungeonEnterPanel>().Initialize();
                failDungeonEnterPanel.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetInt("isDungeonTutorialCleared", 1);
            SceneManager.LoadScene("DungeonTutorial");
        }
    }

    public void OnSparrowShopButtonClicked()
    {
        gameObject.SetActive(false);
    }

    public void OnTycoonEnterButtonClicked()
    {
        if (PlayerPrefs.GetInt("isTycoonTutorialCleared") == 1)
        {
            if (GameManager.instance.gameData.isIncaTernInTown)
            {
                SceneManager.LoadScene("IncaTernTycoon");
            }
            else
            {
                SceneManager.LoadScene("Tycoon");
            }
        }
        else
        {
            SceneManager.LoadScene("TycoonTutorial");
        }
    }

    public void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
        interactionList.SetActive(true);
        countPanel.SetActive(false);
        TownManager.instance.OnBasePanel();
    }
}

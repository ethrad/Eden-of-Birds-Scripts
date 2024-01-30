using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResidentFriendshipReward;
public class FriendshipRewardSlot : MonoBehaviour
{
    public Text titleText;
    public GameObject rewardedButton;
    public GameObject rewardButton;
    public GameObject failAchieveButton;
    [HideInInspector]
    public int level;
    private string resName;
    

    public void UpdateRewardSlot(int level)
    {
        this.level = level;
        resName = TownManager.instance.residentName;
        
        //보상을 받았다면
        if (GameManager.instance.residentFriendships[resName].gotReward[level])
        {
            rewardedButton.SetActive(true);
            rewardButton.SetActive(false);
            failAchieveButton.SetActive(false);
        }
        //레벨은 달성했으나 보상을 받지 않았다면
        if (!GameManager.instance.residentFriendships[resName].gotReward[level] &&
                 GameManager.instance.residentFriendships[resName].levelLimit[level])
        {
            rewardedButton.SetActive(false);
            rewardButton.SetActive(true);
            failAchieveButton.SetActive(false);
        }
        //레벨을 달성하지 못했다면
        if(GameManager.instance.residentFriendships[resName].level < level + 1)
        {
            rewardedButton.SetActive(false);
            rewardButton.SetActive(false);
            failAchieveButton.SetActive(true);
        }

        level++;
        titleText.text = level + "레벨 보상";
    }
    
    public void OnClickedRewardTextButton()
    {
        FriendshipRewardPanel rewardPanel = GetComponentInParent<FriendshipRewardPanel>();
        rewardPanel.UpdateDescriptionPanel(level);
    }

    public void OnClickedRewardButton()
    {
        // 버튼 interactable false로 바꿔주기
        
        GameManager.instance.EarnFriendshipReward(resName, level);
        rewardButton.SetActive(false);
        rewardedButton.SetActive(true);
        GameManager.instance.residentFriendships[resName].gotReward[level] = true;
        GameManager.instance.WriteResidentFriendShip();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentFriendshipReward
{
    public class ResidentFriendshipReward
    {
        public List<string> compensation;
    }
    public class FriendshipRewardPanel : MonoBehaviour
    {
        public GameObject panel;
        public GameObject content;
        public GameObject friendshipRewardSlotPre;
        public GameObject descriptionPanel;
        public Text title;
        public Text description;
        
        public Scrollbar scrollbar;
        
        public Dictionary<string, ResidentFriendshipReward> friendshipRewardList;

        public void ReadFriendshipRewardList()
        {
            friendshipRewardList = IOManager.instance.ReadJsonFromResources<Dictionary<string, ResidentFriendshipReward>>("ResidentFriendshipReward");
        }
        
        public void UpdateSlot()
        {
            panel.SetActive(true);
            if (content.transform.childCount > 0)
            {
                for (int i = 0; i < content.transform.childCount; i++)
                {
                    Destroy(content.transform.GetChild(i).gameObject);
                }
            }
            for (int j = 0; j < friendshipRewardList[TownManager.instance.residentName].compensation.Count; j++)
            {
                GameObject tempSlot = Instantiate(friendshipRewardSlotPre, content.transform);
                tempSlot.GetComponent<FriendshipRewardSlot>().UpdateRewardSlot(j);
            }
        }
            
        public void OnClickedCloseButton()
        {
            scrollbar.value = 1;
            gameObject.SetActive(false);
            TownManager.instance.interactionPanel.SetActive(true);
        }
        
        public void UpdateDescriptionPanel(int level)
        {
            descriptionPanel.SetActive(true);
            description.text = friendshipRewardList[TownManager.instance.residentName].compensation[level];
            level++;
            title.text = level + "레벨 보상";
        }
    }
    
}

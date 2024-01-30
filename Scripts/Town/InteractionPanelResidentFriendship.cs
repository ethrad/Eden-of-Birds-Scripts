using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResidentLevelTitle;

namespace ResidentLevelTitle
{
    public class ResidentFriendshipLevelTite
    {
        public string korName;
        public List<string> levelTitle = new List<string>();
    }
}
public class InteractionPanelResidentFriendship : MonoBehaviour
{
    public Text residentName;
    public Text residentLevelTitleText;
    

    public void UpdatePanel()
    {
        if(TownManager.instance.resLevelTitleDic.TryGetValue(TownManager.instance.residentName, out ResidentFriendshipLevelTite resLevelTitle))
        {
            string resName = TownManager.instance.residentName;
            residentName.text = TownManager.instance.resLevelTitleDic[resName].korName;
            residentLevelTitleText.text = TownManager.instance.resLevelTitleDic[resName]
                .levelTitle[GameManager.instance.residentFriendships[resName].level];
        }
        
    }
}

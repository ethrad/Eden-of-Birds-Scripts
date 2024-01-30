using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePanel : MonoBehaviour
{
    public void OnShowAdButtonClicked()
    {
        LoadMobileAD.Instance.OnRewardedAd(1);
        gameObject.SetActive(false);
    }
    
    public void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
        DungeonManager.instance.FailDungeon();
    }   
}

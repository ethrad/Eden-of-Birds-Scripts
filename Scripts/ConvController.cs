using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvController : MonoBehaviour
{
    private int conversationCount;
    
    public void OnConversationStart()
    {
        TownManager.instance.OnConversationStart();
    }
    
    public void OnConversationEnd()
    {
        TownManager.instance.OnConversationEnd();
        
        if (GameManager.instance.gameData.purchasedAdFree) return;
        
        conversationCount++;
        if (conversationCount < 6) return;

        conversationCount = 0;
        LoadMobileAD.Instance.LoadInterstitialAd();
    }

    private void Start()
    {
        conversationCount = 0;
    }
}

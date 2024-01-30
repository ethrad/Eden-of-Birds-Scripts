using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailDungeonEnterPanel : MonoBehaviour
{
    public Button watchAdButton;
    
    public void OnWatchAdButtonClicked(Button button)
    {
        button.interactable = false;
        LoadMobileAD.Instance.OnRewardedAd(0);
    }

    public void OnUseFeatherButtonClicked()
    {
        if (GameManager.instance.gameData.PurchaseFeather(150))
        {
            GameManager.instance.gameData.dungeonRemainCount++;
            SceneManager.LoadScene("Field");
        }
    }

    public void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        if (GameManager.instance.gameData.dungeonEnterAdCount >= 2)
        {
            watchAdButton.interactable = false;
        }
    }
}

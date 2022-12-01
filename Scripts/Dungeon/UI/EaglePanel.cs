using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EaglePanel : MonoBehaviour
{
    public void OnSpeechBalloonClicked()
    {
        Initialize();
        gameObject.SetActive(true);
    }

    public GameObject dungeonContinueButton;

    public GameObject dialogueText;

    public void OnDungeonContinueButtonClicked()
    {
        gameObject.SetActive(false);
    }

    public void OnDungeonExitButtonClicked()
    {
        DungeonManager.instance.ClearDungeon();
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        if (DungeonManager.instance.isDungeonCleared == true)
        {
            dungeonContinueButton.SetActive(false);
            dialogueText.GetComponent<Text>().text = "시간이 늦었어. 이만 가자.";
        }
    }
}

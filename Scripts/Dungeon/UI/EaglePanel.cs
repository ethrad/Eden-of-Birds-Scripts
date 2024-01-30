using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EaglePanel : MonoBehaviour
{
    public void OnSpeechBalloonClicked()
    {
        Initialize();
        DungeonManager.instance.StopPlayerMoving();
        gameObject.SetActive(true);
    }

    public GameObject dialogueText;

    public void OnEscapeButtonClicked()
    {
        if (DungeonManager.instance.isAllCleared)
        {
            DungeonManager.instance.ClearAllDungeon();
            gameObject.SetActive(false);
            return;
        }
        
        DungeonManager.instance.ClearDungeon();
        gameObject.SetActive(false);
    }

    public void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
        DungeonManager.instance.StartPlayerMoving(true);
    }

    public void Initialize()
    {
        if (DungeonManager.instance.isAllCleared)
        {
            dialogueText.GetComponent<Text>().text = "시간이 늦었어. 이만 가자.";
        }
    }
}

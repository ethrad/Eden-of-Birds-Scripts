using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        if (IOManager.instance.playerSettings.isTutorialCleared == true)
        {
            SceneManager.LoadScene("Town");
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}

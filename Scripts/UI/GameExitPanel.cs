using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitPanel : MonoBehaviour
{
    public void OnExitButtonClicked()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
    
    public void OnCancelButtonClicked()
    {
        gameObject.SetActive(false);
    }
    
    private void Update()
    {
        #if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(true);
        }
        #endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedToConnectServerPanel : MonoBehaviour
{
    public void OnRetryButtonClicked()
    {
        gameObject.SetActive(false);
    }
    
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}

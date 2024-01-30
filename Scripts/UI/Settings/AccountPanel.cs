using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class AccountPanel : MonoBehaviour
{
    public Text loginIDText;
    public Text UUIDText;

    private bool isInitialized = false;

    //BackendReturnObject bro = Backend.BMember.GetUserInfo();
    public string getMyUUID()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        string myUUID = bro.GetReturnValuetoJSON()["row"]["gamerId"].ToString();
        return myUUID;
    }

    public string getMyFID()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        string myFID;

        if (bro.GetReturnValuetoJSON()["row"]["federationId"] != null) {
            myFID = bro.GetReturnValuetoJSON()["row"]["federationId"].ToString();
        } else {
            myFID = "Custom ID User";
        }
        return myFID;
    }

    public void OnAccountButtonClicked()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            loginIDText.text = getMyFID();
            UUIDText.text = getMyUUID();
        }
        
        gameObject.SetActive(true);
    }

    public void OnUUIDCopyButtonClicked()
    {
        GUIUtility.systemCopyBuffer = UUIDText.text;
    }
    
    public void OnYoutubeButtonClicked()
    {
        Application.OpenURL("https://www.youtube.com/@GUKBAPGAMES");
    }

    public void OnInstagramButtonClicked()
    {
        Application.OpenURL("https://instagram.com/gukbap230221");
    }

    public void OnTwitterButtonClicked()
    {
        Application.OpenURL("https://twitter.com/GukbapLove");
    }

    public void OnCSButtonClicked()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSd2s7gTENpCk0TI6fgbBq3qcISaf48E14K0eepQimgy5H48sA/viewform");
    }
}

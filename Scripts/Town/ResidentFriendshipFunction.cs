using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PixelCrushers.DialogueSystem;
public class ResidentFriendshipFunction : MonoBehaviour
{
    public GameObject friendshipAnimationPanel;
    public Text valueText;
    public void ResidentFriendshipAddOrSub(string residentName, double wantVal)
    {
        friendshipAnimationPanel.SetActive(true);
    
        int temp = FriendshipValueCalculate(residentName, Convert.ToInt32(wantVal));
        
        if(temp > -1) valueText.text = "+" + temp;
        else valueText.text = temp.ToString();
        
        GameManager.instance.WriteResidentFriendShip();
        friendshipAnimationPanel.GetComponent<Animator>().SetTrigger("SetFriendship");
    }
    
    public int FriendshipValueCalculate(string residentName, int temp)
    {
        ResidentDiaryData item =
            GameManager.instance.friendshipLevelUpValue.Find(friendshipLevelUpValue => friendshipLevelUpValue.residentName == residentName);
    
        int tempLevel = GameManager.instance.residentFriendships[residentName].level;
        if (GameManager.instance.residentFriendships[residentName].levelLimit[tempLevel] == false)
        {
            if (GameManager.instance.residentFriendships[residentName].friendship + temp < item.friendshipLevel[tempLevel])
            {
                GameManager.instance.SetFriendShip(residentName, temp, false);
                return temp;
            }
            else
            {
                GameManager.instance.residentFriendships[residentName].isAlerted = true;
                int leftTemp = item.friendshipLevel[tempLevel] - GameManager.instance.residentFriendships[residentName].friendship;
                GameManager.instance.SetFriendShip(residentName, temp, false);
                return leftTemp;
            }
        }
        return 0;
    }
    
    
    public void RegisterFriendshipFunction()//에셋 함수 등록
    {
        Lua.RegisterFunction(nameof(ResidentFriendshipAddOrSub), this, SymbolExtensions.GetMethodInfo(() => ResidentFriendshipAddOrSub(string.Empty, 0)));
    }
     void Start()
     {
         RegisterFriendshipFunction();
    }
}

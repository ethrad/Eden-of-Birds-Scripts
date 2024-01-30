using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Diary;
using UnityEngine.UI;

public class UpgradeFriendship : MonoBehaviour
{
    public GameObject resPanel;
    public Text description;
    public Text curCount;
    public Text necessaryCount;
    public GameObject noticePanel;

    public GameObject diary;
    
    private string resTreasure;
    private string resName;
    private int level;

    public void ShowUpgradePanel(string wantName)
    {
        resName = wantName;
        this.gameObject.SetActive(true);
        level = GameManager.instance.residentFriendships[wantName].level;
        resTreasure = wantName + "Treasure";
        if (!GameManager.instance.inventory.TryGetValue(resTreasure, out int temp))
            GameManager.instance.inventory.Add(resTreasure, 0);
        SetCurCountColor();
        curCount.text = GameManager.instance.inventory[resTreasure].ToString();
        necessaryCount.text = GameManager.instance.residentTreasureShopList[resTreasure].requiredTreasureCount[level].ToString();
        description.text = "애장품 "+ necessaryCount.text +"개를 소모하여 호감도 레벨을 업그레이드 하시겠습니까?";
        
    }

    public void SetCurCountColor()
    {
        if (GameManager.instance.inventory[resTreasure] <
            GameManager.instance.residentTreasureShopList[resTreasure].requiredTreasureCount[level])
        {
            curCount.GetComponent<Text>().color = new Vector4(1, 0, 0, 1);
        }
    }
    
    public void OnClickedUpgradeButton()
    {
        //가지고 있는 애장품 개수 부족할 때
        if(GameManager.instance.inventory[resTreasure] <
           GameManager.instance.residentTreasureShopList[resTreasure].requiredTreasureCount[level])
        {
            noticePanel.SetActive(true);
            noticePanel.GetComponent<Animator>().SetTrigger("Notice");

        }
        //
        else if (GameManager.instance.inventory[resTreasure] >=
            GameManager.instance.residentTreasureShopList[resTreasure].requiredTreasureCount[level])
        {
            ResidentDiaryData item =
                GameManager.instance.friendshipLevelUpValue.Find(friendshipLevelUpValue => friendshipLevelUpValue.residentName == resName);
            //수치조정
            GameManager.instance.inventory[resTreasure] -= GameManager.instance.residentTreasureShopList[resTreasure]
                .requiredTreasureCount[level];
            GameManager.instance.residentFriendships[resName].levelLimit[level] = true;
            
            GameManager.instance.ResidentLevelCheck(item, resName);
            GameManager.instance.residentFriendships[resName].isAlerted = false;
            
            //저장
            GameManager.instance.WriteResidentFriendShip();
            GameManager.instance.WriteInventory();
            
            //패널끄기
            diary.GetComponent<DiaryCheckMark>().UpdateCheckMark();
            resPanel.GetComponent<ResidentPage>().Initialize(resPanel.GetComponent<ResidentPage>().residents);
            resPanel.GetComponent<ResidentPage>().SetResidentSlotDetail(resName);
            gameObject.SetActive(false);
            
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TycoonBarTutorial : MonoBehaviour
{
    public GameObject firstConversation;
    public GameObject arrow;
    public Text firstText;

    public GameObject secondConversation;
    public GameObject arrow2;
    public Text secondText;

    public static bool endKitchen  = true;
    int count;
    
    public void OnClickedFirstconversationButton()
    {
        count++;
        arrow.SetActive(false);
        firstText.text = "오른쪽에서 왼쪽으로 슬라이드하면, 주방으로 이동할 수 있어.손님의 대기시간과 메뉴를 숙지한 다음에 요리하러 가보자!";
        if(count > 1)
        {
            firstConversation.SetActive(false);
        }
    }

    public void OnClickedSecondconversationButton()
    {
        count++;
        arrow2.SetActive(false);
        secondText.text = "자, 이제 실전이야. 처음에는 레시피 외우는 것도 힘들고 정신없겠지만, 계속하다보면 보람차고 재밌을 거라구! 앞으로 파이팅이야!";

        if (count > 3)
        {
            secondConversation.SetActive(false);
            Time.timeScale = 1;
            IOManager.instance.playerSettings.isTutorialCleared = true;
            IOManager.instance.WritePlayerSettings();
            ItemManager.instance.WriteInventory();
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        Time.timeScale = 0.0f;
    }

    public void Update()
    {
        if (endKitchen == false)
        {
            secondConversation.SetActive(true);
            arrow2.SetActive(true);
            endKitchen = true;
        }

    }

}

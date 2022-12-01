using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenTycoonTutorial : MonoBehaviour
{
    public GameObject firstConversation;
    public GameObject arrow;
    public GameObject arrow2;
    public Text firstText;

    public GameObject secondConversation;
    public Text secondText;

    public static KitchenTycoonTutorial instance;
    public string foodName;

    int count;
    RectTransform rect;
    public void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        rect = firstConversation.GetComponent<RectTransform>();
        Time.timeScale = 0;
    }

    public void OnClickedFirstconversationButton()
    {
        count++;
        if (count == 1)
        {
            firstText.text = "재료가 다 떨어져서 요리를 못하면 어떡하냐고? 빈 재료함을 한 번 터치해 봐. 돈이 들지만, 독수리씨가 재료를 곧바로 가져다 주실거야.";
        }
        else if (count == 2)
        {
            arrow.SetActive(false);
            arrow2.SetActive(true);
            rect.offsetMin = new Vector2(rect.offsetMin.x, 520);
            rect.offsetMax = new Vector2(rect.offsetMax.x, 520);
            firstText.text = "혹시라도 조리가 망해버린 재료가 있다면 걱정하진 마. 오른쪽 위에 쓰레기통에 버리면 돼.물론 재료는 아깝지만,,,, 맛없는 음식을 서빙할 순 없잖아? 하여튼!조리된 재료들은 아래 선반에 놓을 수 있어. 그리고 내 연륜이 담긴 레시피를 참고해서 알맞게 조합하면 돼!";
        }//쪼개기
        else if(count == 3)
        {
            firstText.text = "메뉴를 완성시키려면 오른쪽에 있는 벨을 눌러봐!";
        }
        else if (count == 4)
        {
            firstConversation.SetActive(false);
            arrow2.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void CompleteFood()
    {
        Time.timeScale = 0;
        secondConversation.SetActive(true);
        secondText.text = "아주 " + foodName + "가(이)완성되었어!  내가 빠르게 테이블 위에 올려놓을테니, 서빙하러 한 번 가보렴!"; //이거 수정
    }

    public void OnClickedSecondconversationButton()
    {
        Time.timeScale = 1;
        secondConversation.SetActive(false);
        TycoonBarTutorial.endKitchen = false;
        Destroy(gameObject);
    }
}
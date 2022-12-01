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
            firstText.text = "��ᰡ �� �������� �丮�� ���ϸ� ��ϳİ�? �� ������� �� �� ��ġ�� ��. ���� ������, ���������� ��Ḧ ��ٷ� ������ �ֽǰž�.";
        }
        else if (count == 2)
        {
            arrow.SetActive(false);
            arrow2.SetActive(true);
            rect.offsetMin = new Vector2(rect.offsetMin.x, 520);
            rect.offsetMax = new Vector2(rect.offsetMax.x, 520);
            firstText.text = "Ȥ�ö� ������ ���ع��� ��ᰡ �ִٸ� �������� ��. ������ ���� �������뿡 ������ ��.���� ���� �Ʊ�����,,,, ������ ������ ������ �� ���ݾ�? �Ͽ�ư!������ ������ �Ʒ� ���ݿ� ���� �� �־�. �׸��� �� ������ ��� �����Ǹ� �����ؼ� �˸°� �����ϸ� ��!";
        }//�ɰ���
        else if(count == 3)
        {
            firstText.text = "�޴��� �ϼ���Ű���� �����ʿ� �ִ� ���� ������!";
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
        secondText.text = "���� " + foodName + "��(��)�ϼ��Ǿ���!  ���� ������ ���̺� ���� �÷������״�, �����Ϸ� �� �� ������!"; //�̰� ����
    }

    public void OnClickedSecondconversationButton()
    {
        Time.timeScale = 1;
        secondConversation.SetActive(false);
        TycoonBarTutorial.endKitchen = false;
        Destroy(gameObject);
    }
}
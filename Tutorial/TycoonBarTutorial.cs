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
        firstText.text = "�����ʿ��� �������� �����̵��ϸ�, �ֹ����� �̵��� �� �־�.�մ��� ���ð��� �޴��� ������ ������ �丮�Ϸ� ������!";
        if(count > 1)
        {
            firstConversation.SetActive(false);
        }
    }

    public void OnClickedSecondconversationButton()
    {
        count++;
        arrow2.SetActive(false);
        secondText.text = "��, ���� �����̾�. ó������ ������ �ܿ�� �͵� ����� ���ž�������, ����ϴٺ��� �������� ����� �Ŷ�! ������ �������̾�!";

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class DungeonTutorial : MonoBehaviour
{
    public GameObject firstConversationPanel;
    public GameObject secondConversationPanel;
    public GameObject thirdConversationPanel;
    public Text secondText;
    public Text thirdText;

    void Start()
    {
        Time.timeScale = 0;
    }

    public void OnClickedFirstConversationButton()
    {
        firstConversationPanel.SetActive(false);
        secondConversationPanel.SetActive(true);
    }

    public void OnClickedSecondConversationButton()
    {
        conversation++;

        if (conversation == 1) secondText.text = "�������� �ʵ��� ������.���� �������� ���� ������� ������ ������ ������,,ġ���� ���� û���ϴ°� ���� ����.";
        else if (conversation == 2) secondText.text = "���� �ֺ��� ���� �����״ϱ�, �� ���� �� ������ �ٽ� �ð�. �׷� ȭ�����ϰ�!";
        else if (conversation == 3)
        {
            secondConversationPanel.SetActive(false);
            Time.timeScale = 1;
        }
        else if (conversation == 4) thirdText.text = "�� �ֺ��� Ž���� �� ������ �� Ž���ϸ鼭 ��������, ���� ������ ���ư��� ���� �� �־�.";
        else if (conversation == 5) thirdText.text = "������ ó���̴ϱ�, ������ ���ư����� ����. �޹������� �� �����ϰ� �������� �߰ŵ�.";
        else if (conversation == 6)
        {
            foreach (string key in ItemManager.instance.itemList.Keys.ToList())
            {
                if (ItemManager.instance.inventory.ContainsKey(key))
                {
                    ItemManager.instance.inventory[key] += 20;
                }
                else
                {
                    ItemManager.instance.inventory.Add(key, 20);
                }
            }
            SceneManager.LoadScene("TownTutorial2");
        }
    }

    int conversation = 0;

    void Update()
    {
        if (DungeonManager.isClear) thirdConversationPanel.SetActive(true);
    }
}

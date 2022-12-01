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

        if (conversation == 1) secondText.text = "쓰러지지 않도록 조심해.물론 쓰러져도 나와 뱁새씨가 마을로 데려갈 테지만,,치료비는 따로 청구하는거 잊지 말고.";
        else if (conversation == 2) secondText.text = "나는 주변을 보고 있을테니까, 다 잡은 것 같으면 다시 올게. 그럼 화이팅하고!";
        else if (conversation == 3)
        {
            secondConversationPanel.SetActive(false);
            Time.timeScale = 1;
        }
        else if (conversation == 4) thirdText.text = "이 주변의 탐색이 다 끝나면 더 탐색하면서 수집할지, 일찍 마을로 돌아갈지 정할 수 있어.";
        else if (conversation == 5) thirdText.text = "오늘은 처음이니까, 마을로 돌아가도록 하자. 앵무새씨가 몸 멀쩡하게 데려오라 했거든.";
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

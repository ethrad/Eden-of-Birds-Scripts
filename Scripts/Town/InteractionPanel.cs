using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionPanel : MonoBehaviour
{
    public GameObject residentName;
    public GameObject residentImage;
    public GameObject tycoonButton;

    public void UpdatePanel()
    {
        if (InteractionManager.instance.residentName == "앵무새")
        {
            tycoonButton.SetActive(true);
        }

        // 이름 변경
        // 이미지 변경
        // 대사 변경
        gameObject.SetActive(true);
    }

    public void OnConversationButtonClicked()
    {

    }

    public void OnTycoonEnterButtonClicked()
    {
        SceneManager.LoadScene("Tycoon");
    }

    public void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
        tycoonButton.SetActive(false);
    }
}

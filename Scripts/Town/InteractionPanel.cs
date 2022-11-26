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
        if (InteractionManager.instance.residentName == "�޹���")
        {
            tycoonButton.SetActive(true);
        }

        // �̸� ����
        // �̹��� ����
        // ��� ����
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

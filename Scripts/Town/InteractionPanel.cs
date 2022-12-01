using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractionPanel : MonoBehaviour
{
    public GameObject residentName;
    public GameObject tycoonButton;

    public GameObject residentImage;
    public GameObject dialogueText;

    public void UpdatePanel()
    {
        if (InteractionManager.instance.residentName == "�޹���")
        {
            tycoonButton.SetActive(true);
        }

        // �̸� ����
        residentName.GetComponent<Text>().text = InteractionManager.instance.residentName;

        // �̹��� ����
        residentImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Illustrations/" + InteractionManager.instance.residentName);

        // ��� ����
        int index = Random.Range(0, 3);

        dialogueText.GetComponent<Text>().text = InteractionManager.instance.dialogues[InteractionManager.instance.residentName][index];

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
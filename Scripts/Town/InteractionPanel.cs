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
        if (InteractionManager.instance.residentName == "앵무새")
        {
            tycoonButton.SetActive(true);
        }

        // 이름 변경
        residentName.GetComponent<Text>().text = InteractionManager.instance.residentName;

        // 이미지 변경
        residentImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Illustrations/" + InteractionManager.instance.residentName);

        // 대사 변경
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
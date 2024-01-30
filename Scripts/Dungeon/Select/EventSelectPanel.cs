using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EventSelectPanel : MonoBehaviour
{
    // ���߿� �����ؾ���
    // ������� ����

    public Sprite clickedButtonSprite;

    public void OnEventButtonClicked()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = clickedButtonSprite;
    }

    public void OnEnterButtonClicked()
    {
        SceneManager.LoadScene("Field");
    }
}

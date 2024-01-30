using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResidentController : MonoBehaviour
{
    public string residentName;
    public GameObject questionMark;
    public GameObject questionMarkMinimap;
    public GameObject exclamationMark;
    public GameObject exclamationMarkMinimap;
    public GameObject furniture;

    public List<Quest> progressQuests;
    public List<Quest> clearQuests;

    public Animator friendshipValuePanel;
    public TextMeshProUGUI valueText;

    [HideInInspector]
    public bool hasQuest;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            TownManager.instance.UpdateInteractionState(this.gameObject, residentName);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            TownManager.instance.ResetInteractionState();
        }
    }

    public void CheckQuest()
    {
        // CName�� ��ġ�ϴ� ����Ʈ�� ���������� Ȯ��
        // EndCName�� ��ġ�ϴ� ����Ʈ�� ��ǥ�� Ȯ��

        progressQuests = GameManager.instance.CheckPreCondition(residentName);
        clearQuests = GameManager.instance.CheckGoalCondition(residentName);

        if (progressQuests.Count > 0)
        {
            exclamationMark.SetActive(true);
            exclamationMarkMinimap.SetActive(true);
            hasQuest = true;
            return;
        }
        else
        {
            exclamationMark.SetActive(false);
            exclamationMarkMinimap.SetActive(false);
            hasQuest = false;
        }

        if (clearQuests.Count > 0)
        {
            questionMark.SetActive(true);
            questionMarkMinimap.SetActive(true);
            hasQuest = true;
        }
        else
        {
            questionMark.SetActive(false);
            questionMarkMinimap.SetActive(false);
            hasQuest = false;
        }
    }

    public void SetFurniture()
    {
        if (GameManager.instance.residentFriendships.TryGetValue(residentName, out var friendship))
        {
            if (friendship.gotFurniture)
            {
                furniture.SetActive(true);
            }
            else
            {
                furniture.SetActive(false);
            }
        }
        else
        {
            furniture.SetActive(false);
        }
    }

    private void Start()
    {
        SetFurniture();
        CheckQuest();
    }
}

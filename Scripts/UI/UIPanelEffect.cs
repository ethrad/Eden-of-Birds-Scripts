using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


//UI ���� �⺻ ����
public class UIPanelEffect : MonoBehaviour
{
    public float fadeTime = 1f;
    public CanvasGroup panelCanvasGroup;
    public RectTransform rect;

    public void PanelFadeIn() // �г��� ��Ÿ���� ȿ��
    {
        panelCanvasGroup.alpha = 0f;
        //rect.transform.localPosition = new Vector3(0f, -1000f, 0f);
        rect.transform.localPosition = new Vector3(0f, 0, 0f);
        //if(GameManager.instance.gameData.purchasedAdFree) //�������ű� ������
        //    rect.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        //else //�������ű� ������
        //    rect.DOAnchorPos(new Vector2(0f, -100f), fadeTime, false).SetEase(Ease.OutElastic);
        panelCanvasGroup.DOFade(1, fadeTime);
    }

    public void PanelFadeOut() // �г��� ������� ȿ��
    {
        panelCanvasGroup.alpha = 1f;
        rect.transform.localPosition = new Vector3(0f, 0f, 0f);
        rect.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        panelCanvasGroup.DOFade(0, fadeTime);
        StartCoroutine(OffPanel(fadeTime));
    }

    public IEnumerator OffPanel(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);

    }
}

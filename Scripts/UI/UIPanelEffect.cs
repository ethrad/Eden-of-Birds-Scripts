using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


//UI 제일 기본 연출
public class UIPanelEffect : MonoBehaviour
{
    public float fadeTime = 1f;
    public CanvasGroup panelCanvasGroup;
    public RectTransform rect;

    public void PanelFadeIn() // 패널이 나타나는 효과
    {
        panelCanvasGroup.alpha = 0f;
        //rect.transform.localPosition = new Vector3(0f, -1000f, 0f);
        rect.transform.localPosition = new Vector3(0f, 0, 0f);
        //if(GameManager.instance.gameData.purchasedAdFree) //광고제거권 있으면
        //    rect.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        //else //광고제거권 없으면
        //    rect.DOAnchorPos(new Vector2(0f, -100f), fadeTime, false).SetEase(Ease.OutElastic);
        panelCanvasGroup.DOFade(1, fadeTime);
    }

    public void PanelFadeOut() // 패널이 사라지는 효과
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

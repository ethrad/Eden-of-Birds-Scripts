using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeEffect : FadeEffectManager
{
    public RectTransform rect;

    public override void PanelFadeIn()
    {
        rect.transform.localPosition = new Vector3(0f, -1000f, 0f);
        rect.DOAnchorPos(new Vector2(0f, 0f), fadeInTime, false).SetEase(Ease.OutElastic);
        base.PanelFadeIn();
    }

    public override void StandardPanelFadeOut()
    {
        base.StandardPanelFadeOut();
        rect.transform.localPosition = new Vector3(0f, 0f, 0f);
        rect.DOAnchorPos(new Vector2(0f, -1000f), fadeInTime, false).SetEase(Ease.InOutQuint);
    }

    public override IEnumerator ItemsAnimation(List<GameObject> items)
    {
        return base.ItemsAnimation(items);
    }

}

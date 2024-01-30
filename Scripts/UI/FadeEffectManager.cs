using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

//화면 까매지고 밝아지고 그거임
public class FadeEffectManager : MonoBehaviour
{
    public float fadeInTime;
    public CanvasGroup panelCanvasGroup;
    public List<GameObject> panels = new List<GameObject>();

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
    }
    
    public virtual void PanelFadeIn()
    {
        panelCanvasGroup.alpha = 0f;
        StartCoroutine(ItemsAnimation(panels));
    }

    
    
    public virtual void StandardPanelFadeIn()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("FadeIn");
    }
    
    public virtual void StandardPanelFadeOut()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("FadeOut");
    }

    public void StandardPanelFadeEffect()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("Fade");
    }

    public void AlertMessage(string message)
    {
        if (message == "FadeInEnd")
        {
            GameManager.instance.isFadeInEnd = true;
            gameObject.SetActive(false);

        }
        else if (message == "FadeOutEnd")
        {
            GameManager.instance.isFadeOutEnd = true;
        }
        else if (message == "FadeEffectEnd")
        {
            GameManager.instance.isFadeEffectEnd = true;
            gameObject.SetActive(false);
        }
        
    }

    public virtual IEnumerator ItemsAnimation(List<GameObject> items)
    {
        foreach (var item in items)
        {
            item.transform.localScale = Vector3.zero;
        }

        foreach (var item in items)
        {
            item.transform.DOScale(1f, fadeInTime).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.25f);
        }
    }
}

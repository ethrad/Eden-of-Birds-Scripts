using MOTM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MOTMPanel : MonoBehaviour
{
    public float fadeTime = 1f;
    public CanvasGroup panelCanvasGroup;
    public RectTransform rect;
    public GameObject[] menuSlots;

    private Dictionary<string, List<string>> menuOfTheMonth;

    // 이달의 메뉴 결정 => 저장
    private Dictionary<string, List<string>> SelectMenuOfTheMonth()
    {
        var newMenuOfTheMonth = new Dictionary<string, List<string>>();
        for (int i = 1; i < 4; i++)
        {
            newMenuOfTheMonth.Add(i.ToString(), new List<string>());
        }

        var currentMenuOfTheMonth = GameManager.instance.menuOfTheMonth;

        // 현재 가지고 있는 레시피 깊은 복사
        var ownRecipes = new Dictionary<string, List<string>>();

        foreach (var recipe in GameManager.instance.ownRecipes)
        {
            ownRecipes.Add(recipe.Key, recipe.Value.ToList());
        }

        // 내가 가지고 있는 레시피에서 현재 이달의 메뉴에 있는 레시피 제거
        foreach (var motm in currentMenuOfTheMonth)
        {
            foreach (var foodName in motm.Value)
            {
                ownRecipes[motm.Key].Remove(foodName);
            }
        }

        // 이달의 메뉴 결정

        if (ownRecipes["1"].Count == 0)
        {
            newMenuOfTheMonth["1"] = currentMenuOfTheMonth["1"];
        }
        else if (ownRecipes["1"].Count == 1)
        {
            int random = Random.Range(0, 2);
            newMenuOfTheMonth["1"].Add(currentMenuOfTheMonth["1"][random]);

            newMenuOfTheMonth["1"].Add(ownRecipes["1"][0]);
        }
        else
        {
            var tempList = ownRecipes["1"].ToList();

            for (int j = 0; j < 2; j++)
            {
                int random = Random.Range(0, tempList.Count);

                newMenuOfTheMonth["1"].Add(tempList[random]);
                tempList.RemoveAt(random);
            }
        }


        // 3성 레시피가 있는가?
        if (GameManager.instance.ownRecipes.TryGetValue("3", out var value) && value.Count > 0)
        {
            for (int i = 2; i <= 3; i++)
            {
                string grade = i.ToString();

                if (ownRecipes[grade].Count > 0)
                {
                    int random = Random.Range(0, ownRecipes[grade].Count);
                    
                    newMenuOfTheMonth[grade].Add(ownRecipes[grade][random]);
                }
                else
                {
                    newMenuOfTheMonth[grade] = currentMenuOfTheMonth[grade];
                }
            }
        }
        else // 3성 레시피가 없는 경우
        {
            if (ownRecipes["2"].Count == 0)
            {
                newMenuOfTheMonth["2"] = currentMenuOfTheMonth["2"];
            }
            else if (ownRecipes["2"].Count == 1)
            {
                int random = Random.Range(0, 2);
                newMenuOfTheMonth["2"].Add(currentMenuOfTheMonth["2"][random]);

                newMenuOfTheMonth["2"].Add(ownRecipes["2"][0]);
            }
            else
            {
                List<string> tempList = ownRecipes["2"].ToList();

                for (int j = 0; j < 2; j++)
                {
                    int random = Random.Range(0, tempList.Count);

                    newMenuOfTheMonth["2"].Add(tempList[random]);
                    tempList.RemoveAt(random);
                }
            }
        }

        return newMenuOfTheMonth;
    }

    private void SetMenuSlots()
    {
        int slotIndex = 0;

        for (int i = 1; i < 4; i++)
        {
            if (!menuOfTheMonth.TryGetValue(i.ToString(), out var foodList)) break;
            
            foreach (var foodName in foodList)
            {
                menuSlots[slotIndex++].GetComponent<MenuSlot>().SetMenuSlot(foodName, i);
            }
        }
    }

    public void OnExitButtonClicked()
    {
        GameManager.instance.menuOfTheMonth = menuOfTheMonth;
        GameManager.instance.WriteMenuOfTheMonth();

        if (GameManager.instance.gameData.month % 3 == 0)
        {
            GameManager.instance.gameData.isIncaTernInTown = true;
            GameManager.instance.WriteGameData();
            SceneManager.LoadScene("TownInca");
        }

        gameObject.SetActive(false);
    }

    private void PanelFadeIn()
    {
        panelCanvasGroup.alpha = 0f;
        rect.transform.localPosition = new Vector3(0f, -1000f, 0f);
        rect.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        panelCanvasGroup.DOFade(1, fadeTime);
        StartCoroutine(ItemsAnimation(menuSlots));
    }

    public void PanelFadeOut()
    {
        panelCanvasGroup.alpha = 1f;
        rect.transform.localPosition = new Vector3(0f, 0f, 0f);
        rect.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        panelCanvasGroup.DOFade(0, fadeTime);
    }

    IEnumerator ItemsAnimation(GameObject[] items)
    {
        foreach (var item in items)
        {
            item.transform.localScale = Vector3.zero;
        }

        foreach (var item in items)
        {
            item.transform.DOScale(1f, fadeTime).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void OnMOTMButtonClicked()
    {
        menuOfTheMonth = GameManager.instance.menuOfTheMonth;
        SetMenuSlots();
        gameObject.SetActive(true);
        PanelFadeIn();
    }

    public void Initialize()
    {
        menuOfTheMonth = SelectMenuOfTheMonth();
        SetMenuSlots();
        gameObject.SetActive(true);
        PanelFadeIn();
    }
}

using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePanel : MonoBehaviour
{
    public List<GameObject> recipePages;
    int pageIndex = 0;

    public GameObject frontPageButton;
    public GameObject backPageButton;

    public GameObject pages;
    public GameObject pagePrefab;

    public void OnRecipeButtonClicked()
    {
        if (pageIndex == 0)
        {
            frontPageButton.SetActive(false);
        }

        if (pageIndex + 1 == recipePages.Count)
        {
            backPageButton.SetActive(false);
        }

        recipePages[pageIndex].SetActive(true);
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void OnFrontPageButtonClicked()
    {
        recipePages[pageIndex].SetActive(false);
        pageIndex--;
        recipePages[pageIndex].SetActive(true);

        if (pageIndex == 0)
        {
            frontPageButton.SetActive(false);
        }

        if (backPageButton.activeSelf == false)
        {
            backPageButton.SetActive(true);
        }
    }

    public void OnBackPageButtonClicked()
    {
        recipePages[pageIndex].SetActive(false);
        pageIndex++;
        recipePages[pageIndex].SetActive(true);

        if (pageIndex + 1 == recipePages.Count)
        {
            backPageButton.SetActive(false);
        }

        if (frontPageButton.activeSelf == false)
        {
            frontPageButton.SetActive(true);
        }
    }

    public void OnExitButtonClicked()
    {
        recipePages[pageIndex].SetActive(false);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void InitializeRecipePanel()
    {
        bool isFull = true;
        GameObject tempPage = null;

        for (int i = 1; i < 4; i++)
        {
            if (!GameManager.instance.ownRecipes.TryGetValue(i.ToString(), out var recipes)) continue;
            foreach (string menuName in recipes)
            {
                if (isFull)
                {
                    tempPage = Instantiate(pagePrefab, pages.transform);
                    tempPage.transform.GetChild(0).GetComponent<RecipePage>().UpdatePage(menuName, GameManager.instance.menus[menuName]);
                    recipePages.Add(tempPage);
                    tempPage.transform.GetChild(0).gameObject.SetActive(true);
                    isFull = false;
                }
                else
                {
                    tempPage.transform.GetChild(1).GetComponent<RecipePage>().UpdatePage(menuName, GameManager.instance.menus[menuName]);
                    tempPage.transform.GetChild(1).gameObject.SetActive(true);
                    isFull = true;
                }
            }
        }
        
        gameObject.SetActive(false);
    }

    void Start()
    {
        InitializeRecipePanel();
    }
}

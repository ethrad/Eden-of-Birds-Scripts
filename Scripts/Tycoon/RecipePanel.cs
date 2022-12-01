using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePanel : MonoBehaviour
{
    public GameObject[] recipePages;
    int pageIndex = 0;

    public GameObject frontPageButton;
    public GameObject backPageButton;

    public void OnRecipeButtonClicked()
    {
        if (pageIndex == 0)
        {
            frontPageButton.SetActive(false);
        }

        if (pageIndex + 1 == recipePages.Length)
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

        if (pageIndex + 1 == recipePages.Length)
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
    
}

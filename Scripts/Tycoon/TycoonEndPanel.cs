using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TycoonEndPanel : MonoBehaviour
{
    public GameObject normalCountText;
    public GameObject specialHeadPanel;
    public GameObject specialCountText;

    void UpdateSpecialPanel()
    {

    }

    public GameObject usedIngredientPanel;
    public GameObject usedIngredientPrefab;

    void UpdateUsedIngredientPanel()
    {

    }

    public GameObject servedFoodPanel;
    public GameObject servedFoodPrefab;

    void UpdateServedFoodPanel()
    {

    }

    public GameObject revenueText; // 총 수익
    public GameObject lossText; // 손해
    public GameObject netText; // 순 수익
    
    void UpdateGoldResultPanel()
    {

    }

    public void UpdateTycoonEndPanel()
    {
        UpdateSpecialPanel();
        UpdateUsedIngredientPanel();
        UpdateServedFoodPanel();
        UpdateGoldResultPanel();
    }

    void UpdateTycoonResult()
    {
        ItemManager.instance.gold += TycoonManager.instance.earnedGold;
        ItemManager.instance.WriteInventory();
    }

    public void OnCloseBarButtonClicked()
    {
        UpdateTycoonResult();
        SceneManager.LoadScene("Town");
    }
}

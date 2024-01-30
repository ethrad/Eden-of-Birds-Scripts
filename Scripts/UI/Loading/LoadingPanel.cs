using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingPanel : MonoBehaviour
{
    public Text dateText;
    public Text tipText;

    [HideInInspector]
    public List<string> loadingTipsList;

    void ReadLoadingTips()
    {
        loadingTipsList = IOManager.instance.ReadCSV("LoadingTips");
    }

    private void Start()
    {
        if (!GameManager.instance.gameData.purchasedAdFree)
        {
            LoadMobileAD.Instance.LoadBanner();
        }
        ReadLoadingTips();
        dateText.text = "< ������ " + GameManager.instance.gameData.year + "�� " + GameManager.instance.gameData.month + "�� >";
        tipText.text = loadingTipsList[Random.Range(0, loadingTipsList.Count)];
    }
}

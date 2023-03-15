using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapPanel : MonoBehaviour
{
    public GameObject exitPanel;

    string mapName;

    public void OnMapButtonClicked()
    {
        mapName = EventSystem.current.currentSelectedGameObject.name;
    }

    public void OnTownButtonClicked()
    {
        exitPanel.SetActive(true);
    }

    public void OnExitYesButtonClicked()
    {

    }

    public void OnExitNoButtonClicked()
    {
        exitPanel.SetActive(false);
    }
}

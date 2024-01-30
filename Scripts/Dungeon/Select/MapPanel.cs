using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MapPanel : MonoBehaviour
{
    public GameObject exitPanel;
    public GameObject eventSelectPanel;

    string mapName;

    public void OnMapButtonClicked()
    {
        //mapName = EventSystem.current.currentSelectedGameObject.name;
        eventSelectPanel.SetActive(true);
    }

    public void OnTownButtonClicked()
    {
        exitPanel.SetActive(true);
    }

    public void OnExitYesButtonClicked()
    {
        SceneManager.LoadScene("Town");
    }

    public void OnExitNoButtonClicked()
    {
        exitPanel.SetActive(false);
    }
}

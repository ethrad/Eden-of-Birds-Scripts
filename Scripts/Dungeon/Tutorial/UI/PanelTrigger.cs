using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTrigger : MonoBehaviour
{
    public int panelType;
    public bool isFade;
    public bool hasButton;

    public GameObject eagle;
    public GameObject slime;

    public void OffEagle()
    {
        eagle.SetActive(false);
    }

    public void OnSlime()
    {
        slime.SetActive(true);
    }
}

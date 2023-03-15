using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public GameObject mainCamera;
    public float xPos;

    public void OnSwitchButtonClicked()
    {
        mainCamera.transform.position = new Vector3(xPos, 0, -10f);
    }
}

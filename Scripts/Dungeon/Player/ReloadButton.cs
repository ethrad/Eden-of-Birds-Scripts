using Dungeon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadButton : MonoBehaviour
{
    private float currentReloadTime;

    private float reloadDelay = 1f;
    
    [SerializeField]
    private Image reloadDelayBar;
    [SerializeField]
    private GameObject shootingJoystick;

    private ShootingJoystick joystickComponent;
        
    public void OnReloadButtonClicked()
    {
        joystickComponent.isReloading = true;
        GetComponent<Button>().interactable = false;
        currentReloadTime = reloadDelay;
        
        StartCoroutine(ReloadDelay());
    }
    
    IEnumerator ReloadDelay()
    {
        reloadDelayBar.color = new Color32(40, 210, 255, 255);
            
        while (currentReloadTime > 0)
        {
            currentReloadTime -= Time.deltaTime;
            reloadDelayBar.fillAmount = currentReloadTime / reloadDelay;

            yield return null;
        }

        joystickComponent.bulletCount = 10;
        reloadDelayBar.color = new Color32(255, 216, 40, 255);
        reloadDelayBar.fillAmount = (float)joystickComponent.bulletCount / 10;
        joystickComponent.isReloading = false;
        GetComponent<Button>().interactable = true;
    }

    private void Start()
    {
        joystickComponent = shootingJoystick.GetComponent<ShootingJoystick>();
    }
}

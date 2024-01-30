using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickSettingPanel : MonoBehaviour
{
    public GameObject Joystick;
    
    private JoystickValues joystickValues;
    public Slider XPositionSlider;
    public Slider YPositionSlider;
    public Slider JoystickSizeSlider;
    public Slider HandleSizeSlider;

    public void OnXPositionChanged(Slider slider)
    {
        Vector3 tempPosition = Joystick.GetComponent<RectTransform>().anchoredPosition;

        Joystick.GetComponent<RectTransform>().anchoredPosition = new Vector3(slider.value, tempPosition.y, tempPosition.z);
    }

    public void OnYPositionChanged(Slider slider)
    {
        Vector3 tempPosition = Joystick.GetComponent<RectTransform>().anchoredPosition;

        Joystick.GetComponent<RectTransform>().anchoredPosition = new Vector3(tempPosition.x, slider.value, tempPosition.z);
    }

    public void OnJoystickSizeChanged(Slider slider)
    {
        Joystick.GetComponent<RectTransform>().sizeDelta = new Vector2(slider.value, slider.value);
    }

    public void OnHandleSizeChanged(Slider slider)
    {
        Joystick.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slider.value, slider.value);
    }

    public void OnJoystickSettingButtonClicked()
    {
        joystickValues = GameManager.instance.settings.joystickValues;
        
        Joystick.GetComponent<Joystick>().SetJoystick(joystickValues);
        XPositionSlider.value = joystickValues.XPosition;
        YPositionSlider.value = joystickValues.YPosition;
        JoystickSizeSlider.value = joystickValues.joystickSize;
        HandleSizeSlider.value = joystickValues.handleSize;
        
        gameObject.SetActive(true);
    }

    public void OnExitButtonClicked()
    {
        joystickValues.XPosition = Joystick.GetComponent<RectTransform>().anchoredPosition.x;
        joystickValues.YPosition = Joystick.GetComponent<RectTransform>().anchoredPosition.y;
        joystickValues.joystickSize = Joystick.GetComponent<RectTransform>().sizeDelta.x;
        joystickValues.handleSize = Joystick.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        
        //GameManager.instance.WriteSettings();
        
        //TownManager.instance.joystick.GetComponent<Joystick>().SetJoystick(joystickValues);
        
        gameObject.SetActive(false);
    }
}

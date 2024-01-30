using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickValues
{
    public float XPosition;
    public float YPosition;
    public float joystickSize;
    public float handleSize;

    public JoystickValues(float xPosition, float yPosition, float joystickSize, float handleSize)
    {
        XPosition = xPosition;
        YPosition = yPosition;
        this.joystickSize = joystickSize;
        this.handleSize = handleSize;
    }

    public void SetValues(float xPosition, float yPosition, float joystickSize, float handleSize)
    {
        XPosition = xPosition;
        YPosition = yPosition;
        this.joystickSize = joystickSize;
        this.handleSize = handleSize;
    }
}

public class Settings
{
    public float backgroundMusicVolume;
    public float soundEffectsVolume;
    public bool minimapHolding;

    public JoystickValues joystickValues;
    
    public Settings()
    {
        backgroundMusicVolume = 1f;
        soundEffectsVolume = 1f;
        minimapHolding = true;

        joystickValues = new JoystickValues(286f, 255f, 351f, 128f);
    }
}

public class SettingsPanel : MonoBehaviour
{
    private Settings settings;

    public Slider backgroundMusicVolumeSlider;
    public Slider soundEffectsVolumeSlider;

    public Button backgroundMusicButton;
    [HideInInspector]
    public bool isBackgroundMusicPlaying;

    public Button soundEffectsButton;

    public Sprite roundButtonOnSprite;
    public Sprite roundButtonOffSprite;

    public Toggle minimapHoldingToggle;

    public GameObject minimapTouchPanle;

    public void OnBackgroundMusicButtonClicked()
    {
        if (isBackgroundMusicPlaying)
        {
            backgroundMusicButton.GetComponent<AudioSource>().Stop();
            backgroundMusicButton.image.sprite = roundButtonOnSprite;
            isBackgroundMusicPlaying = false;
        }
        else
        {
            isBackgroundMusicPlaying = true;
            backgroundMusicButton.GetComponent<AudioSource>().Play();
            backgroundMusicButton.image.sprite = roundButtonOffSprite;
        }
    }

    public void OnSoundEffectsButtonClicked()
    {
        soundEffectsButton.interactable = false;
        StartCoroutine(PlaySoundEffect());
    }

    IEnumerator PlaySoundEffect()
    {
        soundEffectsButton.GetComponent<AudioSource>().Play();

        while (soundEffectsButton.GetComponent<AudioSource>().isPlaying)
        {
            soundEffectsButton.image.sprite = roundButtonOnSprite;
            soundEffectsButton.interactable = true;
            yield return null;
        }
    }

    public void OnMinimapHoldingChanged()
    {
        settings.minimapHolding = minimapHoldingToggle.isOn;
    }

    public void OnSettingsButtonClicked()
    {
        // 게임 매니저에서 기존 설정 불러오기
        settings = GameManager.instance.settings;

        // 불러온 세팅 정보로 슬라이더와 토글, 버튼 등 초기화
        backgroundMusicVolumeSlider.value = settings.backgroundMusicVolume;
        soundEffectsVolumeSlider.value = settings.soundEffectsVolume;

        minimapHoldingToggle.isOn = settings.minimapHolding;
        
        gameObject.SetActive(true);
    }

    public void OnExitButtonClicked()
    {
        // 바뀐 설정을 게임 매니저에 저장, 미니맵 홀딩 설정은 위에서 바뀜
        settings.backgroundMusicVolume = backgroundMusicVolumeSlider.value;
        settings.soundEffectsVolume = soundEffectsVolumeSlider.value;

        //미니맵 작동방식 변경
        minimapTouchPanle.GetComponent<MinimapHoldingPanel>().ChangeMinimapPanelWorking();

        // 마을에 적용
        TownManager.instance.ApplySettings();
        GameManager.instance.WriteSettings();
        
        gameObject.SetActive(false);
    }
}

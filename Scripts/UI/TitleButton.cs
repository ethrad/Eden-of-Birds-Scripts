using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TitleButton : MonoBehaviour
{
    public GameObject BlockPanel;
    public GameObject FailedToConnectServerPanel;
    
    [SerializeField]
    private bool isTestMode;
    
    public void OnStartButtonClicked(string sceneName)
    {
        if (isTestMode){
            StartCoroutine(MoveScene(sceneName));
        }
        else
        {
            if (BackendManager.Instance.successedGPGSLogin)
            {
                StartCoroutine(MoveScene(sceneName));
            }
            else
            {
                FailedToConnectServerPanel.SetActive(true);
            }
        }
    }

    IEnumerator MoveScene(string sceneName)
    {
        GameManager.instance.FadeOutEffect();
        while (!GameManager.instance.isFadeOutEnd)
        {
            yield return null;
        }
        GameManager.instance.ResetFadeFlags();
        SceneManager.LoadScene(sceneName);
    }

    private void Start()
    {
        GetComponent<AudioSource>().volume = GameManager.instance.settings.backgroundMusicVolume;

        if (isTestMode)
        {
            GameManager.instance.ReadDataFromServer();
            BlockPanel.SetActive(false);
        }
        else
        {
            BackendManager.Instance.GPGSLogin();
        }
    }

    private void Update()
    {
        if (BackendManager.Instance.isGPGSEnd)
        {
            BlockPanel.SetActive(false);
        }
    }
}

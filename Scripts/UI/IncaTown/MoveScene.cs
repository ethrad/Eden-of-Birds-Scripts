using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public void MoveSceneButton(string sceneName)
    {
        StartCoroutine(FadeEffect(sceneName));
    }
    
     IEnumerator FadeEffect(string sceneName)
        {
            GameManager.instance.FadeOutEffect();
            while (!GameManager.instance.isFadeOutEnd)
            {
                yield return null;
            }
            GameManager.instance.FadeInEffect();
            GameManager.instance.ResetFadeFlags();
            SceneManager.LoadScene(sceneName);
        }
}

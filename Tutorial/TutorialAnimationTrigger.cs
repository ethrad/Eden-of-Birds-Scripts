using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialAnimationTrigger : MonoBehaviour
{
    public GameObject anim;
    public GameObject panel;

    int conversationCount = 0;
    public void StartConversation()
    {
        panel.SetActive(true);
    }

    public void EndConversation()
    {
        conversationCount++;

        if (conversationCount == 1)
        {
            anim.GetComponent<Animator>().SetTrigger("EndConversation");
        }
        else if (conversationCount == 2)
        {
            SceneManager.LoadScene("TownTutorial");
        }
        else if (conversationCount == 6)
        {
            SceneManager.LoadScene("FieldTutorial");
        }
        else if(conversationCount == 8)
        {
            SceneManager.LoadScene("TutorialTycoon");
            Destroy(this.gameObject);
        }

        panel.SetActive(false);
    }
}

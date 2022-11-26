using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownPortal : MonoBehaviour
{
    public string dungeonName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(dungeonName);
    }
}

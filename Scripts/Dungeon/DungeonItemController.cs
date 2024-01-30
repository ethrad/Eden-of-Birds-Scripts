using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonItemController : MonoBehaviour
{
    public GameObject itemSprite;
    string itemName;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            DungeonManager.instance.GetItem(itemName);
            Destroy(gameObject);
        }
    }

    public void Initialize(string name)
    {
        itemName = name;
        
        itemSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Dots/Items/" + name);            
        GetComponent<AudioSource>().volume = GameManager.instance.settings.soundEffectsVolume;
        
        gameObject.SetActive(true);
    }
}

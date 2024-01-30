using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMonster
{
    public class BossItemController : MonoBehaviour
    {
        public GameObject itemSprite;
        private string itemName;
        private int count;

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                DungeonManager.instance.GetItem(itemName, count);
                Destroy(gameObject);
            }
        }

        public void Initialize(string name, int count)
        {
            itemName = name;
            this.count = count;
        
            itemSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Dots/Items/" + name);
            GetComponent<AudioSource>().volume = GameManager.instance.settings.soundEffectsVolume;
            gameObject.SetActive(true);
        }
    }

}

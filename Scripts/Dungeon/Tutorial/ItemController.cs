using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class ItemController : MonoBehaviour
    {
        public GameObject itemSprite;
        string itemName;

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                TutorialDungeonManager.instance.GetItem(itemName);
                Destroy(gameObject);
            }
        }

        public void Initialize(string name)
        {
            itemName = name;

            itemSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Dots/Items/" + name);

            gameObject.SetActive(true);
        }
    }
}

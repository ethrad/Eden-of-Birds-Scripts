using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeon
{
    public class ChestController : MonoBehaviour
    {
        private bool canOpen = true;

        AudioSource audioSource;
        public Sprite openedChestSprite;
        private GameObject aim;

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player") && canOpen)
            {
                DungeonManager.instance.OnChestButton(gameObject);
                aim.SetActive(true);
            }
        }

        public void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player") && canOpen)
            {
                DungeonManager.instance.OffChestButton();
                aim.SetActive(false);
            }
        }

        public void OpenChest()
        {
            canOpen = false;
            aim.SetActive(false);
            audioSource.Play();
            GetComponent<SpriteRenderer>().sprite = openedChestSprite;

            int random = Random.Range(0, 100);

            int equipmentIndex = 0;
            int tempProb = 0;

            List<Equipment> equipmentList = DungeonManager.instance.equipmentList;

            for (int i = 0; i < equipmentList.Count; i++)
            {
                if (random > equipmentList[i].probs[DungeonManager.instance.floor] + tempProb)
                {
                    equipmentIndex = i;
                }
                else break;

                tempProb += equipmentList[i].probs[DungeonManager.instance.floor];
            }

            if (equipmentIndex != -1)
            {
                StartCoroutine(DelayEquipmentPanel(equipmentList[equipmentIndex]));
            }
        }

        IEnumerator DelayEquipmentPanel(Equipment equipment)
        {
            yield return new WaitForSeconds(1f);

            DungeonManager.instance.OnChestPanel(equipment);

            yield break;
        }

        private void Start()
        {
            aim = transform.GetChild(0).gameObject;
            audioSource = GetComponent<AudioSource>();
        }
    }
}

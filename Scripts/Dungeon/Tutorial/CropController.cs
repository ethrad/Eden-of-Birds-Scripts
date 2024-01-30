using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tutorial
{
    public class CropController : MonoBehaviour
    {
        public string[] itemNames;
        public int[] itemProb;
        private GameObject aim;

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                TutorialDungeonManager.instance.OnHarvestButton(gameObject);
                aim.SetActive(true);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                TutorialDungeonManager.instance.OffHarvestButton();
                aim.SetActive(false);
            }
        }

        public virtual void Harvest()
        {
            TutorialDungeonManager.instance.StopPlayerMoving();

            int random = Random.Range(0, 100);

            int itemIndex = -1;
            int tempProb = 0;

            for (int i = 0; i < itemProb.Length; i++)
            {
                if (random < itemProb[i] + tempProb)
                {
                    itemIndex = i;
                    break;
                }

                tempProb += itemProb[i];
            }

            if (itemIndex != -1)
            {
                StartCoroutine(HarvestDelay(itemIndex));
            }

            TutorialDungeonManager.instance.OnRockHarvested();
        }
        IEnumerator HarvestDelay(int itemIndex)
        {
            yield return new WaitForSeconds(1.6f);

            GameObject tempItem = Instantiate(TutorialDungeonManager.instance.dropItemPrefab, transform.position + Vector3.up * 0.06f, Quaternion.Euler(0, 0, 0));
            tempItem.GetComponent<ItemController>().Initialize(itemNames[itemIndex]);

            TutorialDungeonManager.instance.StartPlayerMoving(false);
            Destroy(gameObject);
        }

        private void Start()
        {
            aim = transform.GetChild(0).gameObject;
        }
    }

}

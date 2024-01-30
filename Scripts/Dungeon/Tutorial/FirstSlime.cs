using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class FirstSlime : SlimeController
    {
        public GameObject eagle;
        
        protected override void Dead()
        {
            audioSource.clip = audioDead;
            audioSource.Play();
        
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
                GameObject tempItem = Instantiate(TutorialDungeonManager.instance.dropItemPrefab, transform.position + Vector3.up * 0.06f, Quaternion.Euler(0, 0, 0));
                tempItem.GetComponent<ItemController>().Initialize(itemNames[itemIndex]);
            }
        
            eagle.SetActive(false);
            TutorialDungeonManager.instance.OnPanel();
            Destroy(gameObject);

        }
    }
}

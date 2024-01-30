using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class ByoPeddlerController : NPCController
    {
        [HideInInspector]
        public Animator anim;
        
        public override void Interact()
        {
            DungeonManager.instance.OnByoPeddlerPanel();
        }

        public void OnVomitAnimation()
        {
            StartCoroutine(WaitVomit());
        }

        private IEnumerator WaitVomit()
        {
            anim.SetTrigger("vomit");
            
            yield return new WaitForSeconds(2f);
            DungeonManager.instance.StartPlayerMoving(true);
            yield break;
        }
        
        private void Start()
        {
            DungeonManager.instance.byoPeddler = gameObject;
            anim = GetComponent<Animator>();
        }
    }
}
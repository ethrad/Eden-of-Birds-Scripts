using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOTM
{
    public class MenuSlot : MonoBehaviour
    {
        public GameObject starPanel;
        public Image foodImage;
        public Text foodName;
        
        public void SetMenuSlot(string name, int grade)
        {
            foodImage.sprite = Resources.Load<Sprite>("Dots/Tycoon/Foods/" + name);
            foodName.text = GameManager.instance.menus[name].koreanName;

            for (int i = 0; i < grade; i++)
            {
                starPanel.transform.GetChild(i).gameObject.SetActive(true);
            }   
        }
    }

}

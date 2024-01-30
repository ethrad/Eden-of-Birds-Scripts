using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diary
{
    public class FoodSlotButton : MonoBehaviour
    {
        [HideInInspector] 
        public bool isDiscovery = false;
        public GameObject questionMark;

        public string engName;
        [HideInInspector] 
        public Menu menu;

        public void OnClickedFoodButton()
        {
            FoodPage temp = this.transform.parent.parent.parent.parent.GetComponent<FoodPage>();
            temp.SetFoodSlotDetail(engName);
            if(isDiscovery)
            {
                temp.nonePanel.SetActive(false);
            }
            else
            {
                temp.nonePanel.SetActive(true);
            }
        }

        public void UpdateFoodInfo(string wantString)
        {
            engName = wantString;
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/Foods/" + engName);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diary
{
    public class Page : MonoBehaviour
    {
        public GameObject page2;
        int pageIndex;
        public GameObject detailPanel;
        
        public virtual void Initialize()
        {
            pageIndex = 0;
        }

        public virtual void OnPage()
        {
            //page2.transform.GetChild(pageIndex).gameObject.SetActive(true);
            gameObject.SetActive(true);
            detailPanel.SetActive(true);
        }

        public virtual void OffPage()
        {
            page2.transform.GetChild(pageIndex).gameObject.SetActive(false);
            pageIndex = 0;
            gameObject.SetActive(false);
        }

        public void OnContentButtonClicked(Button button)
        {
            page2.transform.GetChild(pageIndex).gameObject.SetActive(false);
            pageIndex = Int32.Parse(button.name);
            page2.transform.GetChild(pageIndex).gameObject.SetActive(true);
        }
    }
}

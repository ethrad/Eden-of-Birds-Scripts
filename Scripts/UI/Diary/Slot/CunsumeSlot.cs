using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CunsumeSlot : MonoBehaviour
{
    public Text count;

    public void UpdateCunsumePanel(string wantName, string wantCount)
    {
        count.text = "x " + wantCount;
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Diary/ItemDots/" + wantName);
    }
}

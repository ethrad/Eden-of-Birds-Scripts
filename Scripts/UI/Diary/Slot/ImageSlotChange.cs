using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ImageSlotChange : MonoBehaviour
{
    [FormerlySerializedAs("name")] public string imageName;
    public void UpdateDropItemPanel(string path, string wantName)
    {
        imageName = wantName;
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>(path + wantName);
    }
}

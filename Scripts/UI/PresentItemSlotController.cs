using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentItemSlotController : MonoBehaviour
{
    public GameObject itemImage;
    public GameObject countText;
    string itemName;
    public bool isClickable;

    public void UpdateItem(string name, int count)
    {
        this.itemName = name;
        itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Items/" + name);
        countText.GetComponent<Text>().text = count.ToString();
    }

    public void OnPresentItemSlotClicked()
    {
        transform.parent.parent.GetComponent<PresentInventoryPanel>().UpdateDescriptionPanel(itemName);
    }
    
    void Start()
    {
        if (isClickable == true)
        {
            GetComponent<Button>().onClick.AddListener(OnPresentItemSlotClicked);
        }
        
    }
}

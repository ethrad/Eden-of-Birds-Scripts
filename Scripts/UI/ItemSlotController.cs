using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotController : MonoBehaviour
{
    public GameObject itemImage;
    public GameObject countText;
    string itemName;
    public bool isClickable;

    public void UpdateItem(string itemName, int count)
    {
        this.itemName = itemName;
        // 이미지 바꾸기
        itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Items/" + itemName);
        countText.GetComponent<Text>().text = count.ToString();
    }
    public void OnItemSlotClicked()
    {
        transform.parent.parent.GetComponent<InventoryPanel>().UpdateDescriptionPanel(itemName);
    }
    
    void Start()
    {
        if (isClickable == true)
        {
            GetComponent<Button>().onClick.AddListener(OnItemSlotClicked);
        }
        
    }
}

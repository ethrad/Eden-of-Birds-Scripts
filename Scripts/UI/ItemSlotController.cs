using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotController : MonoBehaviour
{
    public GameObject countText;
    string itemName;
    public bool isClickable;

    public void UpdateItem(string name, int count)
    {
        this.itemName = name;
        // 이미지 바꾸기
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/" + name);
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

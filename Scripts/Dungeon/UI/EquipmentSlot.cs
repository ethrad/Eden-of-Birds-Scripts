using Dungeon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Image equipmentImage;
    public Equipment equipment;

    public void UpdateSlot(Equipment equipment)
    {
        this.equipment = equipment;
        equipmentImage.sprite = Resources.Load<Sprite>("Dots/Dungeon/Equipments/" + equipment.type + "/" + equipment.grade);
    }
}

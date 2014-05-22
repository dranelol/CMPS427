using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equipment{

    public Attributes equipmentAttributes;

    
    public string equipmentName;
    public string equipmentType;
    public string flavorText;

    public equipSlots.slots validSlot;
    public int tier;
    public int minlvl;
    public int maxlvl;

    private Dictionary<string, equipSlots.slots> slotList = new Dictionary<string, equipSlots.slots>();


    public equipment()
    {
        equipmentAttributes = new Attributes();
        equipmentName = "default_equipment";
        equipmentType = "default_type";
        flavorText = "";
        validSlot = equipSlots.slots.Head;
        tier = 0;
        minlvl = 1;
        maxlvl = 20;
        slotList.Add("Head", equipSlots.slots.Head);
        slotList.Add("Legs", equipSlots.slots.Legs);
        slotList.Add("Feet", equipSlots.slots.Feet);
        slotList.Add("Chest", equipSlots.slots.Chest);
        slotList.Add("Main", equipSlots.slots.Main);
        slotList.Add("Off", equipSlots.slots.Off);
    }

    public void setslot(string setto)
    {
        validSlot = slotList[setto];
    }

}

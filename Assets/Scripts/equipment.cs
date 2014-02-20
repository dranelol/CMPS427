using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equipment{

    public Attributes equipmentAttributes;
    public string equipmentName;
    public string equipmentType;
    public equipmentFactory.slots validSlot;
    public int tier;



    private Dictionary<string, equipmentFactory.slots> slotList = new Dictionary<string, equipmentFactory.slots>();

    /*
    public enum slots
    {
        Head,
        Chest,
        Legs,
        Feet,
        Main,
        Off
    }
     * */

    public equipment()
    {
        equipmentAttributes = new Attributes();
        equipmentName = "default_equipment";
        equipmentType = "default_type";
        validSlot = equipmentFactory.slots.Head;
        tier = 0;
        slotList.Add("Head", equipmentFactory.slots.Head);
        slotList.Add("Legs", equipmentFactory.slots.Legs);
        slotList.Add("Feet", equipmentFactory.slots.Feet);
        slotList.Add("Chest", equipmentFactory.slots.Chest);
        slotList.Add("Main", equipmentFactory.slots.Main);
        slotList.Add("Off", equipmentFactory.slots.Off);
    }

    public void setslot(string setto)
    {
        validSlot = slotList[setto];
    }

}

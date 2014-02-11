using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class affix
{

    public Attributes affixAttributes;
    public string affixName;
    public string affixType;

    public ArrayList affixSlots = new ArrayList();

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
     */

    public affix()
    {
        affixAttributes = new Attributes();
        affixName = "default_affix";
        affixType = "prefix";
        slotList.Add("Head", equipmentFactory.slots.Head);
        slotList.Add("Legs", equipmentFactory.slots.Legs);
        slotList.Add("Feet", equipmentFactory.slots.Feet);
        slotList.Add("Chest", equipmentFactory.slots.Chest);
        slotList.Add("Main", equipmentFactory.slots.Main);
        slotList.Add("Off", equipmentFactory.slots.Off);


    }

    public void addslot(string setto)
    {
        equipmentFactory.slots validSlot = slotList[setto];
       affixSlots.Add(validSlot);
    }

}

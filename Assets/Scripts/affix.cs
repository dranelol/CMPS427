using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class affix
{

    public Attributes affixAttributes;
    public string affixName;
    public string affixType;

    public ArrayList affixSlots = new ArrayList();

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
    private Dictionary<string, equipSlots.slots> slotList = new Dictionary<string, equipSlots.slots>();


    public affix()
    {
        affixAttributes = new Attributes();
        affixName = "default_affix";
        affixType = "prefix";
        slotList.Add("Head", equipSlots.slots.Head);
        slotList.Add("Legs", equipSlots.slots.Legs);
        slotList.Add("Feet", equipSlots.slots.Feet);
        slotList.Add("Chest", equipSlots.slots.Chest);
        slotList.Add("Main", equipSlots.slots.Main);
        slotList.Add("Off", equipSlots.slots.Off);

    }

    public void addslot(string setto)
    {
        equipSlots.slots validSlot = slotList[setto];
       affixSlots.Add(validSlot);
    }

}

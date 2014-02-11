using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class affix
{

    public Attributes affixAttributes;
    public string affixName;
    public string affixType;

    public ArrayList affixSlots = new ArrayList();

    private Dictionary<string, slots> slotList = new Dictionary<string, slots>();


    public enum slots
    {
        Head,
        Chest,
        Legs,
        Feet,
        Main,
        Off
    }

    public affix()
    {
        affixAttributes = new Attributes();
        affixName = "default_affix";
        affixType = "prefix";
        slotList.Add("Head", slots.Head);
        slotList.Add("Legs", slots.Legs);
        slotList.Add("Feet", slots.Feet);
        slotList.Add("Chest", slots.Chest);
        slotList.Add("Main", slots.Main);
        slotList.Add("Off", slots.Off);


    }

    public void addslot(string setto)
    {
       slots validSlot = slotList[setto];
       affixSlots.Add(validSlot);
    }

}

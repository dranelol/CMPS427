using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class affix
{

    public Attributes affixAttributes;
    public string affixName;
    public equipSlots.affixtype affixType;

    public ArrayList affixSlots = new ArrayList();

    private Dictionary<string, equipSlots.slots> slotList = new Dictionary<string, equipSlots.slots>();


    public affix()
    {
        affixAttributes = new Attributes();
        affixName = "default_affix";
        affixType = equipSlots.affixtype.Prefix;
        slotList.Add("Head", equipSlots.slots.Head);
        slotList.Add("Legs", equipSlots.slots.Legs);
        slotList.Add("Feet", equipSlots.slots.Feet);
        slotList.Add("Chest", equipSlots.slots.Chest);
        slotList.Add("Main", equipSlots.slots.Main);
        slotList.Add("Off", equipSlots.slots.Off);

    }

    public affix(string name, equipSlots.affixtype type, float health, float resource, float power, float defense, float mindmg, float maxdmg, float movespeed, float attackspeed, equipSlots.slots[] validslots)
    {
        affixAttributes = new Attributes();
        affixName = name;
        affixType = type;
        affixAttributes.Health = health;
        affixAttributes.Resource = resource;
        affixAttributes.Power = power;
        affixAttributes.Defense = defense;
        affixAttributes.MinDamage = mindmg;
        affixAttributes.MaxDamage = maxdmg;
        affixAttributes.AttackSpeed = attackspeed;
        affixAttributes.MovementSpeed = movespeed;

        foreach (equipSlots.slots slot in validslots)
        {
            affixSlots.Add(slot);
        }

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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equipment{

    public Attributes equipmentAttributes;

    
    public string equipmentName;
    public equipSlots.equipmentType equipmentType;
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
        equipmentType = equipSlots.equipmentType.Book;
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

    public equipment(string name, equipSlots.equipmentType type, equipSlots.slots slot, int equipmentTier, int minlevel, int maxlevel, float health, float resource, float power, float defense, float mindmg, float maxdmg, float movespeed, float attackspeed, string flavortxt)
    {

        equipmentAttributes = new Attributes();
        equipmentName = name;
        equipmentType = type;
        flavorText = flavortxt;
        validSlot = slot;
        tier = equipmentTier;
        minlvl = minlevel;
        maxlvl = maxlevel;
        equipmentAttributes.Health = health;
        equipmentAttributes.Resource = resource;
        equipmentAttributes.Power = power;
        equipmentAttributes.Defense = defense;
        equipmentAttributes.MinDamage = mindmg;
        equipmentAttributes.MaxDamage = maxdmg;
        equipmentAttributes.AttackSpeed = attackspeed;
        equipmentAttributes.MovementSpeed = movespeed;
        

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

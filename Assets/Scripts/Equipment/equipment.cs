using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equipment{

    public const string FILEPATH = "weaponmodels/";

    public Attributes equipmentAttributes;

    
    public string equipmentName;
    public equipSlots.equipmentType equipmentType;
    public string flavorText;

    public equipSlots.slots validSlot;
    public int tier;
    public int minlvl;
    public int maxlvl;
    public bool twohand;
    public bool ranged;
    public string onhit;

    public string prefixname;
    public string suffixname;

    public string modelname;

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
        twohand = false;
        ranged = false;
        onhit = "";
        
        prefixname = "";
        suffixname = "";
        modelname = "default";
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

        twohand = false;
        ranged = false;
        onhit = "";

        prefixname = "";
        suffixname = "";

        modelname = "default";

    }

    public equipment(string name, equipSlots.equipmentType type, equipSlots.slots slot, int equipmentTier, int minlevel, int maxlevel, 
                        float health, float resource, float power, float defense, float mindmg, float maxdmg, float movespeed, float attackspeed,
                            string flavortxt, bool istwohand, bool isranged)
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

        twohand = istwohand;
        ranged = isranged;

        onhit = "";

        prefixname = "";
        suffixname = "";

        modelname = "default";

    }


    public equipment(string name, equipSlots.equipmentType type, equipSlots.slots slot, int equipmentTier, int minlevel, int maxlevel, 
                        float health, float resource, float power, float defense, float mindmg, float maxdmg, float movespeed, float attackspeed, 
                            string flavortxt, bool istwohand, bool isranged, string onhitability)
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

        twohand = istwohand;
        ranged = isranged;

        onhit = onhitability;

        prefixname = "";
        suffixname = "";

        modelname = "default";
        

    }


    public equipment(string name, equipSlots.equipmentType type, equipSlots.slots slot, int equipmentTier, int minlevel, int maxlevel,
                        float health, float resource, float power, float defense, float mindmg, float maxdmg, float movespeed, float attackspeed,
                            string flavortxt, bool istwohand, bool isranged, string onhitability, string model)
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

        twohand = istwohand;
        ranged = isranged;

        onhit = onhitability;

        prefixname = "";
        suffixname = "";

        modelname = model;


    }


    public equipment(string name, equipSlots.equipmentType type, equipSlots.slots slot, int equipmentTier, int minlevel, int maxlevel,
                        float health, float resource, float power, float defense, float mindmg, float maxdmg, float movespeed, float attackspeed,
                            string flavortxt, bool istwohand, bool isranged, string onhitability, string model, string prefixstring, string suffixstring)
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

        twohand = istwohand;
        ranged = isranged;

        onhit = onhitability;

        prefixname = prefixstring;
        suffixname = suffixstring;

        modelname = model;


    }


    public void setslot(string setto)
    {
        validSlot = slotList[setto];
    }

}

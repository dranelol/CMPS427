using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// equipment factory! use this to generate equipment!
/// </summary>
public class equipmentFactory 
{


    /*
     * This is the equipment factory!
     * 
     * We're going to need to have the game manager call the constructor, as that will 
     * load the base equipment and affixes into the game.
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */


    private ArrayList basesList = new ArrayList();
    private ArrayList affixeslist = new ArrayList();
    private ArrayList uniqueslist = new ArrayList();
    public bool testflag = true;

    

    public equipmentFactory()
    {
        
        loadBaseItems();
        loadAffixes();
        //loadtest();
    }


    #region monodevelop old code
    /*
	// Use this for initialization
	void Start () {
        loadBaseItems();
        loadAffixes();
    
	}
	
	// Update is called once per frame
	void Update () {



        if (testflag == true)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                loadtest();
            }
            if (Input.GetKeyDown(KeyCode.U))
            {

                equipment tempo = randomEquipment();
                string thingy = "name: " + tempo.equipmentName + " slot: " + tempo.validSlot.ToString() + " attributes: ";
                thingy = thingy + "health: " + tempo.equipmentAttributes.Health.ToString() + " ";
                thingy = thingy + "power: " + tempo.equipmentAttributes.Power.ToString() + " ";
                thingy = thingy + "defense: " + tempo.equipmentAttributes.Defense.ToString() + " ";
                thingy = thingy + "resource: " + tempo.equipmentAttributes.Resource.ToString() + " ";
                thingy = thingy + "attackspeed: " + tempo.equipmentAttributes.AttackSpeed.ToString() + " ";
                thingy = thingy + "movespeed: " + tempo.equipmentAttributes.MovementSpeed.ToString() + " ";
                thingy = thingy + "mindamage: " + tempo.equipmentAttributes.MinDamage.ToString() + " ";
                thingy = thingy + "maxdamage: " + tempo.equipmentAttributes.MaxDamage.ToString() + " ";

                Debug.Log(thingy);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

                equipment tempo = randomEquipment(1);
                string thingy = "name: " + tempo.equipmentName + " slot: " + tempo.validSlot.ToString() + " attributes: ";
                thingy = thingy + "health: " + tempo.equipmentAttributes.Health.ToString() + " ";
                thingy = thingy + "power: " + tempo.equipmentAttributes.Power.ToString() + " ";
                thingy = thingy + "defense: " + tempo.equipmentAttributes.Defense.ToString() + " ";
                thingy = thingy + "resource: " + tempo.equipmentAttributes.Resource.ToString() + " ";
                thingy = thingy + "attackspeed: " + tempo.equipmentAttributes.AttackSpeed.ToString() + " ";
                thingy = thingy + "movespeed: " + tempo.equipmentAttributes.MovementSpeed.ToString() + " ";
                thingy = thingy + "mindamage: " + tempo.equipmentAttributes.MinDamage.ToString() + " ";
                thingy = thingy + "maxdamage: " + tempo.equipmentAttributes.MaxDamage.ToString() + " ";

                Debug.Log(thingy);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {

                equipment tempo = randomEquipment(2);
                string thingy = "name: " + tempo.equipmentName + " slot: " + tempo.validSlot.ToString() + " attributes: ";
                thingy = thingy + "health: " + tempo.equipmentAttributes.Health.ToString() + " ";
                thingy = thingy + "power: " + tempo.equipmentAttributes.Power.ToString() + " ";
                thingy = thingy + "defense: " + tempo.equipmentAttributes.Defense.ToString() + " ";
                thingy = thingy + "resource: " + tempo.equipmentAttributes.Resource.ToString() + " ";
                thingy = thingy + "attackspeed: " + tempo.equipmentAttributes.AttackSpeed.ToString() + " ";
                thingy = thingy + "movespeed: " + tempo.equipmentAttributes.MovementSpeed.ToString() + " ";
                thingy = thingy + "mindamage: " + tempo.equipmentAttributes.MinDamage.ToString() + " ";
                thingy = thingy + "maxdamage: " + tempo.equipmentAttributes.MaxDamage.ToString() + " ";

                Debug.Log(thingy);
            }

        }
	}
    */
    #endregion


    /// <summary>
    /// outputs a bunch of stats about all of the base items and affixes stored
    /// </summary>
    public void loadtest()
    {
        foreach(equipment baseitem in basesList)
        {
            string thingy = "name: " + baseitem.equipmentName + " slot: " + baseitem.validSlot.ToString() + " attributes: ";
            thingy = thingy + "health: " + baseitem.equipmentAttributes.Health.ToString() + " ";
            thingy = thingy + "power: " + baseitem.equipmentAttributes.Power.ToString() + " ";
            thingy = thingy + "defense: " + baseitem.equipmentAttributes.Defense.ToString() + " ";
            thingy = thingy + "resource: " + baseitem.equipmentAttributes.Resource.ToString() + " ";
            thingy = thingy + "attackspeed: " + baseitem.equipmentAttributes.AttackSpeed.ToString() + " ";
            thingy = thingy + "movespeed: " + baseitem.equipmentAttributes.MovementSpeed.ToString() + " ";
            thingy = thingy + "mindamage: " + baseitem.equipmentAttributes.MinDamage.ToString() + " ";
            thingy = thingy + "maxdamage: " + baseitem.equipmentAttributes.MaxDamage.ToString() + " ";
            
            Debug.Log(thingy);
        }
        foreach (affix baseitem in affixeslist)
        {
            string thingy = "name: " + baseitem.affixName + " type: " + baseitem.affixType + " attributes: ";
            thingy = thingy + "health: " + baseitem.affixAttributes.Health.ToString() + " ";
            thingy = thingy + "power: " + baseitem.affixAttributes.Power.ToString() + " ";
            thingy = thingy + "defense: " + baseitem.affixAttributes.Defense.ToString() + " ";
            thingy = thingy + "resource: " + baseitem.affixAttributes.Resource.ToString() + " ";
            thingy = thingy + "attackspeed: " + baseitem.affixAttributes.AttackSpeed.ToString() + " ";
            thingy = thingy + "movespeed: " + baseitem.affixAttributes.MovementSpeed.ToString() + " ";
            thingy = thingy + "mindamage: " + baseitem.affixAttributes.MinDamage.ToString() + " ";
            thingy = thingy + "maxdamage: " + baseitem.affixAttributes.MaxDamage.ToString() + " ";
            

                Debug.Log(thingy);
        }
    }


    /// <summary>
    /// loads the base items from xml
    /// </summary>
    public void loadBaseItems()
    {
        equipment tempequip;
        
        #region weapons

            #region daggers
        
                                 //NAME             TYPE                              SLOT              tier level  h   r   p  d  mndg  mxdg ms  as   flavortext
        tempequip = new equipment("Rusty Dagger", equipSlots.equipmentType.Dagger, equipSlots.slots.Main, 0, 1, 4, 0f, 0f, 0f, 0f, 10f, 40f, 0f, 2.0f, "", false, false, "", "dagger1");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Dagger",  equipSlots.equipmentType.Dagger, equipSlots.slots.Main, 0, 5, 9, 0f, 0f, 0f, 0f, 20f, 50f, 0f, 2.0f, "", false, false, "", "dagger2");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Dagger", equipSlots.equipmentType.Dagger, equipSlots.slots.Main, 0, 10, 14, 0f, 0f, 0f, 0f, 30f, 60f, 0f, 2.0f, "", false, false, "", "dagger3");
        basesList.Add(tempequip);
        tempequip = new equipment("Ritual Dagger", equipSlots.equipmentType.Dagger, equipSlots.slots.Main, 0, 15, 20, 0f, 0f, 0f, 0f, 40f, 70f, 0f, 2.0f, "", false, false, "", "dagger4");
        basesList.Add(tempequip);
            #endregion

            #region swords

                                 //NAME               TYPE                              SLOT            tier level  h   r   p  d  mndg  mxdg ms  as   flavortext
        tempequip = new equipment("Rusty Sword",   equipSlots.equipmentType.Sword, equipSlots.slots.Main, 0, 1, 4, 0f, 0f, 0f, 0f, 10f, 80f, 0f, 1.5f, "", false, false, "", "sword1");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Sword",    equipSlots.equipmentType.Sword, equipSlots.slots.Main, 0, 5, 9, 0f, 0f, 0f, 0f, 20f, 90f, 0f, 1.5f, "", false, false, "", "sword2");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Sword",   equipSlots.equipmentType.Sword, equipSlots.slots.Main, 0, 10, 14, 0f, 0f, 0f, 0f, 30f, 100f, 0f, 1.5f, "", false, false, "", "sword3");
        basesList.Add(tempequip);
        tempequip = new equipment("Diamond Sword", equipSlots.equipmentType.Sword, equipSlots.slots.Main, 0, 15, 20, 0f, 0f, 0f, 0f, 40f, 110f, 0f, 1.5f, "", false, false, "", "sword4");
        basesList.Add(tempequip);

            #endregion

            #region clubs
                              //NAME               TYPE                              SLOT             tier level  h   r   p  d  mndg  mxdg ms  as   flavortext        
        tempequip = new equipment("Wooden Club",  equipSlots.equipmentType.Club, equipSlots.slots.Main, 0, 1, 4, 0f, 0f, 0f, 0f, 10f, 60f, 0f, 1.7f, "", false, false, "", "club1");
        basesList.Add(tempequip);
        tempequip = new equipment("Cudgel",       equipSlots.equipmentType.Club, equipSlots.slots.Main, 0, 5, 9, 0f, 0f, 0f, 0f, 20f, 70f, 0f, 1.7f, "", false, false, "", "club2");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Mace",   equipSlots.equipmentType.Club, equipSlots.slots.Main, 0, 10, 14, 0f, 0f, 0f, 0f, 30f, 80f, 0f, 1.7f, "", false, false, "", "club3");
        basesList.Add(tempequip);
        tempequip = new equipment("Morning Star", equipSlots.equipmentType.Club, equipSlots.slots.Main, 0, 15, 20, 0f, 0f, 0f, 0f, 40f, 90f, 0f, 1.7f, "", false, false, "", "club4");
        basesList.Add(tempequip);


        #endregion

            #region axes
                           //NAME                  TYPE                              SLOT           tier level  h   r   p  d  mndg  mxdg ms  as   flavortext        
        tempequip = new equipment("Rusty Axe",   equipSlots.equipmentType.Axe, equipSlots.slots.Main, 0, 3, 6, 0f, 0f, 0f, 0f, 10f, 100f, 0f, 1f, "", false, false, "", "axe1");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Axe",    equipSlots.equipmentType.Axe, equipSlots.slots.Main, 0, 7, 11, 0f, 0f, 0f, 0f, 20f, 110f, 0f, 1f, "", false, false, "", "axe2");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Axe",   equipSlots.equipmentType.Axe, equipSlots.slots.Main, 0, 12, 17, 0f, 0f, 0f, 0f, 30f, 120f, 0f, 1f, "", false, false, "", "axe3");
        basesList.Add(tempequip);
        tempequip = new equipment("Bearded Axe", equipSlots.equipmentType.Axe, equipSlots.slots.Main, 0, 18, 20, 0f, 0f, 0f, 0f, 40f, 130f, 0f, 1f, "", false, false, "", "axe4");
        basesList.Add(tempequip);

        #endregion

            #region spears
                                //NAME                  TYPE                            SLOT           tier level  h   r   p  d  mndg  mxdg ms  as   flavortext  
        tempequip = new equipment("Wooden Spear", equipSlots.equipmentType.Spear, equipSlots.slots.Main, 0, 1, 4, 0f, 0f, 0f, 0f, 10f, 70f, 0f, 1.5f, "", false, false, "", "spear1");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Spear",   equipSlots.equipmentType.Spear, equipSlots.slots.Main, 0, 5, 9, 0f, 0f, 0f, 0f, 20f, 80f, 0f, 1.5f, "", false, false, "", "spear2");
        basesList.Add(tempequip);
        tempequip = new equipment("Trident",     equipSlots.equipmentType.Spear, equipSlots.slots.Main, 0, 10, 14, 0f, 0f, 0f, 0f, 30f, 90f, 0f, 1.5f, "", false, false, "", "spear3");
        basesList.Add(tempequip);
        tempequip = new equipment("Halberd",     equipSlots.equipmentType.Spear, equipSlots.slots.Main, 0, 15, 20, 0f, 0f, 0f, 0f, 40f, 100f, 0f, 1.5f, "", false, false, "", "spear4");
        basesList.Add(tempequip);

        #endregion

        #region twohand

                                     //NAME                               TYPE                           SLOT               tier  level     h     r    p    d  mndg mxdg      ms      as   flavortext 
        tempequip = new equipment("BIGASS SWORD",              equipSlots.equipmentType.Sword,       equipSlots.slots.Main,   1, 20, 20,   0f,   0f,  0f,  0f,  10f,150f,      0f,     1f, "",true,false, "", "2hsword1");
        basesList.Add(tempequip);
        tempequip = new equipment("BIGASS THROWING SWORD",     equipSlots.equipmentType.Sword,       equipSlots.slots.Main,   1, 20, 20,   0f,   0f,  0f,  0f,  10f,150f,      0f,     1f, "",true,true, "", "2hsword1");
        basesList.Add(tempequip);         

        #endregion

        #endregion

        #region armor

        #region head
        //NAME                              TYPE                            SLOT             tier  level      h     r    p    d  mndg mxdg     ms     as   flavortext 
        tempequip = new equipment("Floppy Cap",                 equipSlots.equipmentType.ClothHelm,   equipSlots.slots.Head, 0,  1,  4,    0f,  25f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Linen Hat",             equipSlots.equipmentType.ClothHelm,   equipSlots.slots.Head, 0,  5,  9,    0f,  60f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Silk Hat",            equipSlots.equipmentType.ClothHelm,   equipSlots.slots.Head, 0, 10, 14,    0f, 100f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Yarmulke",                  equipSlots.equipmentType.ClothHelm,   equipSlots.slots.Head, 0, 15, 20,    0f, 200f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);

                                 //NAME                                   TYPE                            SLOT           tier  level      h     r    p   d   mndg mxdg      ms     as   flavortext 
        tempequip = new equipment("Leather Hat",               equipSlots.equipmentType.LeatherHelm, equipSlots.slots.Head, 0,  1,  4,  15f,  15f,  0f,  0f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Leather Skullcap",          equipSlots.equipmentType.LeatherHelm, equipSlots.slots.Head, 0,  5,  9,  35f,  35f,  0f,  0f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Tricorn",                   equipSlots.equipmentType.LeatherHelm, equipSlots.slots.Head, 0, 10, 14,  70f,  70f,  0f,  0f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Dragonhide Helm",           equipSlots.equipmentType.LeatherHelm, equipSlots.slots.Head, 0, 15, 20, 130f, 130f,  0f,  0f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);


                                     //NAME                          TYPE                              SLOT               tier  level     h     r    p   d   mndg mxdg      ms     as   flavortext 
        tempequip = new equipment("Rusty Chainmail Coif",      equipSlots.equipmentType.ChainHelm,   equipSlots.slots.Head, 0,  1,  4,  15f,   0f,  0f,  7f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Chainmail Coif",       equipSlots.equipmentType.ChainHelm,   equipSlots.slots.Head, 0,  5,  9,  35f,   0f,  0f, 13f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Chainmail Coif",      equipSlots.equipmentType.ChainHelm,   equipSlots.slots.Head, 0, 10, 14,  70f,   0f,  0f, 19f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Mithril Mail Coif",         equipSlots.equipmentType.ChainHelm,   equipSlots.slots.Head, 0, 15, 20, 130f,   0f,  0f, 25f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);

        
                                     //NAME                               TYPE                              SLOT          tier  level     h     r    p    d  mndg mxdg      ms     as   flavortext 
        tempequip = new equipment("Battered Helm",             equipSlots.equipmentType.ScaleHelm,   equipSlots.slots.Head, 0,  1,  4,  20f,   0f,  0f,  5f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Sallet",               equipSlots.equipmentType.ScaleHelm,   equipSlots.slots.Head, 0,  5,  9,  45f,   0f,  0f, 10f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Sallet",              equipSlots.equipmentType.ScaleHelm,   equipSlots.slots.Head, 0, 10, 14,  85f,   0f,  0f, 15f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Greatcrown",                equipSlots.equipmentType.ScaleHelm,   equipSlots.slots.Head, 0, 15, 20, 140f,   0f,  0f, 20f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);

                                     //NAME                              TYPE                              SLOT           tier  level     h     r    p    d  mndg mxdg      ms    as   flavortext 
        tempequip = new equipment("Rusty Barbute",             equipSlots.equipmentType.PlateHelm,   equipSlots.slots.Head, 0,  1,  4,  25f,   0f,  0f,  7f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Barbute",              equipSlots.equipmentType.PlateHelm,   equipSlots.slots.Head, 0,  5,  9,  50f,   0f,  0f, 13f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Bascinet",            equipSlots.equipmentType.PlateHelm,   equipSlots.slots.Head, 0, 10, 14, 100f,   0f,  0f, 19f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Mithril Greathelm",         equipSlots.equipmentType.PlateHelm,   equipSlots.slots.Head, 0, 15, 20, 150f,   0f,  0f, 25f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);


        #endregion

            #region chest

                                      //NAME                            TYPE                              SLOT              tier  level     h     r    p    d  mndg  mxdg    ms     as   flavortext 
        tempequip = new equipment("Clothes",                   equipSlots.equipmentType.ClothArmor,   equipSlots.slots.Chest, 0,  1,  4,   0f,  25f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Linen Robe",            equipSlots.equipmentType.ClothArmor,   equipSlots.slots.Chest, 0,  5,  9,   0f,  60f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Silk Robe",           equipSlots.equipmentType.ClothArmor,   equipSlots.slots.Chest, 0, 10, 14,   0f, 100f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Moonweave Robe",               equipSlots.equipmentType.ClothArmor,   equipSlots.slots.Chest, 0, 15, 20,   0f, 200f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);

                                 //NAME                                    TYPE                            SLOT            tier   level     h    r     p    d  mndg mxdg     ms      as   flavortext 
        tempequip = new equipment("Leather Jerkin",            equipSlots.equipmentType.LeatherArmor, equipSlots.slots.Chest, 0,  1,  4,  15f,  15f,  0f,  0f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Leather Armor",             equipSlots.equipmentType.LeatherArmor, equipSlots.slots.Chest, 0,  5,  9,  35f,  35f,  0f,  0f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Thief's Garb",              equipSlots.equipmentType.LeatherArmor, equipSlots.slots.Chest, 0, 10, 14,  70f,  70f,  0f,  0f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Assassin's Garb",           equipSlots.equipmentType.LeatherArmor, equipSlots.slots.Chest, 0, 15, 20, 130f, 130f,  0f,  0f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);


                                     //NAME                          TYPE                              SLOT                 tier  level     h     r    p    d  mndg mxdg     ms      as   flavortext 
        tempequip = new equipment("Rusty Chainmail Vest",      equipSlots.equipmentType.ChainArmor,   equipSlots.slots.Chest, 0,  1,  4,  15f,   0f,  0f,  7f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Chainmail Coat",       equipSlots.equipmentType.ChainArmor,   equipSlots.slots.Chest, 0,  5,  9,  35f,   0f,  0f, 13f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Chainmail Doublet",   equipSlots.equipmentType.ChainArmor,   equipSlots.slots.Chest, 0, 10, 14,  70f,   0f,  0f, 19f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Mithril Ringmail",          equipSlots.equipmentType.ChainArmor,   equipSlots.slots.Chest, 0, 15, 20, 130f,   0f,  0f, 25f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);

        
                                     //NAME                               TYPE                              SLOT            tier  level     h     r    p    d  mndg mxdg     ms    as   flavortext 
        tempequip = new equipment("Rusty Scale Vest",          equipSlots.equipmentType.ScaleArmor,   equipSlots.slots.Chest, 0,  1,  4,  20f,   0f,  0f,  5f,  0f,  0f,     0f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Scale Doublet",        equipSlots.equipmentType.ScaleArmor,   equipSlots.slots.Chest, 0,  5,  9,  45f,   0f,  0f, 10f,  0f,  0f,     0f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Scale Armor",         equipSlots.equipmentType.ScaleArmor,   equipSlots.slots.Chest, 0, 10, 14,  85f,   0f,  0f, 15f,  0f,  0f,     0f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Wyrmscale",                 equipSlots.equipmentType.ScaleArmor,   equipSlots.slots.Chest, 0, 15, 20, 140f,   0f,  0f, 20f,  0f,  0f,     0f,   0f, "");
        basesList.Add(tempequip);

                                     //NAME                              TYPE                              SLOT             tier  level     h     r    p    d  mndg mxdg      ms    as   flavortext 
        tempequip = new equipment("Rusty Chestplate",          equipSlots.equipmentType.PlateArmor,   equipSlots.slots.Chest, 0,  1,  4,  25f,   0f,  0f,  7f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Battleplate",          equipSlots.equipmentType.PlateArmor,   equipSlots.slots.Chest, 0,  5,  9,  50f,   0f,  0f, 13f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Chestplate",          equipSlots.equipmentType.PlateArmor,   equipSlots.slots.Chest, 0, 10, 14, 100f,   0f,  0f, 19f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Mithril Fullplate",         equipSlots.equipmentType.PlateArmor,   equipSlots.slots.Chest, 0, 15, 20, 150f,   0f,  0f, 25f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);


        #endregion

            #region legs

        
                                 //NAME                           TYPE                                    SLOT              tier  level     h     r    p    d  mndg  mxdg    ms     as   flavortext 
        tempequip = new equipment("Ragged Pants",               equipSlots.equipmentType.ClothPants,   equipSlots.slots.Legs,  0,  1,  4,   0f,  25f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Linen Pants",           equipSlots.equipmentType.ClothPants,   equipSlots.slots.Legs, 0,  5,  9,   0f,  60f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Silk Pants",          equipSlots.equipmentType.ClothPants,   equipSlots.slots.Legs, 0, 10, 14,   0f, 100f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Moonweave Pants",              equipSlots.equipmentType.ClothPants,   equipSlots.slots.Legs, 0, 15, 20,   0f, 200f,  0f,  0f,  0f,  0f,     0f,    0f, "");
        basesList.Add(tempequip);

                                 //NAME                                    TYPE                            SLOT             tier  level     h     r    p    d  mndg mxdg     ms      as   flavortext 
        tempequip = new equipment("Leather Pants",             equipSlots.equipmentType.LeatherPants, equipSlots.slots.Legs,  0,  1,  4,  15f,  15f,  0f,  0f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Leather Breeches",          equipSlots.equipmentType.LeatherPants, equipSlots.slots.Legs,  0,  5,  9,  35f,  35f,  0f,  0f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Thief's Breeches",          equipSlots.equipmentType.LeatherPants, equipSlots.slots.Legs,  0, 10, 14,  70f,  70f,  0f,  0f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Dragonhide Breeches",       equipSlots.equipmentType.LeatherPants, equipSlots.slots.Legs,  0, 15, 20, 130f, 130f,  0f,  0f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);


                                     //NAME                          TYPE                              SLOT                 tier  level     h     r    p    d  mndg mxdg     ms      as   flavortext 
        tempequip = new equipment("Rusty Chainmail Chausses",  equipSlots.equipmentType.ChainPants,   equipSlots.slots.Legs,  0,  1,  4,  15f,   0f,  0f,  7f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Chainmail Chausses",   equipSlots.equipmentType.ChainPants,   equipSlots.slots.Legs,  0,  5,  9,  35f,   0f,  0f, 13f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Chainmail Chausses",  equipSlots.equipmentType.ChainPants,   equipSlots.slots.Legs,  0, 10, 14,  70f,   0f,  0f, 19f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Mithril Ringmail Chausses", equipSlots.equipmentType.ChainPants,   equipSlots.slots.Legs,  0, 15, 20, 130f,   0f,  0f, 25f,  0f,  0f,     0f,     0f, "");
        basesList.Add(tempequip);

        
                                     //NAME                               TYPE                              SLOT            tier  level    h      r    p    d  mndg mxdg   ms    as   flavortext 
        tempequip = new equipment("Rusty Cuisses",             equipSlots.equipmentType.ScalePants,   equipSlots.slots.Legs,  0,  1,  4,  20f,   0f,  0f,  5f,  0f,  0f,   0f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Cuisses",              equipSlots.equipmentType.ScalePants,   equipSlots.slots.Legs,  0,  5,  9,  40f,   0f,  0f, 10f,  0f,  0f,   0f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Cuisses",             equipSlots.equipmentType.ScalePants,   equipSlots.slots.Legs,  0, 10, 14,  85f,   0f,  0f, 15f,  0f,  0f,   0f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Wyrmscale Cuisses",         equipSlots.equipmentType.ScalePants,   equipSlots.slots.Legs,  0, 15, 20, 140f,   0f,  0f, 20f,  0f,  0f,   0f,   0f, "");
        basesList.Add(tempequip);

                                     //NAME                              TYPE                              SLOT             tier  level     h     r    p    d  mndg mxdg      ms    as   flavortext 
        tempequip = new equipment("Rusty Faulds",              equipSlots.equipmentType.PlatePants,   equipSlots.slots.Legs,  0,  1,  4,  25f,   0f,  0f,  7f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Faulds",               equipSlots.equipmentType.PlatePants,   equipSlots.slots.Legs,  0,  5,  9,  50f,   0f,  0f, 13f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Faulds",              equipSlots.equipmentType.PlatePants,   equipSlots.slots.Legs,  0, 10, 14, 100f,   0f,  0f, 19f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Mithril Legplates",         equipSlots.equipmentType.PlatePants,   equipSlots.slots.Legs,  0, 15, 20, 150f,   0f,  0f, 25f,  0f,  0f,   -0.1f,   0f, "");
        basesList.Add(tempequip);



        #endregion

            #region feet

                
                                 //NAME                           TYPE                                    SLOT              tier  level     h     r    p    d  mndg  mxdg     ms     as   flavortext 
        tempequip = new equipment("Footwraps",                 equipSlots.equipmentType.ClothShoes,    equipSlots.slots.Feet, 0,  1,  4,   0f,  25f,  0f,  0f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Linen Slippers",        equipSlots.equipmentType.ClothShoes,    equipSlots.slots.Feet, 0,  5,  9,   0f,  60f,  0f,  0f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Silk Slippers",       equipSlots.equipmentType.ClothShoes,    equipSlots.slots.Feet, 0, 10, 14,   0f, 100f,  0f,  0f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Moonweave Slippers",           equipSlots.equipmentType.ClothShoes,    equipSlots.slots.Feet, 0, 15, 20,   0f, 200f,  0f,  0f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);

                                 //NAME                                    TYPE                            SLOT             tier  level     h     r    p    d  mndg mxdg      ms      as   flavortext 
        tempequip = new equipment("Leather Shoes",             equipSlots.equipmentType.LeatherShoes,  equipSlots.slots.Feet, 0,  1,  4,  15f,  15f,  0f,  0f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Leather Boots",             equipSlots.equipmentType.LeatherShoes,  equipSlots.slots.Feet, 0,  5,  9,  35f,  35f,  0f,  0f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Thief's Shoes",             equipSlots.equipmentType.LeatherShoes,  equipSlots.slots.Feet, 0, 10, 14,  70f,  70f,  0f,  0f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Dragonhide Boots",          equipSlots.equipmentType.LeatherShoes,  equipSlots.slots.Feet, 0, 15, 20, 130f, 130f,  0f,  0f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);


                                     //NAME                          TYPE                              SLOT                 tier  level     h     r    p    d  mndg mxdg      ms      as   flavortext 
        tempequip = new equipment("Rusty Chain Boots",         equipSlots.equipmentType.ChainShoes,    equipSlots.slots.Feet, 0,  1,  4,  15f,   0f,  0f,  7f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Chain Boots",          equipSlots.equipmentType.ChainShoes,    equipSlots.slots.Feet, 0,  5,  9,  35f,   0f,  0f, 13f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Chain Boots",         equipSlots.equipmentType.ChainShoes,    equipSlots.slots.Feet, 0, 10, 14,  70f,   0f,  0f, 19f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Mithril Ringmail Boots",    equipSlots.equipmentType.ChainShoes,    equipSlots.slots.Feet, 0, 15, 20, 130f,   0f,  0f, 25f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);

        
                                     //NAME                               TYPE                              SLOT            tier  level     h     r    p    d  mndg mxdg      ms      as   flavortext 
        tempequip = new equipment("Rusty Sabatons",             equipSlots.equipmentType.ScaleShoes,    equipSlots.slots.Feet, 0,  1,  4,  20f,   0f,  0f,  5f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Sabatons",              equipSlots.equipmentType.ScaleShoes,    equipSlots.slots.Feet, 0,  5,  9,  40f,   0f,  0f, 10f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Sabatons",             equipSlots.equipmentType.ScaleShoes,    equipSlots.slots.Feet, 0, 10, 14,  85f,   0f,  0f, 15f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Wyrmscale Sabatons",         equipSlots.equipmentType.ScaleShoes,    equipSlots.slots.Feet, 0, 15, 20, 140f,   0f,  0f, 20f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);

                                     //NAME                              TYPE                              SLOT             tier  level     h     r    p    d  mndg mxdg      ms      as   flavortext 
        tempequip = new equipment("Rusty Greaves",              equipSlots.equipmentType.PlateShoes,   equipSlots.slots.Feet, 0,  1,  4,  25f,   0f,  0f,  7f,  0f,  0f,   -0.1f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Greaves",               equipSlots.equipmentType.PlateShoes,   equipSlots.slots.Feet, 0,  5,  9,  50f,   0f,  0f, 13f,  0f,  0f,   -0.1f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Greaves",              equipSlots.equipmentType.PlateShoes,   equipSlots.slots.Feet, 0, 10, 14, 100f,   0f,  0f, 19f,  0f,  0f,   -0.1f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Mithril Greaves",            equipSlots.equipmentType.PlateShoes,   equipSlots.slots.Feet, 0, 15, 20, 150f,   0f,  0f, 25f,  0f,  0f,   -0.1f,     0f, "");
        basesList.Add(tempequip);



        #endregion

            #region offhand

                
                                 //NAME                           TYPE                                    SLOT           tier  level     h     r    p    d  mndg  mxdg     ms     as   flavortext 
        tempequip = new equipment("Hide Buckler",              equipSlots.equipmentType.SmallShield, equipSlots.slots.Off, 0,  1,  4,  10f,   0f,  0f,  2f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Iron Buckler",              equipSlots.equipmentType.SmallShield, equipSlots.slots.Off, 0,  5,  9,  25f,   0f,  0f,  5f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Studded Round Shield",      equipSlots.equipmentType.SmallShield, equipSlots.slots.Off, 0, 10, 14,  50f,   0f,  0f,  9f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Ironwood Round Shield",     equipSlots.equipmentType.SmallShield, equipSlots.slots.Off, 0, 15, 20, 100f,   0f,  0f, 14f,  0f,  0f,      0f,    0f, "");
        basesList.Add(tempequip);

                                 //NAME                                    TYPE                            SLOT          tier  level     h     r    p    d  mndg mxdg      ms      as   flavortext 
        tempequip = new equipment("Lashed Planks",             equipSlots.equipmentType.LargeShield, equipSlots.slots.Off, 0,  1,  4,  15f,  15f,  0f,  7f,  0f,  0f,   -0.1f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Kite Shield",               equipSlots.equipmentType.LargeShield, equipSlots.slots.Off, 0,  5,  9,  35f,  35f,  0f, 13f,  0f,  0f,   -0.1f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Steel Tower Shield",        equipSlots.equipmentType.LargeShield, equipSlots.slots.Off, 0, 10, 14,  70f,  70f,  0f, 19f,  0f,  0f,   -0.1f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Mythril Greatshield",       equipSlots.equipmentType.LargeShield, equipSlots.slots.Off, 0, 15, 20, 130f, 130f,  0f, 26f,  0f,  0f,   -0.1f,     0f, "");
        basesList.Add(tempequip);


                                     //NAME                          TYPE                              SLOT              tier  level     h     r    p    d  mndg mxdg      ms      as   flavortext 
        tempequip = new equipment("Moldy Spellbook",           equipSlots.equipmentType.Book,        equipSlots.slots.Off, 0,  1,  4,   0f,  15f,  0f,  5f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Forgotten Folio",           equipSlots.equipmentType.Book,        equipSlots.slots.Off, 0,  5,  9,   0f,  30f,  0f, 10f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Fiendish Codex",            equipSlots.equipmentType.Book,        equipSlots.slots.Off, 0, 10, 14,   0f,  45f,  0f, 15f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Forbidden Text",            equipSlots.equipmentType.Book,        equipSlots.slots.Off, 0, 15, 20,   0f,  60f,  0f, 20f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);

        
                                     //NAME                               TYPE                           SLOT            tier  level     h     r    p    d  mndg mxdg      ms      as   flavortext 
        tempequip = new equipment("Dun Orb",                   equipSlots.equipmentType.Orb,         equipSlots.slots.Off, 0,  1,  4,   0f,  30f,  0f,  0f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Glass Sphere",              equipSlots.equipmentType.Orb,         equipSlots.slots.Off, 0,  5,  9,   0f,  60f,  0f,  0f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Crystal Ball",              equipSlots.equipmentType.Orb,         equipSlots.slots.Off, 0, 10, 14,   0f,  90f,  0f,  0f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);
        tempequip = new equipment("Quicksilver Globe",         equipSlots.equipmentType.Orb,         equipSlots.slots.Off, 0, 15, 20,   0f, 120f,  0f,  0f,  0f,  0f,      0f,     0f, "");
        basesList.Add(tempequip);



        #endregion

        #endregion

        #region uniques

                                     //NAME                               TYPE                           SLOT               tier  level     h     r    p    d  mndg mxdg      ms      as   flavortext 
        tempequip = new equipment("Dux's Leek",                equipSlots.equipmentType.Sword,       equipSlots.slots.Main,   3,  1, 20,   0f,   0f,  0f,  0f,  1f,  1f,      0f,     1f, "Able to CUT the mightiest bush!");
        uniqueslist.Add(tempequip);

        tempequip = new equipment("Shankin' Stick",            equipSlots.equipmentType.Dagger,      equipSlots.slots.Main,   3,  1,  4,   0f,   0f,  0f,  0f, 20f, 50f,    0.5f,     2f, "Dis end da pointy one!");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("Mirror Shard",              equipSlots.equipmentType.Dagger,      equipSlots.slots.Main,   3,  5,  9,  50f,   0f,  0f,  0f, 30f, 60f,    0.5f,   2.0f, "There isn't really a good way to hold this thing.");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("McStabby",                  equipSlots.equipmentType.Dagger,      equipSlots.slots.Main,   3, 10, 15,   0f,  10f,  0f, 10f, 40f, 70f,      0f,   2.0f, "Cut the onions, extra cheese.");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("Vivisector",                equipSlots.equipmentType.Dagger,      equipSlots.slots.Main,   3, 15, 20,   0f, 100f,  0f,  0f, 50f, 80f,      0f,   2.0f, "This valorous visitation of a bygone vexation stands vivified, and has vowed to vanquish these venal and virulent vermin, van guarding vice and vouchsafing the violently vicious and voracious violation of volition.");
        uniqueslist.Add(tempequip);

        tempequip = new equipment("Swoop's Revenge",           equipSlots.equipmentType.Sword,       equipSlots.slots.Main,   3,  1,  4,  50f,   0f,  0f,  0f, 20f, 90f,      0f,   1.5f, "Who names their kid 'Swoop'?");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("The Bing Toolbar",          equipSlots.equipmentType.Sword,       equipSlots.slots.Main,   3,  5,  9,  20f, 100f,  0f,  0f, 30f,100f,      0f,   1.5f, "As painful as it is frustrating");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("Moonflair Spellblade",      equipSlots.equipmentType.Sword,       equipSlots.slots.Main,   3, 10, 15,   0f, 100f,  0f, 10f, 40f,110f,      0f,   1.5f, "What is this, 2009?");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("Darkin Blade",              equipSlots.equipmentType.Sword,       equipSlots.slots.Main,   3, 15, 20,   0f,   0f,  0f,  0f, 50f,120f,      0f,   1.5f, "Don't cut yourself, 'cause this is SO EDGY");
        uniqueslist.Add(tempequip);
        
        tempequip = new equipment("The Wrecking Stick",          equipSlots.equipmentType.Club,      equipSlots.slots.Main,   3,  1,  4,   0f,   0f, 20f,  0f, 20f, 70f,      0f,   1.7f, "This stick reckon's you're in the wrong town...");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("Mace of Spades",            equipSlots.equipmentType.Club,        equipSlots.slots.Main,   3,  5,  9,  40f,  40f,  0f,  0f, 30f, 80f,      0f,   1.7f, "You win some, lose some, all the same to me.");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("The Louisville Pulverizer", equipSlots.equipmentType.Club,        equipSlots.slots.Main,   3, 10, 15,   0f,   0f, 15f,  0f, 40f, 90f,    0.3f,   1.7f, "It's one, two, three strikes you're dead");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("The Tenderizer",            equipSlots.equipmentType.Club,        equipSlots.slots.Main,   3, 15, 20,  50f,   0f, 10f,  0f, 50f,115f,      0f,   1.7f, "It'll tenderize them so good, they'll fall apart right in front of you. Taste not Guaranteed.");
        uniqueslist.Add(tempequip);

        
        tempequip = new equipment("Boneprow",                  equipSlots.equipmentType.Axe,         equipSlots.slots.Main,   3,  3,  6,  50f,   0f,  0f, 20f, 20f,110f,      0f,   1.0f, "Not actually part of a ship. Or made of bones.");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("Noun's Axe of Verbing",     equipSlots.equipmentType.Axe,         equipSlots.slots.Main,   3,  7, 11,  30f,  30f, 10f, 10f, 31f,121f,   0.01f,  1.01f, "For those who want to cut the nerd crap");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("The Jacked Axe",            equipSlots.equipmentType.Axe,         equipSlots.slots.Main,   3, 12, 17,   0f,   0f, 50f,  0f,  0f,140f,      0f,   1.0f, "Do you even lift, brah?");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("The Chillaxe",              equipSlots.equipmentType.Axe,         equipSlots.slots.Main,   3, 18, 20, 100f,   0f, 10f,  0f, 50f,145f,      0f,   1.0f, "You need to chill, bro");
        uniqueslist.Add(tempequip);

        
        tempequip = new equipment("Phil",                      equipSlots.equipmentType.Spear,         equipSlots.slots.Main,   3,  1,  4,  50f,  50f,  0f,  0f, 20f, 80f,      0f,   1.5f, "Literally fedora.");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("Gáe Bolga",                 equipSlots.equipmentType.Spear,         equipSlots.slots.Main,   3,  5,  9,   0f,   0f,  0f,  0f, 35f, 95f,    0.3f,   1.5f, "A terrible barbed spear, said to be impossible to pull from its victim.");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("Gungnir",                   equipSlots.equipmentType.Spear,         equipSlots.slots.Main,   3, 10, 15,   0f,   0f, 15f, 15f, 40f,100f,      0f,   1.5f, "Thought lost in the stomach of a giant wolf");
        uniqueslist.Add(tempequip);
        tempequip = new equipment("Hades' Pitchfork",          equipSlots.equipmentType.Spear,         equipSlots.slots.Main,   3, 15, 20,   0f,1000f,  0f,  0f, 50f,110f,      0f,   1.5f, "The fire makes it great for a weapon, but pretty useless as a pitchfork.");
        uniqueslist.Add(tempequip);

        tempequip = new equipment("ONHITTEST", equipSlots.equipmentType.Sword, equipSlots.slots.Main, 3, 1, 20, 0, 0, 0, 0, 5, 5, 0, 1, "testing onhits", false, false, "onhitnormal");
        uniqueslist.Add(tempequip);

        #endregion


        #region legacy xml loading
        /*
        //xml reading stuff. this is probably a jank way to do it, but IT WORKS
        XmlDocument equipList = new XmlDocument();
        XmlReader reader = new XmlTextReader("Assets/Scripts/equipmentList.xml");
        try
        {
            equipList.Load("Assets/Scripts/equipmentList.xml");
        }
        catch
        {
            Debug.Log("equipmentlist fail");
        }

        //Debug.Log("after load");

        //Debug.Log(equipList.InnerXml);
        
        XmlNode root = equipList.ReadNode(reader);

        XmlNodeList bases = root.SelectNodes("type");

        //this is for each item in the file
        foreach(XmlNode item in bases)
        {
            //Debug.Log("in foreach");
            equipment newE = new equipment();


            XmlNode tempnode = item.SelectSingleNode("name");
            //Debug.Log(tempnode.InnerText);

            //since this is base, with no affixes, the item name and type are the same
            newE.equipmentName = tempnode.InnerText;
            newE.equipmentType = tempnode.InnerText;

            tempnode = item.SelectSingleNode("slot");

            newE.setslot(tempnode.InnerText);
            //Debug.Log(tempnode.InnerText);

            //min and max lvl
            tempnode = item.SelectSingleNode("minlvl");
            newE.minlvl = int.Parse(tempnode.InnerText);
            tempnode = item.SelectSingleNode("maxlvl");
            newE.maxlvl = int.Parse(tempnode.InnerText);

            //tier and flavor text
            if (item.SelectSingleNode("tier") != null)
            {
                tempnode = item.SelectSingleNode("tier");
                newE.tier = int.Parse(tempnode.InnerText);
            }
            if (item.SelectSingleNode("flavor") != null)
            {
                tempnode = item.SelectSingleNode("flavor");
                newE.flavorText = tempnode.InnerText;
            }


            //this is for each attribute for each item
            XmlNodeList atts = item.SelectNodes("attribute");
            foreach(XmlNode att in atts)
            {
                //this is a bit wierd, there probably is a better way to do this
                if(att.InnerText == "Health")
                {
                    newE.equipmentAttributes.Health += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "Power")
                {
                    newE.equipmentAttributes.Power += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "Resource")
                {
                    newE.equipmentAttributes.Resource += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "MoveSpeed")
                {
                    newE.equipmentAttributes.MovementSpeed += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "Defense")
                {
                    newE.equipmentAttributes.Defense += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "AttackSpeed")
                {
                    newE.equipmentAttributes.AttackSpeed += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "MinDamage")
                {
                    newE.equipmentAttributes.MinDamage += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "MaxDamage")
                {
                    newE.equipmentAttributes.MaxDamage += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else
                {
                    Debug.Log("CAUTION: " + att.InnerText + " is not in the Factory!");
                }

                //Debug.Log(att.InnerText + " " + att.Attributes.GetNamedItem("value").InnerText);
                
                
            }
            if (newE.tier == 0)
            {
                basesList.Add(newE);
            }
            else if (newE.tier == 3)
            {
                uniqueslist.Add(newE);
            }
        }
         * */
        #endregion


    }

    /// <summary>
    /// loads the affixes from xml
    /// </summary>
    public void loadAffixes()
    {
        affix tempaffix;
        

        #region prefixes
        /*
                                //NAME          TYPE                          h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("Keen",         equipSlots.affixtype.Prefix,   0f,   0f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Sturdy",       equipSlots.affixtype.Prefix,  10f,   0f,  0f,  5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Inspiring",    equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Swift",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Sure",         equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Wild",         equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f, -8f,  8f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Ancient",      equipSlots.affixtype.Prefix,   0f,  15f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Gilded",       equipSlots.affixtype.Prefix,   0f,  60f,-10f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Celestial",    equipSlots.affixtype.Prefix,   0f,  15f,  0f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Broken",       equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f, -8f, -8f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Ruined",       equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Beserker's",   equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Warrior's",    equipSlots.affixtype.Prefix,   0f,   0f, 10f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Mage's",       equipSlots.affixtype.Prefix,   0f,  40f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Thief's",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Paladin's",    equipSlots.affixtype.Prefix,  40f,   0f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Marauder's",   equipSlots.affixtype.Prefix,   0f,   0f,  0f, 10f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Solid",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  5f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Liquid",       equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Focused",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  8f, -8f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Cracked",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f, -8f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Aetheric",     equipSlots.affixtype.Prefix,  15f,  15f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Shimmering",   equipSlots.affixtype.Prefix,   0f,  40f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        */

        tempaffix = new affix("Stout",        equipSlots.affixtype.Prefix,  15f,   0f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Clever",       equipSlots.affixtype.Prefix,   0f,  15f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Strong",       equipSlots.affixtype.Prefix,   0f,   0f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Tough",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Precise",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Callous",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Brisk",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Quick",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Robust",       equipSlots.affixtype.Prefix,  30f,   0f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Shrewd",       equipSlots.affixtype.Prefix,   0f,  30f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Powerful",     equipSlots.affixtype.Prefix,   0f,   0f, 10f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Resilient",    equipSlots.affixtype.Prefix,   0f,   0f,  0f, 10f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Rapid",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Swift",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Priest's",     equipSlots.affixtype.Prefix,  15f,  15f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Barbarian's",  equipSlots.affixtype.Prefix,  15f,   0f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Fighter's",    equipSlots.affixtype.Prefix,  15f,   0f,  0f,  5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Berserker's",  equipSlots.affixtype.Prefix,  15f,   0f,  0f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Wanderer's",   equipSlots.affixtype.Prefix,  15f,   0f,  0f,  0f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Warlock's",    equipSlots.affixtype.Prefix,   0f,  15f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Paladin's",    equipSlots.affixtype.Prefix,   0f,  15f,  0f,  5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Swashbuckler's",equipSlots.affixtype.Prefix,  0f,  15f,  0f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Pilgrim's",    equipSlots.affixtype.Prefix,   0f,  15f,  0f,  0f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Knight's",     equipSlots.affixtype.Prefix,   0f,   0f,  5f,  5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Samurai's",    equipSlots.affixtype.Prefix,   0f,   0f,  5f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Nomad's",      equipSlots.affixtype.Prefix,   0f,   0f,  5f,  0f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Fencer's",     equipSlots.affixtype.Prefix,   0f,   0f,  0f,  5f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Crusader's",   equipSlots.affixtype.Prefix,   0f,   0f,  0f,  5f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Scout's",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,    0.1f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Vigorous",     equipSlots.affixtype.Prefix,  45f, -15f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Rotund",       equipSlots.affixtype.Prefix,  45f,   0f, -5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Virile",       equipSlots.affixtype.Prefix,  45f,   0f,  0f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Fecund",       equipSlots.affixtype.Prefix,  45f,   0f,  0f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Sound",        equipSlots.affixtype.Prefix,  45f,   0f,  0f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Wholesome",    equipSlots.affixtype.Prefix,  45f,   0f,  0f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Sturdy",       equipSlots.affixtype.Prefix,  45f,   0f,  0f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Glimmering",   equipSlots.affixtype.Prefix, -15f,  45f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Glinting",     equipSlots.affixtype.Prefix,   0f,  45f, -5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Shining",      equipSlots.affixtype.Prefix,   0f,  45f,  0f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Shimmering",   equipSlots.affixtype.Prefix,   0f,  45f,  0f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Aetheric",     equipSlots.affixtype.Prefix,   0f,  45f,  0f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Glistening",   equipSlots.affixtype.Prefix,   0f,  45f,  0f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Gleaming",     equipSlots.affixtype.Prefix,   0f,  45f,  0f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Sinewy",       equipSlots.affixtype.Prefix, -15f,   0f, 15f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Brawny",       equipSlots.affixtype.Prefix,   0f, -15f, 15f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Swarthy",      equipSlots.affixtype.Prefix,   0f,   0f, 15f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Burly",        equipSlots.affixtype.Prefix,   0f,   0f, 15f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Bold",         equipSlots.affixtype.Prefix,   0f,   0f, 15f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Ponderous",    equipSlots.affixtype.Prefix,   0f,   0f, 15f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Hearty",       equipSlots.affixtype.Prefix,   0f,   0f, 15f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Reinforced",   equipSlots.affixtype.Prefix, -15f,   0f,  0f, 15f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Layered",      equipSlots.affixtype.Prefix,   0f, -15f,  0f, 15f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Fortified",    equipSlots.affixtype.Prefix,   0f,   0f, -5f, 15f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Solid",        equipSlots.affixtype.Prefix,   0f,   0f,  0f, 15f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Buttressed",   equipSlots.affixtype.Prefix,   0f,   0f,  0f, 15f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Predatory",   equipSlots.affixtype.Prefix, -15f,   0f,  0f,  0f,  0f,  0f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Bloodthirsty",equipSlots.affixtype.Prefix,   0f, -15f,  0f,  0f,  0f,  0f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Savage",      equipSlots.affixtype.Prefix,   0f,   0f, -5f,  0f,  0f,  0f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Fierce",      equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f,  0f,  0f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Brutal",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,-10f,  0f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Murderous",   equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,-10f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Violent",     equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,     0.3f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Accelerated",  equipSlots.affixtype.Prefix, -15f,   0f,  0f,  0f,  0f,  0f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Fast",         equipSlots.affixtype.Prefix,   0f, -15f,  0f,  0f,  0f,  0f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Rushing",      equipSlots.affixtype.Prefix,   0f,   0f, -5f,  0f,  0f,  0f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Hurtling",     equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f,  0f,  0f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Reckless",     equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,-10f,  0f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Agile",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,-10f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Whizzing",     equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,   -0.1f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Razor-Sharp",  equipSlots.affixtype.Prefix, -15f,   0f,  0f,  0f, 20f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Honed",        equipSlots.affixtype.Prefix,   0f, -15f,  0f,  0f, 20f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Tempered",     equipSlots.affixtype.Prefix,   0f,   0f, -5f,  0f, 20f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Polished",     equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f, 20f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Burnished",    equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f, 20f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Edged",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f, 20f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Curved",       equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f, 20f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Serrated",     equipSlots.affixtype.Prefix, -15f,   0f,  0f,  0f,  0f, 20f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Wicked",       equipSlots.affixtype.Prefix,   0f, -15f,  0f,  0f,  0f, 20f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Spiteful",     equipSlots.affixtype.Prefix,   0f,   0f, -5f,  0f,  0f, 20f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Cruel",        equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f,  0f, 20f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Wild",         equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,-10f, 20f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Heavy",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f, 20f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Tyrranical",   equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f, 20f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Red",          equipSlots.affixtype.Prefix,  30f, -15f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Crimson",      equipSlots.affixtype.Prefix,  30f,   0f, -5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Burgandy",     equipSlots.affixtype.Prefix,  30f,   0f,  0f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Rose",         equipSlots.affixtype.Prefix,  30f,   0f,  0f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Ruby",         equipSlots.affixtype.Prefix,  30f,   0f,  0f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Vermilion",    equipSlots.affixtype.Prefix,  30f,   0f,  0f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Scarlet",      equipSlots.affixtype.Prefix,  30f,   0f,  0f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Blue",         equipSlots.affixtype.Prefix, -15f,  30f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Azure",        equipSlots.affixtype.Prefix,   0f,  30f, -5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Sapphire",     equipSlots.affixtype.Prefix,   0f,  30f,  0f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Cobalt",       equipSlots.affixtype.Prefix,   0f,  30f,  0f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Cerulean",     equipSlots.affixtype.Prefix,   0f,  30f,  0f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Aquamarine",   equipSlots.affixtype.Prefix,   0f,  30f,  0f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Cyan",         equipSlots.affixtype.Prefix,   0f,  30f,  0f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Green",        equipSlots.affixtype.Prefix, -15f,   0f, 10f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Verdant",      equipSlots.affixtype.Prefix,   0f, -15f, 10f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Jade",         equipSlots.affixtype.Prefix,   0f,   0f, 10f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Emerald",      equipSlots.affixtype.Prefix,   0f,   0f, 10f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Vernal",       equipSlots.affixtype.Prefix,   0f,   0f, 10f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Peridot",      equipSlots.affixtype.Prefix,   0f,   0f, 10f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Celadon",      equipSlots.affixtype.Prefix,   0f,   0f, 10f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Yellow",       equipSlots.affixtype.Prefix, -15f,   0f,  0f, 10f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Golden",       equipSlots.affixtype.Prefix,   0f, -15f,  0f, 10f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Marigold",     equipSlots.affixtype.Prefix,   0f,   0f,  5f, 10f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Goldenrod",    equipSlots.affixtype.Prefix,   0f,   0f,  0f, 10f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Beryl",        equipSlots.affixtype.Prefix,   0f,   0f,  0f, 10f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Amber",        equipSlots.affixtype.Prefix,   0f,   0f,  0f, 10f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Topaz",        equipSlots.affixtype.Prefix,   0f,   0f,  0f, 10f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Purple",       equipSlots.affixtype.Prefix, -15f,   0f,  0f,  0f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Violet",       equipSlots.affixtype.Prefix,   0f, -15f,  0f,  0f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Lavender",     equipSlots.affixtype.Prefix,   0f,   0f, -5f,  0f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Amethyst",     equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,-10f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Lilac",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,-10f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Indigo",       equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Mauve",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,    0.2f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Orange",       equipSlots.affixtype.Prefix, -15f,   0f,  0f,  0f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Coral",        equipSlots.affixtype.Prefix,   0f,  -5f,  0f,  0f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Saffron",      equipSlots.affixtype.Prefix,   0f,   0f, -5f,  0f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Harvest",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,-10f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Ochre",        equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,-10f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Coppered",     equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,   -0.1f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Brassed",      equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Steady",       equipSlots.affixtype.Prefix, -15f,   0f,  0f,  0f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Decisive",     equipSlots.affixtype.Prefix,   0f, -15f,  0f,  0f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Reflexive",    equipSlots.affixtype.Prefix,   0f,   0f, -5f,  0f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Darting",      equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Focused",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f, 10f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Careful",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f, 10f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Exacting",     equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f, 10f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("Barbarous",    equipSlots.affixtype.Prefix, -15f,   0f,  0f,  0f,  0f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Destructive",  equipSlots.affixtype.Prefix,   0f, -15f,  0f,  0f,  0f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Merciless",    equipSlots.affixtype.Prefix,   0f,   0f, -5f,  0f,  0f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Monstrous",    equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f,  0f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Disasterous",  equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,-10f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Chaotic",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f, 10f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Menacing",     equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f, 10f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        
        tempaffix = new affix("Flimsy",       equipSlots.affixtype.Prefix, -15f,   0f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Damaged",      equipSlots.affixtype.Prefix,   0f, -15f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Decrepit",     equipSlots.affixtype.Prefix,   0f,   0f, -5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Ruined",       equipSlots.affixtype.Prefix,   0f,   0f,  0f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Broken",       equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Busted",       equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Time-Worn",    equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("Aged",         equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);


        /*
        tempaffix = new affix("Example",      equipSlots.affixtype.Prefix,   0f,   0f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
         * */
        #endregion

        #region suffixes

        /*
        //NAME                   TYPE                                                  h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of Mercy",              equipSlots.affixtype.Suffix,   0f,   0f, -5f, 15f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Owl",            equipSlots.affixtype.Suffix,   0f,  50f,  0f,  0f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Head});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Striking",           equipSlots.affixtype.Suffix,   0f,   0f,  5f,  0f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Bear",           equipSlots.affixtype.Suffix,  20f,   0f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Squid",          equipSlots.affixtype.Suffix,   0f,  15f,  0f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Protection",         equipSlots.affixtype.Suffix,   0f,   0f,  0f,  5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Helix",          equipSlots.affixtype.Suffix,  10f,  10f, 10f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Painkiller",     equipSlots.affixtype.Suffix,  40f,   0f,  0f,  2f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Seventh Son",    equipSlots.affixtype.Suffix,   0f,   0f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("From Hell",             equipSlots.affixtype.Suffix,   0f,  10f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Mantis",         equipSlots.affixtype.Suffix,   0f,  40f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Raven",          equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,    0.1f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Wolf",           equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Monk",           equipSlots.affixtype.Suffix,  15f, -30f, 10f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Dragoon",        equipSlots.affixtype.Suffix, -30f,  30f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Assassin",       equipSlots.affixtype.Suffix, -20f,   0f,  0f,-10f, 14f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Cleric",         equipSlots.affixtype.Suffix,  30f,  20f, -5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Will",               equipSlots.affixtype.Suffix,   0f,  15f,  0f,  8f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Determination",      equipSlots.affixtype.Suffix,  25f,   0f,  6f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Agility",            equipSlots.affixtype.Suffix,   0f,   0f,  0f,  6f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Might",              equipSlots.affixtype.Suffix,   0f,   0f,  4f,  6f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Devastation",        equipSlots.affixtype.Suffix,   0f,   0f,  7f,  0f,  0f,  4f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Destruction",        equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  7f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Precision",          equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  5f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        */

                                        //NAME                   TYPE               h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of Stoutness",       equipSlots.affixtype.Suffix,  15f,   0f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Cleverness",      equipSlots.affixtype.Suffix,   0f,  15f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Strength",        equipSlots.affixtype.Suffix,   0f,   0f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Toughness",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Precision",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Callousness",     equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Briskness",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Quickness",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
                             //NAME                   TYPE                          h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of Robustness",      equipSlots.affixtype.Suffix,  30f,   0f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Shrewdness",      equipSlots.affixtype.Suffix,   0f,  30f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Power",           equipSlots.affixtype.Suffix,   0f,   0f, 10f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Resilience",      equipSlots.affixtype.Suffix,   0f,   0f,  0f, 10f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Rapidness",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Swiftness",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
                              //NAME                   TYPE                         h     r    p    d  mndg mxdg      ms      as          slots          
        tempaffix = new affix("of the Priest",      equipSlots.affixtype.Suffix,  15f,  15f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Barbarian",   equipSlots.affixtype.Suffix,  15f,   0f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Fighter",     equipSlots.affixtype.Suffix,  15f,   0f,  0f,  5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Berserker",   equipSlots.affixtype.Suffix,  15f,   0f,  0f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Wanderer",    equipSlots.affixtype.Suffix,  15f,   0f,  0f,  0f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Warlock",     equipSlots.affixtype.Suffix,   0f,  15f,  5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Paladin",     equipSlots.affixtype.Suffix,   0f,  15f,  0f,  5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Swashbuckler",equipSlots.affixtype.Suffix,  0f,  15f,  0f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Pilgrim",     equipSlots.affixtype.Suffix,   0f,  15f,  0f,  0f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Knight",      equipSlots.affixtype.Suffix,   0f,   0f,  5f,  5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Samurai",     equipSlots.affixtype.Suffix,   0f,   0f,  5f,  0f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Nomad",       equipSlots.affixtype.Suffix,   0f,   0f,  5f,  0f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Fencer",     equipSlots.affixtype.Suffix,   0f,   0f,  0f,  5f,  0f,  0f,    0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Crusader",    equipSlots.affixtype.Suffix,   0f,   0f,  0f,  5f,  0f,  0f,      0f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Scout",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,    0.1f,   0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
     //NAME                   TYPE                                                  h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of the Bison",       equipSlots.affixtype.Suffix,  45f, -15f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Giraffe",     equipSlots.affixtype.Suffix,  45f,   0f, -5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Buffalo",     equipSlots.affixtype.Suffix,  45f,   0f,  0f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Mammoth",     equipSlots.affixtype.Suffix,  45f,   0f,  0f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Sealion",     equipSlots.affixtype.Suffix,  45f,   0f,  0f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Camel",       equipSlots.affixtype.Suffix,  45f,   0f,  0f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the White Whale", equipSlots.affixtype.Suffix,  45f,   0f,  0f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
     //NAME                   TYPE                                                  h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of the Owl",         equipSlots.affixtype.Suffix, -15f,  45f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Orangutan",   equipSlots.affixtype.Suffix,   0f,  45f, -5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Raven",       equipSlots.affixtype.Suffix,   0f,  45f,  0f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Hyena",       equipSlots.affixtype.Suffix,   0f,  45f,  0f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Ferret",      equipSlots.affixtype.Suffix,   0f,  45f,  0f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Squid",       equipSlots.affixtype.Suffix,   0f,  45f,  0f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Octopus",     equipSlots.affixtype.Suffix,   0f,  45f,  0f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
                                //NAME                   TYPE                       h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of the Barracuda",   equipSlots.affixtype.Suffix, -15f,   0f, 15f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Bull",        equipSlots.affixtype.Suffix,   0f, -15f, 15f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Recluse",     equipSlots.affixtype.Suffix,   0f,   0f, 15f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Bear",        equipSlots.affixtype.Suffix,   0f,   0f, 15f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Honey Badger",equipSlots.affixtype.Suffix,   0f,   0f, 15f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Gorilla",     equipSlots.affixtype.Suffix,   0f,   0f, 15f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Crocodile",   equipSlots.affixtype.Suffix,   0f,   0f, 15f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
                                //NAME                   TYPE                       h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of the Armadillo",   equipSlots.affixtype.Suffix, -15f,   0f,  0f, 15f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Crab",        equipSlots.affixtype.Suffix,   0f, -15f,  0f, 15f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Pangolin",    equipSlots.affixtype.Suffix,   0f,   0f, -5f, 15f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Tortoise",    equipSlots.affixtype.Suffix,   0f,   0f,  0f, 15f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Snail",       equipSlots.affixtype.Suffix,   0f,   0f,  0f, 15f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
                                 //NAME                   TYPE                      h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of the Mantis",      equipSlots.affixtype.Suffix, -15f,   0f,  0f,  0f,  0f,  0f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Shark",       equipSlots.affixtype.Suffix,   0f, -15f,  0f,  0f,  0f,  0f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Cobra",       equipSlots.affixtype.Suffix,   0f,   0f, -5f,  0f,  0f,  0f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Wolf",        equipSlots.affixtype.Suffix,   0f,   0f,  0f, -5f,  0f,  0f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Boar",        equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,-10f,  0f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Rat",         equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,-10f,     0.3f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Wolverine",   equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,     0.3f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
                                //NAME                   TYPE                       h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of the Hare",        equipSlots.affixtype.Suffix, -15f,   0f,  0f,  0f,  0f,  0f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Roadrunner",  equipSlots.affixtype.Suffix,   0f, -15f,  0f,  0f,  0f,  0f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Deer",        equipSlots.affixtype.Suffix,   0f,   0f, -5f,  0f,  0f,  0f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Crane",       equipSlots.affixtype.Suffix,   0f,   0f,  0f, -5f,  0f,  0f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Cheetah",     equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,-10f,  0f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Horse",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,-10f,      0f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Hawk",        equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,   -0.1f,   0.3f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
                                     //NAME                   TYPE                  h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of Needling",        equipSlots.affixtype.Suffix, -15f,   0f,  0f,  0f, 20f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Skill",           equipSlots.affixtype.Suffix,   0f, -15f,  0f,  0f, 20f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Ease",            equipSlots.affixtype.Suffix,   0f,   0f, -5f,  0f, 20f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Mastery",         equipSlots.affixtype.Suffix,   0f,   0f,  0f, -5f, 20f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Nimbleness",      equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f, 20f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Talent",          equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f, 20f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Expertise",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f, 20f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
                                 //NAME                   TYPE                      h     r    p    d  mndg mxdg      ms      as          slots
        tempaffix = new affix("of Anger",           equipSlots.affixtype.Suffix, -15f,   0f,  0f,  0f,  0f, 20f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Rage",            equipSlots.affixtype.Suffix,   0f, -15f,  0f,  0f,  0f, 20f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Fury",            equipSlots.affixtype.Suffix,   0f,   0f, -5f,  0f,  0f, 20f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Ferocity",        equipSlots.affixtype.Suffix,   0f,   0f,  0f, -5f,  0f, 20f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Ire",             equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,-10f, 20f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Weight",          equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f, 20f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Calamity",        equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f, 20f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("of the Ox",          equipSlots.affixtype.Suffix,  30f, -15f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Zebra",       equipSlots.affixtype.Suffix,  30f,   0f, -5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Yak",         equipSlots.affixtype.Suffix,  30f,   0f,  0f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Elephant",    equipSlots.affixtype.Suffix,  30f,   0f,  0f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Walrus",      equipSlots.affixtype.Suffix,  30f,   0f,  0f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Llama",       equipSlots.affixtype.Suffix,  30f,   0f,  0f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Beluga",      equipSlots.affixtype.Suffix,  30f,   0f,  0f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("of the Parrot",      equipSlots.affixtype.Suffix, -15f,  30f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Monkey",      equipSlots.affixtype.Suffix,   0f,  30f, -5f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Crow",        equipSlots.affixtype.Suffix,   0f,  30f,  0f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Fox",         equipSlots.affixtype.Suffix,   0f,  30f,  0f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Weasel",      equipSlots.affixtype.Suffix,   0f,  30f,  0f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Nautilus",    equipSlots.affixtype.Suffix,   0f,  30f,  0f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Jellyfish",   equipSlots.affixtype.Suffix,   0f,  30f,  0f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("of the Piranha",     equipSlots.affixtype.Suffix, -15f,   0f, 10f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Wildebeest",  equipSlots.affixtype.Suffix,   0f, -15f, 10f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Spider",      equipSlots.affixtype.Suffix,   0f,   0f, 10f, -5f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Lion",        equipSlots.affixtype.Suffix,   0f,   0f, 10f,  0f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Badger",      equipSlots.affixtype.Suffix,   0f,   0f, 10f,  0f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Baboon",      equipSlots.affixtype.Suffix,   0f,   0f, 10f,  0f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Alligator",   equipSlots.affixtype.Suffix,   0f,   0f, 10f,  0f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("of the Porcupine",   equipSlots.affixtype.Suffix, -15f,   0f,  0f, 10f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Shrimp",      equipSlots.affixtype.Suffix,   0f, -15f,  0f, 10f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix); 
        tempaffix = new affix("of the Echidna",     equipSlots.affixtype.Suffix,   0f,   0f,  5f, 10f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Rhino",       equipSlots.affixtype.Suffix,   0f,   0f,  0f, 10f,-10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Crawfish",    equipSlots.affixtype.Suffix,   0f,   0f,  0f, 10f,  0f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Turtle",      equipSlots.affixtype.Suffix,   0f,   0f,  0f, 10f,  0f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Clam",        equipSlots.affixtype.Suffix,   0f,   0f,  0f, 10f,  0f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("of the Scorpion",    equipSlots.affixtype.Suffix, -15f,   0f,  0f,  0f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Swordfish",   equipSlots.affixtype.Suffix,   0f, -15f,  0f,  0f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Snake",       equipSlots.affixtype.Suffix,   0f,   0f, -5f,  0f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Hound",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,-10f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Warthog",     equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,-10f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Squirrel",    equipSlots.affixtype.Suffix,   0f,   0f,  0f, -5f,  0f,  0f,    0.2f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Mongoose",    equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,    0.2f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("of the Rabbit",      equipSlots.affixtype.Suffix, -15f,   0f,  0f,  0f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Ostrich",     equipSlots.affixtype.Suffix,   0f,  -5f,  0f,  0f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Antelope",    equipSlots.affixtype.Suffix,   0f,   0f, -5f,  0f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Heron",       equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,-10f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Panther",     equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,-10f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Donkey",      equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,   -0.1f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of the Osprey",      equipSlots.affixtype.Suffix,   0f,   0f,  0f, -5f,  0f,  0f,      0f,   0.2f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("of Steadiness",      equipSlots.affixtype.Suffix, -15f,   0f,  0f,  0f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Decision",        equipSlots.affixtype.Suffix,   0f, -15f,  0f,  0f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Reflexes",        equipSlots.affixtype.Suffix,   0f,   0f, -5f,  0f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Darting",         equipSlots.affixtype.Suffix,   0f,   0f,  0f, -5f, 10f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Focus",           equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f, 10f,-10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Care",            equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f, 10f,  0f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Aptitude",        equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f, 10f,  0f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);

        tempaffix = new affix("of the Void",        equipSlots.affixtype.Suffix, -15f,   0f,  0f,  0f,  0f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Destruction",     equipSlots.affixtype.Suffix,   0f, -15f,  0f,  0f,  0f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Unmaking",        equipSlots.affixtype.Suffix,   0f,   0f, -5f,  0f,  0f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Ruin",            equipSlots.affixtype.Suffix,   0f,   0f,  0f, -5f,  0f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Disaster",        equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,-10f, 10f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Havoc",           equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f, 10f,   -0.1f,     0f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        tempaffix = new affix("of Menace",          equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f, 10f,      0f,  -0.1f, new equipSlots.slots[] {equipSlots.slots.Main});
        affixeslist.Add(tempaffix);
        
        

        /*
        tempaffix = new affix("Example",               equipSlots.affixtype.Suffix,   0f,   0f,  0f,  0f,  0f,  0f,      0f,     0f, new equipSlots.slots[] {equipSlots.slots.Main, equipSlots.slots.Head, equipSlots.slots.Chest, equipSlots.slots.Legs, equipSlots.slots.Feet, equipSlots.slots.Off});
        affixeslist.Add(tempaffix);
         * */
        #endregion



        #region legacy xml code
        /*
        //xml reading stuff. this is probably a jank way to do it, but IT WORKS
        XmlDocument affixList = new XmlDocument();
        XmlReader reader = new XmlTextReader("Assets/Scripts/affixList.xml");
        try
        {
            affixList.Load("Assets/Scripts/affixList.xml");
        }
        catch
        {
            Debug.Log("affixlist fail");
        }

        //Debug.Log("after affix load");

        //Debug.Log(affixList.InnerXml);

        XmlNode root = affixList.ReadNode(reader);

        XmlNodeList affixes = root.SelectNodes("affix");

        //this is for each item in the file
        foreach (XmlNode affix in affixes)
        {
            //Debug.Log("in foreach");
            affix newA = new affix();

            newA.affixType = affix.Attributes.GetNamedItem("category").InnerText;

            XmlNode tempnode = affix.SelectSingleNode("name");
            //Debug.Log(tempnode.InnerText);

            
            newA.affixName = tempnode.InnerText;
            
            

            //this is for each slot for the affix
            XmlNodeList atts = affix.SelectNodes("slot");

            foreach (XmlNode att in atts)
            {
                newA.addslot(att.InnerText);
                //Debug.Log(att.InnerText);
            }



            //this is for each attribute for each item
            atts = affix.SelectNodes("attribute");
            foreach (XmlNode att in atts)
            {
                //this is a bit wierd, there probably is a better way to do this
                if (att.InnerText == "Health")
                {
                    newA.affixAttributes.Health += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "Power")
                {
                    newA.affixAttributes.Power += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "Resource")
                {
                    newA.affixAttributes.Resource += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "MoveSpeed")
                {
                    newA.affixAttributes.MovementSpeed += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "Defense")
                {
                    newA.affixAttributes.Defense += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "AttackSpeed")
                {
                    newA.affixAttributes.AttackSpeed += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "MinDamage")
                {
                    newA.affixAttributes.MinDamage += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else if (att.InnerText == "MaxDamage")
                {
                    newA.affixAttributes.MaxDamage += float.Parse(att.Attributes.GetNamedItem("value").InnerText);
                }
                else
                {
                    Debug.Log("CAUTION: " + att.InnerText + " is not in the Factory!");
                }

                //Debug.Log(att.InnerText + " " + att.Attributes.GetNamedItem("value").InnerText);

            }

            affixeslist.Add(newA);

        }
         * */
        #endregion




    }
    /*
    public void webequip()
    {
        string downloadedString;
        WebClient client;
        string dl2;

        client = new WebClient();
        downloadedString = client.DownloadString("http://www.wordgenerator.net/application/p.php?id=nouns&type=50&spaceflag=false");            
        string[] randomnoun = downloadedString.Split(',');
        //dl2 = client.DownloadString("http://www.wordgenerator.net/application/p.php?id=adjectives&type=50&spaceflag=false");
        //string[] randomadj = dl2.Split(',');
        
        Debug.Log(randomnoun[1]+ " "+ randomnoun[2]);

    }
    */
    /// <summary>
    /// builds a default equipment
    /// </summary>
    /// <returns>an equipment object</returns>
    public equipment buildEquipment() {

        equipment tempEquipment = new equipment();
        return tempEquipment;

    }



    /// <summary>
    /// Method for loading an equipment from playerprefs
    /// </summary>
    /// <param name="i">the ID of the equipment in storage</param>
    /// <returns></returns>
    public equipment loadequipment(string i)
    {
        //if (!PlayerPrefs.HasKey(i + "name")) return null;

        equipment tempequip = new equipment(PlayerPrefs.GetString(i + "name"),
                                            (equipSlots.equipmentType)Enum.Parse(typeof(equipSlots.equipmentType), PlayerPrefs.GetString(i + "type"), true),
                                            (equipSlots.slots)Enum.Parse(typeof(equipSlots.slots), PlayerPrefs.GetString(i + "slot"), true),
                                            PlayerPrefs.GetInt(i + "tier"),
                                            PlayerPrefs.GetInt(i + "minlvl"),
                                            PlayerPrefs.GetInt(i + "maxlvl"),
                                            PlayerPrefs.GetFloat(i + "health"),
                                            PlayerPrefs.GetFloat(i + "resource"),
                                            PlayerPrefs.GetFloat(i + "power"),
                                            PlayerPrefs.GetFloat(i + "defense"),
                                            PlayerPrefs.GetFloat(i + "mindmg"),
                                            PlayerPrefs.GetFloat(i + "maxdmg"),
                                            PlayerPrefs.GetFloat(i + "movespeed"),
                                            PlayerPrefs.GetFloat(i + "attackspeed"),
                                            PlayerPrefs.GetString(i + "flavortext"),
                                            Convert.ToBoolean(PlayerPrefs.GetInt(i + "istwohand")),
                                            Convert.ToBoolean(PlayerPrefs.GetInt(i + "isranged")),
                                            PlayerPrefs.GetString(i + "onhitability"),
                                            PlayerPrefs.GetString(i+ "modelname"),
                                            PlayerPrefs.GetString(i+"prefixname"),
                                            PlayerPrefs.GetString(i+"suffixname"));
                   
     
        return tempequip;
    }

    /// <summary>
    /// Method for saving an equipment to playerprefs
    /// </summary>
    /// <param name="i">the ID of the equipment in storage</param>
    /// <param name="item"> the equipment to store</param>
    /// <returns></returns>
    public void saveequipment(string i, equipment item)
    {

        PlayerPrefs.SetString(i + "name", item.equipmentName);
        PlayerPrefs.SetString(i + "type", item.equipmentType.ToString());
        PlayerPrefs.SetString(i + "slot", item.validSlot.ToString());
        PlayerPrefs.SetInt(i + "tier", item.tier);
        PlayerPrefs.SetInt(i + "minlvl", item.minlvl);
        PlayerPrefs.SetInt(i + "maxlvl", item.maxlvl);
        PlayerPrefs.SetFloat(i + "health", item.equipmentAttributes.Health);
        PlayerPrefs.SetFloat(i + "resource", item.equipmentAttributes.Resource);
        PlayerPrefs.SetFloat(i + "power", item.equipmentAttributes.Power);
        PlayerPrefs.SetFloat(i + "defense", item.equipmentAttributes.Defense);
        PlayerPrefs.SetFloat(i + "mindmg", item.equipmentAttributes.MinDamage);
        PlayerPrefs.SetFloat(i + "maxdmg", item.equipmentAttributes.MaxDamage);
        PlayerPrefs.SetFloat(i + "movespeed", item.equipmentAttributes.MovementSpeed);
        PlayerPrefs.SetFloat(i + "attackspeed", item.equipmentAttributes.AttackSpeed);
        PlayerPrefs.SetString(i + "flavortext", item.flavorText);
        if (item.twohand == true)
        {
            PlayerPrefs.SetInt(i + "istwohand", 1);
        }
        else
        {
            PlayerPrefs.SetInt(i + "istwohand", 0);
        }
        if (item.ranged == true)
        {
            PlayerPrefs.SetInt(i + "isranged", 1);
        }
        else
        {
            PlayerPrefs.SetInt(i + "isranged", 0);
        }
        PlayerPrefs.SetString(i + "onhitability", item.onhit);
        PlayerPrefs.SetString(i + "modelname", item.modelname);
        PlayerPrefs.SetString(i + "prefixname", item.prefixname);
        PlayerPrefs.SetString(i + "suffixname", item.suffixname);

    }


    /// <summary>
    /// method for removing an equipment from the save
    /// </summary>
    /// <param name="i">the id of the equipment in storage</param>
    public void unsaveEquipment(string i)
    {
        PlayerPrefs.DeleteKey(i + "name");
        PlayerPrefs.DeleteKey(i + "type");
        PlayerPrefs.DeleteKey(i + "slot");
        PlayerPrefs.DeleteKey(i + "tier");
        PlayerPrefs.DeleteKey(i + "minlvl");
        PlayerPrefs.DeleteKey(i + "maxlvl");
        PlayerPrefs.DeleteKey(i + "health");
        PlayerPrefs.DeleteKey(i + "resource");
        PlayerPrefs.DeleteKey(i + "power");
        PlayerPrefs.DeleteKey(i + "defense");
        PlayerPrefs.DeleteKey(i + "mindmg");
        PlayerPrefs.DeleteKey(i + "maxdmg");
        PlayerPrefs.DeleteKey(i + "movespeed");
        PlayerPrefs.DeleteKey(i + "attackspeed");
        PlayerPrefs.DeleteKey(i + "flavortext");
        PlayerPrefs.DeleteKey(i + "istwohand");
        PlayerPrefs.DeleteKey(i + "isranged");
        PlayerPrefs.DeleteKey(i + "onhitability");
        PlayerPrefs.DeleteKey(i + "modelname");
        PlayerPrefs.DeleteKey(i + "prefixname");
        PlayerPrefs.DeleteKey(i + "suffixname");

    }

    /// <summary>
    /// function to generate a random piece of equipment
    /// </summary>
    /// <returns>an equipment object</returns>
    public equipment randomEquipment()
    {

        equipment randEquipment = new equipment();
        equipment tempEquipment;

        //roll the dice to see if we get affixes
        int randint = UnityEngine.Random.Range(0, 100);

        if (randint >= 97)
        {
            int lootroll = UnityEngine.Random.Range(0, uniqueslist.Count);
            tempEquipment = (equipment)uniqueslist[lootroll];
        }
        else
        {
            int lootroll = UnityEngine.Random.Range(0, basesList.Count);
            tempEquipment = (equipment)basesList[lootroll];
        }
        randEquipment.equipmentName = tempEquipment.equipmentName;
        randEquipment.equipmentType = tempEquipment.equipmentType;
        randEquipment.validSlot = tempEquipment.validSlot;
        randEquipment.maxlvl = tempEquipment.maxlvl;
        randEquipment.minlvl = tempEquipment.minlvl;
        randEquipment.flavorText = tempEquipment.flavorText;
        randEquipment.tier = tempEquipment.tier;
        randEquipment.ranged = tempEquipment.ranged;
        randEquipment.twohand = tempEquipment.twohand;
        randEquipment.onhit = tempEquipment.onhit;
        randEquipment.modelname = tempEquipment.modelname;


        randEquipment.equipmentAttributes.Add(tempEquipment.equipmentAttributes);

        int tier = 0;
        if (randint > 60 && randint <= 85)
            tier = 1;
        if (randint > 85 && randint < 97)
            tier = 2;

        doaffixes(randEquipment, tier);
        
        return randEquipment;


    }

    /// <summary>
    /// function to generate a random equipment
    /// </summary>
    /// <param name="tier">the tier of the desired equipment</param>
    /// <returns>a random equipment of the desired tier</returns>
    public equipment randomEquipment(int tier)
    {

        equipment randEquipment = new equipment();
        equipment tempEquipment;
        

        //grab a random equipment out of the list appropriate for the tier
        if (tier == 3)
        {
            int randint = UnityEngine.Random.Range(0, uniqueslist.Count);
            tempEquipment = (equipment)uniqueslist[randint];
        }
        else
        {
            int randint = UnityEngine.Random.Range(0, basesList.Count);
            tempEquipment = (equipment)basesList[randint];
        }

        randEquipment.equipmentName = tempEquipment.equipmentName;
        randEquipment.equipmentType = tempEquipment.equipmentType;
        randEquipment.validSlot = tempEquipment.validSlot;
        randEquipment.maxlvl = tempEquipment.maxlvl;
        randEquipment.minlvl = tempEquipment.minlvl;
        randEquipment.flavorText = tempEquipment.flavorText;
        randEquipment.tier = tempEquipment.tier;
        randEquipment.ranged = tempEquipment.ranged;
        randEquipment.twohand = tempEquipment.twohand;
        randEquipment.onhit = tempEquipment.onhit;
        randEquipment.modelname = tempEquipment.modelname;

        randEquipment.equipmentAttributes.Add(tempEquipment.equipmentAttributes);

        doaffixes(randEquipment, tier);

        return randEquipment;


    }

    /// <summary>
    /// generate a random equipment
    /// </summary>
    /// <param name="level">the desired level</param>
    /// <returns>a random equipment of the desired level</returns>
    public equipment randomEquipmentByLevel(int level)
    {

        equipment randEquipment = new equipment();
        equipment tempEquipment;

        //roll the dice to see if we get affixes
        int randint = UnityEngine.Random.Range(0, 100);

        if (randint >= 97)
        {
            ArrayList templist = new ArrayList();
            foreach (equipment e in uniqueslist)
            {
                if (e.maxlvl >= level && e.minlvl <= level)
                {
                    templist.Add(e);
                }
            }
            tempEquipment = (equipment)templist[UnityEngine.Random.Range(0, templist.Count)];
        }
        else
        {
            ArrayList templist = new ArrayList();
            foreach (equipment e in basesList)
            {
                if (e.maxlvl >= level && e.minlvl <= level)
                {
                    templist.Add(e);
                }
            }
            tempEquipment = (equipment)templist[UnityEngine.Random.Range(0, templist.Count)];
        }
        randEquipment.equipmentName = tempEquipment.equipmentName;
        randEquipment.equipmentType = tempEquipment.equipmentType;
        randEquipment.validSlot = tempEquipment.validSlot;
        randEquipment.maxlvl = tempEquipment.maxlvl;
        randEquipment.minlvl = tempEquipment.minlvl;
        randEquipment.flavorText = tempEquipment.flavorText;
        randEquipment.tier = tempEquipment.tier;
        randEquipment.ranged = tempEquipment.ranged;
        randEquipment.twohand = tempEquipment.twohand;
        randEquipment.onhit = tempEquipment.onhit;
        randEquipment.modelname = tempEquipment.modelname;


        randEquipment.equipmentAttributes.Add(tempEquipment.equipmentAttributes);

        int tier = 0;
        if (randint > 60 && randint <= 85)
            tier = 1;
        if (randint > 85 && randint < 97)
            tier = 2;

        doaffixes(randEquipment, tier);

        return randEquipment;


    }

    /// <summary>
    /// get a unique equipment from the list
    /// </summary>
    /// <param name="name">the name of the equipment</param>
    /// <returns>the unique equipment</returns>
    public equipment getUnique(string name)
    {

        equipment randEquipment = new equipment();
        equipment tempEquipment;


        
        ArrayList templist = new ArrayList();
        foreach (equipment e in uniqueslist)
        {
            if (e.equipmentName == name)
            {
                templist.Add(e);
            }
        }
        tempEquipment = (equipment)templist[UnityEngine.Random.Range(0, templist.Count)];

        randEquipment.equipmentName = tempEquipment.equipmentName;
        randEquipment.equipmentType = tempEquipment.equipmentType;
        randEquipment.validSlot = tempEquipment.validSlot;
        randEquipment.maxlvl = tempEquipment.maxlvl;
        randEquipment.minlvl = tempEquipment.minlvl;
        randEquipment.flavorText = tempEquipment.flavorText;
        randEquipment.tier = tempEquipment.tier;
        randEquipment.ranged = tempEquipment.ranged;
        randEquipment.twohand = tempEquipment.twohand;
        randEquipment.onhit = tempEquipment.onhit;
        randEquipment.ranged = tempEquipment.ranged;
        randEquipment.twohand = tempEquipment.twohand;
        randEquipment.onhit = tempEquipment.onhit;
        randEquipment.modelname = tempEquipment.modelname;

        randEquipment.equipmentAttributes.Add(tempEquipment.equipmentAttributes);

        return randEquipment;


    }


 /// <summary>
 /// function to generate a random equipment
 /// </summary>
 /// <param name="tier">the tier of the desired equipment</param>
 /// <param name="slot">the slot of the desired equipment</param>
 /// <returns></returns>
    public equipment randomEquipment(int tier, equipSlots.slots slot)
    {

        equipment randEquipment = new equipment();
        equipment tempEquipment;


        //grab a random equipment out of the list appropriate for the tier
        if (tier >= 3)
        {

            ArrayList templist = new ArrayList();
            foreach (equipment e in uniqueslist)
            {
                if (e.validSlot == slot)
                {
                    templist.Add(e);
                }
            }
            tempEquipment = (equipment)templist[UnityEngine.Random.Range(0, templist.Count)];


        }
        else
        {
            ArrayList templist = new ArrayList();
            foreach (equipment e in basesList)
            {
                if (e.validSlot == slot)
                {
                    templist.Add(e);
                }
            }
            tempEquipment = (equipment)templist[UnityEngine.Random.Range(0, templist.Count)];

        }

        randEquipment.equipmentName = tempEquipment.equipmentName;
        randEquipment.equipmentType = tempEquipment.equipmentType;
        randEquipment.validSlot = tempEquipment.validSlot;
        randEquipment.maxlvl = tempEquipment.maxlvl;
        randEquipment.minlvl = tempEquipment.minlvl;
        randEquipment.flavorText = tempEquipment.flavorText;
        randEquipment.tier = tempEquipment.tier;
        randEquipment.ranged = tempEquipment.ranged;
        randEquipment.twohand = tempEquipment.twohand;
        randEquipment.onhit = tempEquipment.onhit;
        randEquipment.modelname = tempEquipment.modelname;

        randEquipment.equipmentAttributes.Add(tempEquipment.equipmentAttributes);

        doaffixes(randEquipment, tier);

        return randEquipment;


    }

    /// <summary>
    /// function to generate a random equipment
    /// </summary>
    /// <param name="tier">the tier of the desired equipment</param>
    /// <param name="level">the level of the desired equipment</param>
    /// <param name="slot">the slot of the desired equipment</param>
    /// <returns></returns>
    public equipment randomEquipment(int tier, int level, equipSlots.slots slot)
    {

        equipment randEquipment = new equipment();
        equipment tempEquipment;
        
        
        //NO! BAD USER! LEVEL CAP IS A THING!
        if (level > 20)
        {
            level = 20;
        }

        //grab a random equipment out of the list appropriate for the tier
        if (tier >= 3)
        {
            
            ArrayList templist = new ArrayList();
            foreach(equipment e in uniqueslist)
            {
                if (e.validSlot == slot && e.minlvl <= level && e.maxlvl >= level)
                {
                    templist.Add(e);
                }
            }
            tempEquipment = (equipment)templist[UnityEngine.Random.Range(0, templist.Count)];


        }
        else
        {
            ArrayList templist = new ArrayList();
            foreach(equipment e in basesList)
            {
                if (e.validSlot == slot && e.minlvl <= level && e.maxlvl >= level)
                {

                    templist.Add(e);
                }
            }
            tempEquipment = (equipment)templist[UnityEngine.Random.Range(0, templist.Count-1)];

        }
        

        randEquipment.equipmentName = tempEquipment.equipmentName;
        randEquipment.equipmentType = tempEquipment.equipmentType;
        randEquipment.validSlot = tempEquipment.validSlot;
        randEquipment.maxlvl = tempEquipment.maxlvl;
        randEquipment.minlvl = tempEquipment.minlvl;
        randEquipment.flavorText = tempEquipment.flavorText;
        randEquipment.tier = tempEquipment.tier;
        randEquipment.ranged = tempEquipment.ranged;
        randEquipment.twohand = tempEquipment.twohand;
        randEquipment.onhit = tempEquipment.onhit;
        randEquipment.modelname = tempEquipment.modelname;

        randEquipment.equipmentAttributes.Add(tempEquipment.equipmentAttributes);

        doaffixes(randEquipment, tier);


        return randEquipment;


    }

    private void doaffixes(equipment randEquipment, int tier)
    {
        //one affix
        if (tier == 1)
        {
            affix tempaffix;

            tempaffix = getrandaffix(randEquipment.validSlot);

            //make the name correctly
            if (tempaffix.affixType == equipSlots.affixtype.Suffix)
            {
                randEquipment.equipmentName = randEquipment.equipmentName + " " + tempaffix.affixName;
                randEquipment.suffixname = tempaffix.affixName;
            }
            else if (tempaffix.affixType == equipSlots.affixtype.Prefix)
            {
                randEquipment.equipmentName = tempaffix.affixName + " " + randEquipment.equipmentName;
                randEquipment.prefixname = tempaffix.affixName;
            }

            //add its attributes to the equipment
            randEquipment.equipmentAttributes.Add(tempaffix.affixAttributes);

        }
        else if (tier == 2)
        {
            //two affixes
            //rollin dem bones

            affix tempaffix;

            tempaffix = getrandaffix(randEquipment.validSlot, equipSlots.affixtype.Prefix);

            //add it to the equipment
            randEquipment.equipmentName = tempaffix.affixName + " " + randEquipment.equipmentName;
            randEquipment.equipmentAttributes.Add(tempaffix.affixAttributes);
            randEquipment.prefixname = tempaffix.affixName;

            //roll for a suffix
            tempaffix = getrandaffix(randEquipment.validSlot, equipSlots.affixtype.Suffix);

            //add it on
            randEquipment.equipmentName = randEquipment.equipmentName + " " + tempaffix.affixName;
            randEquipment.equipmentAttributes.Add(tempaffix.affixAttributes);
            randEquipment.suffixname = tempaffix.affixName;

        }
    }

    private affix getrandaffix(equipSlots.slots slot)
    {
            ArrayList templist = new ArrayList();
            foreach (affix e in affixeslist)
            {
                if (e.affixSlots.Contains(slot) == true)
                {
                    templist.Add(e);
                }
            }
            return (affix)templist[UnityEngine.Random.Range(0, templist.Count)];

    }

    private affix getrandaffix(equipSlots.slots slot, equipSlots.affixtype atype)
    {
        ArrayList templist = new ArrayList();
        foreach (affix e in affixeslist)
        {
            if (e.affixSlots.Contains(slot) == true && e.affixType == atype)
            {
                templist.Add(e);
            }
        }
        return (affix)templist[UnityEngine.Random.Range(0, templist.Count)];

    }

}
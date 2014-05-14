using UnityEngine;
using System;
using System.Collections;
using System.Xml;

/// <summary>
/// equipment factory! use this to generate equipment!
/// </summary>
public class equipmentFactory {


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

    }

    /// <summary>
    /// loads the affixes from xml
    /// </summary>
    public void loadAffixes()
    {

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

    }

    /// <summary>
    /// builds a default equipment
    /// </summary>
    /// <returns>an equipment object</returns>
    public equipment buildEquipment() {

        equipment tempEquipment = new equipment();

        

        return tempEquipment;


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
        if (level < 20)
        {
            level = 20;
        }

        //grab a random equipment out of the list appropriate for the tier
        if (tier <= 3)
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
            tempEquipment = (equipment)templist[UnityEngine.Random.Range(0, templist.Count)];

        }
        

        randEquipment.equipmentName = tempEquipment.equipmentName;
        randEquipment.equipmentType = tempEquipment.equipmentType;
        randEquipment.validSlot = tempEquipment.validSlot;
        randEquipment.maxlvl = tempEquipment.maxlvl;
        randEquipment.minlvl = tempEquipment.minlvl;
        randEquipment.flavorText = tempEquipment.flavorText;
        randEquipment.tier = tempEquipment.tier;

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
            if (tempaffix.affixType == "suffix")
            {
                randEquipment.equipmentName = randEquipment.equipmentName + " " + tempaffix.affixName;
            }
            else if (tempaffix.affixType == "prefix")
            {
                randEquipment.equipmentName = tempaffix.affixName + " " + randEquipment.equipmentName;
            }

            //add its attributes to the equipment
            randEquipment.equipmentAttributes.Add(tempaffix.affixAttributes);

        }
        else if (tier == 2)
        {
            //two affixes
            //rollin dem bones

            affix tempaffix;

            tempaffix = getrandaffix(randEquipment.validSlot, "prefix");

            //add it to the equipment
            randEquipment.equipmentName = tempaffix.affixName + " " + randEquipment.equipmentName;
            randEquipment.equipmentAttributes.Add(tempaffix.affixAttributes);

            //roll for a suffix
            tempaffix = getrandaffix(randEquipment.validSlot, "suffix");

            //add it on
            randEquipment.equipmentName = randEquipment.equipmentName + " " + tempaffix.affixName;
            randEquipment.equipmentAttributes.Add(tempaffix.affixAttributes);

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

    private affix getrandaffix(equipSlots.slots slot, string affixtype)
    {
        ArrayList templist = new ArrayList();
        foreach (affix e in affixeslist)
        {
            if (e.affixSlots.Contains(slot) == true && e.affixType == affixtype)
            {
                templist.Add(e);
            }
        }
        return (affix)templist[UnityEngine.Random.Range(0, templist.Count)];

    }
}
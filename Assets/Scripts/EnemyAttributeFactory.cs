using UnityEngine;
using System.Collections;

public class EnemyAttributeFactory {

    void Awake()
    {
        #region critter init
        critterbase = new Attributes();
        critterbase.Health = 10f;
        critterbase.Resource = 50f;
        critterbase.Power = 0f;
        critterbase.Defense = 0f;
        critterbase.MinDamage = 1;
        critterbase.MaxDamage = 3;
        critterbase.AttackSpeed = 1;
        critterbase.MovementSpeed = 1;
        #endregion

        #region small init
        smallbase = new Attributes();
        smallbase.Health = 30f;
        smallbase.Resource = 70f;
        smallbase.Power = 10f;
        smallbase.Defense = 10f;
        smallbase.MinDamage = 1;
        smallbase.MaxDamage = 5;
        smallbase.AttackSpeed = 1;
        smallbase.MovementSpeed = 1;
        #endregion

        #region med init
        medbase = new Attributes();
        medbase.Health = 60f;
        medbase.Resource = 120f;
        medbase.Power = 10f;
        medbase.Defense = 15f;
        medbase.MinDamage = 1;
        medbase.MaxDamage = 7;
        medbase.AttackSpeed = 1;
        medbase.MovementSpeed = 1;
        #endregion

        #region large init
        largebase = new Attributes();
        largebase.Health = 120f;
        largebase.Resource = 140f;
        largebase.Power = 15f;
        largebase.Defense = 20f;
        largebase.MinDamage = 1;
        largebase.MaxDamage = 8;
        largebase.AttackSpeed = 1;
        largebase.MovementSpeed = 1;
        #endregion
    }

    #region critter stats
    public Attributes critterbase;
    public float critterscalingHP = 2.5f;
    public float critterscalingresource = 5f;
    public float critterscalingpower = 2f;
    public float critterscalingdefense = 2f;
    public float critterscalingmindmg = .5f;
    public float critterscalingmaxdmg = .5f;
    #endregion

    #region small stats
    public Attributes smallbase;
    public float smallscalingHP = 5f;
    public float smallscalingresource = 10f;
    public float smallscalingpower = 4f;
    public float smallscalingdefense = 4f;
    public float smallscalingmindmg = .75f;
    public float smallscalingmaxdmg = .75f;
    #endregion


    #region med stats
    public Attributes medbase;
    public float medscalingHP = 10f;
    public float medscalingresource = 10f;
    public float medscalingpower = 4f;
    public float medscalingdefense = 6f;
    public float medscalingmindmg = .8f;
    public float medscalingmaxdmg = .8f;
    #endregion


    #region large stats
    public Attributes largebase;
    public float largescalingHP = 20f;
    public float largescalingresource = 10f;
    public float largescalingpower = 4f;
    public float largescalingdefense = 6f;
    public float largescalingmindmg = .8f;
    public float largescalingmaxdmg = .8f;
    #endregion

    public Attributes MakeEnemyAttributes(int level, string type)
    {
        Attributes tempatts = new Attributes();

        if (type == "critter")
        {
            tempatts.Health = critterbase.Health + critterscalingHP * (level - 1);
            tempatts.Resource = critterbase.Resource + critterscalingresource * (level - 1);
            tempatts.Power = critterbase.Power + critterscalingpower * (level - 1);
            tempatts.Defense = critterbase.Defense + critterscalingdefense * (level - 1);
            tempatts.MinDamage = critterbase.MinDamage + critterscalingmindmg * (level - 1);
            tempatts.MaxDamage = critterbase.MaxDamage + critterscalingmaxdmg * (level - 1);
            tempatts.MovementSpeed = critterbase.MovementSpeed;
            tempatts.AttackSpeed = critterbase.AttackSpeed;

        }
        else if (type == "small")
        {
            tempatts.Health = smallbase.Health + smallscalingHP * (level - 1);
            tempatts.Resource = smallbase.Resource + smallscalingresource * (level - 1);
            tempatts.Power = smallbase.Power + smallscalingpower * (level - 1);
            tempatts.Defense = smallbase.Defense + smallscalingdefense * (level - 1);
            tempatts.MinDamage = smallbase.MinDamage + smallscalingmindmg * (level - 1);
            tempatts.MaxDamage = smallbase.MaxDamage + smallscalingmaxdmg * (level - 1);
            tempatts.MovementSpeed = smallbase.MovementSpeed;
            tempatts.AttackSpeed = smallbase.AttackSpeed;

        }
        else if (type == "med")
        {
            tempatts.Health = medbase.Health + medscalingHP * (level - 1);
            tempatts.Resource = medbase.Resource + medscalingresource * (level - 1);
            tempatts.Power = medbase.Power + medscalingpower * (level - 1);
            tempatts.Defense = medbase.Defense + medscalingdefense * (level - 1);
            tempatts.MinDamage = medbase.MinDamage + medscalingmindmg * (level - 1);
            tempatts.MaxDamage = medbase.MaxDamage + medscalingmaxdmg * (level - 1);
            tempatts.MovementSpeed = medbase.MovementSpeed;
            tempatts.AttackSpeed = medbase.AttackSpeed;

        }
        else if (type == "large")
        {
            tempatts.Health = largebase.Health + largescalingHP * (level - 1);
            tempatts.Resource = largebase.Resource + largescalingresource * (level - 1);
            tempatts.Power = largebase.Power + largescalingpower * (level - 1);
            tempatts.Defense = largebase.Defense + largescalingdefense * (level - 1);
            tempatts.MinDamage = largebase.MinDamage + largescalingmindmg * (level - 1);
            tempatts.MaxDamage = largebase.MaxDamage + largescalingmaxdmg * (level - 1);
            tempatts.MovementSpeed = largebase.MovementSpeed;
            tempatts.AttackSpeed = largebase.AttackSpeed;

        }


        return tempatts;

    }

    public void GiveEnemyAbilities(Entity enemy, string type)
    {
        /*
        abilityManager.AddAbility(GameManager.Abilities["shadowbolt"], 2);
        abilityManager.AddAbility(GameManager.Abilities["poisonbolt"], 3);
        abilityManager.AddAbility(GameManager.Abilities["ShockMine"], 4);
        abilityManager.AddAbility(GameManager.Abilities["bladewaltz"], 5);

        abilityIndexDict["shadowbolt"] = 2;
        abilityIndexDict["poisonbolt"] = 3;
        abilityIndexDict["ShockMine"] = 4;
        abilityIndexDict["bladewaltz"] = 5;
         * */
        if (type == "critter")
        {
            enemy.abilityManager.abilities[0]=GameManager.Abilities["cleave"];
            enemy.abilityIndexDict["cleave"] = 0;
        }
        else if (type == "small")
        {
            enemy.abilityManager.abilities[0] = GameManager.Abilities["cleave"];
            enemy.abilityIndexDict["cleave"] = 0;
        }
        else if (type == "med")
        {
            enemy.abilityManager.abilities[0] = GameManager.Abilities["cleave"];
            enemy.abilityIndexDict["cleave"] = 0;
        }
        else if (type == "large")
        {
            enemy.abilityManager.abilities[0] = GameManager.Abilities["cleave"];
            enemy.abilityIndexDict["cleave"] = 0;
        }



    }




}

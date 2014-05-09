using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Talent
{

    private int currentPoints; //Current points spent on the talent

    public int CurrentPoints
    {
        get { return currentPoints; }

        set { currentPoints = value; }
    }

    private int maxPoints; //Max number of points the talent will have.
    public int MaxPoints
    {
        get { return maxPoints; }
    }

    private int depth; //Depth in the tree the talent lies. Starts at 0.
    public int Depth
    {
        get { return depth; }
    }

    private Ability talentAbility; //The ability spending points on this talent will unlock.
    public Ability TalentAbility
    {
        get { return talentAbility; }    
    }

    private string name; //Name of the talent. Usually the same as the ability name.
    public string Name
    {
        get { return name; }
    }

    private string bonus;
    public string Bonus
    {
        get { return bonus; }
    }

    private float bonusMagnitude;
    public float BonusMagnitude
    {
        get { return bonusMagnitude; }
    }


    /// <summary>
    /// Talent Constructor
    /// </summary>
    /// <param name="name">Name of the talent.</param>
    /// <param name="maxPoints">Max number of points the talent will have.</param>
    /// <param name="ability">The ability spending points on this talent will unlock.</param>
    /// <param name="depth">Depth the talent is on the tree.</param>
    public Talent(string name, int maxPoints, Ability ability, int depth)
    {
        currentPoints = 0;
        this.maxPoints = maxPoints;
        this.talentAbility = ability;
        this.depth = depth;
        this.name = name;
        this.bonus = "";
        this.bonusMagnitude = 0;
    }

    public Talent(string name, int maxPoints, string bonus, float bonusMagnitude, int depth)
    {
        currentPoints = 0;
        this.maxPoints = maxPoints;
        this.bonus = bonus;
        this.depth = depth;
        this.name = name;
        this.talentAbility = null;
        this.bonusMagnitude = bonusMagnitude;
    }

    public string ReadableBonus()
    {
        float bonusRead = bonusMagnitude  * 100f;

        

        
        if (bonus == "attackDamage")
        {
            return "Adds " + bonusRead.ToString() + "% Attack Damage"; 
        }
        else if (bonus == "attackSpeed")
        {
            return "Adds " + bonusRead.ToString() + "% Attack Speed";
        }
        else if (bonus == "fire")
        {
            return "Adds " + bonusRead.ToString() + "% Fire Damage";
        }
        else if (bonus == "ice")
        {
            return "Adds " + bonusRead.ToString() + "% Ice Damage";
        }
        else if (bonus == "shadow")
        {
            return "Adds " + bonusRead.ToString() + "% Shadow Damage";
        }
        else if (bonus == "onHitDamage")
        {
            return "Adds " + bonusRead.ToString() + "% On Hit Damage";
        }
        else if (bonus == "defense")
        {
            return "Adds " + bonusRead.ToString() + "% Defense";
        }
        else
        {
            return "";
        }
    }

    
}

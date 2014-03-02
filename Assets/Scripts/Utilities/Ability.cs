using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Attack type will determine the attack algorithm used.
/// </summary>
public enum AttackType
{
    PROJECTILE,
    AOE,
    PBAOE,      // Point Blank Area of Effect
    MELEE,
    STATUS      // Maybe an attack type that just inflicts conditions?
};

/// <summary>
/// Damage type will indicate which type of damage to mitigate.
/// This might not be used - it's just being included.
/// </summary>
public enum DamageType
{
    PHYSICAL,
    FIRE,
    WATER,
    EARTH,
    AIR
};

public abstract class Ability {

    

    #region Properties

    private AttackType attackType;
    public AttackType Attack_Type
    {
        get { return attackType; }
    }

    private DamageType damageType;
    public DamageType Damage_Type
    {
        get { return damageType; }
    }

    private float range;
    public float Range
    {
        get { return range; }
    }
    
    private float radius;
    public float Radius
    {
        get { return radius; }
    }
    
    private float cooldown;
    public float Cooldown
    {
        get { return cooldown; }
    }

    private string readable;
    public string Name
    {
        get { return readable; }
    }


    public abstract List<GameObject> OnAttack();

    public abstract void DoDamage(GameObject attacker, GameObject defender);
    
    #endregion

    // TODO: Implement this idea outside of the ability class inside of manager
    //public static Dictionary<string, Ability> abilities;

    // Constructor is private to prevent abilities from being created on the fly.
    private Ability(AttackType attackType, DamageType damageType, float range, float radius, float cooldown, string id, string readable)
    {
        

        this.attackType = attackType;
        this.damageType = damageType;
        this.range = range;
        this.radius = radius;
        this.cooldown = cooldown;
        this.readable = readable;

        // See previous TODO
        //abilities.Add(id, this);
    }

    
}

// The following is a list of example abilities.
    //Ability fireball = new Ability(AttackType.PROJECTILE, DamageType.FIRE, 5.0f, 10.0f, 3.0f, "fireball", "Fireball");
    //Ability cleave = new Ability(AttackType.MELEE, DamageType.PHYSICAL, 1.0f, 45.0f, 5.0f, "cleave", "Cleave");
    //Ability shout = new Ability(AttackType.PBAOE, DamageType.AIR, 20.0f, 360.0f, 10.0f, "fusrodah", "Fus Roh Dah");

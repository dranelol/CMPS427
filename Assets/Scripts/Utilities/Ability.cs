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

public class Ability 
{

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

    /// <summary>
    /// Base modifier from which damage is calculated
    /// </summary>
    private float damageMod;
    public float DamageMod
    {
        get { return damageMod; }
    }
    
    #endregion

    public Ability(AttackType attackType, DamageType damageType, float range, float radius, float cooldown, float damageMod, string id, string readable)
    {
        this.attackType = attackType;
        this.damageType = damageType;
        this.range = range;
        this.radius = radius;
        this.cooldown = cooldown;
        this.readable = readable;
        this.damageMod = damageMod;
    }

    /// <summary>
    /// Figure out who will be affected by this attack
    /// </summary>
    /// <param name="attacker"></param>
    /// <returns>Returns a list of gameobjects this attack will affect</returns>
    public List<GameObject> OnAttack(Transform attacker)
    {
        return Attack.OnAttack(attacker, radius, range);
    }

    /// <summary>
    /// Do damage with this attack
    /// </summary>
    /// <param name="attacker">The gameobject carrying out the attack</param>
    /// <param name="defender">The gameobject defending against the attack</param>
    public void DoDamage(GameObject attacker, GameObject defender)
    {
        Attack.DoDamage(attacker, defender);
    }



    
}

// The following is a list of example abilities.
    //Ability fireball = new Ability(AttackType.PROJECTILE, DamageType.FIRE, 5.0f, 10.0f, 3.0f, "fireball", "Fireball");
    //Ability cleave = new Ability(AttackType.MELEE, DamageType.PHYSICAL, 1.0f, 45.0f, 5.0f, "cleave", "Cleave");
    //Ability shout = new Ability(AttackType.PBAOE, DamageType.AIR, 20.0f, 360.0f, 10.0f, "fusrodah", "Fus Roh Dah");

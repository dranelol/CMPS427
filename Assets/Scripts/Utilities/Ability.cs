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

public abstract class Ability : MonoBehaviour
{

    #region Properties

    protected AttackType attackType;
    public AttackType Attack_Type
    {
        get { return attackType; }
    }

    protected DamageType damageType;
    public DamageType Damage_Type
    {
        get { return damageType; }
    }

    protected float range;
    public float Range
    {
        get { return range; }
    }
    
    protected float angle;
    public float Angle
    {
        get { return angle; }
    }
    
    protected float cooldown;
    public float Cooldown
    {
        get { return cooldown; }
    }

    protected string readable;
    public string Name
    {
        get { return readable; }
    }

    /// <summary>
    /// Base modifier from which damage is calculated
    /// </summary>
    protected float damageMod;
    public float DamageMod
    {
        get { return damageMod; }
    }
    
    #endregion

    public Ability(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, string id, string readable)
    {
        this.attackType = attackType;
        this.damageType = damageType;
        this.range = range;
        this.angle = angle;
        this.cooldown = cooldown;
        this.readable = readable;
        this.damageMod = damageMod;
    }

    /// <summary>
    /// Handler for this attack; figures out who will be attacked, and carries out everything needed for the attack to occur
    /// </summary>
    /// <param name="attacker">The gameobject carrying out the attack</param>
    /// <param name="defender">The gameobject defending against the attack</param>
    public virtual void AttackHandler(GameObject attacker);

    /// <summary>
    /// Figure out who will be affected by this attack
    /// </summary>
    /// <param name="attacker">The gameobject carrying out the attack</param>
    /// <returns>Returns a list of gameobjects this attack will affect</returns>
    public virtual List<GameObject> OnAttack(Transform attacker);

    /// <summary>
    /// Do damage with this attack
    /// </summary>
    /// <param name="attacker">The gameobject carrying out the attack</param>
    /// <param name="defender">The gameobject defending against the attack</param>
    public virtual void DoDamage(GameObject attacker, GameObject defender);

    /// <summary>
    /// Certain attacks have a physics component to them; this resolves those effects
    /// </summary>
    /// <param name="attacker">Gameobject doing the attacking</param>
    /// <param name="defender">Gameobject affected by the attack</param>
    public virtual void DoPhysics(GameObject attacker, GameObject defender);



    
}

// The following is a list of example abilities.
    //Ability fireball = new Ability(AttackType.PROJECTILE, DamageType.FIRE, 5.0f, 10.0f, 3.0f, "fireball", "Fireball");
    //Ability cleave = new Ability(AttackType.MELEE, DamageType.PHYSICAL, 1.0f, 45.0f, 5.0f, "cleave", "Cleave");
    //Ability shout = new Ability(AttackType.PBAOE, DamageType.AIR, 20.0f, 360.0f, 10.0f, "fusrodah", "Fus Roh Dah");

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


public abstract class Ability
{


    #region Properties

    protected AttackType attackType;
    public AttackType AttackType
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

    protected GameObject particleSystem;
    public GameObject ParticleSystem
    {
        get
        {
            return particleSystem;
        }
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

    public Ability(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, string id, string readable, GameObject particles)
    {
        this.attackType = attackType;
        this.damageType = damageType;
        this.range = range;
        this.angle = angle;
        this.cooldown = cooldown;
        this.readable = readable;
        this.damageMod = damageMod;
        this.particleSystem = particles;
    }

    public virtual void AttackHandler(GameObject source, Entity attacker, bool isPlayer)
    {

    }

    public virtual void AttackHandler(GameObject source, Entity attacker, Entity defender, bool isPlayer)
    {

    }

    public virtual List<GameObject> OnAttack(GameObject source, bool isPlayer)
    {
        List<GameObject> attacked = new List<GameObject>();

        return attacked;
    }

    public virtual void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {
    }

    public virtual void DoPhysics(GameObject source, GameObject target)
    {
    }


    public virtual IEnumerator DoAnimation(GameObject attacker, GameObject particlePrefab, float time, bool isPlayer, GameObject defender = null)
    {
        yield return null;
    }
    
}

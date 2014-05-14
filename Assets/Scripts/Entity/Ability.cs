using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Attack type will determine the attack algorithm used.
/// </summary>
public enum AttackType
{
    PROJECTILE,
    HONINGPROJECTILE,
    GROUNDTARGET, 
    SINGLETARGET, // needs a selected target
    PBAOE,      // Point Blank Area of Effect
    MELEE,
    STATUS     
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
    AIR,
    SHADOW,
    POISON,
    NONE
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

    protected float resourceCost;
    public float ResourceCost
    {
        get { return resourceCost; }
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

    protected GameManager gameManager;


    protected string id;
    public string ID
    {
        get
        {
            return id;
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

    public Ability(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
    {
        this.attackType = attackType;
        this.damageType = damageType;
        this.range = range;
        this.angle = angle;
        this.cooldown = cooldown;
        this.readable = readable;
        this.damageMod = damageMod;
        this.particleSystem = particles;
        this.id = id;
        this.resourceCost = resourceCost;
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public virtual void AttackHandler(GameObject source, Entity attacker, bool isPlayer)
    {

    }

    //onhit
    public virtual void AttackHandler(GameObject attacker, GameObject defender, bool isPlayer)
    {
        
    }


    // Projectiles
    public virtual void AttackHandler(GameObject source, GameObject target, Entity attacker, bool isPlayer)
    {
        
    }

    // groundtargets
    public virtual void AttackHandler(GameObject source, Vector3 AoEPoint, Entity attacker, bool isPlayer)
    {

    }


    /// <summary>
    /// Spawns a projectile at the position of source, owned by owner, rotated by rotation, tied to abilityID
    /// </summary>
    /// <param name="source">Source of the projectile</param>
    /// <param name="owner">Owner of the projectile</param>
    /// <param name="forward">Direction forward of the projectile</param>
    /// <param name="abilityID">The game abilityID of the ability tied to this projectile's onhit</param>
    /// <param name="isPlayer">Whether or not the owner is a player</param>
    public virtual void SpawnProjectile(GameObject source, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)

    {

    }

    /// <summary>
    /// Spawns a projectile with a known target
    /// </summary>
    /// <param name="source">Source of the projectile</param>
    /// <param name="target">Target of the projectile</param>
    /// <param name="owner">Owner of the projectile</param>
    /// <param name="forward">Direction forward of the projectile</param>
    /// <param name="abilityID">The game abilityID of the ability tied to this projectile's onhit</param>
    /// <param name="isPlayer">Whether or not the owner is a player</param>
    public virtual void SpawnProjectile(GameObject source, Vector3 target, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
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

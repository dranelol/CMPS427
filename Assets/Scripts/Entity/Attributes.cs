using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Container for entity attributes. Contains accessor methods for each attribute.
/// If an entity needs more attributes than what this class provides, this class should be extended to include
/// the extra attributes.
/// </summary>
public class Attributes
{
    public enum Stats
    {
        HEALTH,
        RESOURCE,
        POWER,
        DEFENSE,
        ATTACK_SPEED,
        MOVEMENT_SPEED,
        MIN_DAMAGE,
        MAX_DAMAGE
    };

    private Dictionary<Stats, float> statList = new Dictionary<Stats,float>();

    /// <summary>
    /// Adds the required keys to the dictionary.
    /// </summary>
    public Attributes()
    {
        foreach (Stats stat in Enum.GetValues(typeof(Stats)))
            statList.Add(stat, 0);
    }

    #region Health
    public float Health
    {
        get
        { return statList[Stats.HEALTH]; }
        set { statList[Stats.HEALTH] = value; }
    }
    #endregion

    #region Resource
    public float Resource
    {
        get { return statList[Stats.RESOURCE]; }
        set { statList[Stats.RESOURCE] = value; }
    }
    #endregion

    #region Power
    public float Power
    {
        get { return statList[Stats.POWER]; }
        set { statList[Stats.POWER] = value; }
    }
    #endregion

    #region Defense
    public float Defense
    {
        get { return statList[Stats.DEFENSE]; }
        set { statList[Stats.DEFENSE] = value; }
    }
    #endregion

    #region Attack Speed
    public float AttackSpeed
    {
        get { return statList[Stats.ATTACK_SPEED]; }
        set { statList[Stats.ATTACK_SPEED] = value; }
    }
    #endregion

    #region Movement Speed
    public float MovementSpeed
    {
        get { return statList[Stats.MOVEMENT_SPEED]; }
        set { statList[Stats.MOVEMENT_SPEED] = value; }
    }

    #endregion


    #region Min Damage
    public float MinDamage
    {
        get { return statList[Stats.MIN_DAMAGE]; }
        set { statList[Stats.MIN_DAMAGE] = value; }
    }

    #endregion


    #region Max Damage
    public float MaxDamage
    {
        get { return statList[Stats.MAX_DAMAGE]; }
        set { statList[Stats.MAX_DAMAGE] = value; }
    }

    #endregion



    #region Arithmetic Operations
    /// <summary>
    /// Adds attributes to the current attribute object.
    /// </summary>
    /// <param name="other">Object whose attributes will be added to the caller.</param>
    public void Add(Attributes other)
    {
        Health += other.Health;
        Resource += other.Resource;
        Power += other.Power;
        Defense += other.Defense;
        AttackSpeed += other.AttackSpeed;
        MovementSpeed += other.MovementSpeed;
        MinDamage += other.MinDamage;
        MaxDamage += other.MaxDamage;
    }

    /// <summary>
    /// Subtracts attributes to the current attribute object.
    /// </summary>
    /// <param name="other">Object whose attributes will be subtracted from the caller.</param>
    public void Subtract(Attributes other)
    {
        Health -= other.Health;
        Resource -= other.Resource;
        Power -= other.Power;
        Defense -= other.Defense;
        AttackSpeed -= other.AttackSpeed;
        MovementSpeed -= other.MovementSpeed;
        MinDamage -= other.MinDamage;
        MaxDamage -= other.MaxDamage;
    }
    #endregion
}

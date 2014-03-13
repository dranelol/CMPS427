using UnityEngine;
using System.Collections;

public class DoT : AuraModule
{
    #region Properties

    private ModificationType _modType;
    public ModificationType ModType
    {
        get { return _modType; }
    }

    private float _magnitude;
    public float Magnitude
    {
        get { return _magnitude; }
    }

    private DamageType _damageType;
    public DamageType TypeOfDamage
    {
        get { return _damageType; }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Define a DoT object that deals the given magnitude of the given damage type to a target 
    /// every second for the duration of the effect. Only one DoT can be defined per aura.
    /// </summary>
    /// <param name="damageType">The type of damage to deal.</param>
    /// <param name="magnitude">The amount of damage to deal. Must be greater than 0.</param>
    public DoT(DamageType damageType, float magnitude) : base()
    {
        _modType = ModificationType.Value;
        _damageType = damageType;

        if (magnitude > 0)
        {
            _magnitude = magnitude;
        }

        else
        {
            Debug.LogError("You cannot create a DoT obect that deals 0 or negative damage. Use a HoT obect if the target should be healed over time.");
            _magnitude = 0;
        }
    }

    /// <summary>
    /// Define a DoT object that deals a percentage of the target's health every second
    /// for the duration of the effect. Only one DoT can be defined per aura.
    /// </summary>
    /// <param name="magnitude">The percentage of health to remove. (0.03 = 3%.)</param>
    public DoT(float magnitude) : base()
    {
        _modType = ModificationType.Percentage;
        _damageType = DamageType.PHYSICAL;

        if (magnitude > 0)
        {
            _magnitude = magnitude;
        }

        else
        {
            Debug.LogError("You cannot create a DoT obect that deals 0 or negative damage. Use a HoT obect if the target should be healed over time.");
            _magnitude = 0;
        }
    }

    #endregion
}

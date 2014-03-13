using UnityEngine;
using System.Collections;

public class HoT : AuraModule
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

    #endregion

    #region Constructors

    /// <summary>
    /// Define a HoT object that heals the target for an amount every second. If ModType is a Percentage, the target
    /// will be healed for the given percentage of its health every second for the duration of the effect. If ModType is Value,
    /// the target will be healed by some function of the given magnitude and the caster's ability to heal.
    /// </summary>
    /// <param name="modType">The type of healing to be done.</param>
    /// <param name="magnitude">The percentage or magnitude of healing to be done every second.</param>
    public HoT(ModificationType modType, float magnitude) : base()
    {
        _modType = modType;
        _magnitude = magnitude;

        if (magnitude > 0)
        {
            _magnitude = magnitude;
        }

        else
        {
            Debug.LogError("You cannot create a HoT object that heals for 0 or negative damage. Use a DoT obect if the target should be lose health over time.");
            _magnitude = 0;
        }
    }

    #endregion
}
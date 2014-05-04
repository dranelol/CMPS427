using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MasterAuraManager : MonoBehaviour 
{
    public static Dictionary<string, Aura> Auras;

	void Awake() 
    {
        Auras = new Dictionary<string, Aura>();

        Auras["test"] = new AuraTemplate("test");
        Auras["heal"] = new Heal("heal");
        Auras["Corruption"] = new Corruption("Corruption");
        Auras["BlessingOfMight"] = new BlessingOfMight("Blessing of Might");

        Auras["root"] = new Root("root");
        Auras["chaosbarrage"] = new chaosbarrage("chaosbarrage");
        Auras["fireballbarrage"] = new fireballbarrage("fireballbarrage");


	}

    #region Public Methods

    /// <summary>
    /// Get a new instance of the aura to apply to an entity. This will return a non protoype aura.
    /// </summary>
    /// <param name="id">The name of the aura to instantiate.</param>
    /// <returns>The aura.</returns>
    public static Aura GetInstance(string name, Entity target, Entity caster)
    {
        return Auras[name].Clone(target, caster, Auras[name]);
    }

    /// <summary>
    /// Checks if the name exists in the aura list.
    /// </summary>
    /// <param name="name">The name of the aura.</param>
    /// <returns>Returns true if the aura exists, false otherwise.</returns>
    public static bool Contains(string name)
    {
        return Auras.ContainsKey(name);
    }

    #endregion
}

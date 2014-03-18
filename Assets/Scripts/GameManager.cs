using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{

    public equipmentFactory EquipmentFactory;

    public static Dictionary<string, Ability> Abilities;
    public static Dictionary<int, Aura> Auras;
    public static Dictionary<string, int> AuraStringToIntMap;

    public static float GLOBAL_COOLDOWN = 0.5f;

    public GameObject thing;
	// Use this for initialization
	public void Awake () 
    {
        //EquipmentFactory = new equipmentFactory();

        #region ability initialization
        Abilities = new Dictionary<string, Ability>();

        // Attack type, damage type, range, angle, cooldown, damagemod

        Abilities["hadouken"] = new Hadouken(AttackType.PBAOE, DamageType.FIRE, 5.0f, 360.0f, 3.0f, 1.0f, "hadouken", "Hadouken");
        Abilities["deathgrip"] = new Deathgrip(AttackType.PBAOE, DamageType.FIRE, 25.0f, 360.0f, 3.0f, 1.0f, "deathgrip", "AoE Deathgrip", thing);
        Abilities["cleave"] = new Cleave(AttackType.MELEE, DamageType.PHYSICAL, 3.0f, 45.0f, 5.0f, 1.0f, "cleave", "Cleave");
        Abilities["fusrodah"] = new Fusrodah(AttackType.PBAOE, DamageType.AIR, 5.0f, 45.0f, 10.0f, 1.0f, "fusrodah", "Fus Roh Dah");

        #endregion

        #region Aura Initialization
        // quite shittily done, will revisit soon

        Auras = new Dictionary<int, Aura>();
        AuraStringToIntMap = new Dictionary<string, int>();

        NewAura(new AuraTemplate(Auras.Count)); // repeat for each aura

        #endregion
    }

    private void NewAura(Aura auraToAdd)
    {
        if (!AuraStringToIntMap.ContainsKey(auraToAdd.Name))
        {
            Auras.Add(auraToAdd.ID, auraToAdd);
            AuraStringToIntMap.Add(auraToAdd.Name, auraToAdd.ID);
        }

        else
        {
            Debug.LogError("Duplicate name found for " + auraToAdd.Name + ". This aura was not added. Handle that shit.");
        }
    }

    public void RemovePhysics(Rigidbody toRemove, float time = 0.0f)
    {
        StartCoroutine(removePhysics(toRemove, time));
    }

    IEnumerator removePhysics(Rigidbody target, float time = 0.0f)
    {
        yield return new WaitForSeconds(time);

        if (target != null)
        {
            target.isKinematic = true;
        }

        yield break;
    }
}

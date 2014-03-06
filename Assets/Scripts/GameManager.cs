using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{

    public equipmentFactory EquipmentFactory;

    public static Dictionary<string, Ability> Abilities;

    public static float GLOBAL_COOLDOWN = 0.5f;

	// Use this for initialization
	void Start () 
    {
        EquipmentFactory = new equipmentFactory();

        #region ability initialization
        Abilities = new Dictionary<string, Ability>();

        // Attack type, damage type, range, angle, cooldown, damagemod

        Abilities["hadouken"] = new Hadouken(AttackType.PBAOE, DamageType.FIRE, 5.0f, 360.0f, 3.0f, 1.0f, "hadouken", "Hadouken");
        Abilities["deathgrip"] = new Deathgrip(AttackType.PBAOE, DamageType.FIRE, 5.0f, 360.0f, 3.0f, 1.0f, "deathgrip", "AoE Deathgrip");
        Abilities["cleave"] = new Cleave(AttackType.MELEE, DamageType.PHYSICAL, 5.0f, 45.0f, 5.0f, 1.0f, "cleave", "Cleave");
        Abilities["fusrodah"] = new Fusrodah(AttackType.PBAOE, DamageType.AIR, 5.0f, 45.0f, 10.0f, 1.0f, "fusrodah", "Fus Roh Dah");

        #endregion
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

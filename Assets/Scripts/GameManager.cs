using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public equipmentFactory EquipmentFactory;

    public Dictionary<string, Ability> Abilities;

	// Use this for initialization
	void Start () 
    {
        EquipmentFactory = new equipmentFactory();

        #region ability initialization
        Abilities = new Dictionary<string, Ability>();

        // Attack type, damage type, range, radius, cooldown, 

        Abilities["hadouken"] = new Ability(AttackType.PROJECTILE, DamageType.FIRE, 5.0f, 10.0f, 3.0f, 1.0f, "fireball", "Fireball");
        Abilities["cleave"] = new Ability(AttackType.MELEE, DamageType.PHYSICAL, 1.0f, 45.0f, 5.0f, 1.0f, "cleave", "Cleave");
        Abilities["fusrodah"] = new Ability(AttackType.PBAOE, DamageType.AIR, 20.0f, 360.0f, 10.0f, 1.0f, "fusrodah", "Fus Roh Dah");

        #endregion
    }
}

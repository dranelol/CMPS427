using UnityEngine;
using System.Collections;

public class PlayerEntity : Entity {
    public float power, defense, attackSpeed, movementSpeed, minDamage, maxDamage;

	// Use this for initialization
	new void Start () 
    {
        base.Start();
        abilities[2] = GameManager.Abilities["cleave"];
        abilities[3] = GameManager.Abilities["fusrodah"];
        abilities[4] = GameManager.Abilities["hadouken"];
        abilities[5] = GameManager.Abilities["deathgrip"];
	}
	
	// Update is called once per frame
	void Update () 
    {
        // update these on-demand instead of every update

        //power = currentAtt.Power;
        //defense = currentAtt.Defense;
        //attackSpeed = currentAtt.AttackSpeed;
        //movementSpeed = currentAtt.MovementSpeed;
        //minDamage = currentAtt.MinDamage;
        //maxDamage = currentAtt.MaxDamage;

        //Debug.Log(abilities.Count);
	}
}

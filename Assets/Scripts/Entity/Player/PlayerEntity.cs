using UnityEngine;
using System.Collections;

public class PlayerEntity : Entity {
    public float power, defense, attackSpeed, movementSpeed, minDamage, maxDamage;

	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        power = currentAtt.Power;
        defense = currentAtt.Defense;
        attackSpeed = currentAtt.AttackSpeed;
        movementSpeed = currentAtt.MovementSpeed;
        minDamage = currentAtt.MinDamage;
        maxDamage = currentAtt.MaxDamage;
	}
}

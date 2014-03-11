using UnityEngine;
using System.Collections;

public class PlayerEntity : Entity {
    public float power, defense, attackSpeed, movementSpeed, minDamage, maxDamage;

	// Use this for initialization
    public void Awake()
    {
        
        base.Awake();
        maxHP = 3000;
        currentHP = 3000;
    }


	public void Start () 
    {
        base.Start();
        
        abilities[2] = GameManager.Abilities["cleave"];
        abilities[3] = GameManager.Abilities["fusrodah"];
        abilities[4] = GameManager.Abilities["hadouken"];
        abilities[5] = GameManager.Abilities["deathgrip"];

	}
	
	// Update is called once per frame
	public void Update () 
    {
        abilities[4].AttackHandler(GameObject.FindGameObjectWithTag("Player"), true);
        abilities[5].AttackHandler(GameObject.FindGameObjectWithTag("Player"), true);

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

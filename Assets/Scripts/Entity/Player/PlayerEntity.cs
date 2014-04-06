using UnityEngine;
using System.Collections;

public class PlayerEntity : Entity 
{
    public float power, defense, attackSpeed, movementSpeed, minDamage, maxDamage;

	// Use this for initialization
    public void Awake()
    {
        base.Awake();
        maxHP = 3000;
        currentHP = 3000;
    }


	public void Start() 
    {
        base.Start();

        abilityManager.AddAbility(GameManager.Abilities["fireball"], 2);
        abilityManager.AddAbility(GameManager.Abilities["fusrodah"], 3);
        abilityManager.AddAbility(GameManager.Abilities["hadouken"], 4);
        abilityManager.AddAbility(GameManager.Abilities["deathgrip"], 5);

        abilityIndexDict["fireball"] = 2;
        abilityIndexDict["fusrodah"] = 3;
        abilityIndexDict["hadouken"] = 4;
        abilityIndexDict["deathgrip"] = 5;
	}
	
	// Update is called once per frame
	public void Update () 
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit target;
        Physics.Raycast(ray, out target, Mathf.Infinity);
        Vector3 vectorToMouse = target.point - gameObject.transform.position;
        Vector3 normalizedVectorToMouse = new Vector3(vectorToMouse.x, gameObject.transform.forward.y, vectorToMouse.z);
        Debug.DrawRay(gameObject.transform.position, normalizedVectorToMouse.normalized * 5.0f, Color.yellow);
        //Debug.Log(ray.direction);


        //Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        //abilities[4].AttackHandler(GameObject.FindGameObjectWithTag("Player"), true);
        //abilities[5].AttackHandler(GameObject.FindGameObjectWithTag("Player"), true);

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

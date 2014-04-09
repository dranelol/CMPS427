using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour {
    // Range at which the player stops moving and begins attacking. BAM
    public float attackRange;

	// Need to keep track of enemy if we click to attack it.
	private Vector3 targetPosition;

	public float RotationSpeed = 10f;
    public NavMeshAgent agent;

    private bool hadouken = false;

    public PlayerEntity entity;
    public MovementFSM moveFSM;
    public CombatFSM combatFSM;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

	// Use this for initialization
	void Start () {
		targetPosition = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
		agent.acceleration = 100f;
        agent.updateRotation = false;

        agent.avoidancePriority = 1;

        entity = GetComponent<PlayerEntity>();
        moveFSM = GetComponent<MovementFSM>();
        combatFSM = GetComponent<CombatFSM>();

        


    }
	
	// Update is called once per frame
	void FixedUpdate()
	{
		if (agent.hasPath)
		{
			Vector3 newVector = (transform.position + agent.velocity.normalized);
			Vector3 target = newVector - transform.position;
			Vector3 tempRotation = transform.rotation.eulerAngles ;
			tempRotation.y = Mathf.LerpAngle(transform.rotation.eulerAngles.y,  Quaternion.LookRotation(target).eulerAngles.y,Time.deltaTime * RotationSpeed);
			transform.rotation = Quaternion.Euler(tempRotation);
		}
	}
	void Update () 
    {
        
        if (GameObject.FindWithTag("UI Controller").GetComponent<UIController>().GuiState != UIController.States.INGAME)
            return;

        Debug.DrawRay(transform.position, transform.forward);
        //Debug.DrawRay(transform.position, Rotations.RotateAboutY(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f), -22.5f));
        //Debug.DrawRay(transform.position, Rotations.RotateAboutY(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f), 22.5f));
        
        // if our agent actually has a path to move to
        if (agent.hasPath == true)
        {
            /*
            // find the next steering target and his current position, without caring about y-axis
            Vector3 steeringTarget = new Vector3(agent.steeringTarget.x, 0, agent.steeringTarget.z);
            Vector3 playerPosition = new Vector3(transform.position.x, 0, transform.position.z);

            // create a quaternion to rotate towards the next target
            Quaternion quat = Quaternion.LookRotation(steeringTarget - playerPosition);

            //apply quaternion to the player's rotation
            //transform.rotation = quat;
            */
        }
        
        /*
        if (Vector3.Distance(transform.position, agent.destination) < 1.0f)
        {
            Debug.Log("Destination: " + agent.destination);
            Debug.Log("STOPPING 1");
            moveFSM.Stop();
        }
        */
        // If we have a target...

        if (targetPosition != Vector3.zero)
        {
            Debug.Log("this shouldnt happen ever");
            // If we're in attack range...
            Vector3 diff = targetPosition - transform.position;
            if (diff.magnitude <= attackRange)
            {
                // attack enemy
                targetPosition = Vector3.zero;
                Debug.Log("STOPPING 2");
                moveFSM.Stop();
            }
            else
            {
                moveFSM.SetPath(targetPosition);
            }
        }

        // If the move/attack key was pressed...
        if (Input.GetAxis("Move/Attack") != 0) 
        {
            Debug.Log("controller has path: " + agent.hasPath);

            int terrainMask = LayerMask.NameToLayer("Terrain");

            int enemyMask = LayerMask.NameToLayer("Enemy");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
			RaycastHit target;

            // If the raycast hit a collider...

			if (Physics.Raycast(ray, out target, Mathf.Infinity, 1 << terrainMask))
			{
               // Debug.Log(target.collider.gameObject.layer);
                //Debug.Log(target.collider.name);

                

                // If the collider was an enemy...
                if (target.collider.gameObject.tag == "Enemy")
                {
                    // Set the target position to the enemy's position.
                    //Debug.Log("this should also never happen");
                    targetPosition = target.collider.gameObject.transform.position;
                }

                else
                {
                    // Otherwise, move towards the point of collision.
                    targetPosition = Vector3.zero;
                    NavMeshHit hit;

                    if (NavMesh.SamplePosition(target.point, out hit, 20, 1 << LayerMask.NameToLayer("Default")))
                    {
                        //Debug.Log("it should always reach here");
                        //Debug.Log("target: " + target);
                        moveFSM.SetPath(hit.position);
                    }

                }
			}

        }

        #region abilities


        #region ability 1

        if (Input.GetKey(KeyCode.Q))
        {


            if (entity.abilityManager.abilities[2] != null)
            {
                if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[2] <= Time.time)
                {

                    Debug.Log("Attack Speed: " + entity.currentAtt.AttackSpeed.ToString());


                    if (entity.abilityManager.abilities[2].AttackType == AttackType.MELEE)
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                        entity.abilityManager.abilities[2].AttackHandler(gameObject, entity, true);

                    }

                    else if (entity.abilityManager.abilities[2].AttackType == AttackType.PROJECTILE)
                    {
                        //combatFSM.Attack(0.0f);

                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                        // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object



                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayCastTarget;
                        Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                        Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                        Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;


                        entity.abilityManager.abilities[2].SpawnProjectile(gameObject, gameObject, forward, entity.abilityManager.abilities[2].ID, true);
                    }

                    else if (entity.abilityManager.abilities[2].AttackType == AttackType.HONINGPROJECTILE)
                    {
                        //combatFSM.Attack(0.0f);

                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                        // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayCastTarget;
                        Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                        Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                        Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;


                        entity.abilityManager.abilities[2].SpawnProjectile(gameObject, rayCastTarget.point, gameObject, forward, entity.abilityManager.abilities[2].ID, true);
                    }
                    else
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                        entity.abilityManager.abilities[2].AttackHandler(gameObject, entity, true);


                    }


                    entity.abilityManager.activeCoolDowns[2] = Time.time + entity.abilityManager.abilities[2].Cooldown;


                }
            }
        }

        #endregion

        #region ability 2

        

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (entity.abilityManager.abilities[3] != null)
            {
                if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[3] <= Time.time)
                {
                    if (entity.abilityManager.abilities[3].AttackType == AttackType.MELEE)
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                        entity.abilityManager.abilities[3].AttackHandler(gameObject, entity, true);
                    }


                    else if (entity.abilityManager.abilities[3].AttackType == AttackType.PROJECTILE)
                    {
                        //combatFSM.Attack(0.0f);

                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                        // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayCastTarget;
                        Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                        Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                        Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;

                        entity.abilityManager.abilities[3].SpawnProjectile(gameObject, gameObject, forward, entity.abilityManager.abilities[3].ID, true);
                    }

                    else if (entity.abilityManager.abilities[3].AttackType == AttackType.HONINGPROJECTILE)
                    {
                        //combatFSM.Attack(0.0f);

                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                        // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayCastTarget;
                        Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                        Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                        Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;


                        entity.abilityManager.abilities[3].SpawnProjectile(gameObject, rayCastTarget.point, gameObject, forward, entity.abilityManager.abilities[3].ID, true);
                    }


                    else
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                        entity.abilityManager.abilities[3].AttackHandler(gameObject, entity, true);


                    }


                    entity.abilityManager.activeCoolDowns[3] = Time.time + entity.abilityManager.abilities[3].Cooldown;
                }
            }
        }
        #endregion

        #region ability 3
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (entity.abilityManager.abilities[4] != null)
            {
                if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[4] <= Time.time)
                {
                    if (entity.abilityManager.abilities[4].AttackType == AttackType.MELEE)
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                        entity.abilityManager.abilities[4].AttackHandler(gameObject, entity, true);
                    }


                    else if (entity.abilityManager.abilities[4].AttackType == AttackType.PROJECTILE)
                    {
                        //combatFSM.Attack(0.0f);

                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                        // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayCastTarget;
                        Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                        Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                        Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;

                        entity.abilityManager.abilities[4].SpawnProjectile(gameObject, gameObject, forward, entity.abilityManager.abilities[4].ID, true);
                    }

                    else if (entity.abilityManager.abilities[4].AttackType == AttackType.HONINGPROJECTILE)
                    {
                        //combatFSM.Attack(0.0f);

                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                        // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayCastTarget;
                        Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                        Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                        Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;


                        entity.abilityManager.abilities[4].SpawnProjectile(gameObject, rayCastTarget.point, gameObject, forward, entity.abilityManager.abilities[4].ID, true);
                    }


                    else
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                        entity.abilityManager.abilities[4].AttackHandler(gameObject, entity, true);


                    }


                    entity.abilityManager.activeCoolDowns[4] = Time.time + entity.abilityManager.abilities[4].Cooldown;
                }
            }
        }
        #endregion

        #region ability 4
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (entity.abilityManager.abilities[5] != null)
            {
                if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[5] <= Time.time)
                {
                    if (entity.abilityManager.abilities[5].AttackType == AttackType.MELEE)
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                        entity.abilityManager.abilities[5].AttackHandler(gameObject, entity, true);
                    }
                    else if (entity.abilityManager.abilities[5].AttackType == AttackType.PROJECTILE)
                    {
                        //combatFSM.Attack(0.0f);

                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                        // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayCastTarget;
                        Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                        Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                        Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;

                        entity.abilityManager.abilities[5].SpawnProjectile(gameObject, gameObject, forward, entity.abilityManager.abilities[5].ID, true);
                    }

                    else if (entity.abilityManager.abilities[5].AttackType == AttackType.HONINGPROJECTILE)
                    {
                        //combatFSM.Attack(0.0f);

                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                        // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayCastTarget;
                        Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                        Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                        Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;


                        entity.abilityManager.abilities[5].SpawnProjectile(gameObject, rayCastTarget.point, gameObject, forward, entity.abilityManager.abilities[5].ID, true);
                    }
                    else
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                        entity.abilityManager.abilities[5].AttackHandler(gameObject, entity, true);
                    }


                    entity.abilityManager.activeCoolDowns[5] = Time.time + entity.abilityManager.abilities[5].Cooldown;
                }
            }
        }
        #endregion


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            entity.abilityManager.AddAbility(GameManager.Abilities["shadowbolt"], 2);
            entity.abilityManager.AddAbility(GameManager.Abilities["poisonbolt"], 3);

            entity.abilityIndexDict["shadowbolt"] = 2;
            entity.abilityIndexDict["poisonbolt"] = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {

            for (int i = 0; i < 6; i++)
            {
                if (entity.HasEquipped((equipSlots.slots)i))
                {
                    gameManager.EquipmentFactory.saveequipment(i.ToString(), entity.GetEquip((equipSlots.slots)i));
                }
            }

            for( int i=2;i<6;i++)
            {
                if (entity.abilityManager.abilities[i] != null)
                {
                    PlayerPrefs.SetString("ability" + i, entity.abilityManager.abilities[i].ID);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            string blah = "";

            //blah = blah + entity.GetEquip(equipSlots.slots.Main).equipmentName + " \n" + entity.GetEquip(equipSlots.slots.Off).equipmentName;
            if (entity.HasEquipped(equipSlots.slots.Main))
                blah = blah + entity.GetEquip(equipSlots.slots.Main).equipmentName + " \n";
            else blah = blah + "HAS NO SWORD \n";
            if (entity.HasEquipped(equipSlots.slots.Off))
                blah = blah + entity.GetEquip(equipSlots.slots.Off).equipmentName + " \n";
            else blah = blah + "HAS NO OFFHAND \n";
            if (entity.HasEquipped(equipSlots.slots.Head))
                blah = blah + entity.GetEquip(equipSlots.slots.Head).equipmentName + " \n";
            else blah = blah + "HAS NO HAT \n";
            if (entity.HasEquipped(equipSlots.slots.Chest))
                blah = blah + entity.GetEquip(equipSlots.slots.Chest).equipmentName + " \n";
            else blah = blah + "HAS NO SHIRT \n";
            if (entity.HasEquipped(equipSlots.slots.Legs))
                blah = blah + entity.GetEquip(equipSlots.slots.Legs).equipmentName + " \n";
            else blah = blah + "HAS NO PANTS \n";
            if (entity.HasEquipped(equipSlots.slots.Feet))
                blah = blah + entity.GetEquip(equipSlots.slots.Feet).equipmentName + " \n";
            else blah = blah + "HAS NO SHOE \n";

            Debug.Log(blah);
        }

        #region ABILITY TESTS


        #endregion
        #region equipment stuff
        if (Input.GetKeyDown(KeyCode.A))
        {
            // small sword

            if (entity.HasEquipped(equipSlots.slots.Main))
            {
                Debug.Log("bro has a sword already! its called: " + entity.GetEquip(equipSlots.slots.Main).equipmentName);
            }

            Debug.Log("min damage before equip change to low sword: " + entity.currentAtt.MinDamage);
            Debug.Log("max damage before equip change to low sword: " + entity.currentAtt.MaxDamage);

            bool result = entity.removeEquipment(equipSlots.slots.Main);

            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(0, equipSlots.slots.Main);
            if (entity.addEquipment(equipSlots.slots.Main, tempEquip))
            {
                Debug.Log("min damage after equip change to low sword: " + entity.currentAtt.MinDamage);
                Debug.Log("max damage after equip change to low sword: " + entity.currentAtt.MaxDamage);
            }
            else
            {
                Debug.Log("CAN'T EQUIP THE SWORD FOR SOME REASON");
            }

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // better sword
            if (entity.HasEquipped(equipSlots.slots.Main))
            {
                Debug.Log("bro has a sword already! its called: " + entity.GetEquip(equipSlots.slots.Main).equipmentName);
            }


            Debug.Log("min damage before equip change to high sword: " + entity.currentAtt.MinDamage);
            Debug.Log("max damage before equip change to high sword: " + entity.currentAtt.MaxDamage);



            bool result = entity.removeEquipment(equipSlots.slots.Main);

            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Main);
            if(entity.addEquipment(equipSlots.slots.Main, tempEquip))
            {
            Debug.Log("min damage after equip change to high sword: " + entity.currentAtt.MinDamage);
            Debug.Log("max damage after equip change to high sword: " + entity.currentAtt.MaxDamage);
            }
            else
            {
                Debug.Log("CAN'T EQUIP THE SWORD FOR SOME REASON");
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            // chestpiece
            if (entity.HasEquipped(equipSlots.slots.Chest))
            {
                Debug.Log("bro has a chestpiece already! its called: " + entity.GetEquip(equipSlots.slots.Chest).equipmentName);
            }

            bool result = entity.removeEquipment(equipSlots.slots.Chest);

            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Chest);
            if (entity.addEquipment(equipSlots.slots.Chest, tempEquip))
            {

            }
            else
            {
                Debug.Log("CAN'T EQUIP THE chest FOR SOME REASON");
            }

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // chestpiece
            if (entity.HasEquipped(equipSlots.slots.Legs))
            {
                Debug.Log("bro has a pants already! its called: " + entity.GetEquip(equipSlots.slots.Legs).equipmentName);
            }

            bool result = entity.removeEquipment(equipSlots.slots.Legs);

            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Legs);
            if(entity.addEquipment(equipSlots.slots.Legs, tempEquip))
            {

            }
            else
            {
                Debug.Log("CAN'T EQUIP THE pants FOR SOME REASON");
            }

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            // chestpiece
            if (entity.HasEquipped(equipSlots.slots.Feet))
            {
                Debug.Log("bro has a feets already! its called: " + entity.GetEquip(equipSlots.slots.Feet).equipmentName);
            }


            bool result = entity.removeEquipment(equipSlots.slots.Feet);

            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Feet);
            if (entity.addEquipment(equipSlots.slots.Feet, tempEquip))
            {

            }
            else
            {

            }

        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            // chestpiece
            if (entity.HasEquipped(equipSlots.slots.Head))
            {
                Debug.Log("bro has a hat already! its called: " + entity.GetEquip(equipSlots.slots.Head).equipmentName);
            }

            bool result = entity.removeEquipment(equipSlots.slots.Head);

            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Head);
            if(entity.addEquipment(equipSlots.slots.Head, tempEquip))
            {

            }
            else
            {
                Debug.Log("CAN'T EQUIP THE hat FOR SOME REASON");
            }

        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            // chestpiece
            if (entity.HasEquipped(equipSlots.slots.Off))
            {
                Debug.Log("bro has a offhand already! its called: " + entity.GetEquip(equipSlots.slots.Off).equipmentName);
            }

            bool result = entity.removeEquipment(equipSlots.slots.Off);

            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Off);
            if (entity.addEquipment(equipSlots.slots.Off, tempEquip))
            {

            }
            else
            {
                Debug.Log("CAN'T EQUIP THE SWORD FOR SOME REASON");
            }

        }




        #endregion

        #endregion

    }

    
}

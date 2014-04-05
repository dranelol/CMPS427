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
                Debug.Log(target.collider.name);

                

                // If the collider was an enemy...
                if (target.collider.gameObject.tag == "Enemy")
                {
                    // Set the target position to the enemy's position.
                    Debug.Log("this should also never happen");
                    targetPosition = target.collider.gameObject.transform.position;
                }

                else
                {
                    // Otherwise, move towards the point of collision.
                    targetPosition = Vector3.zero;
                    NavMeshHit hit;

                    if (NavMesh.SamplePosition(target.point, out hit, 20, 1 << LayerMask.NameToLayer("Default")))
                    {
                        Debug.Log("it should always reach here");
                        Debug.Log("target: " + target);
                        moveFSM.SetPath(hit.position);
                    }

                }
			}

        }

        #region abilities


        #region ability 1

        if (Input.GetKey(KeyCode.Q))
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
                    combatFSM.Attack(0.0f);
                    // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                    // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object

                    entity.abilityManager.abilities[2].SpawnProjectile(gameObject, 2);

                }
                else
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                    entity.abilityManager.abilities[2].AttackHandler(gameObject, entity, true);


                }

                
                entity.abilityManager.activeCoolDowns[2] = Time.time + entity.abilityManager.abilities[2].Cooldown;
                
                
            }
        }

        #endregion

        #region ability 2

        

        if (Input.GetKeyDown(KeyCode.W))
        {

            if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[3] <= Time.time)
            {
                if (entity.abilityManager.abilities[3].AttackType == AttackType.MELEE)
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                }
                else
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                }

                entity.abilityManager.abilities[3].AttackHandler(gameObject, entity, true);
                entity.abilityManager.activeCoolDowns[3] = Time.time + entity.abilityManager.abilities[3].Cooldown;

            }
        }
        #endregion

        #region ability 3
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[4] <= Time.time)
            {
                Debug.Log(transform.position);
                if (entity.abilityManager.abilities[4].AttackType == AttackType.MELEE)
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                }
                else
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                }
                entity.abilityManager.abilities[4].AttackHandler(gameObject, entity, true);
                entity.abilityManager.activeCoolDowns[4] = Time.time + entity.abilityManager.abilities[4].Cooldown;

            }
        }
        #endregion

        #region ability 4
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[5] <= Time.time)
            {
                if (entity.abilityManager.abilities[5].AttackType == AttackType.MELEE)
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                }
                else
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                }
                entity.abilityManager.abilities[5].AttackHandler(gameObject, entity, true);
                entity.abilityManager.activeCoolDowns[5] = Time.time + entity.abilityManager.abilities[5].Cooldown;

            }
        }
        #endregion


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {



            
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
            entity.addEquipment(equipSlots.slots.Main, tempEquip);

            Debug.Log("min damage after equip change to low sword: " + entity.currentAtt.MinDamage);
            Debug.Log("max damage after equip change to low sword: " + entity.currentAtt.MaxDamage);


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
            entity.addEquipment(equipSlots.slots.Main, tempEquip);

            Debug.Log("min damage after equip change to high sword: " + entity.currentAtt.MinDamage);
            Debug.Log("max damage after equip change to high sword: " + entity.currentAtt.MaxDamage);

        }

        #endregion





        #endregion



    }

    
}

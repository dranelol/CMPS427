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
			
			
			//	Quaternion quat = Quaternion.LookRotation(target);
			//  transform.rotation = quat;
			
			Vector3 tempRotation = transform.rotation.eulerAngles ;
			tempRotation.y = Mathf.LerpAngle(transform.rotation.eulerAngles.y,  Quaternion.LookRotation(target).eulerAngles.y,Time.deltaTime * RotationSpeed);
			transform.rotation = Quaternion.Euler(tempRotation);
		}
	}
	void Update () {

        Debug.DrawRay(transform.position, transform.forward);
        Debug.DrawRay(transform.position, Rotations.RotateAboutY(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f), -22.5f));
        Debug.DrawRay(transform.position, Rotations.RotateAboutY(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f), 22.5f));
        
        // if our agent actually has a path to move to
        

        if (agent.hasPath == true)
        {
            // find the next steering target and his current position, without caring about y-axis
            Vector3 steeringTarget = new Vector3(agent.steeringTarget.x, 0, agent.steeringTarget.z);
            Vector3 playerPosition = new Vector3(transform.position.x, 0, transform.position.z);

            // create a quaternion to rotate towards the next target
            Quaternion quat = Quaternion.LookRotation(steeringTarget - playerPosition);

            //apply quaternion to the player's rotation
            //transform.rotation = quat;
        }
        
        if (Vector3.Distance(transform.position, agent.destination) < 1.0f)
        {
            agent.ResetPath();
        }
        // If we have a target...

        if (targetPosition != Vector3.zero)
        {
            // If we're in attack range...
            Vector3 diff = targetPosition - transform.position;
            if (diff.magnitude <= attackRange)
            {
                // attack enemy
                targetPosition = Vector3.zero;
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

            int terrainMask= LayerMask.NameToLayer("Terrain");

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
                    targetPosition = target.collider.gameObject.transform.position;
                }

                else
                {


                    // Otherwise, move towards the point of collision.
                    targetPosition = Vector3.zero;

                    moveFSM.SetPath(target.point);


                }
			}

        }

        #region new key-bound attacks




        #region ability 1
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (combatFSM.IsIdle() == true)
            {
                combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);


                Debug.Log(entity.abilities[2].ToString());
                entity.abilities[2].AttackHandler(gameObject, true);
            }
        }

        #endregion

        #region ability 2
        if (Input.GetKeyDown(KeyCode.W))
        {

            if (combatFSM.IsIdle() == true)
            {
                combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                entity.abilities[3].AttackHandler(gameObject, true);

            }
        }
        #endregion

        #region ability 3

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (combatFSM.IsIdle() == true)
            {

                combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                entity.abilities[4].AttackHandler(gameObject, true);

            }
        }
        #endregion

        #region ability 4

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (combatFSM.IsIdle() == true)
            {

                combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                entity.abilities[5].AttackHandler(gameObject, true);

            }
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.A))
        {
            // small sword

            Debug.Log("min damage before equip change to low sword: " + entity.currentAtt.MinDamage);
            Debug.Log("max damage before equip change to low sword: " + entity.currentAtt.MaxDamage);

            if(entity.HasEquipped(equipSlots.slots.Main))
            {
                Debug.Log("bro has a sword! its called: " + entity.GetEquip(equipSlots.slots.Main).equipmentName);
            }

            bool result = entity.removeEquipment(equipSlots.slots.Main);

            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(0, equipSlots.slots.Main);
            entity.addEquipment(equipSlots.slots.Main, tempEquip);

            Debug.Log("min damage after equip change to low sword: " + entity.currentAtt.MinDamage);
            Debug.Log("max damage after equip change to low sword: " + entity.currentAtt.MaxDamage);


        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // better sword

            Debug.Log("min damage before equip change to high sword: " + entity.currentAtt.MinDamage);
            Debug.Log("max damage before equip change to high sword: " + entity.currentAtt.MaxDamage);

            if (entity.HasEquipped(equipSlots.slots.Main))
            {
                Debug.Log("bro has a sword! its called: " + entity.GetEquip(equipSlots.slots.Main).equipmentName);
            }


            bool result = entity.removeEquipment(equipSlots.slots.Main);

            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Main);
            entity.addEquipment(equipSlots.slots.Main, tempEquip);

            Debug.Log("min damage after equip change to high sword: " + entity.currentAtt.MinDamage);
            Debug.Log("max damage after equip change to high sword: " + entity.currentAtt.MaxDamage);

        }





        #endregion
    }
}

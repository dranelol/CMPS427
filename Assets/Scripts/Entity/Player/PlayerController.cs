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
<<<<<<< HEAD
=======
        entity = GetComponent<PlayerEntity>();
        moveFSM = GetComponent<MovementFSM>();
        combatFSM = GetComponent<CombatFSM>();
>>>>>>> upstream/master
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
<<<<<<< HEAD




            int terrainMask =  LayerMask.NameToLayer("Terrain");
=======
            int terrainMask= LayerMask.NameToLayer("Terrain");
>>>>>>> upstream/master
            int enemyMask = LayerMask.NameToLayer("Enemy");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
			RaycastHit target;

            // If the raycast hit a collider...
<<<<<<< HEAD
			if (Physics.Raycast(ray, out target, Mathf.Infinity, 1 << terrainMask))
			{
               // Debug.Log(target.collider.gameObject.layer);
                //Debug.Log(target.collider.name);
=======
			if (Physics.Raycast(ray, out target, 1 << terrainMask))
			{

>>>>>>> upstream/master
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
<<<<<<< HEAD
                    GetComponent<MovementFSM>().SetPath(target.point);
=======
                    moveFSM.SetPath(target.point);
>>>>>>> upstream/master

                }
			}

        }

        #region new key-bound attacks

<<<<<<< HEAD
            else
            {
                agent.radius = 0.5f;
                hadouken = false;
            }
             * */
            List<GameObject> attacked = Attack.OnAttack(transform, 360f, 5f);
            //Debug.Log(attacked.Count);
            foreach (GameObject enemy in attacked)
            {
                if (enemy.GetComponent<AIController>().IsResetting() == false
                    && enemy.GetComponent<AIController>().IsDead() == false)
                {
                    Vector3 relativeVector = (enemy.transform.position - transform.position);
                    float normalizedMagnitude = 5f - Vector3.Distance(enemy.transform.position, transform.position);
                    float force = (normalizedMagnitude / (Mathf.Pow(0.4f, 2)));
                    enemy.GetComponent<MovementFSM>().Stop(0.2f);
                    enemy.rigidbody.isKinematic = false;
                    enemy.rigidbody.AddForce(relativeVector.normalized * force, ForceMode.Impulse);
                    
                    //enemy.rigidbody.AddForceAtPosition(50f, 
                    //enemy.rigidbody.AddExplosionForce(50f, transform.position, 5f, 3f);
                    //Destroy(enemy.rigidbody);
                    
                    Attack.DoDamage(gameObject, enemy);
                    StartCoroutine(Attack.RemovePhysics(enemy.rigidbody, 0.2f));
                }
            }
=======


        #region ability 1
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (combatFSM.IsIdle() == true)
            {
                combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
>>>>>>> upstream/master

                Debug.Log(entity.abilities[2].ToString());
                entity.abilities[2].AttackHandler(gameObject, true);
            }
        }

        #endregion

        #region ability 2
        if (Input.GetKeyDown(KeyCode.W))
        {
<<<<<<< HEAD
            List<GameObject> attacked = Attack.OnAttack(transform, 360f, 5f);
            //Debug.Log(attacked.Count);
            foreach (GameObject enemy in attacked)
            {
                
                if (enemy.GetComponent<AIController>().IsResetting() == false
                    && enemy.GetComponent<AIController>().IsDead() == false)
                {
                    Vector3 relativeVector = (enemy.transform.position - transform.position);
                    float normalizedMagnitude = 5f - Vector3.Distance(enemy.transform.position, transform.position);


                    
                    float force = (-1) * (normalizedMagnitude / (Mathf.Pow(0.4f, 2)));
                    enemy.GetComponent<MovementFSM>().Stop(0.2f);
                    enemy.rigidbody.AddForce(relativeVector.normalized * force, ForceMode.Impulse);
                    
                    //enemy.rigidbody.AddForceAtPosition(50f, 
                    //enemy.rigidbody.AddExplosionForce(50f, transform.position, 5f, 3f);
                    //Destroy(enemy.rigidbody);
                    Attack.DoDamage(gameObject, enemy);
                    StartCoroutine(Attack.RemovePhysics(enemy.rigidbody, 0.2f));
                }
=======
            if (combatFSM.IsIdle() == true)
            {
                combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                entity.abilities[3].AttackHandler(gameObject, true);
>>>>>>> upstream/master
            }
        }
        #endregion

        #region ability 3

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (combatFSM.IsIdle() == true)
            {
<<<<<<< HEAD
                if (enemy.GetComponent<AIController>().IsResetting() == false
                    && enemy.GetComponent<AIController>().IsDead() == false)
                {
                    Debug.Log(enemy.GetInstanceID().ToString());
                    Attack.DoDamage(gameObject, enemy);
                }
=======
                combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                entity.abilities[4].AttackHandler(gameObject, true);
>>>>>>> upstream/master
            }
        }
        #endregion

        #region ability 4

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (combatFSM.IsIdle() == true)
            {
<<<<<<< HEAD
                if (enemy.GetComponent<AIController>().IsResetting() == false
                    && enemy.GetComponent<AIController>().IsDead() == false)
                {
                    Vector3 relativeVector = (enemy.transform.position - transform.position);
                    float normalizedMagnitude = 5f - Vector3.Distance(enemy.transform.position, transform.position);


                    
                    float force = (normalizedMagnitude / (Mathf.Pow(0.35f, 2)));
                    enemy.GetComponent<MovementFSM>().Stop(0.17f);
                    enemy.rigidbody.AddForce(relativeVector.normalized * force, ForceMode.Impulse);
                    
                    Attack.DoDamage(gameObject, enemy);
                    StartCoroutine(Attack.RemovePhysics(enemy.rigidbody, 0.17f));
                }
=======
                combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                entity.abilities[5].AttackHandler(gameObject, true);
>>>>>>> upstream/master
            }
        }
        #endregion

        #endregion
    }
}

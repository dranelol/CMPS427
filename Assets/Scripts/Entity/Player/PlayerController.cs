using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    // Range at which the player stops moving and begins attacking. BAM
    public float attackRange;

	// Need to keep track of enemy if we click to attack it.
	private Vector3 targetPosition;

	public float RotationSpeed = 10f;
    public NavMeshAgent agent;

    private bool hadouken = false;

	// Use this for initialization
	void Start () {
		targetPosition = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
		agent.acceleration = 100f;
        agent.updateRotation = false;
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
                // TODO: Attack the enemy.
                Debug.Log("WE'Z GONNA ATTACK NAO");
                targetPosition = Vector3.zero;
                GetComponent<MovementFSM>().Stop();
            }
            else
            {
                // Otherwise, move towards the enemy.
                Debug.Log("We chasin' da enemy.");
                GetComponent<MovementFSM>().SetPath(targetPosition);
            }
        }

        // If the move/attack key was pressed...
        if (Input.GetAxis("Move/Attack") != 0) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit target;

            // If the raycast hit a collider...
			if (Physics.Raycast(ray, out target))
			{
                // If the collider was an enemy...
				if (target.collider.gameObject.tag == "Enemy")
                    // Set the target position to the enemy's position.
					targetPosition = target.collider.gameObject.transform.position;
				else
				{

                    
                    // Otherwise, move towards the point of collision.
					targetPosition = Vector3.zero;
					GetComponent<MovementFSM>().SetPath(target.point);

				}
			}

        }

        #region hadouken
        if (Input.GetKeyDown(KeyCode.B))
        {
            /*
            if (hadouken == false)
            {
                agent.radius = 10f;
                hadouken = true;
            }

            else
            {
                agent.radius = 0.5f;
                hadouken = false;
            }
             * */
            List<GameObject> attacked = Attack.OnAttack(transform, 360f, 5f);

            foreach (GameObject enemy in attacked)
            {
                if (enemy.GetComponent<Entity>().IsDead() == false)
                {
                    Vector3 relativeVector = (enemy.transform.position - transform.position);
                    float normalizedMagnitude = 5f - Vector3.Distance(enemy.transform.position, transform.position);


                    enemy.AddComponent<Rigidbody>();
                    float force = (normalizedMagnitude / (Mathf.Pow(0.4f, 2)));
                    enemy.GetComponent<MovementFSM>().Stop(0.2f);
                    enemy.rigidbody.AddForce(relativeVector.normalized * force, ForceMode.Impulse);
                    Destroy(enemy.rigidbody, 0.2f);
                    //enemy.rigidbody.AddForceAtPosition(50f, 
                    //enemy.rigidbody.AddExplosionForce(50f, transform.position, 5f, 3f);
                    //Destroy(enemy.rigidbody);
                    Attack.DoDamage(gameObject, enemy);
                }
            }

        }

        #endregion

        #region reverse hadouken
        if (Input.GetKeyDown(KeyCode.V))
        {
            List<GameObject> attacked = Attack.OnAttack(transform, 360f, 5f);
            
            foreach (GameObject enemy in attacked)
            {
                if (enemy.GetComponent<Entity>().IsDead() == false)
                {
                    Vector3 relativeVector = (enemy.transform.position - transform.position);
                    float normalizedMagnitude = 5f - Vector3.Distance(enemy.transform.position, transform.position);


                    enemy.AddComponent<Rigidbody>();
                    float force = (-1) * (normalizedMagnitude / (Mathf.Pow(0.4f, 2)));
                    enemy.GetComponent<MovementFSM>().Stop(0.2f);
                    enemy.rigidbody.AddForce(relativeVector.normalized * force, ForceMode.Impulse);
                    Destroy(enemy.rigidbody, 0.2f);
                    //enemy.rigidbody.AddForceAtPosition(50f, 
                    //enemy.rigidbody.AddExplosionForce(50f, transform.position, 5f, 3f);
                    //Destroy(enemy.rigidbody);
                    Attack.DoDamage(gameObject, enemy);
                }
            }

        }

        #endregion

        #region cleave
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("lelele");
            List<GameObject> attacked = Attack.OnAttack(transform, 45f, 5f);
            foreach (GameObject enemy in attacked)
            {
                if (enemy.GetComponent<Entity>().IsDead() == false)
                {
                    Debug.Log(enemy.GetInstanceID().ToString());
                    //Color enemyColor = enemy.renderer.material.color;
                    //enemy.renderer.material.color = ;
                    //Color enemyColor = enemy.renderer.material.GetColor("_Color");
                    //Color enemyColorTint = enemy.renderer.material.GetColor("_TintColor");
                    //float intensity = enemyColorTint.a + 50;
                    //Debug.Log(enemyColor.ToString());
                    //enemyColor.r -= (byte)0.5;
                    //enemyColor.b -= (byte)0.5;
                    //enemyColor.g -= (byte)0.5;

                    //Color tintColor = new Color32(255, 23, 0, (byte)intensity);
                    //enemy.renderer.material.SetColor("_TintColor", tintColor);
                    //enemy.renderer.material.SetColor("_Color", enemyColor);
                    //enemy.renderer.material.SetColor("_Color", new Color(enemyColor.r - (byte)1, enemyColor.g - (byte)1, enemyColor.b - (byte)1, enemyColor.a));
                    //enemy.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
                    //enemy.renderer.material.SetColor("_Color", Color.red);
                    Attack.DoDamage(gameObject, enemy);
                }
            }
        }

        #endregion

        #region fusrodah
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("lelele");
            List<GameObject> attacked = Attack.OnAttack(transform, 45f, 5f);
            foreach (GameObject enemy in attacked)
            {
                if (enemy.GetComponent<Entity>().IsDead() == false)
                {
                    Vector3 relativeVector = (enemy.transform.position - transform.position);
                    float normalizedMagnitude = 5f - Vector3.Distance(enemy.transform.position, transform.position);


                    enemy.AddComponent<Rigidbody>();
                    float force = (normalizedMagnitude / (Mathf.Pow(0.35f, 2)));
                    enemy.GetComponent<MovementFSM>().Stop(0.17f);
                    enemy.rigidbody.AddForce(relativeVector.normalized * force, ForceMode.Impulse);
                    Destroy(enemy.rigidbody, 0.17f);
                    Attack.DoDamage(gameObject, enemy);
                }
            }
        }
        #endregion
    }
}

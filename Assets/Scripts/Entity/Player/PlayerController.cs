using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    // Range at which the player stops moving and begins attacking. BAM
    public float attackRange;

	// Need to keep track of enemy if we click to attack it.
	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
		targetPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
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
	}
}

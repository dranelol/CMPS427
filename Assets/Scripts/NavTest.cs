using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavTest : MonoBehaviour 
{
    public MovementFSM MoveFSM;
    public NavMeshAgent agent;
    public CharacterController controller;

    void Start()
    {
        MoveFSM = GetComponent<MovementFSM>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        agent.updateRotation = false;
        
    }

	void Update () 
    {
        Debug.DrawRay(transform.position, transform.forward);
        Debug.DrawRay(transform.position, Rotations.RotateAboutY(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f), -22.5f));
        Debug.DrawRay(transform.position, Rotations.RotateAboutY(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f), 22.5f));
        //Debug.Log(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f).magnitude);
        //Debug.Log(transform.forward);
        //Debug.Log(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f));
        
        if (agent.velocity != Vector3.zero)
        {
            Vector3 newVector = (transform.position + agent.velocity.normalized);
            Vector3 target = newVector - transform.position;
            
            Quaternion quat = Quaternion.LookRotation(target);
            transform.rotation = quat;
        }
        
        
        
        if (Input.GetMouseButtonDown(0))
        {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray theRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0;

            if (playerPlane.Raycast(theRay, out hitdist))
            {
                Vector3 NavTarget = theRay.GetPoint(hitdist);
                MoveFSM.SetPath(NavTarget);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            List<GameObject> attacked = Attack.OnAttack(transform, 45f, 5f);
            foreach (GameObject enemy in attacked)
            {
                Debug.Log(enemy.GetInstanceID().ToString());
            }
        }
	}
}

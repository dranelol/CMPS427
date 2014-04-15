using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavTest : MonoBehaviour 
{
	void Update () 
    {
        if (!this.gameObject.GetComponent<NavMeshAgent>().hasPath)
        {
            animation.animation.Play("idle", PlayMode.StopAll);
        }
	}
    public MovementFSM MoveFSM;
    public NavMeshAgent agent;
    public CharacterController controller;
	public float RotationSpeed = 10f;

    void Start()
    {
        MoveFSM = GetComponent<MovementFSM>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();

		agent.acceleration = 100f;
        agent.updateRotation = false;
        
    }
	void FixedUpdate ()
    {
        Debug.DrawRay(transform.position, transform.forward);
        Debug.DrawRay(transform.position, Rotations.RotateAboutY(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f), -22.5f));
        Debug.DrawRay(transform.position, Rotations.RotateAboutY(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f), 22.5f));
        //Debug.Log(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f).magnitude);
        //Debug.Log(transform.forward);
        //Debug.Log(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f));
        
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
 
		/*
        if (agent.velocity != Vector3.zero)
        {
            Vector3 newVector = (transform.position += agent.velocity.normalized);
            Vector3 target = newVector - transform.position;
            
            Quaternion quat = Quaternion.LookRotation(target);
            transform.rotation = quat;
        }
        */
        
        
        if (Input.GetMouseButtonDown(0))
        {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray theRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0;

            if (playerPlane.Raycast(theRay, out hitdist))
            {
                //NavTarget = theRay.GetPoint(hitdist);
                //this.gameObject.GetComponent<NavMeshAgent>().SetDestination(NavTarget);
                //animation.Rewind("run");
                //animation.animation.Play("run", PlayMode.StopAll);
                Vector3 NavTarget = theRay.GetPoint(hitdist);
                MoveFSM.SetPath(NavTarget);
            }
        }
	}
}

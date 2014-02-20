using UnityEngine;
using System.Collections;

public class NavTest : MonoBehaviour 
{
	void Update () 
    {
        if (!this.gameObject.GetComponent<NavMeshAgent>().hasPath)
        {
            animation.animation.Play("idle", PlayMode.StopAll);
        }

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
        
        
        if (agent.velocity != Vector3.zero)
        {
            Vector3 newVector = (transform.position + agent.velocity.normalized);
            Vector3 target = newVector - transform.position;
            
            Quaternion quat = Quaternion.LookRotation(target);
            transform.rotation = quat;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 NavTarget;

            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray theRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0;

            if (playerPlane.Raycast(theRay, out hitdist))
            {
                NavTarget = theRay.GetPoint(hitdist);
                this.gameObject.GetComponent<NavMeshAgent>().SetDestination(NavTarget);
                animation.Rewind("run");
                animation.animation.Play("run", PlayMode.StopAll);
            }
        }
	}
}

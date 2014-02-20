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

    public MovementFSM MoveFSM;
    public NavMeshAgent agent;
    public CharacterController controller;
    public Material lerpMaterial;

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

        if (Input.GetKeyDown(KeyCode.A))
        {
            List<GameObject> attacked = Attack.OnAttack(transform, 45f, 5f);
            foreach (GameObject enemy in attacked)
            {
                Debug.Log(enemy.GetInstanceID().ToString());
                //Color enemyColor = enemy.renderer.material.color;
                //enemy.renderer.material.color = ;
                Color enemyColor = enemy.renderer.material.GetColor("_Color");
                //Color enemyColorTint = enemy.renderer.material.GetColor("_TintColor");
                //float intensity = enemyColorTint.a + 50;
                Debug.Log(enemyColor.ToString());
                enemyColor.r -= (byte)0.5;
                enemyColor.b -= (byte)0.5;
                enemyColor.g -= (byte)0.5;
                
                //Color tintColor = new Color32(255, 23, 0, (byte)intensity);
                //enemy.renderer.material.SetColor("_TintColor", tintColor);
                //enemy.renderer.material.SetColor("_Color", enemyColor);
                enemy.renderer.material.SetColor("_Color", new Color(enemyColor.r - (byte)1, enemyColor.g - (byte)1, enemyColor.b - (byte)1, enemyColor.a));
                enemy.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
	}
}

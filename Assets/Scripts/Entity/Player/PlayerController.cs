using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Move/Attack") != 0) 
        {
            Plane player = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float hitDistance = 0;
            // TODO: Check for ray hit on enemy.
            if (player.Raycast(ray, out hitDistance))
            {
                GetComponent<MovementFSM>().SetPath(ray.GetPoint(hitDistance));
            }
        }
	}
}

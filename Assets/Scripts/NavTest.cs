using UnityEngine;
using System.Collections;

public class NavTest : MonoBehaviour 
{
    public MovementFSM MoveFSM;

    void Start()
    {
        MoveFSM = GetComponent<MovementFSM>();
    }

	void Update () 
    {
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
	}
}

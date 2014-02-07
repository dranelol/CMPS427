using UnityEngine;
using System.Collections;

public class MouseToWorldScript : MonoBehaviour 
{
    private Vector3 _worldMousePosition;
    public bool isActive = true;

	void Start () 
    {
        _worldMousePosition = new Vector3(0, 0, 0);
	}
	
	void Update () 
    {
        if (isActive)
        {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray theRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            float hitdist = 0;

            if (playerPlane.Raycast(theRay, out hitdist))
            {
                transform.position = theRay.GetPoint(hitdist);
            }
        }
	}

    public Vector3 WorldMousePosition
    {
        get { return _worldMousePosition; }
    }
}

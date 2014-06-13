using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


    public Transform Target;

	void Start () 
    {
	
	}
	
	void Update () 
    {
        transform.position = new Vector3(Target.position.x, transform.position.y, Target.position.z);
	}
}

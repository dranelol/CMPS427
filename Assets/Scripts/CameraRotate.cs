using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour {
    public Transform orbitObject;
    Vector3 orbit;
	// Use this for initialization
	void Start () {
        orbit = orbitObject.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(orbit, Vector3.up, 10 * Time.deltaTime);
	}
}

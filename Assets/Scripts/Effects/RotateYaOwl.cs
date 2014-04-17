using UnityEngine;
using System.Collections;

public class RotateYaOwl : MonoBehaviour 
{
    public Vector3 owlsPerSecond;

	void Start () 
    {
	
	}
	
	void Update () 
    {
        transform.Rotate(owlsPerSecond);
	}
}

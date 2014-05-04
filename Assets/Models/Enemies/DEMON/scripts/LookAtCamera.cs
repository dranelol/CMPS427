using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour 
{
    public Transform _pivot;

	void Update () 
    {
        Vector3 vectorToCamera = Camera.main.transform.position - _pivot.position;
        Vector3 vectorToDistorn = Camera.main.transform.position - transform.position;
        transform.RotateAround(_pivot.position, Vector3.Cross(vectorToCamera, vectorToDistorn), Vector3.Angle(vectorToDistorn, vectorToCamera));

        transform.LookAt(Camera.main.transform.position);
	}
}

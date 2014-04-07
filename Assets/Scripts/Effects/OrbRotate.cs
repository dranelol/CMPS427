using UnityEngine;
using System.Collections;

public class OrbRotate : MonoBehaviour 
{
    public float minHeight;
    public float maxHeight;
    public float angularSpeed;
    public float oscillationSpeed;

    void Awake()
    {
        
    }

	void Update () 
    {
        //newPosition.y = Mathf.Lerp(minHeight, maxHeight, Time.time);

        transform.RotateAround(transform.parent.transform.position, Vector3.up, angularSpeed * Time.deltaTime);
        Vector3 newPosition = transform.position;

        //newPosition.y = MathHelper.Sinerp(minHeight, maxHeight, Time.time);
        newPosition.y = Mathf.Sin(Time.time * oscillationSpeed) * maxHeight + maxHeight;

        transform.position = newPosition;
	}
}

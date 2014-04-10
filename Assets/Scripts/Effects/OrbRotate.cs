using UnityEngine;
using System.Collections;

public class OrbRotate : MonoBehaviour 
{
    public float minHeight;
    public float maxHeight;
    public float angularSpeed;
    public float oscillationSpeed;
    public bool yOrbit;
    public float orbitScale;
    public GameObject orbitObject;

    void Awake()
    {
        
    }

	void Update () 
    {
        //newPosition.y = Mathf.Lerp(minHeight, maxHeight, Time.time);
        if (yOrbit == true)
        {
            transform.RotateAround(orbitObject.transform.position, Vector3.up, angularSpeed * Time.deltaTime);
            Vector3 newPosition = transform.position;


            //newPosition.x = MathHelper.Sinerp(minHeight, maxHeight, Time.time);
            if (oscillationSpeed > 0.0f)
            {
                newPosition.y = Mathf.Sin(Time.time * oscillationSpeed) * maxHeight + maxHeight;
            }

            transform.position = newPosition;
        }

        else
        {
            transform.RotateAround(orbitObject.transform.position, orbitObject.transform.forward, angularSpeed * Time.deltaTime);
        }

        

	}
}

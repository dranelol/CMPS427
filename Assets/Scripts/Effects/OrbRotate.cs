using UnityEngine;
using System.Collections;

public class OrbRotate : MonoBehaviour 
{
    public float minHeight;
    public float maxHeight;
    public float angularSpeed;
    public float oscillationSpeed;
    public string rotationAxis;
    public float orbitScale;

    void Awake()
    {
        
    }

	void Update () 
    {
        //newPosition.y = Mathf.Lerp(minHeight, maxHeight, Time.time);
        if (rotationAxis == "x" || rotationAxis == "X")
        {
            transform.RotateAround(transform.parent.transform.position, Vector3.left, angularSpeed * Time.deltaTime);

            Vector3 newPosition = transform.position;

            //newPosition.x = MathHelper.Sinerp(minHeight, maxHeight, Time.time);
            if (oscillationSpeed > 0.0f)
            {
                newPosition.x = Mathf.Sin(Time.time * oscillationSpeed) * maxHeight + maxHeight;
            }

            transform.position = newPosition;
        }

        else if (rotationAxis == "y" || rotationAxis == "Y")
        {
            transform.RotateAround(transform.parent.transform.position, Vector3.up, angularSpeed * Time.deltaTime);

            Vector3 newPosition = transform.position;

            //newPosition.y = MathHelper.Sinerp(minHeight, maxHeight, Time.time);
            if (oscillationSpeed > 0.0f)
            {
                newPosition.y = Mathf.Sin(Time.time * oscillationSpeed) * maxHeight + maxHeight;
            }

            transform.position = newPosition;
        }

        else
        {
            transform.RotateAround(transform.parent.transform.position, Vector3.forward, angularSpeed * Time.deltaTime);

            

            //newPosition.z = MathHelper.Sinerp(minHeight, maxHeight, Time.time);
            if (oscillationSpeed > 0.0f)
            {
                Vector3 newPosition = transform.position;
                newPosition.z = Mathf.Sin(Time.time * oscillationSpeed) * maxHeight + maxHeight;
                transform.position = newPosition;
            }

            

           
        }

        
	}
}

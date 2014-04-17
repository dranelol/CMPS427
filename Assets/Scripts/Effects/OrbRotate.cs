using UnityEngine;
using System.Collections;

public class OrbRotate : MonoBehaviour 
{
    public float minHeight;
    public float maxHeight;
    public float angularSpeed;
    public float oscillationSpeed;

    /// <summary>
    /// Whether or not this is orbiting about y-axis
    /// </summary>
    public bool yOrbit;
    
    /// <summary>
    /// Scale of orbit
    /// </summary>
    public float orbitScale;

    /// <summary>
    /// Origin of rotation
    /// </summary>
    public GameObject orbitObject;

    /// <summary>
    /// Whether or not this is orbiting clockwise
    /// </summary>
    public bool clockwiseRotate;

    /// <summary>
    /// Rotations to complete before death
    /// </summary>
    public float rotations;

    /// <summary>
    /// Total rotations since start
    /// </summary>
    private float currentRotations = 0.0f;

    /// <summary>
    /// Current vector between position and origin of rotation
    /// </summary>
    private Vector3 currentPositionVector;

    /// <summary>
    /// Previous vector between position and origin of rotation
    /// </summary>
    private Vector3 previousPositionVector;

    void Awake()
    {
        previousPositionVector = currentPositionVector = (transform.position - orbitObject.transform.position);
    }

	void Update () 
    {
        //newPosition.y = Mathf.Lerp(minHeight, maxHeight, Time.time);

        // orbiting about y-axis
        if (yOrbit == true)
        {
            if (clockwiseRotate == true)
            {
                transform.RotateAround(orbitObject.transform.position, Vector3.up, angularSpeed * Time.deltaTime);
            }

            else
            {
                transform.RotateAround(orbitObject.transform.position, Vector3.up, angularSpeed * Time.deltaTime * (-1)); 
            }

            Vector3 newPosition = transform.position;


            //newPosition.x = MathHelper.Sinerp(minHeight, maxHeight, Time.time);
            if (oscillationSpeed > 0.0f)
            {
                newPosition.y = Mathf.Sin(Time.time * oscillationSpeed) * maxHeight + maxHeight;
            }

            transform.position = newPosition;
        }

        // orbiting around forward vector of origin object
        else
        {   
            if (clockwiseRotate == true)
            {
                transform.RotateAround(orbitObject.transform.position, orbitObject.transform.forward, angularSpeed * Time.deltaTime);
            }
            
            else
            {
                transform.RotateAround(orbitObject.transform.position, orbitObject.transform.forward, angularSpeed * Time.deltaTime * (-1));
            }
        }

        

        

	}
}

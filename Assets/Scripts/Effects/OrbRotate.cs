using UnityEngine;
using System.Collections;

public class OrbRotate : MonoBehaviour 
{
    public float minHeight;
    public float maxHeight;
    public float angularSpeed;
    public float oscillationSpeed;

    /// <summary>
    /// Whether or not this is orbiting about y-axis; if this is false, it is assumed we are rotating around an object, so orbitObject is set
    /// </summary>
    public bool yOrbit;
    
    /// <summary>
    /// Scale of orbit
    /// </summary>
    public float orbitScale;

    /// <summary>
    /// Origin of rotation if the orb is following an object
    /// </summary>
    public GameObject orbitObject;

    /// <summary>
    /// Origin of rotation
    /// </summary>
    public Vector3 orbitPosition;

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

    /// <summary>
    /// Whether or not the orbitPosition needs to update based on an orbitObject
    /// </summary>
    public bool movingOrbit;

    void Start()
    {
        if (movingOrbit == true)
        {
            orbitPosition = orbitObject.transform.position;
        }

        previousPositionVector = currentPositionVector = (transform.position - orbitPosition);
        Debug.Log("orb starting position: " + transform.position.ToString());
    }

	void Update () 
    {
        if (movingOrbit == true)
        {
            orbitPosition = orbitObject.transform.position;
        }

        // figuring out how much we've rotated so far
        previousPositionVector = currentPositionVector;
        currentPositionVector = (transform.position - orbitPosition);

        float angleTravelled = Vector3.Angle(previousPositionVector, currentPositionVector);

        currentRotations = currentRotations + (angleTravelled / 360.0f);

        if (currentRotations >= rotations)
        {
            // suicide and cleanup 
            //StartCoroutine(orbCleanup());
            Debug.Log("destroying");
            Destroy(gameObject, GetComponent<TrailRenderer>().time);
            this.enabled = false;
        }

        // orbiting about y-axis
        if (yOrbit == true)
        {
            Vector3 newPosition = transform.position;
            float angleToRotate = angularSpeed * Time.deltaTime;

            if (clockwiseRotate == true)
            {
                
                //transform.RotateAround(orbitObject.transform.position, Vector3.up, angularSpeed * Time.deltaTime);
                /*
                newPosition = new Vector3(orbitObject.transform.position.x + orbitScale * Mathf.Cos(angularSpeed * Mathf.Deg2Rad) * Time.deltaTime,
                                    orbitObject.transform.position.y,
                                    orbitObject.transform.position.z + orbitScale * Mathf.Sin(angularSpeed * Mathf.Deg2Rad) * Time.deltaTime);
                */
                newPosition = new Vector3((Mathf.Cos(angleToRotate * Mathf.Deg2Rad) * (transform.position.x - orbitPosition.x)) - (Mathf.Sin(angleToRotate * Mathf.Deg2Rad) * (transform.position.z - orbitPosition.z)) + orbitObject.transform.position.x,
                                           orbitObject.transform.position.y,
                                           (Mathf.Sin(angleToRotate * Mathf.Deg2Rad) * (transform.position.x - orbitPosition.x)) + (Mathf.Cos(angleToRotate * Mathf.Deg2Rad) * (transform.position.z - orbitPosition.z)) + orbitObject.transform.position.z);


            }

            else
            {
                //transform.RotateAround(orbitObject.transform.position, Vector3.up, angularSpeed * Time.deltaTime * (-1)); 
                newPosition = new Vector3((Mathf.Cos(angleToRotate * Mathf.Deg2Rad) * (transform.position.x - orbitPosition.x)) + (Mathf.Sin(angleToRotate * Mathf.Deg2Rad) * (transform.position.z - orbitPosition.z)) + orbitObject.transform.position.x,
                                           orbitObject.transform.position.y,
                                           (Mathf.Sin(angleToRotate * Mathf.Deg2Rad) * (transform.position.x - orbitPosition.x)) - (Mathf.Cos(angleToRotate * Mathf.Deg2Rad) * (transform.position.z - orbitPosition.z)) + orbitObject.transform.position.z);


            }

            


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
                transform.RotateAround(orbitPosition, orbitObject.transform.forward, angularSpeed * Time.deltaTime);
            }
            
            else
            {
                transform.RotateAround(orbitPosition, orbitObject.transform.forward, angularSpeed * Time.deltaTime * (-1));
            }
        }
	}

    private IEnumerator orbCleanup()
    {

        yield return null;
    }
}

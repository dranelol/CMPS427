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

    /// <summary>
    /// Whether or not this will rotate till death
    /// </summary>
    public bool infiniteRotation;

    private bool movingUp = true;

    private Vector3 testVector = new Vector3(199.3f, 9.86f, 204.98f);

    void Start()
    {
        if (movingOrbit == true)
        {
            orbitPosition = orbitObject.transform.position;
        }

        previousPositionVector = currentPositionVector = (transform.position - orbitPosition);

        
        
    }

	void Update () 
    {
        if (movingOrbit == true)
        {
            orbitPosition = orbitObject.transform.position;
        }
        //Debug.Log(Vector3.Distance(transform.position, orbitObject.transform.position).ToString());
        // figuring out how much we've rotated so far
        previousPositionVector = currentPositionVector;
        currentPositionVector = (transform.position - orbitPosition);

        float angleTravelled = Vector3.Angle(previousPositionVector, currentPositionVector);
        
        currentRotations = currentRotations + (angleTravelled / 360.0f);

        if (currentRotations >= rotations && infiniteRotation == false)
        {
            // suicide and cleanup 
            //StartCoroutine(orbCleanup());
            Destroy(gameObject, GetComponent<TrailRenderer>().time);
            this.enabled = false;
        }

        // orbiting about y-axis
        if (yOrbit == true)
        {
            float angleToRotate = angularSpeed * Time.deltaTime;

            if (clockwiseRotate == true)
            {
                transform.RotateAround(orbitPosition, Vector3.up, angularSpeed * Time.deltaTime);
                //newPosition = new Vector3((Mathf.Cos(angleToRotate * Mathf.Deg2Rad) * (transform.position.x - orbitPosition.x)) - (Mathf.Sin(angleToRotate * Mathf.Deg2Rad) * (transform.position.z - orbitPosition.z)) + orbitPosition.x,
                                           //orbitPosition.y,
                                           //(Mathf.Sin(angleToRotate * Mathf.Deg2Rad) * (transform.position.x - orbitPosition.x)) + (Mathf.Cos(angleToRotate * Mathf.Deg2Rad) * (transform.position.z - orbitPosition.z)) + orbitPosition.z);
                

            }

            else
            {
                //transform.RotateAround(orbitObject.transform.position, Vector3.up, angularSpeed * Time.deltaTime * (-1)); 
                //newPosition = new Vector3((Mathf.Cos(angleToRotate * Mathf.Deg2Rad) * (transform.position.x - orbitPosition.x)) - (Mathf.Sin(angleToRotate * Mathf.Deg2Rad) * (transform.position.z - orbitPosition.z)) + orbitPosition.x,
                                           //orbitPosition.y,
                                           //(Mathf.Sin(angleToRotate * Mathf.Deg2Rad) * (transform.position.x - orbitPosition.x)) + (Mathf.Cos(angleToRotate * Mathf.Deg2Rad) * (transform.position.z - orbitPosition.z)) + orbitPosition.z);
                transform.RotateAround(orbitPosition, Vector3.up, angularSpeed * Time.deltaTime * (-1));
            }

            // y-oscillation

            

            if (oscillationSpeed > 0.0f)
            {
                Vector3 newPosition = transform.position;

                if (transform.position.y >= maxHeight + orbitPosition.y)
                {
                    movingUp = false;
                }

                if (transform.position.y <= minHeight + orbitPosition.y)
                {
                    movingUp = true;
                }
                //newPosition.y = Mathf.Sin(Time.time * oscillationSpeed) * maxHeight + maxHeight;
                //newPosition.y = Mathf.Lerp(minHeight, maxHeight, Mathf.PingPong(Time.time, 1.0f));

                //newPosition.y = transform.position.y + Mathf.Sin(Time.deltaTime);

                if (movingUp == true)
                {
                    //newPosition.y += Mathf.Lerp(minHeight, maxHeight, Time.time);
                    newPosition.y = transform.position.y + oscillationSpeed * Time.deltaTime;
                }

                else
                {
                    //newPosition.y += Mathf.Lerp(minHeight, maxHeight, Time.time);
                    newPosition.y = transform.position.y - oscillationSpeed * Time.deltaTime;
                }

                transform.position = newPosition;
            }

            
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

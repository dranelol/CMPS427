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

    void Start()
    {
        previousPositionVector = currentPositionVector = (transform.position - orbitObject.transform.position);
    }

	void Update () 
    {
        // figuring out how much we've rotated so far
        previousPositionVector = currentPositionVector;
        currentPositionVector = (transform.position - orbitObject.transform.position);

        float angleTravelled = Vector3.Angle(previousPositionVector, currentPositionVector);

        currentRotations = currentRotations + (angleTravelled / 360.0f);

        if (currentRotations >= rotations)
        {
            // suicide and cleanup 
            //StartCoroutine(orbCleanup());
            Destroy(gameObject, GetComponent<TrailRenderer>().time);
            this.enabled = false;
        }

        // orbiting about y-axis
        if (yOrbit == true)
        {
            Vector3 newPosition = transform.position;

            if (clockwiseRotate == true)
            {
                //transform.RotateAround(orbitObject.transform.position, Vector3.up, angularSpeed * Time.deltaTime);
                newPosition = new Vector3(orbitObject.transform.position.x + orbitScale * Mathf.Sin(angularSpeed * Mathf.Deg2Rad),
                                    orbitObject.transform.position.y,
                                    orbitObject.transform.position.z + orbitScale * Mathf.Cos(angularSpeed * Mathf.Deg2Rad));
            }

            else
            {
                //transform.RotateAround(orbitObject.transform.position, Vector3.up, angularSpeed * Time.deltaTime * (-1)); 
                newPosition = new Vector3(orbitObject.transform.position.x - orbitScale * Mathf.Sin(angularSpeed * Mathf.Deg2Rad),
                                    orbitObject.transform.position.y,
                                    orbitObject.transform.position.z - orbitScale * Mathf.Cos(angularSpeed * Mathf.Deg2Rad));
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
                transform.RotateAround(orbitObject.transform.position, orbitObject.transform.forward, angularSpeed * Time.deltaTime);
            }
            
            else
            {
                transform.RotateAround(orbitObject.transform.position, orbitObject.transform.forward, angularSpeed * Time.deltaTime * (-1));
            }
        }
	}

    private IEnumerator orbCleanup()
    {

        yield return null;
    }
}

using UnityEngine;
using System.Collections;

public class OrbSpawnSingle : MonoBehaviour
{
    public GameObject orb;
    public float minHeight;
    public float maxHeight;
    public float angularSpeed;
    public float oscillationSpeed;
    public bool yOrbit;
    public float orbitScale;
    public GameObject orbitObject;
    public float initialAngleFromForward;
    public bool clockwiseRotate;
    public float rotations;
    public bool movingOrbit;
    /// <summary>
    /// Whether or not this will rotate till death
    /// </summary>
    public bool infiniteRotation;

    void Start()
    {
        Vector3 newOrbPos = Vector3.zero;

        if (yOrbit == true)
        {
            newOrbPos = new Vector3(transform.position.x + orbitScale * Mathf.Cos(initialAngleFromForward * Mathf.Deg2Rad),
                                    transform.position.y,
                                    transform.position.z + orbitScale * Mathf.Sin(initialAngleFromForward * Mathf.Deg2Rad));

            // rotate based on forward vector of orbit object

            //float turnAngle = Vector3.Angle(Vector3.back, orbitObject.transform.forward);

            
        }

        else
        {
            newOrbPos = new Vector3(transform.position.x + orbitScale * Mathf.Sin(initialAngleFromForward * Mathf.Deg2Rad),
                                    transform.position.y + orbitScale * Mathf.Cos(initialAngleFromForward * Mathf.Deg2Rad),
                                    transform.position.z);

            

        }

        Debug.Log("spawning orb at: " + newOrbPos.ToString());

        GameObject newOrb = (GameObject)GameObject.Instantiate(orb, newOrbPos, transform.rotation);
        newOrb.GetComponent<OrbRotate>().minHeight = minHeight;
        newOrb.GetComponent<OrbRotate>().maxHeight = maxHeight;
        newOrb.GetComponent<OrbRotate>().angularSpeed = angularSpeed;
        newOrb.GetComponent<OrbRotate>().oscillationSpeed = oscillationSpeed;
        newOrb.GetComponent<OrbRotate>().yOrbit = yOrbit;
        newOrb.GetComponent<OrbRotate>().orbitScale = orbitScale;
        newOrb.GetComponent<OrbRotate>().rotations = rotations;
        newOrb.GetComponent<OrbRotate>().clockwiseRotate = clockwiseRotate;
        newOrb.GetComponent<OrbRotate>().movingOrbit = movingOrbit;
        newOrb.GetComponent<OrbRotate>().infiniteRotation = infiniteRotation;

        if (movingOrbit == true)
        {
            if (orbitObject == null)
            {
                Debug.Log("asd");
                if (transform.parent == null)
                {
                    newOrb.GetComponent<OrbRotate>().orbitObject = gameObject;
                }

                else
                {
                    newOrb.GetComponent<OrbRotate>().orbitObject = transform.parent.gameObject;
                }
            }

            else
            {
                newOrb.GetComponent<OrbRotate>().orbitObject = orbitObject;
            }
        }

        else
        {
            newOrb.GetComponent<OrbRotate>().orbitPosition = transform.position;
        }


        

        newOrb.transform.parent = transform;
    }
}

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
            newOrbPos = new Vector3(transform.position.x + orbitScale,// * Mathf.Cos(initialAngleFromForward * Mathf.Deg2Rad),*/
                                    CombatMath.GetCenter(transform).y,
                                    transform.position.z + orbitScale);// * Mathf.Sin(initialAngleFromForward * Mathf.Deg2Rad));

            // rotate based on forward vector of orbit object

            

            
        }

        else
        {
            newOrbPos = new Vector3(transform.position.x + orbitScale,// * Mathf.Sin(initialAngleFromForward * Mathf.Deg2Rad),
                                    CombatMath.GetCenter(transform).y + orbitScale,// * Mathf.Cos(initialAngleFromForward * Mathf.Deg2Rad),
                                    transform.position.z);

            

        }

        //Debug.Log("spawning orb at: " + newOrbPos.ToString());

        Debug.Log("spawn distance: " + Vector3.Distance(newOrbPos, transform.position));

        GameObject newOrb = (GameObject)GameObject.Instantiate(orb, newOrbPos, transform.rotation);
        OrbRotate orbRotate = newOrb.GetComponent<OrbRotate>();
        orbRotate.minHeight = minHeight;
        orbRotate.maxHeight = maxHeight;
        orbRotate.angularSpeed = angularSpeed;
        orbRotate.oscillationSpeed = oscillationSpeed;
        orbRotate.yOrbit = yOrbit;
        orbRotate.orbitScale = orbitScale;
        orbRotate.rotations = rotations;
        orbRotate.clockwiseRotate = clockwiseRotate;
        orbRotate.movingOrbit = movingOrbit;
        orbRotate.infiniteRotation = infiniteRotation;

        

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

        newOrb.transform.RotateAround(newOrb.GetComponent<OrbRotate>().orbitPosition, Vector3.up, initialAngleFromForward);

        newOrb.transform.parent = transform;


    }
}

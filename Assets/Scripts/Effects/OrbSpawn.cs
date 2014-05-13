using UnityEngine;
using System.Collections;

public class OrbSpawn : MonoBehaviour
{
    public int orbAmount;
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



	void Start () 
    {
        for (int i = 0; i < orbAmount; i++)
        {
            Vector3 newOrbPos = Vector3.zero;
            Debug.Log("orbitScale: " + orbitScale);
            // trig math to spawn orbs around the orbit object, evenly spaced
            if (yOrbit == true)
            {
                newOrbPos = new Vector3(transform.position.x + orbitScale * Mathf.Cos((360 / orbAmount) * i * Mathf.Deg2Rad),
                                        transform.position.y + minHeight,
                                        transform.position.z + orbitScale * Mathf.Sin((360 / orbAmount) * i * Mathf.Deg2Rad));
            }

            else
            {
                newOrbPos = new Vector3(transform.position.x + orbitScale * Mathf.Sin((360 / orbAmount) * i * Mathf.Deg2Rad),
                                        transform.position.y + orbitScale * Mathf.Cos((360 / orbAmount) * i * Mathf.Deg2Rad),
                                        transform.position.z);

            }

            // set all orb stuff that needs to be set from master 
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
            orbRotate.orbitObject = orbitObject;

            Debug.Log("spawn distance: " + Vector3.Distance(newOrbPos, transform.position));

            if (movingOrbit == true)
            {
                if (orbitObject == null)
                {
                    // if we need to move our orbit, and we dont have a parent, we need to set one as the current attached gameobject
                    if (transform.parent == null)
                    {
                        newOrb.GetComponent<OrbRotate>().orbitObject = gameObject;
                    }

                    // else, set it as our parent
                    else
                    {
                        newOrb.GetComponent<OrbRotate>().orbitObject = transform.parent.gameObject;
                    }
                }
                // if the orbitobject isnt null, set it as the orbitobject
                else
                {
                    newOrb.GetComponent<OrbRotate>().orbitObject = orbitObject;
                }
            }

            // if we dont need to move our orbit, set the position as the current transform's position
            else
            {
                newOrb.GetComponent<OrbRotate>().orbitPosition = transform.position;
            }




            newOrb.transform.parent = transform;
            
        }
	}
}

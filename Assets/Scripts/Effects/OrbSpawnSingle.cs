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

    void Awake()
    {
        Vector3 newOrbPos = Vector3.zero;

        if (yOrbit == true)
        {
            newOrbPos = new Vector3(orbitObject.transform.position.x + orbitScale * Mathf.Sin(initialAngleFromForward * Mathf.Deg2Rad),
                                    orbitObject.transform.position.y,
                                    orbitObject.transform.position.z + orbitScale * Mathf.Cos(initialAngleFromForward * Mathf.Deg2Rad));
        }

        else
        {
            newOrbPos = new Vector3(orbitObject.transform.position.x + orbitScale * Mathf.Sin(initialAngleFromForward * Mathf.Deg2Rad),
                                    orbitObject.transform.position.y + orbitScale * Mathf.Cos(initialAngleFromForward * Mathf.Deg2Rad),
                                    orbitObject.transform.position.z);

        }

        GameObject newOrb = (GameObject)GameObject.Instantiate(orb, newOrbPos, transform.rotation);
        newOrb.GetComponent<OrbRotate>().minHeight = minHeight;
        newOrb.GetComponent<OrbRotate>().maxHeight = maxHeight;
        newOrb.GetComponent<OrbRotate>().angularSpeed = angularSpeed;
        newOrb.GetComponent<OrbRotate>().oscillationSpeed = oscillationSpeed;
        newOrb.GetComponent<OrbRotate>().yOrbit = yOrbit;
        newOrb.GetComponent<OrbRotate>().orbitScale = orbitScale;
        newOrb.GetComponent<OrbRotate>().rotations = rotations;
        newOrb.GetComponent<OrbRotate>().clockwiseRotate = clockwiseRotate;


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

        newOrb.transform.parent = transform;
    }
}

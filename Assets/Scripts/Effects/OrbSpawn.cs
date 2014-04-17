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

    /// <summary>
    /// Whether or not this will rotate till death
    /// </summary>
    public bool infiniteRotation;



	void Awake () 
    {
        for (int i = 0; i < orbAmount; i++)
        {
            Vector3 newOrbPos = Vector3.zero;

            if (yOrbit == true)
            {
                newOrbPos = new Vector3(orbitObject.transform.position.x + orbitScale * Mathf.Sin((360 / orbAmount) * i * Mathf.Deg2Rad),
                                        orbitObject.transform.position.y,
                                        orbitObject.transform.position.z + orbitScale * Mathf.Cos((360 / orbAmount) * i * Mathf.Deg2Rad));
            }
            

            else 
            {
                newOrbPos = new Vector3(orbitObject.transform.position.x + orbitScale * Mathf.Sin((360 / orbAmount) * i * Mathf.Deg2Rad),
                                        orbitObject.transform.position.y + orbitScale * Mathf.Cos((360 / orbAmount) * i * Mathf.Deg2Rad),
                                        orbitObject.transform.position.z);

                
            }

            //newOrbPos = Rotations.RotateAboutY(transform.parent.transform.forward, (360 / orbAmount) * i) * 2;
            //newOrbPos.y = minHeight;

            GameObject newOrb = (GameObject)GameObject.Instantiate(orb, newOrbPos, transform.rotation);
            newOrb.GetComponent<OrbRotate>().minHeight = minHeight;
            newOrb.GetComponent<OrbRotate>().maxHeight = maxHeight;
            newOrb.GetComponent<OrbRotate>().angularSpeed = angularSpeed;
            newOrb.GetComponent<OrbRotate>().oscillationSpeed = oscillationSpeed;
            newOrb.GetComponent<OrbRotate>().yOrbit = yOrbit;
            newOrb.GetComponent<OrbRotate>().orbitScale = orbitScale;

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
}

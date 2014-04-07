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

	void Awake () 
    {
        for (int i = 0; i < orbAmount; i++)
        {
            GameObject newOrb = (GameObject)GameObject.Instantiate(orb, transform.parent.transform.forward + Rotations.RotateAboutY(transform.parent.transform.forward, (360 / orbAmount) * i) * 2, Quaternion.identity);
            newOrb.GetComponent<OrbRotate>().minHeight = minHeight;
            newOrb.GetComponent<OrbRotate>().maxHeight = maxHeight;
            newOrb.GetComponent<OrbRotate>().angularSpeed = angularSpeed;
            newOrb.GetComponent<OrbRotate>().oscillationSpeed = oscillationSpeed;
            newOrb.transform.parent = transform;
            
        }
	}
}

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
    public string rotationAxis;
    public float orbitScale;

	void Awake () 
    {
        for (int i = 0; i < orbAmount; i++)
        {
            Vector3 newOrbPos = Vector3.zero;

            if (rotationAxis == "x" || rotationAxis == "X")
            {
                newOrbPos = Rotations.RotateAboutX(transform.parent.transform.forward * orbitScale, (360 / orbAmount) * i) * 2;
                newOrbPos.x = minHeight;
            }

            else if (rotationAxis == "y" || rotationAxis == "Y")
            {
                newOrbPos = Rotations.RotateAboutY(transform.parent.transform.forward * orbitScale, (360 / orbAmount) * i) * 2;
                newOrbPos.y = minHeight;
            }

            else
            {
                newOrbPos = Rotations.RotateAboutZ(transform.parent.transform.forward * orbitScale, (360 / orbAmount) * i) * 2;
                //newOrbPos.z = minHeight;

                Debug.Log((transform.parent.transform.forward * orbitScale).ToString());
                Debug.Log(newOrbPos);
            }

            //newOrbPos = Rotations.RotateAboutY(transform.parent.transform.forward, (360 / orbAmount) * i) * 2;
            //newOrbPos.y = minHeight;

            GameObject newOrb = (GameObject)GameObject.Instantiate(orb, newOrbPos, transform.rotation);
            newOrb.GetComponent<OrbRotate>().minHeight = minHeight;
            newOrb.GetComponent<OrbRotate>().maxHeight = maxHeight;
            newOrb.GetComponent<OrbRotate>().angularSpeed = angularSpeed;
            newOrb.GetComponent<OrbRotate>().oscillationSpeed = oscillationSpeed;
            newOrb.GetComponent<OrbRotate>().rotationAxis = rotationAxis;
            newOrb.GetComponent<OrbRotate>().orbitScale = orbitScale;
            newOrb.transform.parent = transform;
            
        }
	}
}

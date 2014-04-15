using UnityEngine;
using System.Collections;

public class ParticleCleanup : MonoBehaviour 
{
	void Update () 
    {
        if (particleSystem.enableEmission == false)
        {
            Destroy(gameObject, particleSystem.duration);
        }
	}
}

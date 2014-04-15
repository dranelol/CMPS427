using UnityEngine;
using System.Collections;

public class ParticleCleanup : MonoBehaviour 
{
	void Update () 
    {
        if (particleSystem.emissionRate == 0)
        {
            Destroy(gameObject, particleSystem.duration);
        }
	}
}

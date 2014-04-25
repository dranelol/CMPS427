using UnityEngine;
using System.Collections;

/// <summary>
/// destroy particle systems once they stop emitting
/// </summary>
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

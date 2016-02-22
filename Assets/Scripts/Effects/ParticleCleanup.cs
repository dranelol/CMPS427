using UnityEngine;
using System.Collections;

/// <summary>
/// destroy particle systems once they stop emitting
/// </summary>
public class ParticleCleanup : MonoBehaviour 
{
	void Update () 
    {
        if (GetComponent<ParticleSystem>().emissionRate == 0)
        {
            Destroy(gameObject, GetComponent<ParticleSystem>().duration);
        }
	}
}

using UnityEngine;
using System.Collections;

public class ExpandParticleSystemRadius : MonoBehaviour 
{
    private ParticleSystem particleSystem;
    public float delta;
	void Awake () 
    {
        particleSystem = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    //particleSystem.ra
	}
}

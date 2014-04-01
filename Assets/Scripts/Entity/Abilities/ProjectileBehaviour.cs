using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileBehaviour : MonoBehaviour 
{
    /// <summary>
    /// The creator of this projectile
    /// </summary>
    public GameObject owner;

    /// <summary>
    /// Velocity of the rigidbody 
    /// </summary>
    public Vector3 velocity;

    /// <summary>
    /// Constant of acceleration to apply to the projectile. For a consistant speed, this is default set to 0.0f
    /// </summary>
    public float accelerationConstant = 0.0f;

    /// <summary>
    /// This is a maximum time that the projectile can remain alive before activating
    /// </summary>
    public float timeToActivate;

    /// <summary>
    /// The particle system applied to the projectile, if any
    /// </summary>
    public GameObject trailParticles;

    void Awake()
    {

    }

	void Start () 
    {
	    
	}
	

	void Update () 
    {
        timeToActivate -= Time.deltaTime;

        if (timeToActivate <= 0.0f)
        {
            // do attack things
            
            // call do animation

            // clean up and suicide
        }

        // update position of projectile

	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // do attack things

        }

        else if(collision.gameObject.tag == "Player")
        {
            // do attack things

        }

        // call do animation here

        // clean up and suicide
    }
}

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

        //Debug.Log(timeToActivate);

        velocity = rigidbody.velocity;

        if (timeToActivate <= 0.0f)
        {
            // do attack things
            Debug.Log("explode!");
            
            // call do animation

            // clean up and suicide
        }

        // update position of projectile

	}
    /*
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && owner.gameObject.tag == "Player")
        {
            Debug.Log("attacked an enemy!");

            Destroy(gameObject);

            
        }

        else if (collision.gameObject.tag == "Player" && owner.gameObject.tag == "Enemy")
        {
            Debug.Log("attacked a player");
        }

        // call do animation here

        // clean up and suicide

        //Destroy(gameObject);


    }
    */

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && owner.gameObject.tag == "Player")
        {
            Debug.Log("attacked an enemy!");

            Destroy(gameObject);


        }

        else if (other.gameObject.tag == "Player" && owner.gameObject.tag == "Enemy")
        {
            Debug.Log("attacked a player");
        }

        // call do animation here

        // clean up and suicide

        //Destroy(gameObject);
    }
}

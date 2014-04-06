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

    /// The public identifier of the ability tied to this projectile on hit
    /// </summary>
    public string abilityID;

    /// <summary>
    /// Constant of acceleration to apply to the projectile. For a consistant speed, this is default set to 0.0f
    /// </summary>
    public float accelerationConstant = 0.0f;

    /// <summary>
    /// This is a maximum time that the projectile can remain alive before activating
    /// </summary>
    public float timeToActivate;


    /// <summary>
    /// this sees if the projectile has collided with anything yet. Keeps it from colliding with two things
    /// </summary>
    private bool hascollided = false;

    void Awake()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

        // if we need to fuck with particle scale, do it here

        /*
        foreach (ParticleSystem item in particles)
        {
            item.transform.localScale = new Vector3(1, 1, 1);
        }
        */
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
            DetachParticleSystem();
            Destroy(gameObject);
        }

        // update position of projectile

	}

    void OnTriggerEnter(Collider other)
    {
        if (hascollided == false)
        {
            if (other.gameObject.tag == "Enemy" && owner.gameObject.tag == "Player")
            {
                Debug.Log("attacked an enemy!");


                Entity ownerEntity = owner.GetComponent<Entity>();

                int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

                ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(owner, other.gameObject, owner.GetComponent<Entity>(), true);

                hascollided = true;
                DetachParticleSystem();
                Destroy(gameObject);


            }

            else if (other.gameObject.tag == "Player" && owner.gameObject.tag == "Enemy")
            {
                Debug.Log("attacked a player");


                Entity ownerEntity = owner.GetComponent<Entity>();

                int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

                ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(owner, other.gameObject, owner.GetComponent<Entity>(), false);
                hascollided = true;
                DetachParticleSystem();
                Destroy(gameObject);
            }
            /*
        else if (other.gameObject.tag == "Enemy" && owner.gameObject.tag == "Enemy")
        {
            Debug.Log("attacked a friendly enemy");

            Entity ownerEntity = owner.GetComponent<Entity>();

            int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

            ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(owner, other.gameObject, owner.GetComponent<Entity>(), true);

            DetachParticleSystem();
            Destroy(gameObject);
        }
        */
            // call attackhandler on this projectile's ability

            // clean up and suicide



            //Destroy(gameObject);
            
        }
    }

    public void DetachParticleSystem()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in particles)
        {
            item.transform.parent = null;
            item.emissionRate = 0;
            item.enableEmission = false; 


        }

        //particles.GetComponent<ParticleAnimator>().autodestruct = true;
    }
}

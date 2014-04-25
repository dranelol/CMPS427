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
    public bool hascollided = false;

    /// <summary>
    /// flag for if the projectile collides with other projectiles instead of players/enemies
    /// </summary>
    public bool CollidesWithProjectiles = false;

    /// <summary>
    /// flag for if the projectile does it's attack when it times out
    /// </summary>
    public bool ExplodesOnTimeout = false;

    /// <summary>
    /// counter for... things. use it for whatever.
    /// </summary>
    public int count;

    /// <summary>
    /// If this projectile needs a target, this is it
    /// </summary>
    public Vector3 target = Vector3.zero;

    /// <summary>
    /// Is this a homing projectile?
    /// </summary>
    public bool homing = false;

    /// <summary>
    /// Speed of the projectile
    /// </summary>
    public float speed;

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

        //velocity = rigidbody.velocity;

        // if this is a homing projectile, rotate towards our homing target
        if (homing == true)
        {
            Vector3 direction = target - transform.position;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, 0.22f, 0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        // move along the path
        transform.position = transform.position + transform.forward * Time.deltaTime * speed;

        // explode time, if applicable
        if (timeToActivate <= 0.0f)
        {
            // if we need to explode
            if (ExplodesOnTimeout == true)
            {

                if (owner.gameObject.tag == "Player")
                {

                    // carry out explosion ability

                    Entity ownerEntity = owner.GetComponent<Entity>();

                    int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

                    ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(gameObject, owner, ownerEntity, true);

                    hascollided = true;
                    DetachParticleSystem();
                    Destroy(gameObject);


                }

                else if (owner.gameObject.tag == "Enemy")
                {

                    // carry out explosion ability

                    Entity ownerEntity = owner.GetComponent<Entity>();

                    int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

                    ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(gameObject, owner, ownerEntity, false);
                    hascollided = true;
                    DetachParticleSystem();
                    Destroy(gameObject);
                }
            }

            else
            {
                DetachParticleSystem();
                Destroy(gameObject);
            }
        }


	}

    void OnTriggerEnter(Collider other)
    {
        if (hascollided == false)
        {
            // if we havent collided with, and we collide with projectiles, check if the trigger we just entered was another projectile

            if (CollidesWithProjectiles == true)
            {
                
                if (other.gameObject.tag == "Projectile")
                {
                    if (other.gameObject.GetComponent<ProjectileBehaviour>().owner.tag == "Player")
                    {
                        // carry out projectile explosion ability
                        Debug.Log("attacked an enemy!");

                        Entity ownerEntity = owner.GetComponent<Entity>();

                        int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

                        ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(owner, other.gameObject, owner.GetComponent<Entity>(), true);

                        hascollided = true;
                        DetachParticleSystem();
                        Destroy(gameObject);
                    }

                }
            }

            // else, normal explosion

            else
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

                if (other.gameObject.tag == "Terrain")
                {
                }

                if (homing == true && other.gameObject.tag == "Terrain")
                {
                    hascollided = true;
                    DetachParticleSystem();
                    Destroy(gameObject);
                }
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
        /*
        foreach (Transform child in transform)
        {
            Debug.Log("asd");
            if (child.gameObject.tag == "OrbRotate")
            {
                Debug.Log("unparenting orbs");
                child.parent = null;
            }


        }
        */

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in particles)
        {
            item.transform.parent = null;
            item.emissionRate = 0;
            //item.enableEmission = false; 


        }

        

        //particles.GetComponent<ParticleAnimator>().autodestruct = true;
    }
}

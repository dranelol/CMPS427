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
    /// Whether or not this projectile exists until interacted with
    /// </summary>
    public bool Infinite = false;

    /// <summary>
    /// this sees if the projectile has collided with anything yet. Keeps it from colliding with two things
    /// </summary>
    public bool hasCollided = false;

    /// <summary>
    /// flag for if the projectile collides with other projectiles instead of players/enemies
    /// </summary>
    public bool CollidesWithProjectiles = false;

    /// <summary>
    /// flag for if the projectile(homing only right now) cares about terrain 
    /// </summary>
    public bool CollidesWithTerrain = true;

    /// <summary>
    /// For projectiles that have an AoE attack on explode
    /// </summary>
    public bool AOEOnExplode = false;

    /// <summary>
    /// flag for if the projectile does it's attack when it times out
    /// </summary>
    public bool ExplodesOnTimeout = false;

    /// <summary>
    /// flag for if the projectile kills itself when it hits a target
    /// </summary>
    public bool DiesOnHit = true;

    /// <summary>
    /// flag for if the projectile kills itself when it hits its owner
    /// </summary>
    public bool DiesOnOwnerHit = false;

    /// <summary>
    /// counter for... things. use it for whatever.
    /// </summary>
    public int count;

    /// <summary>
    /// A list of every player and enemy the projectile has collided with.
    /// </summary>
    public List<GameObject> HasCollidedWith;



    /// <summary>
    /// If this projectile needs a target, this is it
    /// </summary>
    public Vector3 target = Vector3.zero;

    /// <summary>
    /// if the projectile needs to home to an object, this is it
    /// </summary>
    public GameObject targetObject = null;

    /// <summary>
    /// Is this a homing projectile?
    /// </summary>
    public bool homing = false;

    /// <summary>
    /// Speed of the projectile
    /// </summary>
    public float speed;

    /// <summary>
    /// If true, then this projectile isn't attributed to any ability
    /// </summary>
    public bool EnvironmentProjectile = false;

    void Awake()
    {
        //ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

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
            if (targetObject != null)
            {
                Vector3 direction = targetObject.transform.position - transform.position;

                Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, 0.3f, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else
            {

                Vector3 direction = target - transform.position;

                Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, 0.3f, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }

            
        }

        // move along the path
        transform.position = transform.position + transform.forward * Time.deltaTime * speed;
        speed += accelerationConstant * Time.deltaTime;

        // explode time, if applicable
        if (timeToActivate <= 0.0f)
        {
            // if we need to explode
            if (ExplodesOnTimeout == true)
            {

                if (owner.gameObject.tag == "Player")
                {

                    // carry out explosion ability
                    if (EnvironmentProjectile == false)
                    {
                        Entity ownerEntity = owner.GetComponent<Entity>();

                        int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

                        ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(gameObject, owner, ownerEntity, true);
                    }
                    hasCollided = true;
                    DetachParticleSystem();
                    Destroy(gameObject);


                }

                else if (owner.gameObject.tag == "Enemy")
                {

                    // carry out explosion ability
                    if (EnvironmentProjectile == false)
                    {
                        Entity ownerEntity = owner.GetComponent<Entity>();

                        int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

                        ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(gameObject, owner, ownerEntity, false);
                    }
                    hasCollided = true;
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
        if (hasCollided == false)
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

                        if (EnvironmentProjectile == false)
                        {
                            Entity ownerEntity = owner.GetComponent<Entity>();

                            int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

                            ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(owner, other.gameObject, owner.GetComponent<Entity>(), true);
                        }
                        hasCollided = true;
                        DetachParticleSystem();
                        Destroy(gameObject);
                    }

                }
            }

            // else, normal explosion

            else if (EnvironmentProjectile == true && other.gameObject.tag == "Terrain")
            {
                Debug.Log("environment hit!");
                if (DiesOnHit == true)
                {
                    hasCollided = true;
                    DetachParticleSystem();
                    Destroy(gameObject);
                }
                
            }

            else if (CollidesWithTerrain == true && other.gameObject.tag == "Terrain")
            {
                // if this is a pbaoe attack, do the attack handler at point of collision
                if (AOEOnExplode == true)
                {
                    Entity ownerEntity = owner.GetComponent<Entity>();
                    int abilityIndex = ownerEntity.abilityIndexDict[abilityID];
                    ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(gameObject, other.gameObject, owner.GetComponent<Entity>(), true);

                }

                if (DiesOnHit == true)
                {
                    hasCollided = true;
                    DetachParticleSystem();
                    Destroy(gameObject);
                }
            }

            else
            {
                Debug.Log(other.gameObject.tag + " and " + owner.gameObject.tag);
                if (other.gameObject.tag == "Enemy" && owner.gameObject.tag == "Player")
                {
                    if (HasCollidedWith.Contains(other.gameObject) == false)
                    {
                        Debug.Log("attacked an enemy: " + gameObject.name.ToString());
                        HasCollidedWith.Add(other.gameObject);

                        if (EnvironmentProjectile == false)
                        {
                            Entity ownerEntity = owner.GetComponent<Entity>();

                            int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

                            ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(owner, other.gameObject, owner.GetComponent<Entity>(), true);
                        }

                        if (DiesOnHit == true)
                        {

                            hasCollided = true;
                            DetachParticleSystem();
                            Destroy(gameObject);
                        }
                    }

                }

                else if (other.gameObject.tag == "Player" && owner.gameObject.tag == "Enemy")
                {
                    if (HasCollidedWith.Contains(other.gameObject) == false)
                    {
                        Debug.Log("attacked a player");
                        HasCollidedWith.Add(other.gameObject);

                        if (EnvironmentProjectile == false)
                        {
                            Entity ownerEntity = owner.GetComponent<Entity>();

                            int abilityIndex = ownerEntity.abilityIndexDict[abilityID];

                            ownerEntity.abilityManager.abilities[abilityIndex].AttackHandler(owner, other.gameObject, owner.GetComponent<Entity>(), false);
                        }

                        if (DiesOnHit == true)
                        {

                            hasCollided = true;
                            DetachParticleSystem();
                            Destroy(gameObject);
                        }
                    }
                }

                

                
                if (other.gameObject == owner.gameObject && DiesOnOwnerHit == true)
                {
                    Debug.Log("hit owner");
                    hasCollided = true;
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

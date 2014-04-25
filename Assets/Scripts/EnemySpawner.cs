using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour 
{
    public GameObject enemyPrefab;

    public SphereCollider trigger;

    public int enemyCount;
    public float spawnRadius;
    public int level;
    public string enemytype;

    // Continuous generation
    public bool isStatic; // This determines if the spawner should generate 1 group or continuously spawn enemies over time.

    // Only visible if continuous generation is selected
    public bool isTrigger;
    public float triggerRadius; // The radius in which a player must be before to trigger generation
    public int spawnInterval; // The time in between spawns while a player is within the radius

    private float spawnCounter = 0;


    void Awake()
    {
        NavMeshHit meshLocation;

        if (NavMesh.SamplePosition(transform.position, out meshLocation, 30, 1 << LayerMask.NameToLayer("Default")))
        {
            transform.position = meshLocation.position;
        }

        else
        {
            this.gameObject.SetActive(false);
            throw new NullReferenceException("Place the node closer to the NavMesh.");
        }

        trigger = GetComponent<SphereCollider>();
        trigger.radius = triggerRadius;
        level = 1;
    }

    void Start()
    {
        //man = GameObject.Find("GameManager");

        if (!isStatic || !isTrigger)
        {
            trigger.enabled = false;
            spawnCounter = spawnInterval;

            for (int i = 0; i < enemyCount; i++)
            {
                GenerateEnemy();
            }

            if (!isStatic)
            {
                this.enabled = false;
            }
        }
    }

    void Update()
    {
        /*
        if (transform.childCount < enemyCount)
        {
            if (spawnCounter > 0)
            {
                spawnCounter -= Time.deltaTime;

                if (spawnCounter <= 0)
                {
                    if (!isTrigger)
                    {
                        GenerateEnemy();
                        spawnCounter = spawnInterval;
                    }

                    else
                    {
                        spawnCounter = 0;
                    }
                }
            }
        }
         */
    }

    void OnTriggerStay()
    {
        if (spawnCounter <= 0)
        {
            GenerateEnemy();
            spawnCounter = spawnInterval;
        }
    }

    private void GenerateEnemy()
    {

        
        
        Vector3 newPosition = transform.position + new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius), 0, UnityEngine.Random.Range(-spawnRadius, spawnRadius));

        NavMeshHit meshLocation;

        if (NavMesh.SamplePosition(newPosition, out meshLocation, SPAWN_RADIUS_MAX, 1 << LayerMask.NameToLayer("Default")))
        {
            
            GameObject newEnemy = Instantiate(enemyPrefab, meshLocation.position, Quaternion.identity) as GameObject;
            newEnemy.rigidbody.Sleep();
            newEnemy.name = "Enemy(" + newEnemy.GetInstanceID() + ")";
            newEnemy.transform.parent = transform;
            newEnemy.transform.Find("EnemyAggroCollider").gameObject.AddComponent<AggroRadius>();
            newEnemy.AddComponent<AIController>();

            Entity enemyEntity = newEnemy.GetComponent<Entity>();


            # region giving enemies stats and abilities
            
            enemyEntity.baseAtt = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EnemyStatFactory.MakeEnemyAttributes(level, enemytype);

            enemyEntity.UpdateCurrentAttributes();
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EnemyStatFactory.GiveEnemyAbilities(enemyEntity, enemytype);
            
            /*
            enemyEntity.abilityManager.abilities[0] = GameManager.Abilities["cleave"];
            enemyEntity.abilityManager.abilities[1] = GameManager.Abilities["hadouken"];

            enemyEntity.abilityIndexDict["cleave"] = 0;
            enemyEntity.abilityIndexDict["hadouken"] = 1;
            */

            #endregion

        }

        else
        {
            this.gameObject.SetActive(false);
            throw new NullReferenceException("Could not find a place to spawn enemy. Check node location.");
        }
    }

    public const int ENEMY_COUNT_MIN = 1;
    public const int ENEMY_COUNT_MAX = 20;

    public const float SPAWN_RADIUS_MIN = 2;
    public const float SPAWN_RADIUS_MAX = 12;

    public const float TRIGGER_RADIUS_MIN = 5;
    public const float TRIGGER_RADIUS_MAX = 50;

    public const int SPAWN_INTERVAL_MIN = 1;
    public const int SPAWN_INTERVAL_MAX = 60;
}


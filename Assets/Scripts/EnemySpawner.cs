using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour 
{
    public GameObject enemyPrefab;
    public SphereCollider trigger;

    public int enemyCount;
    public float spawnRadius;

    // Continuous generation
    public bool isStatic = false; // This determines if the spawner should generate 1 group or continuously spawn enemies over time.

    // Only visible if continuous generation is selected
    public bool isTrigger;
    public float triggerRadius; // The radius in which a player must be before to trigger generation
    public int spawnInterval; // The time in between spawns while a player is within the radius
    public float spawnCounter = 0;

    private GameObject Group;

    void Start()
    {
        Group = new GameObject();
        Group.name = "Enemy Group";
        Group.transform.parent = transform;
        Group.transform.position = transform.position;
        Group.AddComponent<AIGroupController>();
        
        trigger = gameObject.AddComponent<SphereCollider>();
        trigger.radius = triggerRadius;
        trigger.isTrigger = true;

        if (!isStatic || !isTrigger)
        {
            trigger.enabled = false;

            for (int i = 0; i < enemyCount; i++)
            {
                GenerateEnemy();
            }

            if (!isStatic)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    void Update()
    {
        if (Group.transform.childCount < enemyCount)
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
        Vector3 newPosition = transform.position + new Vector3(UnityEngine.Random.Range(SPAWN_INTERVAL_MIN, spawnRadius), 0, UnityEngine.Random.Range(SPAWN_INTERVAL_MIN, spawnRadius));

        NavMeshHit meshLocation;

        if (NavMesh.SamplePosition(newPosition, out meshLocation, 30, 1 << LayerMask.NameToLayer("Default")))
        {
            GameObject newEnemy = Instantiate(enemyPrefab, newPosition, Quaternion.identity) as GameObject;
            newEnemy.name = "Enemy(" + newEnemy.GetInstanceID() + ")";
            newEnemy.transform.parent = Group.transform;

            newEnemy.transform.GetChild(0).gameObject.AddComponent<AggroRadius>();
            newEnemy.AddComponent<AIController>();
        }

        else
        {
            this.gameObject.SetActive(false);
            throw new NullReferenceException("Move the spawn node closer to the NavMesh");
        }
    }

    /*
    public void GenerateEnemies(Vector3 playerPosition)
    {
        int numberOfGroups = 5;
        float groupRadius = 10;

        for (int i = 0; i < numberOfGroups; i++)
        {
            float randomRadius = UnityEngine.Random.Range(15, 50);
            Vector2 spawnCircle = UnityEngine.Random.insideUnitCircle * randomRadius;
            
            NavMeshHit meshHit;

            if (NavMesh.SamplePosition(newPosition, out meshHit, 30, 1 << LayerMask.NameToLayer("Default")))
            {
                if (meshHit.hit)
                {
                    meshHit.position = transform.position;
                    Collider[] nearbyObjects = Physics.OverlapSphere(meshHit.position, groupRadius, 1 << LayerMask.NameToLayer("Not Walkable"));

                    if (nearbyObjects.Length > 0)
                    {
                        i--;
                    }

                    else
                    {
                        GameObject enemyGroup = new GameObject();
                        enemyGroup.name = "Group " + (i + 1);
                        enemyGroup.AddComponent<AIGroupController>();

                        int enemyCount = UnityEngine.Random.Range(3, 6);

                        for (int j = 0; j < enemyCount; j++)
                        {
                            Vector3 internalLocation = UnityEngine.Random.insideUnitSphere * groupRadius;
                            internalLocation = meshHit.position + new Vector3(internalLocation.x, meshHit.position.y, internalLocation.z);

                            GameObject newEnemy = Instantiate(enemyPrefab, internalLocation, Quaternion.identity) as GameObject;
                            newEnemy.name = "Enemy " + (j + 1) + ", Group " + (i + 1);
                            newEnemy.transform.parent = enemyGroup.transform;

                            newEnemy.transform.GetChild(0).gameObject.AddComponent<AggroRadius>();
                            newEnemy.AddComponent<AIController>();
                        }
                    }
                }
            }

            else
            {
                i--;
            }
        }
    }
    */

    public const int ENEMY_COUNT_MIN = 1;
    public const int ENEMY_COUNT_MAX = 20;

    public const float SPAWN_RADIUS_MIN = 2;
    public const float SPAWN_RADIUS_MAX = 12;

    public const float TRIGGER_RADIUS_MIN = 5;
    public const float TRIGGER_RADIUS_MAX = 50;

    public const int SPAWN_INTERVAL_MIN = 1;
    public const int SPAWN_INTERVAL_MAX = 60;
}


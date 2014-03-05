using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour 
{
    public GameObject enemyPrefab;

    public int enemyCount;
    public float spawnRadius;

    // Continuous generation
    public bool isStatic = false; // This determines if the spawner should generate 1 group or continuously spawn enemies over time.

    // Only visible if continuous generation is selected
    public bool isTrigger;
    public float triggerRadius; // The radius in which a player must be before to trigger generation
    public int spawnInterval; // The time in between spawns while a player is within the radius
    public float spawnCounter = 0;

    void Start()
    {
        SphereCollider trigger = GetComponent<SphereCollider>();
        trigger.radius = triggerRadius;

        if (!isStatic || !isTrigger)
        {
            Destroy(trigger);

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

        if (NavMesh.SamplePosition(newPosition, out meshLocation, 30, 1 << LayerMask.NameToLayer("Default")))
        {
            GameObject newEnemy = Instantiate(enemyPrefab, newPosition, Quaternion.identity) as GameObject;
            newEnemy.name = "Enemy(" + newEnemy.GetInstanceID() + ")";
            newEnemy.transform.parent = transform;

            newEnemy.transform.GetChild(0).gameObject.AddComponent<AggroRadius>();
            newEnemy.AddComponent<AIController>();
        }

        else
        {
            this.gameObject.SetActive(false);
            throw new NullReferenceException("Move the enemy node closer to the NavMesh");
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


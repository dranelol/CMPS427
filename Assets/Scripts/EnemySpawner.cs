using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour 
{
    private SphereCollider trigger;

    public float triggerRadius;
    public float spawnRadius;
    public int level;

    public int _resources;
    public int _maxCount;
    public int _maxCost;
    public int _minCost;

    private bool HasSpawned = false;

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
        trigger.radius = triggerRadius = Mathf.Clamp(triggerRadius, TRIGGER_RADIUS_MIN, TRIGGER_RADIUS_MAX);
        spawnRadius = Mathf.Clamp(spawnRadius, SPAWN_RADIUS_MIN, SPAWN_RADIUS_MAX);
        level = 1;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!HasSpawned)
        {
            if (other.tag == "Player" && !other.GetComponent<Entity>().IsDead())
            {
                level = other.GetComponent<PlayerEntity>().Level;

                List<GameObject> enemies = EnemyAttributeFactory.GetEnemies(_resources, _maxCount, _maxCost, _minCost);
                StartCoroutine(GenerateEnemies(enemies));
                HasSpawned = true;
            }
        }
    }

    IEnumerator GenerateEnemies(List<GameObject> enemyPrefabs)
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            Vector3 newPosition = transform.position + new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius), 0, UnityEngine.Random.Range(-spawnRadius, spawnRadius));

            NavMeshHit meshLocation;

            if (NavMesh.SamplePosition(newPosition, out meshLocation, SPAWN_RADIUS_MAX, 1 << LayerMask.NameToLayer("Default")))
            {
                GameObject newEnemy = Instantiate(enemyPrefabs[i], meshLocation.position, Quaternion.identity) as GameObject;

                newEnemy.transform.parent = transform;

                newEnemy.transform.Find("EnemyAggroCollider").gameObject.AddComponent<AggroRadius>();
                newEnemy.AddComponent<AIController>();
                Entity enemyEntity = newEnemy.GetComponent<Entity>();
                EnemyBaseAtts enemyBase = newEnemy.GetComponent<EnemyBaseAtts>();

                enemyEntity.SetLevel(level);
                enemyBase.InitializeStats(level);
                enemyBase.SetAbilities();
            }

            else
            {
                this.gameObject.SetActive(false);
                Debug.LogError("Could not find a place to spawn enemy. Check node location.");
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public const float SPAWN_RADIUS_MIN = 5;
    public const float SPAWN_RADIUS_MAX = 10;

    public const float TRIGGER_RADIUS_MIN = 20;
    public const float TRIGGER_RADIUS_MAX = 50;
}


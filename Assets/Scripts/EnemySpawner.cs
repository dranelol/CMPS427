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

    //If we're spawning a group of enemies only once, this'll stop it from happening again.
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
        trigger.radius = triggerRadius;
        level = 1;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!HasSpawned)
        {
            if (other.tag == "Player")
            {
                level = other.GetComponent<PlayerEntity>().Level;

                List<GameObject> enemies = EnemyAttributeFactory.GetEnemies(_resources, _maxCount, _maxCost, _minCost);//enemyattributefacory get enemies stuff

                for (int i = 0; i < enemies.Count; i++)
                {
                    GenerateEnemy(enemies[i]);
                }

                HasSpawned = true;
            }
        }
    }


    private void GenerateEnemy(GameObject enemydude)
    {
        Vector3 newPosition = transform.position + new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius), 0, UnityEngine.Random.Range(-spawnRadius, spawnRadius));

        NavMeshHit meshLocation;

        if (NavMesh.SamplePosition(newPosition, out meshLocation, SPAWN_RADIUS_MAX, 1 << LayerMask.NameToLayer("Default")))
        {

            GameObject newEnemy = Instantiate(enemydude, meshLocation.position, Quaternion.identity) as GameObject;
            newEnemy.name = "Enemy(" + newEnemy.GetInstanceID() + ")";
            newEnemy.transform.parent = transform;
            newEnemy.transform.Find("EnemyAggroCollider").gameObject.AddComponent<AggroRadius>();
            newEnemy.AddComponent<AIController>();

            Entity enemyEntity = newEnemy.GetComponent<Entity>();


            # region giving enemies stats and abilities

            enemyEntity.SetLevel(level);
            enemyEntity.GetComponent<EnemyBaseAtts>().InitializeStats();
            enemyEntity.GetComponent<EnemyBaseAtts>().SetAbilities();  
            
            enemyEntity.UpdateCurrentAttributes();
            //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EnemyStatFactory.GiveEnemyAbilities(enemyEntity, enemytype);

            //enemyEntity.SetLevel(level);


            //enemyEntity.abilityManager.abilities[0] = GameManager.Abilities["cleave"];
            //enemyEntity.abilityManager.abilities[1] = GameManager.Abilities["hadouken"];


            //enemyEntity.abilityIndexDict["cleave"] = 0;
            //enemyEntity.abilityIndexDict["hadouken"] = 1;


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


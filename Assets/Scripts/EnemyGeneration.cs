using UnityEngine;
using System.Collections;

public class EnemyGeneration : MonoBehaviour 
{
    public GameObject enemyPrefab;
    public GameObject player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GenerateEnemies(player.transform.position);
        }
    }

    public void GenerateEnemies(Vector3 playerPosition)
    {
        int numberOfGroups = 5;
        float groupRadius = 10;

        for (int i = 0; i < numberOfGroups; i++)
        {
            float randomRadius = UnityEngine.Random.Range(15, 50);
            Vector2 spawnCircle = UnityEngine.Random.insideUnitCircle * randomRadius;
            Vector3 newPosition = playerPosition + new Vector3(spawnCircle.x, playerPosition.y, playerPosition.z);
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
}

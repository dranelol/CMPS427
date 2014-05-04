using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class EnemyAttributeFactory : MonoBehaviour
{
    #region Constants

    public const int MIN_ENEMY_COST = 1;
    public const int MAX_ENEMY_COST = 20;

    public const int MIN_NODE_RESOURCES = 1;
    public const int MAX_NODE_RESOURCES = 30;

    public const int MIN_NODE_COUNT = 1;
    public const int MAX_NODE_COUNT = 10;

    public const float MIN_PERCENT_RESOURCES = 0.75f;

    public const string PREFAB_FOLDER_PATH = "Enemy Prefabs/";

    #endregion

    #region Sub-Classes

    private class EnemyType
    {
        #region Properties

        private int _cost;
        public int Cost
        {
            get { return _cost; }
        }

        private GameObject _prefab;
        public GameObject Prefab
        {
            get { return _prefab; }
        }

        #endregion

        #region Constructors

        public EnemyType(string name, int cost)
        {
            _prefab = (GameObject)Resources.Load(PREFAB_FOLDER_PATH + name, typeof(GameObject));

            if (_prefab == null)
            {
                throw new ArgumentException("Cannot find prefab with name " + name + ".");
            }

            _cost = Mathf.Clamp(cost, MIN_ENEMY_COST, MAX_ENEMY_COST);
        }

        #endregion

        #region Methods

        public EnemyType Copy()
        {
            return (EnemyType)this.MemberwiseClone();
        }

        #endregion
    }

    #endregion

    #region Properties

    // The master list containing all of the possible enemies.
    private static List<EnemyType> EnemyList;

    // The costs for each type of enemy.
    public int _ogreCost = 10;

    #endregion

    public void Awake()
    {
        EnemyList = new List<EnemyType>();

        EnemyList.Add(new EnemyType("OgreEnemy", _ogreCost)); // Add each type of prefab to the master list.
    }

    public static List<GameObject> GetEnemies(int resources, int maxCount, int maxCost, int minCost)
    {
        resources = Mathf.Clamp(resources, MIN_NODE_RESOURCES, MAX_NODE_RESOURCES);
        maxCount = Mathf.Clamp(maxCount, MIN_NODE_COUNT, MAX_NODE_COUNT);
        maxCost = Mathf.Clamp(maxCost, MIN_ENEMY_COST, MAX_ENEMY_COST);
        minCost = Mathf.Clamp(minCost, MIN_ENEMY_COST, maxCost);

        int resourceCutoff = (int)Math.Ceiling(UnityEngine.Random.Range((float)resources * MIN_PERCENT_RESOURCES, (float)resources));

        List<EnemyType> enemyPool = new List<EnemyType>(); // The list of possible enemies to spawn.

        foreach (EnemyType enemy in EnemyList)
        {
            if (enemy.Cost >= minCost && enemy.Cost <= maxCost) // Copy the list but exclude enemies that are not within the cost range
            {
                enemyPool.Add(enemy.Copy());
            }
        }

        enemyPool = enemyPool.OrderBy(EnemyType => EnemyType.Cost).ToList();

        List<GameObject> spawnList = new List<GameObject>();

        while (enemyPool.Count > 0 && spawnList.Count <= maxCount && resources > resourceCutoff)
        {
            if (enemyPool.Last().Cost > resources)
            {
                enemyPool.RemoveAt(EnemyList.Count - 1);
                continue;
            }

            else
            {
                EnemyType randomEnemy = enemyPool[UnityEngine.Random.Range(0, EnemyList.Count)];
                spawnList.Add(randomEnemy.Prefab);
                resources = Mathf.Max(0, resources - randomEnemy.Cost);
            }
        }

        return spawnList;
    }
}

using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class EnemyAttributeFactory : MonoBehaviour
{
    #region oldcode with statblocks
	
    /*
    #region critter stats
    public Attributes critterbase;
    public float critterscalingHP = 2.5f;
    public float critterscalingresource = 5f;
    public float critterscalingpower = 2f;
    public float critterscalingdefense = 2f;
    public float critterscalingmindmg = .5f;
    public float critterscalingmaxdmg = .5f;
    #endregion

    #region small stats
    public Attributes smallbase;
    public float smallscalingHP = 5f;
    public float smallscalingresource = 10f;
    public float smallscalingpower = 4f;
    public float smallscalingdefense = 4f;
    public float smallscalingmindmg = .75f;
    public float smallscalingmaxdmg = .75f;
	
     */
		
	
    #endregion
	
    #region Constants
	
    public const int MIN_ENEMY_COST = 1;
    public const int MAX_ENEMY_COST = 15;

    public const int MIN_NODE_RESOURCES = 1;
    public const int MAX_NODE_RESOURCES = 60;

    public const int MIN_NODE_COUNT = 1;
    public const int MAX_NODE_COUNT = 10;

    public const float MIN_PERCENT_RESOURCES = 0.25f;

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
    public int _ogreCost = 12;
    public int _ghostCost = 15;
    public int _trollCost = 9;
    public int _treeEntCost = 10;
    public int _orcCost = 6;
    public int _undeadCost = 4;
    public int _goblinCost = 2;

    #endregion

    public void Awake()
    {
        EnemyList = new List<EnemyType>();

        EnemyList.Add(new EnemyType("OgreEnemy", _ogreCost)); // Add each type of prefab to the master list.
        EnemyList.Add(new EnemyType("GhostEnemy", _ghostCost));
        EnemyList.Add(new EnemyType("TrollEnemy", _trollCost));
        EnemyList.Add(new EnemyType("TreeEntEnemy", _treeEntCost));
        EnemyList.Add(new EnemyType("OrcEnemy", _orcCost));
        EnemyList.Add(new EnemyType("UndeadEnemy", _undeadCost));
        EnemyList.Add(new EnemyType("GoblinEnemy", _goblinCost));
    }

    public static List<GameObject> GetEnemies(int resources, int maxCount, int maxCost, int minCost)
    {
        resources = Mathf.Clamp(resources, MIN_NODE_RESOURCES, MAX_NODE_RESOURCES);
        maxCount = Mathf.Clamp(maxCount, MIN_NODE_COUNT, MAX_NODE_COUNT);
        maxCost = Mathf.Clamp(maxCost, MIN_ENEMY_COST, MAX_ENEMY_COST);
        minCost = Mathf.Clamp(minCost, MIN_ENEMY_COST, maxCost);

        int resourceCutoff = (int)Math.Ceiling(UnityEngine.Random.Range(1f, (float)resources * MIN_PERCENT_RESOURCES));

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

        while (enemyPool.Count > 0 && maxCount > spawnList.Count  && resources > resourceCutoff)
        {
            if (enemyPool.Last().Cost > resources)
            {
                enemyPool.RemoveAt(enemyPool.Count - 1);
                continue;
            }

            else
            {
                int i = UnityEngine.Random.Range((int)0, (int)enemyPool.Count);
                EnemyType randomEnemy = enemyPool[i];
                spawnList.Add(randomEnemy.Prefab);
                resources = Mathf.Max(0, resources - randomEnemy.Cost);
            }
        }

		return spawnList;
	}
}

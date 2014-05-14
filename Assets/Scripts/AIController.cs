using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AIController : StateMachine 
{
    private class Hostile
    {
        private GameObject target; // The player the enemy is currently pursuing
        private float level; // The level of the player
        private float threat; // The current amount of threat the target object has

        public Hostile(GameObject newTarget)
        {
            target = newTarget;
            level = 1;
            threat = 0;
        }

        public GameObject Target
        {
            get { return target; }
        }

        public float Level
        {
            get { return level; }
        }

        public float Threat
        {
            get { return threat; }
        }

        public void Threaten(float magnitude)
        {
            threat += magnitude;
        }
    }

    public enum AIStates
    {
        idle,
        pursuit,
        dead,
        reset,
        wander
    }

    // Script references
    private AIGroupController Group; // The script managing the group of enemies
    private AggroRadius Aggro; // The script managing the aggro
    private MovementFSM MoveFSM; // The Movement FSM the enemy uses
    private AIPursuit PursuitFSM; // The script that managers AI behavior when pursuing a target
    private Entity EntityObject; // our entity object
    private AnimationController _animationController;
    // Reset variables
    public Vector3 localHomePosition; // The position around the home position this unit returns to upon reset

    // Target variables
    private Dictionary<GameObject, Hostile> ThreatTable; // A dictionary of all threat targets
    private GameObject target; // The Target object that holds information about the current target
    private EntitySoundManager _soundManager;

    private float wanderInterval; //Time between wanders in seconds
    private float wanderDistance; //Radius around the wanderer that it will travel
    private float nextWander;    //Time the next wander will take place.
    private float wanderDistanceFromNode; //Max distance the enemy will wander away from the node.
    private Vector3 nodePosition; //Position of the EnemyNode
    public bool doesWander;

    public float aggroRadius;

    void Awake()
    {
        Group = transform.parent.GetComponent<AIGroupController>();
        Aggro = GetComponentInChildren<AggroRadius>();
        MoveFSM = GetComponent<MovementFSM>();
        EntityObject = GetComponent<Entity>();
        _animationController = GetComponent<AnimationController>();

        SetupMachine(AIStates.idle);

        HashSet<Enum> idleTransitions = new HashSet<Enum>();
        idleTransitions.Add(AIStates.pursuit);
        idleTransitions.Add(AIStates.wander);

        HashSet<Enum> pursuitTransitions = new HashSet<Enum>();
        pursuitTransitions.Add(AIStates.dead);
        pursuitTransitions.Add(AIStates.reset);

        HashSet<Enum> resetTransitions = new HashSet<Enum>();
        resetTransitions.Add(AIStates.idle);

        HashSet<Enum> wanderTransitions = new HashSet<Enum>();
        wanderTransitions.Add(AIStates.idle);

        AddTransitionsFrom(AIStates.idle, idleTransitions);
        AddTransitionsFrom(AIStates.pursuit, pursuitTransitions);
        AddTransitionsFrom(AIStates.reset, resetTransitions);
        AddTransitionsFrom(AIStates.wander, wanderTransitions);

        StartMachine(AIStates.idle);
        _soundManager = GetComponent<EntitySoundManager>();
    }

	void Start() 
    {
        PursuitFSM = GetComponent<AIPursuit>();
        localHomePosition = transform.position;

        wanderDistance = 5.0f;
        wanderInterval = 5.0f;
        wanderDistanceFromNode = 7.0f;

        nodePosition = new Vector3(transform.parent.position.x, transform.position.y, transform.parent.position.z);

        nextWander = Time.time + wanderInterval;

        ThreatTable = new Dictionary<GameObject, Hostile>();
        target = null;
    }

    #region public functions

    /// <summary>
    /// Apply threat to the individual enemy. The enemy always attacks the
    /// target with the highest threat.
    /// </summary>
    /// <param name="source">The source of the threat.</param>
    /// <param name="magnitude">The amount of threat to apply.</param>
    public void Threat(GameObject source, float magnitude = 0)
    {
        if ((AIStates)CurrentState != AIStates.dead && (AIStates)CurrentState != AIStates.reset && source != null)
        {
            if (source.tag == "Player")
            {
                if (!ThreatTable.ContainsKey(source))
                {
                    Hostile newTargetObject = new Hostile(source);

                    ThreatTable.Add(source, newTargetObject);
                    Group.Threat(source);

                    if ((AIStates)CurrentState == AIStates.idle)
                    {
                        target = source;
                        Transition(AIStates.pursuit);
                    }
                }

                ThreatTable[source].Threaten(magnitude);

                if (ThreatTable[source].Threat > ThreatTable[target].Threat)
                {
                    target = source;
                    PursuitFSM.Pursue(target);
                }
                
            }

            else
            {
                throw new InvalidOperationException("Threat cannot be applied by a non-player GameObject. Dumbass.");
            }
        }
    }


    /// <summary>
    /// Tells the enemy's group to attack the attacker
    /// </summary>
    /// <param name="attacker">The entity attacking.</param>
    public void BeenAttacked(GameObject attacker)
    {
        Group.Threat(attacker, 1);
    }

    public Vector3 homeNodePosition
    {
        get { return Group.HomePosition; }
    }

    /// <summary>
    /// Return the current target.
    /// </summary>
    public GameObject Target
    {
        get { return target; }
    }

    public bool IsDead()
    {
        return (AIStates)CurrentState == AIStates.dead;
    }

    public bool IsResetting()
    {
        return (AIStates)CurrentState == AIStates.reset;
    }

    public bool IsInCombat()
    {
        return (AIStates)CurrentState == AIStates.pursuit;
    }


    #endregion

    #region private functions

    private bool TargetInRange()
    {
        if (target != null)
        {
            return CombatMath.DistanceLessThan(Group.transform.position, target.transform.position, Group.ResetDistance);
        }

        else
        {
            return false;
        }
    }

    public void RemoveTarget(GameObject lostTarget)
    {
        if ((AIStates)CurrentState == AIStates.pursuit)
        {
            if (ThreatTable.Remove(lostTarget))
            {
                if (target == lostTarget)
                {
                    if (ThreatTable.Count == 0)
                    {
                        target = Group.GetNewTarget();

                        if (target != null)
                        {
                            Hostile newTargetObject = new Hostile(target);
                            ThreatTable.Add(target, newTargetObject);
                        }
                    }

                    else
                    {
                        GameObject highestThreatTarget = ThreatTable.Keys.First();

                        foreach (GameObject targetInTable in ThreatTable.Keys)
                        {
                            if (ThreatTable[targetInTable].Threat > ThreatTable[highestThreatTarget].Threat)
                            {
                                highestThreatTarget = targetInTable;
                            }
                        }

                        target = highestThreatTarget;
                    }

                    PursuitFSM.Pursue(target);
                }
            }
        }
    }

    /// <summary>
    /// Initiates the wander behaviour
    /// </summary>
    /// <param name="currentPosition">Current location of the enemy</param>
    /// <param name="centerPosition">Position the path needs to be around</param>
    /// <param name="distance">How far to travel each wander.</param>
    /// <param name="distanceFromCenter">Max distance to travel from the center</param>
    private void Wander(Vector3 currentPosition, Vector3 centerPosition, float distance, float distanceFromCenter)
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * distance; // Pick a random point on the edge of the circle

        Vector3 targetPosition = new Vector3(randomDirection.x, 0, randomDirection.y);

        targetPosition += currentPosition;

        if (Vector3.Distance(centerPosition, targetPosition) > distanceFromCenter)
        {
            Vector3 newDirection = (centerPosition - currentPosition).normalized * distance;

            targetPosition = new Vector3(newDirection.x, 0, newDirection.y);
            targetPosition += currentPosition;
        }

        MoveFSM.WalkPath(targetPosition);
    }

    private void Reset()
    {
        if (IsValidTransition((AIStates)CurrentState, AIStates.reset) == true)
        {
            Transition(AIStates.reset);
        }
    }

    #endregion

    #region state based functions

    #region idle functions

    IEnumerator idle_EnterState()
    {
        _animationController.WalkToMove();
        Aggro.Trigger.enabled = true;
        yield break;
    }

    void idle_Update()
    {
        if (doesWander)
        {
            if (Time.time >= nextWander)
            {
                Wander(transform.position, nodePosition, wanderDistance, wanderDistanceFromNode);
                nextWander = Time.time + wanderInterval;
            }

            else if (Vector3.Distance(transform.position, GetComponent<NavMeshAgent>().destination) < GetComponent<NavMeshAgent>().stoppingDistance)
            {
                MoveFSM.Stop();
            }
        }

        else
        {
            _animationController.Sleep();
        }
    }

    IEnumerator idle_ExitState()
    {
        Aggro.Trigger.enabled = false;
        yield break;
    }

    #endregion

    #region pursuit functions

    IEnumerator pursuit_EnterState()
    {
        _animationController.RunToMove();
        _soundManager.Aggro();
        PursuitFSM.Pursue(target);
        yield break;
    }

    void pursuit_Update()
    {
        if (EntityObject.CurrentHP <=0.0f) // Check health for death || if (health <= 0)
        {
            if (target != null)
            {
                target.GetComponent<PlayerEntity>().GiveExperience(EntityObject.Experience);
            }

            Transition(AIStates.dead);
        }

        else
        {
            if (!TargetInRange() || target.GetComponent<Entity>().IsDead())
            {
                Group.RemoveTarget(target);
            }

            if (target == null)
            {
                Reset();
            }
        }
    }

    IEnumerator pursuit_ExitState()
    {
        // Make invincible
        PursuitFSM.StopPursuit();
        ThreatTable.Clear();
        yield break;
    }

    #endregion 

    #region reset functions

    IEnumerator reset_EnterState()
    {
        PursuitFSM.StopPursuit();
        _soundManager.Victor();
        MoveFSM.SetPath(localHomePosition);
        
        yield break;
    }

    void reset_Update()
    {
        if (CombatMath.DistanceLessThan(transform.position, localHomePosition, MoveFSM.Radius))
        {
            MoveFSM.Stop();
            Transition(AIStates.idle);
        }

        else
        {
            MoveFSM.SetPath(localHomePosition);
        }
    }

    #endregion

    #region dead functions

    IEnumerator dead_EnterState()
    {
        _soundManager.Death();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>().Experience += EntityObject.Level * 10;

        PursuitFSM.StopPursuit();
        MoveFSM.LockMovement(MovementFSM.LockType.ShiftLock);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Aggro.gameObject.SetActive(false);

        try
        {
            GetComponent<BossInfernal>().Death();
        }

        catch
        {
            _animationController.Death();
        }

        #region heal orb spawning
        GameObject healOrb = (GameObject)Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EnvironmentHealOrbProjectile, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);

        ProjectileBehaviour healOrbProjectile = healOrb.GetComponent<ProjectileBehaviour>();

        healOrbProjectile.EnvironmentProjectile = true;
        healOrbProjectile.homing = true;
        healOrbProjectile.speed = 10.0f;
        healOrbProjectile.timeToActivate = 5.0f;
        healOrbProjectile.owner = gameObject;
        

        Vector3 randPosition = transform.position + UnityEngine.Random.onUnitSphere*3;
        Vector3 randDirection = (randPosition - transform.position).normalized;
        randPosition.Set(randPosition.x, 0, randPosition.z);
        healOrbProjectile.target = randPosition;

        healOrbProjectile.transform.rotation = Quaternion.LookRotation(randDirection);
        #endregion

        #region loot spawning

        //roll to see if a chest spawns
        
        float chestroll = UnityEngine.Random.Range(0f, 1f);
        Debug.Log("doing loot: Rolled a " + chestroll);
        if (chestroll <= gameObject.GetComponent<EnemyBaseAtts>().LootDropChance)
        {

            //find a valid point on the navmesh for the chest
            Vector3 newPosition = transform.position + new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1));

            NavMeshHit meshLocation;

            if (NavMesh.SamplePosition(newPosition, out meshLocation, 1, 1 << LayerMask.NameToLayer("Default")))
            {

                GameObject chest = Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LootChestPrefab, meshLocation.position, Quaternion.identity) as GameObject;

                int diceroll = UnityEngine.Random.Range(gameObject.GetComponent<EnemyBaseAtts>().MinLootDrops, gameObject.GetComponent<EnemyBaseAtts>().MaxLootDrops + 1);

                for (int i = 0; i < diceroll; i++)
                {
                    chest.GetComponentInChildren<LootTrigger>().Inventory.AddItem(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipmentByLevel(EntityObject.Level));
                    //chest.GetComponentInChildren<LootTrigger>().Inventory.AddItem(new equipment());
                }
            }
        }



        #endregion


        yield return new WaitForSeconds(5.0f);


        #region chest spawning

        #endregion

        #region fading


        #endregion

        

        #region cleanup and destroy



        Destroy(gameObject);

        #endregion



        yield return null;
    }

    #endregion

    #region wander functions WANDER IS CURRENTLY BEING DONE IN IDLE

    /*
    void wander_Update()
    {        
        if (Time.time >= nextWander)
        {
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * wanderDistance; // Pick a random point on the edge of the circle

            Vector3 targetPosition = new Vector3(randomDirection.x, 0, randomDirection.y);



            targetPosition += transform.position;
            targetPosition.y = transform.position.y;

            //transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

            Debug.DrawRay(transform.position, targetPosition - transform.position, Color.blue); // Draw vector to target position

            Debug.Log(targetPosition.ToString());

            //Debug.Log("world target: " + transform.TransformPoint(targetPosition).ToString());



            MoveFSM.SetPath(targetPosition);

            //NavAgent.SetDestination(targetPosition);

            nextWander = Time.time + wanderInterval;
        }
    }*/

    #endregion 

    #endregion
}

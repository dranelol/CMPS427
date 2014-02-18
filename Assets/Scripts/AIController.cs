using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AIController : StateMachine 
{
    private class TargetObject
    {
        private GameObject target; // The player the enemy is currently pursuing
        private float levelFactor; // A factor determined by the difference in levels of the enemy and its target
        private float targetThreat; // The current amount of threat the target object has

        public TargetObject(GameObject newTarget)
        {
            target = newTarget;
            levelFactor = baseLevelFactor;
            targetThreat = 0;
            //levelFactor = math
        }

        public GameObject Target
        {
            get { return target; }
        }

        public float LevelFactor
        {
            get { return levelFactor; }
        }

        public float TargetThreat
        {
            get { return targetThreat; }
        }

        public void Threaten(float magnitude)
        {
            targetThreat += magnitude;
        }
    }

    public enum AIStates
    {
        idle,
        pursuit,
        dead,
        reset,
    }

    private const float baseLevelFactor = 0.5f; // The level factor when the enemy is the same level as its target
    private const float levelFactorRange = 0.5f; // The range between the min and max possible level factors
    private const float maxDistanceFromTargetPositionToTarget = 10; // The distance before position recalculation

    private AIGroupController Group; // The script managing the group of enemies
    private AggroRadius Aggro; // The script managing the aggro
    private MovementFSM MoveFSM; // The Movement FSM the enemy uses
    private NavMeshAgent NavAgent; // NavMeshAgent for this enemy

    private Dictionary<GameObject, TargetObject> ThreatTable; // A dictionary of all threat targets
    private GameObject target; // The Target object that holds information about the current target
    private Vector3 targetPosition; // The target world position to move to while pursuing the player

    public Vector3 localHomePosition; // The position around the home position this unit returns to

    private float attackRange = 20; // The range the enemy must be within in order to attack

    private const float fleeTime = 5; // The time until the enemy will stop fleeing and resume pursuit
    private float timeFled; // The time fleeing started
    public bool canFlee = true; // True if the enemy will flee, false otherwise
    public bool hasFled; // True if the enemy has fled this combat
    
    private float resetDistanceDelta; // The change in pursuit distance each time the enemy is attacked

    private bool alive = true; // Enemy state

    void Awake()
    {
        Group = transform.parent.GetComponent<AIGroupController>();
        Aggro = GetComponentInChildren<AggroRadius>();
        MoveFSM = GetComponent<MovementFSM>();
        NavAgent = GetComponent<NavMeshAgent>();
    }

	void Start() 
    {
        ThreatTable = new Dictionary<GameObject, TargetObject>();
        target = null;
        timeFled = 0;
        hasFled = false;
        localHomePosition = Group.HomePosition;
        resetDistanceDelta = Group.BaseResetDistance;

        GetComponent<NavMeshAgent>().updatePosition = true;
        GetComponent<NavMeshAgent>().updateRotation = true;

        List<Enum> idleTransitions = new List<Enum>();
        idleTransitions.Add(AIStates.pursuit);

        List<Enum> pursuitTransitions = new List<Enum>();
        pursuitTransitions.Add(AIStates.dead);
        pursuitTransitions.Add(AIStates.reset);

        List<Enum> resetTransitions = new List<Enum>();
        resetTransitions.Add(AIStates.idle);

        Transitions.Add(AIStates.idle, idleTransitions);
        Transitions.Add(AIStates.pursuit, pursuitTransitions);
        Transitions.Add(AIStates.reset, resetTransitions);

        StartMachine(AIStates.idle);
    }

    #region public functions

    /// <summary>
    /// Used to apply threat.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="magnitude"></param>
    public void Threat(GameObject source, float magnitude = 0)
    {
        if (source.tag == "Player")
        {

            if ((AIStates)CurrentState == AIStates.idle)
            {
                Transition(AIStates.pursuit);
                Group.BeginCombat(source);
            }

            if (!ThreatTable.ContainsKey(source))
            {
                TargetObject newTarget = new TargetObject(source);

                ThreatTable.Add(source, newTarget);
                Group.BeginCombat(source); // If a new target is found, add it to every enemy's threat table with 0 threat
            }

            ThreatTable[source].Threaten(magnitude);

            if (target == null || ThreatTable[source].TargetThreat > ThreatTable[target].TargetThreat)
            {
                target = source;
            }

            resetDistanceDelta *= ThreatTable[source].LevelFactor;
            Group.ResetDistance = resetDistanceDelta;
        }

        else
        {
            throw new InvalidOperationException("Threat cannot be applied by a non-player GameObject. Dumbass.");
        }
    }

    public void RemoveTarget(GameObject lostTarget)
    {
        if (ThreatTable.Remove(lostTarget))
        {
            GameObject highestThreatTarget = null;

            foreach (GameObject targetInTable in ThreatTable.Keys)
            {
                if (ThreatTable[targetInTable].TargetThreat > ThreatTable[target].TargetThreat)
                {
                    highestThreatTarget = targetInTable;
                }
            }

            target = highestThreatTarget;
        }
    }

    public void Reset()
    {
        if (CheckTransition(AIStates.reset))
        {
            Transition(AIStates.reset);
        }
    }

    public GameObject Target
    {
        get { return target; }
    }

    public bool Alive
    {
        get { return alive; }
    }

    #endregion

    #region private functions

    private bool Pursue()
    {
        return (Vector3.Distance(Group.transform.position, transform.position) < Group.ResetDistance);
    }

    private void Fight()
    {
        MoveFSM.SetPath(target.transform.position);
    }

    private bool CheckTransition(AIStates state)
    {
        try
        {
            return Transitions[CurrentState].Contains(state);
        }

        catch
        {
            return false;
        }
    }

    #endregion

    #region state based functions

    #region idle functions

    IEnumerator idle_EnterState()
    {
        Aggro.AggroTrigger.enabled = true;
        yield return null;
    }

    IEnumerator idle_ExitState()
    {
        Aggro.AggroTrigger.enabled = false;
        yield return null;
    }

    #endregion

    #region pursuit functions

    void pursuit_Update()
    {
        if (false) // Check health for death || if (health <= 0)
        {
            Transition(AIStates.dead);
        }

        else if (ThreatTable.Count == 0 || !Pursue())
        {
            Group.EndCombat();
        }

        else
        {
            Fight();
        }
    }

    #endregion 

    #region dead functions

    IEnumerator dead_EnterState()
    {
        alive = false;
        yield return null;
    }

    #endregion

    #region reset functions

    IEnumerator reset_EnterState()
    {
        MoveFSM.SetPath(localHomePosition);
        ThreatTable.Clear();
        // Make invincible
        hasFled = false;
        resetDistanceDelta = Group.BaseResetDistance;
        yield return null;
    }

    void reset_Update()
    {
        if (Vector3.Distance(transform.position, localHomePosition) < 1)
        {
            MoveFSM.Stop();
            Transition(AIStates.idle);
        }

        else
        {
            if (NavAgent.destination != localHomePosition)
            {
                MoveFSM.SetPath(localHomePosition);
            }
        }
    }

    #endregion

    #endregion
}

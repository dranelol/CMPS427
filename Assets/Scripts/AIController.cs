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
    }

    // Script references
    private AIGroupController Group; // The script managing the group of enemies
    private AggroRadius Aggro; // The script managing the aggro
    private MovementFSM MoveFSM; // The Movement FSM the enemy uses
    private NavMeshAgent NavAgent; // NavMeshAgent for this enemy
    private AIPursuit PursuitFSM; // The script that managers AI behavior when pursuing a target
    private Entity EntityObject; // our entity object
    // Reset variables
    public Vector3 localHomePosition; // The position around the home position this unit returns to upon reset

    // Target variabels
    private Dictionary<GameObject, Hostile> ThreatTable; // A dictionary of all threat targets
    private GameObject target; // The Target object that holds information about the current target

    void Awake()
    {
        Group = transform.parent.GetComponent<AIGroupController>();
        Aggro = GetComponentInChildren<AggroRadius>();
        MoveFSM = GetComponent<MovementFSM>();
        NavAgent = GetComponent<NavMeshAgent>();
        EntityObject = GetComponent<Entity>();
    }

	void Start() 
    {
        PursuitFSM = GetComponent<AIPursuit>();
        localHomePosition = transform.position;

        ThreatTable = new Dictionary<GameObject, Hostile>();
        target = null;

        SetupMachine(AIStates.idle);

        HashSet<Enum> idleTransitions = new HashSet<Enum>();
        idleTransitions.Add(AIStates.pursuit);

        HashSet<Enum> pursuitTransitions = new HashSet<Enum>();
        pursuitTransitions.Add(AIStates.dead);
        pursuitTransitions.Add(AIStates.reset);

        HashSet<Enum> resetTransitions = new HashSet<Enum>();
        resetTransitions.Add(AIStates.idle);

        AddTransitionsFrom(AIStates.idle, idleTransitions);
        AddTransitionsFrom(AIStates.pursuit, pursuitTransitions);
        AddTransitionsFrom(AIStates.reset, resetTransitions);

        StartMachine(AIStates.idle);
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
        if ((AIStates)CurrentState != AIStates.dead && (AIStates)CurrentState != AIStates.reset)
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
    #endregion

    #region private functions

    private bool TargetInRange()
    {
        if (target != null)
        {
            return Vector3.Distance(Group.transform.position, target.transform.position) < Group.ResetDistance;
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
        Aggro.Trigger.enabled = true;
        yield break;
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
        PursuitFSM.Pursue(target);
        yield break;
    }

    void pursuit_Update()
    {
        if (EntityObject.currentHP <=0.0f) // Check health for death || if (health <= 0)
        {
            Transition(AIStates.dead);
        }

        else
        {
            if (!TargetInRange())
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
        MoveFSM.SetPath(localHomePosition);
        yield break;
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

    #region dead functions

    IEnumerator dead_EnterState()
    {
        Destroy(GetComponent<CapsuleCollider>());
        PursuitFSM.StopPursuit();
        MoveFSM.LockMovement();
        yield return null;
    }

    #endregion

    #endregion

    
}

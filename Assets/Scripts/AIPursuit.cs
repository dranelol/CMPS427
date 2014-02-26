using UnityEngine;
using System.Collections.Generic;
using System;

public class AIPursuit : StateMachine 
{
    private const float stoppingDistance = 4; // The normal distance before position recalculation

    private enum PursuitStates
    {
        inactive,
        seek,
        attack,
    }

    private MovementFSM MoveFSM;
    private NavMeshAgent NavAgent;

    private GameObject currentTarget = null;

    private float attackRange = 4; // The range the enemy must be within in order to attack
    

    void Awake()
    {
        SetupMachine(PursuitStates.inactive);

        HashSet<Enum> inactiveTransitions = new HashSet<Enum>();
        inactiveTransitions.Add(PursuitStates.seek);
        AddTransitionsFrom(PursuitStates.inactive, inactiveTransitions);

        AddAllTransitionsFrom(PursuitStates.seek);
        AddAllTransitionsTo(PursuitStates.seek);
        AddAllTransitionsTo(PursuitStates.inactive);

        StartMachine(PursuitStates.inactive);

        MoveFSM = GetComponent<MovementFSM>();
        NavAgent = GetComponent<NavMeshAgent>();
    }

    public void Pursue(GameObject target)
    {
        currentTarget = target;

        if ((PursuitStates)CurrentState == PursuitStates.inactive)
        {
            Transition(PursuitStates.seek);
        }
    }

    public void Retarget(GameObject newTarget)
    {
        currentTarget = newTarget;

        if ((PursuitStates)CurrentState != PursuitStates.inactive)
        {
            Transition(PursuitStates.seek);
        }
    }

    public void StopPursuit()
    {
        currentTarget = null;
        Transition(PursuitStates.inactive);
    }

    #region transition functions

    #region inactive functions

    #endregion

    #region seek functions

    private void seek_EnterState()
    {
        MoveFSM.SetPath(currentTarget.transform.position);
    }

    private void seek_OnStay()
    {
        /*
        if (Vector3.Distance(transform.position, currentTarget.transform.position) > attackRange)
        {
            if (!NavAgent.hasPath || Vector3.Distance(transform.position, NavAgent.destination) < maxDistanceFromTargetPositionToTarget)
            {
                //NavAgent.avoidancePriority = 49;
                MoveFSM.SetPath(currentTarget.transform.position);
            }
        }

        else
        {
            
            Vector3 directionToTarget = currentTarget.transform.position - transform.position;
            RaycastHit hit;
            Physics.Raycast(transform.position, directionToTarget, out hit, 1 << LayerMask.NameToLayer("Player"));

            if (hit.transform.tag == "Player")
            {
                Debug.Log("I see player");
                // set to a higher priority, we want lower priority things to path around
                //NavAgent.avoidancePriority = 51;

                MoveFSM.Stop();
            }

            else
            {
                Debug.Log("I wanna get up in dat");
                //NavAgent.avoidancePriority = 50;

                if (!NavAgent.hasPath || Vector3.Distance(transform.position, NavAgent.destination) < minDistanceFromTargetPositionToTarget)
                {
                    MoveFSM.SetPath(currentTarget.transform.position);
                }
            }
        }

        if (Vector3.Distance(transform.position, currentTarget.transform.position) > attackRange)
        {
            MoveFSM.SetPath(currentTarget.transform.position);
        }

        else
        {
            Vector3 directionToTarget = currentTarget.transform.position - transform.position;
            RaycastHit hit;
            Physics.Raycast(transform.position, directionToTarget, out hit, 1 << LayerMask.NameToLayer("Player"));

            if (hit.transform.tag == "Player")
            {
                MoveFSM.Stop();
            }
        }
         */
    }

    #endregion

    #endregion
}

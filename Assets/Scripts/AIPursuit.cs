using UnityEngine;
using System.Collections;
using System;

public class AIPursuit : MonoBehaviour 
{
    /*
    private const float stoppingDistance = 4; // The normal distance before position recalculation

    private enum PursuitStates
    {
        inactive,
        seek,
        conservative,
        defensive,
        flee,
        aggressive,
        berserker,
        cheap,
    }

    private FSM<PursuitStates> PursuitFSM;
    private MovementFSM MoveFSM;
    private NavMeshAgent NavAgent;

    private GameObject currentTarget;

    private float attackRange = 4; // The range the enemy must be within in order to attack
    

    void Awake()
    {
        PursuitFSM = new FSM<PursuitStates>();

        PursuitFSM.AddTransitionsFromAToB(PursuitStates.inactive, PursuitStates.seek);

        PursuitStates[] listtest = new PursuitStates[3] { PursuitStates.defensive, PursuitStates.flee, PursuitStates.seek };

        //PursuitFSM.AddTransitionsFromAToB(PursuitStates.conservative, PursuitStates.flee, PursuitStates.defensive, PursuitStates.seek);

        PursuitFSM.AddTransitionsFromAToB(PursuitStates.conservative, listtest);
        PursuitFSM.AddTransitionsToAFromB(PursuitStates.conservative, PursuitStates.flee, PursuitStates.defensive, PursuitStates.seek, PursuitStates.aggressive);
 
        PursuitFSM.AddTransitionsFromAToB(PursuitStates.aggressive, PursuitStates.berserker, PursuitStates.cheap, PursuitStates.seek);
        PursuitFSM.AddTransitionsToAFromB(PursuitStates.aggressive, PursuitStates.berserker, PursuitStates.cheap, PursuitStates.seek, PursuitStates.conservative);

        PursuitFSM.AddTransitionsToAFromB(PursuitStates.inactive);

        PursuitFSM.AddTransitionBehavior(PursuitStates.inactive, null, inactive_OnStay, null);
        PursuitFSM.AddTransitionBehavior(PursuitStates.seek, seek_EnterState, seek_OnStay);

        PursuitFSM.Start(PursuitStates.inactive);

        MoveFSM = GetComponent<MovementFSM>();
        NavAgent = GetComponent<NavMeshAgent>();
    }

    public void Pursue(GameObject target)
    {
        currentTarget = target; 
        PursuitFSM.Transition(PursuitFSM.Current_State);
    }

    public void StopPursuit()
    {
        PursuitFSM.Transition(PursuitStates.inactive);
    }

    #region transition functions

    #region inactive functions

    private void inactive_OnStay()
    {
        PursuitFSM.Transition(PursuitStates.seek);
    }

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
    }

    #endregion

    #endregion
*/
}

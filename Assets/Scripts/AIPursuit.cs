using UnityEngine;
using System.Collections;
using System;

public class AIPursuit : MonoBehaviour 
{
    private const float maxDistanceFromTargetPositionToTarget = 4; // The distance before position recalculation

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

    private float attackRange = 20; // The range the enemy must be within in order to attack

    void Awake()
    {
        PursuitFSM = new FSM<PursuitStates>();

        PursuitFSM.AddTransitionsFromAToB(PursuitStates.inactive, PursuitStates.seek);

        PursuitFSM.AddTransitionsFromAToB(PursuitStates.conservative, PursuitStates.flee, PursuitStates.defensive, PursuitStates.seek);
        PursuitFSM.AddTransitionsToAFromB(PursuitStates.conservative, PursuitStates.flee, PursuitStates.defensive, PursuitStates.seek, PursuitStates.aggressive);
 
        PursuitFSM.AddTransitionsFromAToB(PursuitStates.aggressive, PursuitStates.berserker, PursuitStates.cheap, PursuitStates.seek);
        PursuitFSM.AddTransitionsToAFromB(PursuitStates.aggressive, PursuitStates.berserker, PursuitStates.cheap, PursuitStates.seek, PursuitStates.conservative);

        PursuitFSM.AddTransitionsToAFromB(PursuitStates.inactive);

        PursuitFSM.Start(PursuitStates.inactive);

        MoveFSM = GetComponent<MovementFSM>();
        NavAgent = GetComponent<NavMeshAgent>();
    }

    public void Pursue(GameObject target)
    {
        PursuitFSM.Transition(PursuitFSM.Current_State);
        currentTarget = target; 
    }

    public void StopPursuit()
    {
        PursuitFSM.Transition(PursuitStates.inactive);
    }

    #region transition functions

    #region seek functions

    private void seek_EnterState()
    {
        MoveFSM.SetPath(currentTarget.transform.position);
    }

    private void seek_OnStay()
    {
        if (Vector3.Distance(transform.position, NavAgent.destination) < maxDistanceFromTargetPositionToTarget)
        {
            if (Vector3.Distance(transform.position, currentTarget.transform.position) > 6)
            {
                MoveFSM.SetPath(currentTarget.transform.position);
            }
        }
    }

    #endregion

    #endregion
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class AIPursuit : StateMachine 
{
    private enum PursuitStates
    {
        inactive,
        seek,
        attack,
    }

    private MovementFSM MoveFSM;
    private NavMeshAgent NavAgent;

    public bool wuduriketomakingfuk = false;
    public bool katamarimdoe = false;

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
        if (target != null)
        {
            currentTarget = target;
            Transition(PursuitStates.seek);
        }
    }

    public void StopPursuit()
    {
        Transition(PursuitStates.inactive);
    }

    #region state based functions

    #region inactive functions

    IEnumerator inactive_EnterState()
    {
        currentTarget = null;
        yield break;
    }

    #endregion

    #region seek functions

    void seek_Update()
    {
        if (!wuduriketomakingfuk)
        {


            if (Vector3.Distance(transform.position, currentTarget.transform.position) < attackRange)
            {
                Vector3 directionToTarget = currentTarget.transform.position - transform.position;
                RaycastHit hit;
                bool raycastSuccess = Physics.Raycast(transform.position, directionToTarget, out hit, 1 << LayerMask.NameToLayer("Player"));

                // if we succeeded our raycast, and we hit the player first: we're in attack range and LoS
                if (raycastSuccess == true && hit.transform.tag == "Player")
                {
                    MoveFSM.Stop();
                }

                // raycast was false, we either hit nothing, or hit something that wasnt the player
                else
                {
                    MoveFSM.SetPath(currentTarget.transform.position);
                }
               
            }

            // we're outside of range
            else
            {
                MoveFSM.SetPath(currentTarget.transform.position);
            }
        }

        else
        {
            transform.position = currentTarget.transform.position + UnityEngine.Random.insideUnitSphere * 5;
        }

        if (katamarimdoe)
        {
            NavAgent.Warp(currentTarget.transform.position);
        }
    }

    #endregion

    #endregion
}

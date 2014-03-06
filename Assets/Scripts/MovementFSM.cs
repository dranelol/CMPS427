using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MovementFSM : StateMachine 
{
    private NavMeshAgent navAgent;
    private float lockTime = 0;

    public enum MoveStates
    {
        idle,
        moving,
        moveLocked
    }

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.stoppingDistance = navAgent.radius + 1;

        SetupMachine(MoveStates.idle);

        HashSet<Enum> idleTransitions = new HashSet<Enum>();
        idleTransitions.Add(MoveStates.moving);
        idleTransitions.Add(MoveStates.moveLocked);

        HashSet<Enum> movingTransitions = new HashSet<Enum>();
        movingTransitions.Add(MoveStates.idle);
        movingTransitions.Add(MoveStates.moveLocked);

        HashSet<Enum> moveLockedTransitions = new HashSet<Enum>();
        moveLockedTransitions.Add(MoveStates.idle);
        moveLockedTransitions.Add(MoveStates.moveLocked);

        AddTransitionsFrom(MoveStates.idle, idleTransitions);
        AddTransitionsFrom(MoveStates.moving, movingTransitions);
        AddTransitionsFrom(MoveStates.moveLocked, moveLockedTransitions);

        StartMachine(MoveStates.idle);
    }

    #region public functions

    public void SetPath(Vector3 targetPosition)
    {
        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            navAgent.SetDestination(targetPosition);
        }
    }

    public void Stop(float time = 0)
    {
        lockTime += Mathf.Max(time, 0);

        if (lockTime > 0)
        {
            Transition(MoveStates.moveLocked);
        }

        else
        {
            navAgent.ResetPath();
        }
    }

    public void LockMovement()
    {
        lockTime = 0;
        Transition(MoveStates.moveLocked);
    }

    public void UnlockMovement()
    {
        if ((MoveStates)CurrentState == MoveStates.moveLocked)
        {
            Transition(MoveStates.idle);
        }
    }

    #endregion

    #region idle functions

    void idle_Update()
    {
        if (navAgent.velocity != Vector3.zero)
        {
            Transition(MoveStates.moving);
        }
    }

    #endregion

    #region moving functions

    void moving_Update()
    {
        if (navAgent.velocity == Vector3.zero)
        {
            Transition(MoveStates.idle);
        }
    }

    #endregion

    #region moveLocked functions

    IEnumerator moveLocked_EnterState()
    {
        navAgent.ResetPath();
        yield break;
    }

    void moveLocked_Update()
    {
        if (lockTime > 0)
        {
            lockTime -= Time.deltaTime;

            if (lockTime <= 0)
            {
                Transition(MoveStates.idle);
            }
        }
    }

    IEnumerator moveLocked_ExitState()
    {
        lockTime = 0;
        navAgent.ResetPath();
        yield break;
    }

    #endregion
}

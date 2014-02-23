using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MovementFSM : StateMachine 
{
    private NavMeshAgent thisAgent;
    private bool timedLock = false;
    private float lockTime = 0;

    public enum MoveStates
    {
        idle,
        moving,
        moveLocked
    }

    void Start()
    {
        thisAgent = GetComponent<NavMeshAgent>();

        Setup(typeof(MoveStates));

        List<Enum> idleTransitions = new List<Enum>();
        idleTransitions.Add(MoveStates.moving);
        idleTransitions.Add(MoveStates.moveLocked);



        List<Enum> movingTransitions = new List<Enum>();
        //movingTransitions.Add(MoveStates.idle);
        //movingTransitions.Add(MoveStates.moveLocked);





        List<Enum> moveLockedTransitions = new List<Enum>();
        moveLockedTransitions.Add(MoveStates.idle);

        Transitions.Add(MoveStates.idle, idleTransitions);
        Transitions.Add(MoveStates.moving, movingTransitions);
        Transitions.Add(MoveStates.moveLocked, moveLockedTransitions);



        StartMachine(MoveStates.idle);

    }

    #region public functions

    public void SetPath(Vector3 targetPosition)
    {
        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            thisAgent.SetDestination(targetPosition);
        }
    }

    public void Stop(float time = 0)
    {
        lockTime = Mathf.Max(time, 0);
        timedLock = true;
        Transition(MoveStates.moveLocked);
    }

    public void LockMovement()
    {
        Transition(MoveStates.moveLocked);
    }

    public void UnlockMovement()
    {
        Transition(MoveStates.idle);
    }

    #endregion

    #region idle functions

    void idle_Update()
    {
        if (thisAgent.velocity != Vector3.zero)
        {
            Transition(MoveStates.moving);
        }
    }

    #endregion

    #region moving functions

    void moving_Update()
    {
        if (thisAgent.velocity == Vector3.zero)
        {
            Transition(MoveStates.idle);
        }
    }

    #endregion

    #region moveLocked functions

    IEnumerator moveLocked_EnterState()
    {
        thisAgent.ResetPath();
        yield break;
    }

    void moveLocked_Update()
    {
        if (timedLock == true)
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
        timedLock = false;
        lockTime = 0;
        thisAgent.ResetPath();
        yield break;
    }

    #endregion
}

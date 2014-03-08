using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MovementFSM : StateMachine 
{
    private NavMeshAgent _navMeshAgent;
    private float _lockTimer;

    public enum MoveStates
    {
        idle,
        moving,
        moveLocked
    }

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = _navMeshAgent.radius;
        _lockTimer = 0;

        SetupMachine(MoveStates.idle);

        HashSet<Enum> moveLockedTransitions = new HashSet<Enum>();
        moveLockedTransitions.Add(MoveStates.idle);
        moveLockedTransitions.Add(MoveStates.moveLocked);

        AddAllTransitionsFrom(MoveStates.idle);
        AddAllTransitionsFrom(MoveStates.moving);
        AddTransitionsFrom(MoveStates.moveLocked, moveLockedTransitions);

        StartMachine(MoveStates.idle);
    }

    #region public functions

    public void SetPath(Vector3 targetPosition)
    {
        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            Transition(MoveStates.moving);
            _navMeshAgent.SetDestination(targetPosition); 
        }
    }

    public void Stop()
    {
        if ((MoveStates)CurrentState == MoveStates.moving)
        {
            _navMeshAgent.ResetPath();
            Transition(MoveStates.idle);
        }
    }

    public void LockMovement(float duration)
    {
        if (duration > 0)
        {
            _lockTimer = Mathf.Max(_lockTimer, duration);

            if ((MoveStates)CurrentState != MoveStates.moveLocked)
            {
                Transition(MoveStates.moveLocked);
            }
        }
    }

    public void UnlockMovement()
    {
        if ((MoveStates)CurrentState == MoveStates.moveLocked)
        {
            Transition(MoveStates.idle);
        }
    }

    public void AddForce(Vector3 force, float duration, ForceMode forceMode = ForceMode.Force)
    {
        if (force.magnitude > 0)
        {
            LockMovement(duration);
            rigidbody.AddForce(force, forceMode);
        }
    }

    #endregion

    #region state behaviour

    #region moving functions

    void moving_Update()
    {
        if (!_navMeshAgent.hasPath)
        {
            Stop();
        }
    }

    #endregion

    #region moveLocked functions

    private IEnumerator moveLocked_EnterState()
    {
        _navMeshAgent.ResetPath();
        _navMeshAgent.enabled = false;
        yield return null;
    }

    private void moveLocked_Update()
    {
        _lockTimer -= Time.deltaTime;

        if (_lockTimer <= 0)
        {
            Transition(MoveStates.idle);
        }
    }

    private IEnumerator moveLocked_ExitState()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.ResetPath();
        _lockTimer = 0;
        yield return null;
    }

    #endregion

    #endregion
}

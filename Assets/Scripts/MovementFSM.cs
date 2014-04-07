using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MovementFSM : StateMachine 
{
    public const float DEFAULT_MOVEMENT_SPEED = 5;

    private NavMeshAgent _navMeshAgent;

    public float _movementSpeed;
    public float MovementSpeed
    {
        get { return _movementSpeed; }
        set
        {
            _movementSpeed = Mathf.Clamp(value * DEFAULT_MOVEMENT_SPEED, 0, 15f);
            _navMeshAgent.speed = _movementSpeed;
        }
    }

    public enum MoveStates
    {
        idle,
        moving,
        moveLocked
    }

    void Awake()
    {
        SetupMachine(MoveStates.idle);

        HashSet<Enum> moveLockedTransitions = new HashSet<Enum>();
        moveLockedTransitions.Add(MoveStates.idle);
        moveLockedTransitions.Add(MoveStates.moveLocked);

        AddAllTransitionsFrom(MoveStates.idle);
        AddAllTransitionsFrom(MoveStates.moving);
        AddTransitionsFrom(MoveStates.moveLocked, moveLockedTransitions);

        StartMachine(MoveStates.idle);
    }

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = _navMeshAgent.radius;
        MovementSpeed = GetComponent<Entity>().currentAtt.MovementSpeed;
    }

    #region public functions

    public void SetPath(Vector3 targetPosition)
    {
        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            Transition(MoveStates.moving);

            NavMeshHit navMeshHit;

            if (NavMesh.SamplePosition(targetPosition, out navMeshHit, 15, 1 << LayerMask.NameToLayer("Default")))
            {
                _navMeshAgent.SetDestination(navMeshHit.position); 
            }      
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

    public void LockMovement()
    {
        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            Transition(MoveStates.moveLocked);
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
            LockMovement();
            Invoke("UnlockMovement", duration);
            rigidbody.AddForce(force, forceMode);
        }
    }

    public void Warp(Vector3 targetLocation)
    {
        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            Transition(MoveStates.idle);
            _navMeshAgent.Warp(targetLocation);
        }
    }

    #endregion

    #region state behaviour

    #region moving functions

    void moving_Update()
    {
        if (!_navMeshAgent.hasPath && !_navMeshAgent.pathPending)
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

    private IEnumerator moveLocked_ExitState()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.ResetPath();
        yield return null;
    }

    #endregion

    #endregion
}

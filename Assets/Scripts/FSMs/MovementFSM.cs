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
    private AnimationController _animController;

    public Vector3 Destination
    {
        get { return _navMeshAgent.destination; }
    }

    private float _movementSpeed;
    public float MovementSpeed
    {
        get { return _movementSpeed; }
    }

    public float Radius
    {
        get { return _navMeshAgent.radius; }
    }

    public float RemainingDistance
    {
        get { return _navMeshAgent.remainingDistance; }
    }

    public Vector3 SteeringTarget
    {
        get { return _navMeshAgent.steeringTarget; }
    }

    public float StoppingDistance
    {
        get { return _navMeshAgent.stoppingDistance; }
    }

    public Vector3 Velocity
    {
        get { return _navMeshAgent.velocity; }
    }

    public void UpdateMovementSpeed(float value)
    {
        _navMeshAgent.speed = _movementSpeed = DEFAULT_MOVEMENT_SPEED * value;
    }

    public enum MoveStates
    {
        idle,
        moving,
        moveLocked
    }

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animController = GetComponent<AnimationController>();

        SetupMachine(MoveStates.idle);

        HashSet<Enum> moveLockedTransitions = new HashSet<Enum>();
        moveLockedTransitions.Add(MoveStates.idle);
        moveLockedTransitions.Add(MoveStates.moveLocked);

        AddAllTransitionsFrom(MoveStates.idle);
        AddAllTransitionsFrom(MoveStates.moving);
        AddTransitionsFrom(MoveStates.moveLocked, moveLockedTransitions);

        StartMachine(MoveStates.idle);

        _movementSpeed = DEFAULT_MOVEMENT_SPEED;
        _navMeshAgent.updateRotation = false;
    }

    void Start()
    {
        _navMeshAgent.stoppingDistance = _navMeshAgent.radius;
    }

    #region public functions

    public void SetPath(Vector3 targetPosition)
    {
        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            NavMeshHit navMeshHit;

            if (NavMesh.SamplePosition(targetPosition, out navMeshHit, 15, 1 << LayerMask.NameToLayer("Default")))
            {
                _navMeshAgent.SetDestination(navMeshHit.position);
                Transition(MoveStates.moving);
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

    #region idle functions

    private IEnumerator idle_EnterState()
    {
        _animController.StopMovingAnim();
        yield return null;
    }

    #endregion

    #region moving functions

    private IEnumerator moving_EnterState()
    {
        _animController.StartMovingAnim();
        yield return null;
    }

    void moving_Update()
    {
        if (!_navMeshAgent.hasPath && !_navMeshAgent.pathPending) // || _navMeshAgent.hasPath && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            Stop();
        }

        else if (_navMeshAgent.hasPath)
        {
            /*
            Vector3 tempRotation = transform.rotation.eulerAngles;
            tempRotation.y = Mathf.LerpAngle(transform.rotation.eulerAngles.y, Quaternion.LookRotation(SteeringTarget).eulerAngles.y, Time.time );
            transform.rotation = Quaternion.Euler(tempRotation);*/

            Vector3 direction = SteeringTarget - transform.position;

            transform.forward = Vector3.Slerp(transform.forward, new Vector3(direction.x, 0, direction.z).normalized, Time.deltaTime * 10f);
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

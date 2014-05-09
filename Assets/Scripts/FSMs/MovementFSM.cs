using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MovementFSM : StateMachine
{
    #region Constants

    public const float MINIMUM_HEIGHT = 0.5f;
    public const float MAXIMUM_HEIGHT = 5f;

    public const float MINIMUM_RADIUS = 0.05f;
    public const float MAXIMUM_RADIUS = 4f;

    public const float MINIMUM_BASE_MOVEMENT_SPEED = 3f;
    public const float MAXIMUM_BASE_MOVEMENT_SPEED = 7f;

    #endregion

    #region Properties

    private NavMeshAgent _navMeshAgent;
    private AnimationController _animationController;
    private CapsuleCollider _collider;

    private float _baseMovementSpeed;
    public float BaseMovementSpeed
    {
        get { return _baseMovementSpeed; }
    }

    private float _movementSpeed;
    public float MovementSpeed
    {
        get { return _movementSpeed; }
    }

    public Vector3 Destination
    {
        get { return _navMeshAgent.destination; }
    }

    public bool HasPath
    {
        get { return _navMeshAgent.hasPath; }
    }

    public bool PathPending
    {
        get { return _navMeshAgent.pathPending; }
    }

    public float Height
    {
        get { return _navMeshAgent.height; }
        set 
        {
            _navMeshAgent.height = Mathf.Clamp(value, MINIMUM_HEIGHT, MAXIMUM_HEIGHT);
            _collider.height = Height;
        }
    }

    public float Radius
    {
        get { return _navMeshAgent.radius; }
        set 
        {
            _navMeshAgent.radius = Mathf.Clamp(value, MINIMUM_RADIUS, MAXIMUM_RADIUS);
            _navMeshAgent.stoppingDistance = Radius * 1.1f * transform.lossyScale.magnitude;
        }
    }

    public float RemainingDistance
    {
        get { return _navMeshAgent.remainingDistance; }
    }

    public Vector3 SteeringTarget
    {
        get { return _navMeshAgent.steeringTarget; }
    }

    public Vector3 Velocity
    {
        get { return _navMeshAgent.velocity; }
    }

    public void UpdateMovementSpeed(float value)
    {
        _navMeshAgent.speed = _movementSpeed = _baseMovementSpeed * value;
        _animationController.UpdateMovementSpeed(value);
    }

    public enum MoveStates
    {
        idle,
        moving,
        moveLocked,
    }

    public enum LockType
    {
        ShiftLock,
        MovementLock,
    }

    private bool _shiftLocked = false;
    private float _lockTime = 0f;

    #endregion

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animationController = GetComponent<AnimationController>();
        _collider = GetComponent<CapsuleCollider>();

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
        _navMeshAgent.stoppingDistance = Radius * 1.1f * transform.lossyScale.magnitude;
        _navMeshAgent.updateRotation = false;

        if (tag == "Player")
        {
            _navMeshAgent.avoidancePriority = 1;
            _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            _navMeshAgent.autoRepath = false;
        }

        _baseMovementSpeed = Mathf.Lerp(MAXIMUM_BASE_MOVEMENT_SPEED, MINIMUM_BASE_MOVEMENT_SPEED, Radius / MAXIMUM_RADIUS);
        UpdateMovementSpeed(1f);
    }

    #region public functions

    public bool SetPath(Vector3 targetPosition)
    {
        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            _navMeshAgent.speed = _movementSpeed;
            NavMeshHit navMeshHit;

            if (tag == "Player")
            {
                if (CombatMath.DistanceGreaterThan(targetPosition, transform.position, 1))
                {
                    if (NavMesh.SamplePosition(targetPosition, out navMeshHit, 15, 1 << LayerMask.NameToLayer("Default")))
                    {
                        NavMeshHit playerNavMeshHit;

                        if (_navMeshAgent.Raycast(navMeshHit.position, out playerNavMeshHit))
                        {
                            _navMeshAgent.Move((playerNavMeshHit.position - transform.position).normalized / 50f);
                            _navMeshAgent.SetDestination(playerNavMeshHit.position);
                        }

                        else
                        {
                            _navMeshAgent.SetDestination(navMeshHit.position);
                        }

                        Transition(MoveStates.moving);
                        return true;
                    }
                }

                return false;
            }
            
            else
            {
                if (NavMesh.SamplePosition(targetPosition, out navMeshHit, 15, 1 << LayerMask.NameToLayer("Default")))
                {
                    _navMeshAgent.SetDestination(navMeshHit.position);
                    Transition(MoveStates.moving);
                    return true;
                }
            }
        }

        return false;
    }

    public void WalkPath(Vector3 targetPosition)
    {
        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            _navMeshAgent.speed = _movementSpeed * 0.3f;

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

    public void LockMovement(LockType type, float duration = 0)
    {
        if (type == LockType.ShiftLock)
        {
            _shiftLocked = true;
        }

        else
        {
            _lockTime = Mathf.Max(_lockTime, Time.time + duration);
        }

        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            Transition(MoveStates.moveLocked);
        }
    }

    public void UnlockMovement(LockType type)
    {
        if ((MoveStates)CurrentState == MoveStates.moveLocked)
        {
            if (type == LockType.MovementLock)
            {
                _lockTime = Time.time;
            }

            else
            {
                _shiftLocked = false;
            }
        }
    }

    public void Turn(Vector3 steeringTarget, float amount = 1)
    {
        Vector3 direction = steeringTarget - transform.position;
        transform.forward = Vector3.Slerp(transform.forward, new Vector3(direction.x, 0, direction.z).normalized, 0.2f * amount);
    }

    public void AddForce(Vector3 force, float duration)
    {
        if (force.magnitude > 0)
        {
            LockMovement(LockType.MovementLock, duration);
            _navMeshAgent.Move(force / 50f);
        }
    }

    public void Warp(Vector3 targetLocation)
    {
        if ((MoveStates)CurrentState != MoveStates.moveLocked)
        {
            Transition(MoveStates.idle);
        }
        _navMeshAgent.Warp(targetLocation);
    }

    #endregion

    #region state behaviour

    #region idle functions

    private IEnumerator idle_EnterState()
    {
        _animationController.StopMoving();
        yield return null;
    }

    #endregion

    #region moving functions

    private IEnumerator moving_EnterState()
    {
        _animationController.StartMoving();
        yield return null;
    }

    void moving_Update()
    {
        if (!_navMeshAgent.hasPath && !_navMeshAgent.pathPending)
        {
            Stop();
        }

        else if (_navMeshAgent.hasPath)
        {
            Turn(SteeringTarget);
        }
    }

    #endregion

    #region moveLocked functions

    private IEnumerator moveLocked_EnterState()
    {
        _navMeshAgent.ResetPath();
        _animationController.StopMoving();
        yield return null;
    }

    void moveLocked_Update()
    {
        if (!_shiftLocked && _lockTime <= Time.time)
        {
            Transition(MoveStates.idle);
        }
    }

    private IEnumerator moveLocked_ExitState()
    {
        _navMeshAgent.ResetPath();
        yield return null;
    }

    #endregion

    #endregion
}

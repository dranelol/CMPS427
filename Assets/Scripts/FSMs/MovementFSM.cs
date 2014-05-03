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
    private AnimationController _animController;
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
            _collider.radius = _navMeshAgent.stoppingDistance = Radius;
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
    }

    public enum MoveStates
    {
        idle,
        moving,
        moveLocked,
    }

    #endregion

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animController = GetComponent<AnimationController>();
        _collider = GetComponent<CapsuleCollider>();

        SetupMachine(MoveStates.idle);

        HashSet<Enum> moveLockedTransitions = new HashSet<Enum>();
        moveLockedTransitions.Add(MoveStates.idle);
        moveLockedTransitions.Add(MoveStates.moveLocked);

        AddAllTransitionsFrom(MoveStates.idle);
        AddAllTransitionsFrom(MoveStates.moving);
        AddTransitionsFrom(MoveStates.moveLocked, moveLockedTransitions);

        StartMachine(MoveStates.idle);

        _navMeshAgent.updateRotation = false;
    }

    void Start()
    {
        _navMeshAgent.stoppingDistance = 1f;
        _navMeshAgent.acceleration = 1000f;
        _navMeshAgent.autoBraking = true;
        _navMeshAgent.autoRepath = true;
        _baseMovementSpeed = Mathf.Lerp(MAXIMUM_BASE_MOVEMENT_SPEED, MINIMUM_BASE_MOVEMENT_SPEED, Radius / MAXIMUM_RADIUS);

        UpdateMovementSpeed(1f);

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

                if (tag == "Player")
                {
                    if (_navMeshAgent.Raycast(targetPosition, out navMeshHit))
                    {
                        _navMeshAgent.SetDestination(navMeshHit.position);
                    }

                    else
                    {
                        _navMeshAgent.SetDestination(targetPosition);
                    }

                    Transition(MoveStates.moving);
                }

                else
                {
                    if (NavMesh.SamplePosition(targetPosition, out navMeshHit, 15, 1 << LayerMask.NameToLayer("Default")))
                    {
                        _navMeshAgent.SetDestination(navMeshHit.position);
                    }

                    Transition(MoveStates.moving);
                }
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
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            Transition(MoveStates.idle);
        }
    }

    public void Turn(Vector3 steeringTarget)
    {
        Vector3 direction = steeringTarget - transform.position;
        transform.forward = Vector3.Slerp(transform.forward, new Vector3(direction.x, 0, direction.z).normalized, 0.2f);
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
        _animController.StopMoving();
        yield return null;
    }

    #endregion

    #region moving functions

    private IEnumerator moving_EnterState()
    {
        _animController.StartMoving();
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

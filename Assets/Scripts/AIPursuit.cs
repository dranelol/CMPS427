using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

public class AIPursuit : StateMachine
{
    private enum PursuitStates
    {
        inactive,
        approach,
        seek,
        attack,
        flee
    }

    public const float _rangeAccuracy = 0.9f;

    private Entity entity;
    private MovementFSM MoveFSM;
    private CombatFSM combatFSM;
    private AbilityManager _abilityManager;
    private AnimationController _animationController;
    private EntitySoundManager _soundManager;
    private CapsuleCollider _collider;

    private GameObject currentTarget = null;
    private int _nextAbilityIndex = -1;
    public int _abilityCount;
    private float _adjustedRange;
    public float AdjustedRange
    {
        get { return _adjustedRange; }
    }

    public bool doesFlee;
    public float fleeDistance; //Max distance to travel from the position the enemy starts fleeing from.
    public float fleeTime; //Time the enemy will flee before returning to attack
    private float fleeEnd; //Future time the enemy will stop fleeing.
    private bool hasFled;

    private bool _swinging;
    private float _swingSpeed;

    void Awake()
    {
        SetupMachine(PursuitStates.inactive);

        HashSet<Enum> inactiveTransitions = new HashSet<Enum>();
        inactiveTransitions.Add(PursuitStates.approach);
        AddTransitionsFrom(PursuitStates.inactive, inactiveTransitions);
        AddAllTransitionsTo(PursuitStates.inactive);

        HashSet<Enum> approachTransitions = new HashSet<Enum>();
        approachTransitions.Add(PursuitStates.seek);
        approachTransitions.Add(PursuitStates.flee);
        AddTransitionsFrom(PursuitStates.approach, approachTransitions);

        AddAllTransitionsFrom(PursuitStates.seek);

        HashSet<Enum> attackTransitions = new HashSet<Enum>();
        attackTransitions.Add(PursuitStates.seek);
        attackTransitions.Add(PursuitStates.flee);
        AddTransitionsFrom(PursuitStates.attack, attackTransitions);

        HashSet<Enum> fleeTransitions = new HashSet<Enum>();
        fleeTransitions.Add(PursuitStates.approach);
        fleeTransitions.Add(PursuitStates.inactive);
        AddTransitionsFrom(PursuitStates.flee, fleeTransitions);

        StartMachine(PursuitStates.inactive);

        hasFled = false;
        doesFlee = true;
        _swinging = false;

        entity = GetComponent<Entity>();
        MoveFSM = GetComponent<MovementFSM>();
        combatFSM = GetComponent<CombatFSM>();
        _abilityManager = GetComponent<AbilityManager>();
        _animationController = GetComponent<AnimationController>();
        _soundManager = GetComponent<EntitySoundManager>();
        _collider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        UpdateAbilities();
        _swingSpeed = GetComponent<EnemyBaseAtts>()._swingSpeed;
    }

    #region public functions

    public void Pursue(GameObject target)
    {
        if (target != null)
        {
            currentTarget = target;
            Transition(PursuitStates.approach);
        }
    }

    public void StopPursuit()
    {
        currentTarget = null;
        Transition(PursuitStates.inactive);
    }

    public bool IsFleeing()
    {
        return (PursuitStates)CurrentState == PursuitStates.flee;
    }

    #endregion

    #region private functions

    private void Fear(Vector3 currentPosition, Vector3 centerPosition, float distance, float distanceFromCenter)
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * distance; // Pick a random point on the edge of the circle

        Vector3 targetPosition = new Vector3(randomDirection.x, 0, randomDirection.y);

        targetPosition += currentPosition;
        targetPosition.y = currentPosition.y;

        if (Vector3.Distance(centerPosition, targetPosition) > distanceFromCenter)
        {
            Vector3 newDirection = (centerPosition - currentPosition).normalized * distance;

            targetPosition = new Vector3(newDirection.x, 0, newDirection.y);
            targetPosition += currentPosition;
            targetPosition.y = currentPosition.y;
        }

        MoveFSM.SetPath(targetPosition);
    }

    private void Flee()
    {

        GameObject nodeOfSmallestDistance = null;
        Vector3 newdirection = Vector3.zero;
        Vector3 targetPosition = Vector3.zero;

        GameObject[] enemyNodes = GameObject.FindGameObjectsWithTag("EnemyNode");

        foreach (GameObject node in enemyNodes)
        {
            if (nodeOfSmallestDistance == null
                && node.transform.position != GetComponent<AIController>().homeNodePosition)
            {
                nodeOfSmallestDistance = node;
            }
            else
            {
                if (node.transform.position != GetComponent<AIController>().homeNodePosition
                    && Vector3.Distance(transform.position, node.transform.position) < Vector3.Distance(transform.position, nodeOfSmallestDistance.transform.position))
                {
                    nodeOfSmallestDistance = node;
                }
            }
        }

        if (nodeOfSmallestDistance == null)
        {
            newdirection = ((transform.position - currentTarget.transform.position).normalized * fleeDistance);
            targetPosition = Rotations.RotateAboutY(newdirection, UnityEngine.Random.Range(-90f, 90f));
        }
        else
        {
            newdirection = ((nodeOfSmallestDistance.transform.position - transform.position).normalized * fleeDistance);
            targetPosition = new Vector3(newdirection.x, 0, newdirection.z);
        }

        targetPosition += transform.position;
        targetPosition.y = transform.position.y;

        MoveFSM.SetPath(targetPosition);
    }

    private void UpdateAbilities()
    {
        int next = 0;

        for (int i = 0; i < _abilityCount; i++)
        {
            try
            {
                if (_abilityManager.abilities[i].Cooldown > _abilityManager.abilities[next].Cooldown
                    && _abilityManager.activeCoolDowns[i] <= Time.time)
                {
                    next = i;
                }
            }

            catch
            {
                break;
            }
        }

        if (next != _nextAbilityIndex)
        {
            _nextAbilityIndex = next;

            try
            {
                if (_abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.MELEE || _abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.PBAOE)
                {
                    _adjustedRange = (_abilityManager.abilities[_nextAbilityIndex].Range + MoveFSM.Radius) * _rangeAccuracy;
                }

                else
                {
                    _adjustedRange = _abilityManager.abilities[_nextAbilityIndex].Range * _rangeAccuracy;
                }
            }

            catch
            {
                _nextAbilityIndex = 0;
                _adjustedRange = (_abilityManager.abilities[_nextAbilityIndex].Range + MoveFSM.Radius) * _rangeAccuracy;
            }
        }
    }

    #endregion

    #region state based functions

    #region inactive functions

    IEnumerator inactive_EnterState()
    {
        currentTarget = null;
        yield break;
    }

    #endregion

    #region approach functions

    IEnumerator approach_EnterState()
    {
        MoveFSM.SetPath(currentTarget.transform.position);
        yield break;
    }

    void approach_Update()
    {
        if (currentTarget != null)
        {
            if (doesFlee && (entity.CurrentHP < (entity.currentAtt.Health * 0.2f)) && hasFled == false)
            {
                Transition(PursuitStates.flee);
            }

            else if (!MoveFSM.HasPath)
            {
                if (!MoveFSM.PathPending)
                {
                    MoveFSM.SetPath(currentTarget.transform.position);
                }

                else
                {
                    MoveFSM.Turn(currentTarget.transform.position);
                }
            }

            else if ((currentTarget.transform.position - MoveFSM.Destination).sqrMagnitude > Mathf.Pow(_adjustedRange, 2f))
            {
                MoveFSM.SetPath(currentTarget.transform.position);
            }

            else if ((currentTarget.transform.position - transform.position).sqrMagnitude <= Mathf.Pow(_adjustedRange, 2f))
            {
                Transition(PursuitStates.seek);
            }

            else
            {
                UpdateAbilities();
            }
        }

        else
        {
            Transition(PursuitStates.inactive);
        }
    }

    #endregion

    #region seek functions

    void seek_Update()
    {
        if (currentTarget != null)
        {
            if (doesFlee && (entity.CurrentHP < (entity.currentAtt.Health * 0.2f)) && hasFled == false)
            {
                Transition(PursuitStates.flee);
            }

            if ((MovementFSM.MoveStates)MoveFSM.CurrentState == MovementFSM.MoveStates.idle)
            {
                MoveFSM.Turn(currentTarget.transform.position);
            }

            if (combatFSM.IsIdle())
            {
                Vector3 entityCenterPosition = CombatMath.GetCenter(transform);
                Vector3 targetCenterPosition = CombatMath.GetCenter(currentTarget.transform);

                RaycastHit hit;

                if (Physics.Raycast(entityCenterPosition, targetCenterPosition - entityCenterPosition, out hit, _adjustedRange, ~(1 << LayerMask.NameToLayer("Enemy"))))
                {
                    if (hit.collider.tag == "Player" && _abilityManager.activeCoolDowns[_nextAbilityIndex] <= Time.time)
                    {
                        MoveFSM.Turn(currentTarget.transform.position, 5);
                        MoveFSM.Stop();
                        Transition(PursuitStates.attack);
                    }

                    else if (!MoveFSM.PathPending)
                    {
                        MoveFSM.SetPath(currentTarget.transform.position);
                    }

                    else
                    {
                        UpdateAbilities();
                    }
                }

                else
                {
                    Transition(PursuitStates.approach);
                }
            }       
        }

        else
        {
            Transition(PursuitStates.inactive);
        }
    }

    #endregion

    #region attack functions

    private void SwingTimer()
    {
        _swinging = false;
    }

    IEnumerator attack_EnterState()
    {
        _swinging = true;
        float offset = 0;

        if (_abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.MELEE)
        {
            _animationController.Attack(AnimationType.Melee, 0);
        }

        else if (_abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.PROJECTILE)
        {
            _animationController.Attack(AnimationType.Melee, 1);
        }

        else if (_abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.HONINGPROJECTILE)
        {
            _animationController.Attack(AnimationType.Melee, 1);
        }

        else
        {
            _animationController.Attack(AnimationType.Melee, 2);
            offset = 0.3f;
        }

        Invoke("SwingTimer", _swingSpeed + offset);
        yield break;
    }

    void attack_Update()
    {
        if (currentTarget != null)
        {
            if (!_swinging)
            {
                if (_abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.MELEE)
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                    _abilityManager.abilities[_nextAbilityIndex].AttackHandler(gameObject, entity, false);
                }

                else if (_abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.PROJECTILE)
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                    _abilityManager.abilities[_nextAbilityIndex].SpawnProjectile(gameObject, gameObject, (CombatMath.GetCenter(currentTarget.transform) - CombatMath.GetCenter(transform)).normalized, _abilityManager.abilities[_nextAbilityIndex].ID, false);
                }

                else if (_abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.HONINGPROJECTILE)
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                    _abilityManager.abilities[_nextAbilityIndex].SpawnProjectile(gameObject, CombatMath.GetCenter(currentTarget.transform), gameObject, (CombatMath.GetCenter(currentTarget.transform) - CombatMath.GetCenter(transform)).normalized, _abilityManager.abilities[_nextAbilityIndex].ID, false);
                }

                else
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                    _abilityManager.abilities[_nextAbilityIndex].AttackHandler(gameObject, entity, false);
                }

                _abilityManager.activeCoolDowns[_nextAbilityIndex] = Time.time + _abilityManager.abilities[_nextAbilityIndex].Cooldown;
                UpdateAbilities();
                _soundManager.Attack();

                Transition(PursuitStates.seek);
            }
        }

        else
        {
            Transition(PursuitStates.inactive);
        }
    }

    #endregion

    #region flee functions

    IEnumerator flee_EnterState()
    {
        fleeEnd = Time.time + fleeTime;
        hasFled = true;
        Flee();
        yield break;
    }

    void flee_Update()
    {
        if (fleeEnd < Time.time)
        {
            Transition(PursuitStates.approach);
        }

        else if (entity.CurrentHP <= 0)
        {
            StopPursuit();
        }
    }

    #endregion

    #endregion
}

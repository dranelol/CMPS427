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
        seek,
        attack,
        flee
    }

    private MovementFSM MoveFSM;
    private Entity entity;
    private CombatFSM combatFSM;

    private GameObject currentTarget = null;

    private int _nextAbilityIndex;
    private List<int> _abilityIndices;
    private AbilityManager _abilityManager;
    private float _adjustedRange;
    private AnimationController _animationController;

    public bool doesFlee;
    public float fleeDistance; //Max distance to travel from the position the enemy starts fleeing from.
    public float fleeTime; //Time the enemy will flee before returning to attack

    private float fleeEnd; //Future time the enemy will stop fleeing.
    private bool hasFled;
    private bool swinging;

    private float swingSpeed;

    private GameObject debugNSD;
    
    void Awake()
    {
        SetupMachine(PursuitStates.inactive);

        HashSet<Enum> inactiveTransitions = new HashSet<Enum>();
        inactiveTransitions.Add(PursuitStates.seek);
        AddTransitionsFrom(PursuitStates.inactive, inactiveTransitions);

        HashSet<Enum> fleeTransitions = new HashSet<Enum>();
        fleeTransitions.Add(PursuitStates.seek);
        fleeTransitions.Add(PursuitStates.inactive);
        AddTransitionsFrom(PursuitStates.flee, fleeTransitions);

        HashSet<Enum> attackTransitions = new HashSet<Enum>();
        attackTransitions.Add(PursuitStates.flee);
        AddTransitionsFrom(PursuitStates.attack, attackTransitions);
        
        AddAllTransitionsFrom(PursuitStates.seek);
        AddAllTransitionsTo(PursuitStates.seek);
        AddAllTransitionsTo(PursuitStates.inactive);

        StartMachine(PursuitStates.inactive);

        MoveFSM = GetComponent<MovementFSM>();
        entity = GetComponent<Entity>();
        combatFSM = GetComponent<CombatFSM>();

        hasFled = false;
        doesFlee = true;
        swinging = false;

        swingSpeed = GetComponent<EnemyBaseAtts>()._swingSpeed;
        _animationController = GetComponent<AnimationController>();
    }

    void Start()
    {
        _abilityManager = GetComponent<AbilityManager>();
        _nextAbilityIndex = 0;
        _adjustedRange = 5f;

    }

    #region public functions

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


    public bool IsFleeing()
    {
        return (PursuitStates)CurrentState == PursuitStates.flee;
    }

    #endregion

    #region private functions

    /// <summary>
    /// Initiates the fear behaviour
    /// </summary>
    /// <param name="currentPosition">Current location of the enemy</param>
    /// <param name="centerPosition">Position the path needs to be around</param>
    /// <param name="distance">How far to travel each wander.</param>
    /// <param name="distanceFromCenter">Max distance to travel from the center</param>
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

        GameObject nodeOfSmallestDistance  = null;
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
                if ( node.transform.position != GetComponent<AIController>().homeNodePosition
                    && Vector3.Distance(transform.position, node.transform.position) < Vector3.Distance(transform.position, nodeOfSmallestDistance.transform.position))
                {
                    nodeOfSmallestDistance = node;
                }

            }
        }

        debugNSD = nodeOfSmallestDistance;

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

        for (int i = 0; i < 4; i++)
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
            _adjustedRange = _abilityManager.abilities[_nextAbilityIndex].Range + MoveFSM.Radius + currentTarget.GetComponent<MovementFSM>().Radius;
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

    #region seek functions

    void seek_Update()
    {
        if ((MovementFSM.MoveStates)MoveFSM.CurrentState == MovementFSM.MoveStates.idle)
        {
            MoveFSM.Turn(currentTarget.transform.position);
        }

        if (doesFlee && (entity.CurrentHP < (entity.currentAtt.Health * 0.2f)) && hasFled == false)
        {
            Transition(PursuitStates.flee);
        }

        else
        {
            if (currentTarget != null)
            {
                UpdateAbilities();

                if (combatFSM.IsIdle() && _abilityManager.activeCoolDowns[_nextAbilityIndex] <= Time.time) // check resource as well
                {
                    Vector3 directionToTarget = currentTarget.transform.position - transform.position;

                    // If the enemy is within range of its next attack, transition to attack.
                    if (directionToTarget.magnitude < _adjustedRange)
                    {
                        RaycastHit hit;

                        // Cast a ray from the enemy to the player, ignoring other enemy colliders.
                        bool raycastSuccess = Physics.Raycast(transform.position, directionToTarget, out hit, _adjustedRange, ~(1 << LayerMask.NameToLayer("Enemy")));

                        // if we succeeded our raycast, and we hit the player first: we're in attack range and LoS
                        if (raycastSuccess == true && hit.transform.tag == "Player")
                        {
                            Transition(PursuitStates.attack);
                        }
                    }

                    // Otherwise, get closer
                    else
                    {
                        MoveFSM.SetPath(currentTarget.transform.position);
                    }
                }

                else if (Vector3.Distance(transform.position, currentTarget.transform.position) <= _adjustedRange)
                {
                    MoveFSM.Stop();
                }
            }

            // Go idle if target does not exist
            else
            {
                Transition(PursuitStates.inactive);
            }
        }
    }

    #endregion

    #region attack functions

    private void SwingTimer()
    {
        swinging = false;
    }

    IEnumerator attack_EnterState()
    {
        swinging = true;
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

        Invoke("SwingTimer", swingSpeed + offset);
        yield break;
    }

    void attack_Update()
    {
        if (currentTarget != null)
        {
            if (!swinging)
            {
                Debug.DrawRay(transform.position, currentTarget.transform.position - transform.position, Color.blue, 0.1f);

                if (_abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.MELEE)
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                    _abilityManager.abilities[_nextAbilityIndex].AttackHandler(gameObject, entity, false);
                }

                else if (_abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.PROJECTILE)
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                    _abilityManager.abilities[_nextAbilityIndex].SpawnProjectile(gameObject, gameObject, (currentTarget.transform.position - transform.position).normalized, _abilityManager.abilities[_nextAbilityIndex].ID, false);
                }

                else if (_abilityManager.abilities[_nextAbilityIndex].AttackType == AttackType.HONINGPROJECTILE)
                {
                    Debug.Log("homing");
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                    _abilityManager.abilities[_nextAbilityIndex].SpawnProjectile(gameObject, currentTarget.transform.position, gameObject, (currentTarget.transform.position - transform.position).normalized, _abilityManager.abilities[_nextAbilityIndex].ID, false);
                }

                else
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                    _abilityManager.abilities[_nextAbilityIndex].AttackHandler(gameObject, entity, false);
                }

                _abilityManager.activeCoolDowns[_nextAbilityIndex] = Time.time + _abilityManager.abilities[_nextAbilityIndex].Cooldown;

                UpdateAbilities();

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

        //Debug.DrawRay(transform.position, debugNSD.transform.position - transform.position, Color.blue);
        
        if (fleeEnd < Time.time)
        {
            Transition(PursuitStates.seek);
        }
        else if(entity.CurrentHP <= 0)
        {
            StopPursuit();
        }
    }


    #endregion

    #endregion
}

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
        wander
    }

    
    private MovementFSM MoveFSM;
    private NavMeshAgent NavAgent;
    private Entity entity;
    private CombatFSM combatFSM;

    private GameObject currentTarget = null;

    private GameManager gameManager;

    private List<Ability> _abilityList = new List<Ability>(); // List of usuable abilities. This will be sorted by cooldown time then damagemod (for now)

    public float wanderInterval;
    public float wanderDistance;
    private float nextWander;

    
    void Awake()
    {
        SetupMachine(PursuitStates.inactive);

        HashSet<Enum> inactiveTransitions = new HashSet<Enum>();
        inactiveTransitions.Add(PursuitStates.seek);
        AddTransitionsFrom(PursuitStates.inactive, inactiveTransitions);

        AddAllTransitionsTo(PursuitStates.wander);
        AddAllTransitionsFrom(PursuitStates.wander);
        AddAllTransitionsFrom(PursuitStates.seek);
        AddAllTransitionsTo(PursuitStates.seek);
        AddAllTransitionsTo(PursuitStates.inactive);

        StartMachine(PursuitStates.wander);

        MoveFSM = GetComponent<MovementFSM>();
        NavAgent = GetComponent<NavMeshAgent>();
        entity = GetComponent<Entity>();
        combatFSM = GetComponent<CombatFSM>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();



        nextWander = Time.time + wanderInterval;
    }

    void Start()
    {
        // Make Enemy Entity class later
        

        _abilityList.Add(entity.abilityManager.abilities[0]);
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
        if (entity.abilityManager.activeCoolDowns[0] > Time.time)
        {
            float timeLeft = entity.abilityManager.activeCoolDowns[0] - Time.time;
            //Debug.Log("Enemy Ability 1 Cooldown Left: " + timeLeft.ToString());
        }
        
        if (currentTarget != null)
        {
            if (combatFSM.IsIdle()) // && _abilityList[nextAbilityIndex].OnCooldown == false) MATT DO THIS
            {
                Vector3 directionToTarget = currentTarget.transform.position - transform.position;

                // If the enemy is within range of its next attack, transition to attack.
                if (directionToTarget.magnitude < _abilityList[0].Range)
                {
                    RaycastHit hit;

                    // Cast a ray from the enemy to the player, ignoring other enemy colliders.
                    bool raycastSuccess = Physics.Raycast(transform.position, directionToTarget, out hit, _abilityList[0].Range, ~(1 << LayerMask.NameToLayer("Enemy")));

                    // if we succeeded our raycast, and we hit the player first: we're in attack range and LoS
                    if (raycastSuccess == true && hit.transform.tag == "Player")
                    {
                        MoveFSM.Stop();
                        Transition(PursuitStates.attack);
                    }
                }

                // Otherwise, get closer
                else
                {
                    MoveFSM.SetPath(currentTarget.transform.position);
                }
            }
        }

        // Go idle if target does not exist
        else
        {
            Transition(PursuitStates.inactive);
        }
    }

    #endregion

    #region attack functions

    void attack_Update()
    {

        if (currentTarget != null && entity.abilityManager.activeCoolDowns[0] <= Time.time)
        {
            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
            Debug.DrawRay(transform.position, currentTarget.transform.position - transform.position, Color.blue, 0.1f);

            entity.abilityManager.abilities[0].AttackHandler(gameObject, false);
            entity.abilityManager.activeCoolDowns[0] = Time.time + entity.abilityManager.abilities[0].Cooldown;

            /*
            _abilityList[0].AttackHandler(gameObject, false);
            _abilityList.OrderBy(Ability => Ability.Cooldown).ThenBy(Ability => Ability.DamageMod); Use this later */
            Transition(PursuitStates.seek);
        }

        else if (entity.abilityManager.activeCoolDowns[0] > Time.time)
        {
            Transition(PursuitStates.seek);
        }
        else
        {
            Transition(PursuitStates.inactive);
        }
    }

    #endregion

    #region wander functions

    void wander_Update()
    {
        
        
        if (currentTarget == null)
        {
            if (Time.time >= nextWander)
            {
                Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * wanderDistance; // Pick a random point on the edge of the circle

                Vector3 targetPosition = new Vector3(randomDirection.x, 0, randomDirection.y);

                

                targetPosition += transform.position;
                targetPosition.y = transform.position.y;

                //transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
                
                Debug.DrawRay(transform.position, targetPosition - transform.position, Color.blue); // Draw vector to target position

                Debug.Log(targetPosition.ToString());

                //Debug.Log("world target: " + transform.TransformPoint(targetPosition).ToString());

                
                
                MoveFSM.SetPath(targetPosition);

                //NavAgent.SetDestination(targetPosition);

                nextWander = Time.time + wanderInterval;
            }

        }
        else
        {
            //Transition(PursuitStates.seek);
        }

    }

    #endregion 

    #endregion
}

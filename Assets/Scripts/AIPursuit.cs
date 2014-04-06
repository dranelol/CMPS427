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
    private NavMeshAgent NavAgent;
    private Entity entity;
    private CombatFSM combatFSM;

    private GameObject currentTarget = null;

    private GameManager gameManager;

    private List<Ability> _abilityList = new List<Ability>(); // List of usuable abilities. This will be sorted by cooldown time then damagemod (for now)


    
    public float fleeDistance; //Max distance to travel from the position the enemy starts fleeing from.
    public float fleeTime; //Time the enemy will flee before returning to attack
    private Vector3 fleeStartLocation; //The position the enemy starts to flee from. 
    private float fleeEnd; //Future time the enemy will stop fleeing.
    private bool hasFled;
    
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
        NavAgent = GetComponent<NavMeshAgent>();
        entity = GetComponent<Entity>();
        combatFSM = GetComponent<CombatFSM>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();


        hasFled = false;
        
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
        Vector3 newdirection = ((transform.position - currentTarget.transform.position).normalized * fleeDistance);



        Vector3 targetPosition = Rotations.RotateAboutY(newdirection, UnityEngine.Random.Range(-90f, 90f));

        

        targetPosition += transform.position;
        targetPosition.y = transform.position.y;

        

        MoveFSM.SetPath(targetPosition);
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

        if ((entity.currentHP < (entity.maxHP * 0.2f)) && hasFled == false)
        {

            Transition(PursuitStates.flee);
        }
        else
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
    }

    #endregion

    #region attack functions

    void attack_Update()
    {

        if ((entity.currentHP < (entity.maxHP * 0.2f)) && hasFled == false)
        {

            Transition(PursuitStates.flee);
        }
        else
        {

            if (currentTarget != null && entity.abilityManager.activeCoolDowns[0] <= Time.time)
            {
                combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                Debug.DrawRay(transform.position, currentTarget.transform.position - transform.position, Color.blue, 0.1f);

                if (entity.abilityManager.abilities[0].AttackType == AttackType.MELEE)
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                    entity.abilityManager.abilities[0].AttackHandler(gameObject, entity, false);
                }

                else if (entity.abilityManager.abilities[0].AttackType == AttackType.PROJECTILE)
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                    // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                    // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object

                    //entity.abilityManager.abilities[0].SpawnProjectile(gameObject, 2, false);

                }
                else
                {
                    combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                    entity.abilityManager.abilities[0].AttackHandler(gameObject, entity, false);


                }

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
    }

    #endregion

    #region flee functions

    IEnumerator flee_EnterState()
    {
        fleeStartLocation = transform.position;
        fleeEnd = Time.time + fleeTime;
        hasFled = true;
        Flee();
        yield break;
    }
    
    void flee_Update()
    {

        

        if (fleeEnd < Time.time)
        {

            Transition(PursuitStates.seek);
            
            
        }
        else if(entity.currentHP <= 0)
        {
            StopPursuit();
        }



    }


    #endregion

    #endregion
}

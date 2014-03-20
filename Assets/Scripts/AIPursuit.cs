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
    }

    private MovementFSM MoveFSM;
    private NavMeshAgent NavAgent;
    private Entity entity;
    private CombatFSM combatFSM;

    private GameObject currentTarget = null;

    private List<Ability> _abilityList = new List<Ability>(); // List of usuable abilities. This will be sorted by cooldown time then damagemod (for now)
    
    void Awake()
    {
        SetupMachine(PursuitStates.inactive);

        HashSet<Enum> inactiveTransitions = new HashSet<Enum>();
        inactiveTransitions.Add(PursuitStates.seek);
        AddTransitionsFrom(PursuitStates.inactive, inactiveTransitions);

        AddAllTransitionsFrom(PursuitStates.seek);
        AddAllTransitionsTo(PursuitStates.seek);
        AddAllTransitionsTo(PursuitStates.inactive);

        StartMachine(PursuitStates.inactive);

        MoveFSM = GetComponent<MovementFSM>();
        NavAgent = GetComponent<NavMeshAgent>();
        entity = GetComponent<Entity>();
        combatFSM = GetComponent<CombatFSM>();
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
            Debug.Log("Enemy Ability 1 Cooldown Left: " + timeLeft.ToString());
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

    #endregion
}

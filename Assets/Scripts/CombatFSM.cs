using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CombatFSM : StateMachine
{
    private bool timeLocked = false;
    public bool attack = false;
    private float lockedTime = 0;

    public enum CombatStates
    {
        idle,
        attacking,
        combatLocked
    }

	// Use this for initialization
	void Start () {

        List<Enum> idleTransitions = new List<Enum>();
        idleTransitions.Add(CombatStates.attacking);
        idleTransitions.Add(CombatStates.combatLocked);

        List<Enum> attackingTransitions = new List<Enum>();
        attackingTransitions.Add(CombatStates.combatLocked);

        List<Enum> combatLockedTransitions = new List<Enum>();
        combatLockedTransitions.Add(CombatStates.idle);

        Transitions.Add(CombatStates.idle, idleTransitions);
        Transitions.Add(CombatStates.attacking, attackingTransitions);
        Transitions.Add(CombatStates.combatLocked, combatLockedTransitions);

        StartMachine(CombatStates.idle);
	}

    #region public functions

    public void Attack(float time = 0)
    {
        attack = true;
        lockedTime = Mathf.Max(time, 0);
        Transition(CombatStates.attacking);
    }

    #endregion

    #region idle functions

    void idle_Update()
    {
        Debug.Log("Idle");
        if (attack == true && timeLocked == false)
        {
            Transition(CombatStates.attacking);
        }
        else if (timeLocked == true)
        {
            Transition(CombatStates.combatLocked);
        }
    }

    #endregion

    #region attacking functions

    void attacking_Update()
    {
        Debug.Log("Attacking");
        timeLocked = true;
        Transition(CombatStates.combatLocked);
    }

    #endregion

    #region combatLocked functions

    void combatLocked_Update()
    {
        Debug.Log("Combat Locked");
        if (timeLocked == true)
        {
            lockedTime -= Time.deltaTime;

            if (lockedTime <= 0)
            {
                Transition(CombatStates.idle);
            }
        }
    }

    IEnumerator combatLocked_ExitState()
    {
        timeLocked = false;
        lockedTime = 0;
        yield break;
    }

    #endregion
}

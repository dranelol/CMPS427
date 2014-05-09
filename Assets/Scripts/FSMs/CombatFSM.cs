using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CombatFSM : StateMachine
{
    private bool timeLocked = false;
    private bool attack = false;
    private float lockedTime = 1.0f;

    public enum CombatStates
    {
        idle,
        attacking,
        combatLocked
    }

	// Use this for initialization
	void Start () 
    {
        SetupMachine(CombatStates.idle);

        HashSet<Enum> idleTransitions = new HashSet<Enum>();
        idleTransitions.Add(CombatStates.attacking);
        idleTransitions.Add(CombatStates.combatLocked);

        HashSet<Enum> attackingTransitions = new HashSet<Enum>();
        attackingTransitions.Add(CombatStates.combatLocked);
        attackingTransitions.Add(CombatStates.attacking);

        HashSet<Enum> combatLockedTransitions = new HashSet<Enum>();
        combatLockedTransitions.Add(CombatStates.idle);

        AddTransitionsFrom(CombatStates.idle, idleTransitions);
        AddTransitionsFrom(CombatStates.attacking, attackingTransitions);
        AddTransitionsFrom(CombatStates.combatLocked, combatLockedTransitions);

        StartMachine(CombatStates.idle);
	}

    #region public functions

    public void Attack(float time)
    {
        attack = true;
        if (timeLocked == false)
        {
            lockedTime = Mathf.Max(time, 0);
            Transition(CombatStates.attacking);
        }
    }

    public bool IsIdle()
    {
        if ((CombatStates)CurrentState == CombatStates.idle)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    #endregion

    #region idle functions

    void idle_Update()
    {
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
        timeLocked = true;
        attack = false;
        Transition(CombatStates.combatLocked);
    }

    #endregion

    #region combatLocked functions

    void combatLocked_Update()
    {
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
        yield break;
    }

    #endregion
}



using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CursorFSM : StateMachine
{
    public enum CursorStates
    {
        idle,
        ingame,
        menu
    }

    void Start()
    {
        List<Enum> idleTransitions = new List<Enum>();

        idleTransitions.Add(CursorStates.ingame);
        idleTransitions.Add(CursorStates.menu);

        List<Enum> ingameTransitions = new List<Enum>();

        ingameTransitions.Add(CursorStates.idle);
        ingameTransitions.Add(CursorStates.menu);

        List<Enum> menuTransitions = new List<Enum>();

        menuTransitions.Add(CursorStates.idle);
        menuTransitions.Add(CursorStates.ingame);

        Transitions.Add(CursorStates.idle, idleTransitions);
        Transitions.Add(CursorStates.ingame, ingameTransitions);
        Transitions.Add(CursorStates.menu, menuTransitions);

        StartMachine(CursorStates.idle);
    }

    #region idle functions
    #endregion

    #region ingame functions
    #endregion


    #region menu functions

    #endregion
}

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
        SetupMachine(CursorStates.idle);

        HashSet<Enum> idleTransitions = new HashSet<Enum>();

        idleTransitions.Add(CursorStates.ingame);
        idleTransitions.Add(CursorStates.menu);

        HashSet<Enum> ingameTransitions = new HashSet<Enum>();

        ingameTransitions.Add(CursorStates.idle);
        ingameTransitions.Add(CursorStates.menu);

        HashSet<Enum> menuTransitions = new HashSet<Enum>();

        menuTransitions.Add(CursorStates.idle);
        menuTransitions.Add(CursorStates.ingame);

        AddTransitionsFrom(CursorStates.idle, idleTransitions);
        AddTransitionsFrom(CursorStates.ingame, ingameTransitions);
        AddTransitionsFrom(CursorStates.menu, menuTransitions);

        StartMachine(CursorStates.idle);
    }

    #region idle functions
    IEnumerator idle_EnterState()
    {
        Transition(CursorStates.ingame);
        yield break;
    }
    #endregion

    #region ingame functions

    void ingame_Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            Plane cursorPlane = new Plane(Vector3.up, transform.position);
            Ray theRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0;

            Vector3 animationPosition = Vector3.zero;

            if (cursorPlane.Raycast(theRay, out hitdist))
            {
                animationPosition = theRay.GetPoint(hitdist);
            }
            //Debug.Log("playing swooshy swoosh 'MOVING HERE' animation at: " + animationPosition.ToString());
        }
    }
    #endregion


    #region menu functions

    #endregion
}

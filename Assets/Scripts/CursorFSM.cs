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
    IEnumerator ingame_EnterState()
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
            Debug.Log("playing swooshy swoosh 'MOVING HERE' animation at: " + animationPosition.ToString());
        }
    }
    #endregion


    #region menu functions

    #endregion
}

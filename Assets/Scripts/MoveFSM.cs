using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MoveFSM : StateMachine {
    public enum MoveStates
    {
        idle,
        move,
        run
    }

	// Use this for initialization
	void Start () 
    {
        // set up transitions here
        List<Enum> idleTransitions = new List<Enum>();
        idleTransitions.Add(MoveStates.move);

        List<Enum> moveTransitions = new List<Enum>();
        moveTransitions.Add(MoveStates.idle);
        moveTransitions.Add(MoveStates.run);

        List<Enum> runTransitions = new List<Enum>();
        runTransitions.Add(MoveStates.move);

        Transitions.Add(MoveStates.idle, idleTransitions);
        Transitions.Add(MoveStates.move, moveTransitions);
        Transitions.Add(MoveStates.run, runTransitions);

        // start state machine
        StartMachine(MoveStates.idle);
	}
	
	// Update is called once per frame
	

    #region idle functions
    
    void idle_Update()
    {
        Debug.Log("idle update");
    }

    #endregion


    #region move functions
    void move_Update()
    {
        Debug.Log("move update");
    }
    #endregion

    #region run functions
    void run_Update()
    {
        Debug.Log("run update");
    }
    #endregion
}

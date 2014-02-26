using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Reflection;

public abstract class StateMachine : MonoBehaviour
{
    #region fields

    public bool Debugging = false;

    /// <summary>
    /// Returns whether or not the current state's next state is itself
    /// </summary>
    private bool reflexiveTransition;



    private string FSMName;

    private Type stateType = null;

    private Enum currentState = null;

    public Enum CurrentState
    {
        get
        {
            return currentState;
        }
    }

    private Enum previousState = null;

    public Enum PreviousState
    {
        get
        {
            return previousState;
        }
    }

    private Dictionary<Enum, HashSet<Enum>> Transitions = null; 
    #endregion

    #region constructors
    #endregion

    #region methods

    #region static default methods
    static void DefaultFunction()
    {
    }

    static void DefaultCollider(Collider other)
    {
    }

    static void DefaultCollision(Collision other)
    {
    }

    static IEnumerator DefaultCoroutine()
    {
        yield break;
    }

    #endregion

    #region delegate functions

    public Action DoUpdate = DefaultFunction;
    public Action DoLateUpdate = DefaultFunction;
    public Action DoFixedUpdate = DefaultFunction;

    public Action<Collider> DoOnTriggerEnter = DefaultCollider;
    public Action<Collider> DoOnTriggerStay = DefaultCollider;
    public Action<Collider> DoOnTriggerExit = DefaultCollider;
    public Action<Collision> DoOnCollisionEnter = DefaultCollision;
    public Action<Collision> DoOnCollisionStay = DefaultCollision;
    public Action<Collision> DoOnCollisionExit = DefaultCollision;

    public Action DoOnMouseEnter = DefaultFunction;
    public Action DoOnMouseUp = DefaultFunction;
    public Action DoOnMouseDown = DefaultFunction;
    public Action DoOnMouseOver = DefaultFunction;
    public Action DoOnMouseExit = DefaultFunction;
    public Action DoOnMouseDrag = DefaultFunction;
    public Action DoOnGUI = DefaultFunction;

    public Func<IEnumerator> EnterState = DefaultCoroutine;
    public Func<IEnumerator> ExitState = DefaultCoroutine;
    public Func<IEnumerator> StayState = DefaultCoroutine;

    #endregion

    /// <summary>
    /// Sets up the state machine by giving it an example of a state it describes
    /// </summary>
    /// <param name="typeSet">Example of type of states handled by this FSM</param>
    public void SetupMachine(Enum typeSet)
    {
        Transitions = new Dictionary<Enum, HashSet<Enum>>();
        stateType = typeSet.GetType();
        var stateTypes = Enum.GetValues(stateType);

        

        foreach(Enum state in stateTypes)
        {
            Transitions.Add(state, new HashSet<Enum>());
        }
    }

    /// <summary>
    /// Starts the FSM. Call this after setting up the entire FSM and transitions
    /// </summary>
    /// <param name="startState">The state from which the FSM is started</param>
    public void StartMachine(Enum startState)
    {
        if (currentState == null)
        {
            currentState = startState;
        }
        
        ConfigureCurrentState();
        FSMName = this.GetType().Name;
    }

    /// <summary>
    /// Transition to the next state given by "nextState"
    /// </summary>
    /// <param name="nextState"></param>
    public void Transition(Enum nextState)
    {
        if (Debugging == true)
        {
            Debug.Log("Transition from " + currentState.ToString() + " to " + nextState.ToString());
        }
        if (currentState == null)
        {
            throw new NullReferenceException("null current state");
        }

        //if the hashset of transitions contains a valid transition to next state
        if (Transitions[currentState].Contains(nextState))
        {
            // 0. set/verify all needed values
            // 1. need to call current state's exit
            // 2. set up new state functions
            // 3. need to call new state enter

            previousState = currentState;
            currentState = nextState;

            ConfigureCurrentState();

        }

        // a transition from current to next doesnt exist
        else
        {
            throw new NullReferenceException("no valid transition from: " 
                + currentState.ToString() 
                + " to next state: " 
                + nextState.ToString()
                + " on object " + gameObject.name
                );
        }

    }
    /// <summary>
    /// Returns whether or not there is a valid transition from "fromState" to "toState"
    /// </summary>
    /// <param name="fromState"></param>
    /// <param name="toState"></param>
    /// <returns></returns>
    public bool IsValidTransition(Enum fromState, Enum toState)
    {
        return Transitions[fromState].Contains(toState);
    }

    /// <summary>
    /// magic.
    /// </summary>
    private void ConfigureCurrentState()
    {
       // Debug.Log("previous state: " + previousState);
       // Debug.Log("current state: " + currentState);

        if (previousState != null)
        {
            if (previousState.ToString() == currentState.ToString())
            {

                if (Debugging == true)
                {
                    Debug.Log("calling stay state for state: " + currentState.ToString() + " of fsm: " + FSMName);
                }

                StartCoroutine(StayState());
                // dont need to do anything else, we're in a reflexive transition
                return;
            }
            
           
        }

        // call exit state function for old current state
        if (ExitState != null)
        {
            if (Debugging == true)
            {
                Debug.Log("calling exit state for state: " + currentState.ToString() + " of fsm: " + FSMName);
            }
            StartCoroutine(ExitState());
        }

        // update all state function delegates

        DoUpdate = ConfigureDelegate<Action>("Update", DefaultFunction);
        DoOnGUI = ConfigureDelegate<Action>("OnGUI", DefaultFunction);
        DoLateUpdate = ConfigureDelegate<Action>("LateUpdate", DefaultFunction);
        DoFixedUpdate = ConfigureDelegate<Action>("FixedUpdate", DefaultFunction);
        DoOnMouseUp = ConfigureDelegate<Action>("OnMouseUp", DefaultFunction);
        DoOnMouseDown = ConfigureDelegate<Action>("OnMouseDown", DefaultFunction);
        DoOnMouseEnter = ConfigureDelegate<Action>("OnMouseEnter", DefaultFunction);
        DoOnMouseExit = ConfigureDelegate<Action>("OnMouseExit", DefaultFunction);
        DoOnMouseDrag = ConfigureDelegate<Action>("OnMouseDrag", DefaultFunction);
        DoOnMouseOver = ConfigureDelegate<Action>("OnMouseOver", DefaultFunction);
        DoOnTriggerEnter = ConfigureDelegate<Action<Collider>>("OnTriggerEnter", DefaultCollider);
        DoOnTriggerExit = ConfigureDelegate<Action<Collider>>("OnTriggerExit", DefaultCollider);
        DoOnTriggerStay = ConfigureDelegate<Action<Collider>>("OnTriggerEnter", DefaultCollider);
        DoOnCollisionEnter = ConfigureDelegate<Action<Collision>>("OnCollisionEnter", DefaultCollision);
        DoOnCollisionExit = ConfigureDelegate<Action<Collision>>("OnCollisionExit", DefaultCollision);
        DoOnCollisionStay = ConfigureDelegate<Action<Collision>>("OnCollisionStay", DefaultCollision);
        EnterState = ConfigureDelegate<Func<IEnumerator>>("EnterState", DefaultCoroutine);
        ExitState = ConfigureDelegate<Func<IEnumerator>>("ExitState", DefaultCoroutine);
        StayState = ConfigureDelegate<Func<IEnumerator>>("StayState", DefaultCoroutine);
        // call enter state function for new current state
        if (EnterState != null)
        {
            if (Debugging == true)
            {
                Debug.Log("calling enter state for state: " + currentState.ToString() + " of fsm: " + FSMName);
            }
            StartCoroutine(EnterState());
        }
        
    }

    /// <summary>
    /// More magic.
    /// </summary>
    /// <typeparam name="T">Magic.</typeparam>
    /// <param name="methodRoot">More magic.</param>
    /// <param name="Default">Voodoo.</param>
    /// <returns></returns>
    private T ConfigureDelegate<T>(string methodRoot, T Default) where T : class
    {
        var mtd = GetType().GetMethod(currentState.ToString() + "_" + methodRoot, System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod);

        if (mtd != null)
        {
            if (Debugging == true)
            {
                Debug.Log("setting delegate for: " + currentState.ToString() + "_" + methodRoot);
            }
            return Delegate.CreateDelegate(typeof(T), this, mtd) as T;
        }
        else
        {
            return Default;
        }
    }

    /// <summary>
    /// Adds a transition from "state" to every state in statesList
    /// </summary>
    /// <param name="state">State to transition from</param>
    protected void AddAllTransitionsFrom(Enum state)
    {
        foreach (Enum transition in Transitions.Keys)
        {
            AddTransition(state, transition);
        }
    }

    /// <summary>
    /// Adds a transition from every state in statesList to "state"
    /// </summary>
    /// <param name="state">State to tranistion to</param>
    protected void AddAllTransitionsTo(Enum state)
    {
        foreach (Enum transition in Transitions.Keys)
        {
            AddTransition(transition, state);
        }     
    }

    /// <summary>
    /// Adds a transition from "state" to every state in "transitions"
    /// </summary>
    /// <param name="state">State to transition from</param>
    /// <param name="transitions">States to transitions to</param>
    protected void AddTransitionsFrom(Enum state, HashSet<Enum> transitions)
    {
        if (transitions.Count > 0)
        {
            foreach (Enum transition in transitions)
            {
                AddTransition(state, transition);
            }
        }
    }

    /// <summary>
    /// Adds a transition from each state in "transitions" to "state"
    /// </summary>
    /// <param name="state">State to transitions to</param>
    /// <param name="transitions">States to transition from</param>
    protected void AddTransitionsTo(Enum state, HashSet<Enum> transitions)
    {
        if (transitions.Count > 0)
        {
            foreach (Enum transition in transitions)
            {
                AddTransition(transition, state);
            }
        }
    }

    /// <summary>
    /// adds transition "transition" to state "state"
    /// </summary>
    /// <param name="state"></param>
    /// <param name="transition"></param>
    protected void AddTransition(Enum state, Enum transition)
    {
        // check state for statetype
        if(state.GetType() != stateType)
        {
            throw new Exception("Attempting to transition from an invalid state type");
        }

        else
        {
            // check transition for statetype
            if (transition.GetType() != stateType)
            {
                throw new Exception("Attempting to transition to an invalid state type");
            }

            else
            {
                // we can add the transition!
                Transitions[state].Add(transition);
            }
        } 
    }
    #region game loop methods

    void Update()
    {
        if (Debugging == true)
        {
            Debug.Log("calling update for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoUpdate();
    }

    void LateUpdate()
    {
        if (Debugging == true)
        {
            Debug.Log("calling late update for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoLateUpdate();
    }

    void OnMouseEnter()
    {
        if (Debugging == true)
        {
            Debug.Log("calling on mouse enter for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnMouseEnter();
    }

    void OnMouseUp()
    {
        if (Debugging == true)
        {
            Debug.Log("calling on mouse up for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnMouseUp();
    }

    void OnMouseDown()
    {
        if (Debugging == true)
        {
            Debug.Log("calling on mouse down for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnMouseDown();
    }

    void OnMouseExit()
    {
        if (Debugging == true)
        {
            Debug.Log("calling on mouse exit for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnMouseExit();
    }

    void OnMouseDrag()
    {
        if (Debugging == true)
        {
            Debug.Log("calling on mouse drag for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnMouseDrag();
    }

    void FixedUpdate()
    {
        if (Debugging == true)
        {
            Debug.Log("calling fixed update for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoFixedUpdate();
    }
    void OnTriggerEnter(Collider other)
    {
        if (Debugging == true)
        {
            Debug.Log("calling on trigger enter for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnTriggerEnter(other);
    }
    void OnTriggerExit(Collider other)
    {
        if (Debugging == true)
        {
            Debug.Log("calling on trigger exit for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnTriggerExit(other);
    }
    void OnTriggerStay(Collider other)
    {
        if (Debugging == true)
        {
            Debug.Log("calling on trigger stay for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnTriggerStay(other);
    }
    void OnCollisionEnter(Collision other)
    {
        if (Debugging == true)
        {
            Debug.Log("calling on collision enter for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnCollisionEnter(other);
    }
    void OnCollisionExit(Collision other)
    {
        if (Debugging == true)
        {
            Debug.Log("calling on collision exit for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnCollisionExit(other);
    }
    void OnCollisionStay(Collision other)
    {
        if (Debugging == true)
        {
            Debug.Log("calling on collision stay for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnCollisionStay(other);
    }
    void OnGUI()
    {
        if (Debugging == true)
        {
            Debug.Log("calling on gui for state: " + currentState.ToString() + " of fsm: " + FSMName);
        }
        DoOnGUI();
    }

    #endregion

    #endregion

}

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

    private Enum previousState;

    public Enum PreviousState
    {
        get
        {
            return previousState;
        }
    }

    public Dictionary<Enum, HashSet<Enum>> Transitions = new Dictionary<Enum, HashSet<Enum>>();
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

    #endregion

    public void SetupMachine(Enum stateType)
    {
        stateType = typeSet;
        Debug.Log("setting type: " + stateType.ToString());
    }

    //spins up the FSM, called on start
    public void StartMachine(Enum startState)
    {
        if (currentState == null)
        {
            currentState = startState;
        }
        ConfigureCurrentState();
        FSMName = this.GetType().Name;
    }

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

        //returns the valid transitions for current state
        List<Enum> searchTransitions = Transitions[currentState];

        //if the returned list contains a valid transition to next state
        if (searchTransitions.Contains(nextState))
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
            throw new NullReferenceException("no valid transition to next state: " + nextState.ToString());
        }

    }

    void ConfigureCurrentState()
    {
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

    T ConfigureDelegate<T>(string methodRoot, T Default) where T : class
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

    protected void Setup(Type typeSet)
    {

        


    }

    private void AddAllTransitions(Enum toState)
    {

    }

    private void AddTransitionsFrom(Enum fromState, HashSet<Enum> states)
    {
        if (fromState.GetType() != stateType)
        {
            throw new Exception("Attempting to add an invalid state type");
        }

        else
        {
            if (states.Count > 0)
            {
                // check for type of each transition
                // if valid, add to transition set for that state
                foreach (Enum toState in states)
                {

                    if (toState.GetType() != stateType)
                    {
                        throw new Exception("Attempting to add an invalid state transition to state");
                    }

                    AddTransition(toState, fromState);
                }
            }
        }
    }

    private void AddTransitionsTo(Enum state, HashSet<Enum> transitions)
    {

    }

    /// <summary>
    /// adds transition "transition" to state "state"
    /// </summary>
    /// <param name="state"></param>
    /// <param name="transition"></param>
    private void AddTransition(Enum state, Enum transition)
    {

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

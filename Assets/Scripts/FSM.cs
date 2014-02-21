/* Disclaimer!
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * The following program named FSM was written entirely by Ryan M. Adair
 * on a personal computer. Proper credit should be given to the author
 * for its use.
 * © 2014 Ryan M. Adair.  All rights reserved.
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 */
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    private Dictionary<T, StateObject> States;

    private T start_state = default(T);
    private T current_state = default(T);

    private bool active = false;

    #region Public Methods

    /// <summary>
    /// Initialize a new finite state machine. The FSM class only accepts an enumeration as the type.
    /// The typed enumeration can't be empty.
    /// </summary>
    public FSM()
    {
        if (typeof(T).BaseType != typeof(Enum))
        {
            throw new InvalidCastException("Only enumeration types may be given as the finite state machine type.");
        }

        else
        {
            Array enumArray = Enum.GetValues(typeof(T));

            if (enumArray.Length < 1)
            {
                throw new NullReferenceException("The finite state machine cannot be initialized with an empty enumeration.");
            }

            else
            {
                States = new Dictionary<T, StateObject>();

                foreach (T enum_state in enumArray)
                {
                    StateObject new_state = new StateObject(enum_state);
                    States.Add(enum_state, new_state);
                }

                start_state = (T)enumArray.GetValue(0);
                current_state = start_state;
            }
        }
    }

    /// <summary>
    /// Add a transition from [a] to every state in the list [b]. 
    /// If [b] is empty, this method will instead add transitions
    /// to all possible states from [a]. New transitions cannot be defined 
    /// once the finite state machine is active.
    /// </summary>
    /// <param name="a">The state to transition from.</param>
    /// <param name="b">The list of states to transition to.</param>
    public void AddTransitionsFromAToB(T a, params T[] b)
    {
        if (!active)
        {
            if (b.Count<T>() <= 0)
            {
                b = States.Keys.ToArray<T>();
            }

            foreach (T to in b)
            {
                AddTransition(a, to);
            }
        }
    }

    /// <summary>
    /// Add a transition to [a] from every state in the list [b]. 
    /// If [b] is empty, this method will instead add transitions
    /// from all defined states to [a]. New transitions cannot be defined 
    /// once the finite state machine is active.
    /// </summary>
    /// <param name="a">The state to transition to.</param>
    /// <param name="b">The list of states to transition from.</param>
    public void AddTransitionsToAFromB(T a, params T[] b)
    {
        if (!active)
        {
            if (b.Count<T>() <= 0)
            {
                b = States.Keys.ToArray<T>();
            }

            foreach (T from in b)
            {
                AddTransition(from, a);
            }
        }
    }

    /// <summary>
    /// Adds transition behavior for the specified state [state]. [OnEnter] refers to
    /// the name of the method to call when a transition is first made to [state]. 
    /// [OnStay] refers to the name of the method to call when a transition is made 
    /// from [state] to [state] (reflexive transition). [OnExit] refers to the 
    /// name of the method to call before a transition is made from [state] to another
    /// state. The parameters [OnStay] and [OnExit] are both optional parameters. 
    /// Furthermore, if no action should be taken on enter, stay, and/or exit 
    /// transitions, 'null' can be passed in place of a method name. New transition
    /// behaviors cannot be defined once the finite state machine is active.
    /// </summary>
    /// <param name="state">The state for which to define transition behavior.</param>
    /// <param name="OnEnter">The name of the method to call upon entering a new state.</param>
    /// <param name="OnStay">The name of the method to call upon a reflexive transition.</param>
    /// <param name="OnExit">The name of the method to call before leaving the current state.</param>
	public void AddTransitionBehavior(T state, Action OnEnter, Action OnStay = null, Action OnExit = null)
	{
        if (!active)
        {
            if (States.ContainsKey(state))
            {
                States[state].OnTransitionEnter = OnEnter;
                States[state].OnTransitionStay = OnStay;
                States[state].OnTransitionExit = OnExit;
            }

            else
            {
                throw new NullReferenceException("Transition behavior cannot be defined for states that do not exist in the typed enumeration.");
            }
        }
	}

    /// <summary>
    /// Begin the finite state machine. The specified state [state] will 
    /// become the start state. The start state must be exist in the typed enumeration.
    /// While the finite state machine is active, no new transitions or transition
    /// behaviors may be defined.
    /// </summary>
    /// <param name="state">The state in which the finite state machine should start.</param>
    public void Start(T state)
    {
        if (!active)
        {
            if (States.ContainsKey(state))
            {
                start_state = state;
                current_state = start_state;
                active = true;
            }

            else
                throw new NullReferenceException("The start state must exist in the typed enumeration.");
        }
    }

    /// <summary>
    /// Check if there is a valid transition defined between the states [a]
    /// and [b]. 
    /// </summary>
    /// <param name="a">The state indicating where the transition would start.</param>
    /// <param name="b">The state indicating where the transition would end.</param>
    /// <returns>Returns true if there is a valid transition defined, false otherwise.</returns>
    public bool CheckTransition(T a, T b)
    {
        try
        {
            return States[a].valid_transitions.Contains(b);
        }

        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Attempt to transition from the current state to the specified state [state]. 
    /// Successful if there is a transition defined between these states. Transitions 
    /// cannot be made before the finite state machine is active.
    /// </summary>
    /// <param name="state">The state to transition to.</param>
    /// <returns>Returns true if a transition was made successfully, false otherwise.</returns>
    public bool Transition(T state)
    {
        if (active)
        {
            if (States.ContainsKey(state))
            {
                if (CheckTransition(current_state, state))
                {
                    if (!Equals(current_state, state))
                    {
                        if (States[current_state].OnTransitionExit != null)
                        {
                            States[current_state].OnTransitionExit.Invoke();
                        }

                        current_state = state;

                        if (States[current_state].OnTransitionEnter != null)
                        {
                            States[current_state].OnTransitionEnter.Invoke();
                        }
                    }

                    else
                    {
                        if (States[current_state].OnTransitionStay != null)
                        {
                            States[current_state].OnTransitionStay.Invoke();
                        }
                    }

                    return true;
                }

                else
                    return false;
            }

            else
                return false;
        }

        return false;
    }

    /// <summary>
    /// Returns the start state of the finite state machine. The default start state
    /// before Start is called is always the first member of the enumeration.
    /// </summary>
    /// <returns>The start state.</returns>
    public T Start_State
    {
        get { return start_state; }
    }

    /// <summary>
    /// Returns the current state of the finite state machine. The default current state
    /// before Start is called is always the first member of the enumeration.
    /// </summary>
    /// <returns>The current state.</returns>
    public T Current_State
    {
        get { return current_state; }
    }

    /// <summary>
    /// Returns the status of the finite state machine.
    /// </summary>
    /// <returns>Returns true if the finite state machine is active, false otherwise.</returns>
    public bool Active
    {
        get { return active; }
    }

    #endregion

    #region Other

    private void AddTransition(T from, T to)
    {
        if (States.ContainsKey(from) && States.ContainsKey(to))
        {
            if (!CheckTransition(from, to))
                States[from].valid_transitions.Add(to);
        }

        else
        {
            throw new NullReferenceException("One or more of the states specified does not exist in the typed enumeration.");
        }
    }

    private bool Equals(T a, T b)
    {
        try
        {
            return Enum.GetName(typeof(T), a) == Enum.GetName(typeof(T), b);
        }

        catch
        {
            return false;
        }
    }

    private class StateObject
    {
        public HashSet<T> valid_transitions;

        public Action OnTransitionEnter = null;

        public Action OnTransitionStay = null;

        public Action OnTransitionExit = null;

        public StateObject(T state)
        {
            valid_transitions = new HashSet<T>();
            valid_transitions.Add(state);
        }
    }

    #endregion
}
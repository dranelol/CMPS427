using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIStateMachine : UIState {

	private Dictionary<int, UIState> states;
	private UIState currentState, defaultState;

    public UIStateMachine(int id, UIController controller)
        : base(id, controller)
    {
        states = new Dictionary<int, UIState>();
    }

    public void AddState(UIState state)
    {
        states[state.ID] = state;
    }

    public void AddDefaultState(UIState state)
    {
        AddState(state);

        defaultState = currentState = state;
    }

    public void TransitionState(int id)
    {
        currentState.Exit();
        currentState = states[id];
        currentState.Enter();
    }

    public override int CheckTransitions()
    {
        return ID;
    }

    public override void Update()
    {
        currentState.Update();
        int nextState = currentState.CheckTransitions();

        if (nextState != currentState.ID)
            TransitionState(nextState);
    }

    public override void OnGui()
    {
        currentState.OnGui();
    }
}

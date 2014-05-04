using UnityEngine;
using System.Collections;

public abstract class UIState 
{
	private int stateId;
    private UIController controller;

    public int ID { get { return stateId; } }
    public UIController Controller { get { return controller; } }

	public UIState(int stateId, UIController controller) {
		this.stateId = stateId;
        this.controller = controller;
	}
	
	/// <summary>
	/// Each state is responsible for checking transitions.
	/// Do whatever you gotta do bro.
	/// </summary>
	/// <returns>ID representing which state to transition to. If not transition is required, return this ID.</returns>
    public virtual int CheckTransitions() { return (int)controller.GuiState; }

	/// <summary>
	/// Update this instance.
	/// </summary>
	public abstract void Update();

	/// <summary>
	/// Raises the GUI event. Render whatever UI elements need to exist in this state.
	/// </summary>
	public abstract void OnGui();

    public virtual void Enter() { }
    public virtual void Exit() 
    {
        Controller.PlayerController.MouseOverGUI = false;
    }
}

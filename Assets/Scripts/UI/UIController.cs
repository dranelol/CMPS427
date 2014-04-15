using UnityEngine;
using System.Collections;

// This class listesns for input strictly for UI
// transitions.
public class UIController : MonoBehaviour {

	public enum States {
        MACHINE_ROOT,
		INGAME,
		MENU,
		CHARACTER
	}

	private States guiState;
	public States GuiState 
    { 
        get { return guiState; }
        set { guiState = value; }
    }

    private UIStateMachine stateMachine;

	// Use this for initialization
	void Start () {
		guiState = States.INGAME;

        stateMachine = new UIStateMachine((int)States.MACHINE_ROOT, this);
        stateMachine.AddDefaultState(new InGameUI((int)States.INGAME, this));
        stateMachine.AddState(new MenuUI((int)States.MENU, this));
        stateMachine.AddState(new CharacterUI((int)States.CHARACTER, this));
	}
	
	// Update is called once per frame
	void Update () {
		/* Menu (Settings, Quit, etc.)
		 * 
		 * Can be accessed from any UI state.
		 * If menu key is pressed and current UI state is menu,
		 * return to ingame UI.
		 * 
		 */
		if (Input.GetKeyUp(KeyCode.Escape)) {
			if (guiState == States.MENU)
				guiState = States.INGAME;
			else
				guiState = States.MENU;
		}

		/* Character Info Screen
		 * 
		 * Can only be accessed from iteslf and ingame UI.
		 */
		if (Input.GetKeyUp(KeyCode.I)) {
			if (guiState == States.CHARACTER)
				guiState = States.INGAME;
			else if (guiState == States.INGAME)
				guiState = States.CHARACTER;
		}

        stateMachine.Update();
	}

    void OnGUI()
    {
        stateMachine.OnGui();
    }
}

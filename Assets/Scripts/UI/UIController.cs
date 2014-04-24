using UnityEngine;
using System.Collections;

// This class listesns for input strictly for UI
// transitions.
public class UIController : MonoBehaviour {

	public enum States {
        MACHINE_ROOT,
		INGAME,
		MENU,
		CHARACTER,
		LEVELUP
	}

	private States guiState;
	public States GuiState 
    { 
        get { return guiState; }
        set { guiState = value; }
    }

    public GUIStyle style;

    private PlayerEntity player;
    public PlayerEntity Player { get { return player; } }

    private Vector2 nativeResolution;
    public Vector2 NativeResolution { get { return nativeResolution; } }

    public Camera Camera { get { return GameObject.FindGameObjectWithTag("UI Camera").GetComponent<Camera>(); } }

    private UIStateMachine stateMachine;

	// Use this for initialization
	void Start () {
        nativeResolution.x = Screen.width;
        nativeResolution.y = Screen.height;

        player = GameObject.FindWithTag("Player").GetComponent<PlayerEntity>();

		guiState = States.INGAME;

        stateMachine = new UIStateMachine((int)States.MACHINE_ROOT, this);
        stateMachine.AddDefaultState(new InGameUI((int)States.INGAME, this));
        stateMachine.AddState(new MenuUI((int)States.MENU, this));
        stateMachine.AddState(new CharacterUI((int)States.CHARACTER, this));
		stateMachine.AddState(new LevelupUI((int)States.LEVELUP, this));
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

		/* Levelup Screen
		 * 
		 * Can only be accessed ingame. 
		 * Accessed by Key Input for testing purposes.
		 */
		if (Input.GetKeyUp (KeyCode.L)) {
			if (guiState == States.LEVELUP)
				guiState = States.INGAME;
			else if (guiState == States.INGAME)
				guiState = States.LEVELUP;
		}

        stateMachine.Update();
	}

    void OnGUI()
    {
        float rx = Screen.width / nativeResolution.x;
        float ry = Screen.height / nativeResolution.y;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, new Vector3(rx, ry, 1));

        stateMachine.OnGui();
    }
}

using UnityEngine;
using System.Collections;

public class LevelupUI : UIState 
{
	private const float WIDTH = 400;
	private const float HEIGHT = 500;
	private Rect windowDim;

	public LevelupUI(int id, UIController controller)
		: base(id, controller)
	{
		windowDim = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
	}
	public override void Enter ()
	{
		base.Enter ();
	}

	public override void Exit ()
	{
		base.Exit ();
	}

	public override void Update () 
	{
	}

	public override void OnGui ()
	{
		GUI.Window (0, windowDim, OnWindow, "Levelup");
	}

	void OnWindow(int windowID)
	{
	}
}

using UnityEngine;
using System.Collections;

public class CharacterUI : UIState {
    private const float WIDTH = 400;
    private const float HEIGHT = 500;
    private string[] HEADERS = { "Stats", "Inventory", "Skills" };

    private Rect windowDimensions;
    private int selection = 0;

    public CharacterUI(int id, UIController controller)
        : base(id, controller) 
    {
        windowDimensions = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
    }

    public override void Enter()
    {
        Debug.Log("Entering Character state.");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Character state.");
    }

    public override void Update()
    {
        
    }

    public override void OnGui()
    {
        GUI.Window(0, windowDimensions, OnWindow, "Character");
    }

    void OnWindow(int windowID)
    {
        selection = GUI.Toolbar(new Rect(10, 300, WIDTH - 20, 50), selection, HEADERS);
    }
}

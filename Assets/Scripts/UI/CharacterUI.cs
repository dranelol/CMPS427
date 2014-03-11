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

        DrawStats();
    }

    void DrawStats()
    {
        GUI.BeginScrollView(new Rect(10, 350, WIDTH, 200), Vector2.zero, new Rect(0, 0, 220, 220));

        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.Label(new Rect(0, 0, WIDTH - 20, 20), "Health: ");
        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        GUI.Label(new Rect(0, 0, WIDTH - 20, 20), "" + Controller.Player.currentAtt.Health);

        GUI.EndScrollView();
    }

    void DrawInventory()
    {

    }

    void DrawSkills()
    {

    }
}

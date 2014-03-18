using UnityEngine;
using System.Collections;

public class CharacterUI : UIState {
    private const float WIDTH = 400;
    private const float HEIGHT = 500;
    private string[] HEADERS = { "Stats", "Inventory", "Skills" };

    private Rect windowDimensions;
    private int selection = 0;
    private int yOffset = 0;
    private Vector2 scrollViewVector;

    public CharacterUI(int id, UIController controller)
        : base(id, controller) 
    {
        windowDimensions = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        scrollViewVector = Vector2.zero;
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

        if (selection == 0)
            DrawStats();
    }

    void DrawStats()
    {
        int viewSize = Controller.Player.currentAtt.StatList.Count * 30;

        scrollViewVector = GUI.BeginScrollView(new Rect(10, 350, WIDTH - 10, 150), scrollViewVector, 
            new Rect(0, 0, 10, viewSize));

        yOffset = 0;
        foreach (var pair in Controller.Player.currentAtt.StatList)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.Label(new Rect(0, yOffset, WIDTH - 30, 20), pair.Key.ToString());
            GUI.skin.label.alignment = TextAnchor.MiddleRight;
            GUI.Label(new Rect(0, yOffset, WIDTH - 30, 20), "" + pair.Value.ToString());
            yOffset += 30;
        }

        GUI.EndScrollView();
    }

    void DrawInventory()
    {

    }

    void DrawSkills()
    {

    }
}

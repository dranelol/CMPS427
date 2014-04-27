using UnityEngine;
using System.Collections;

public class CharacterUI : UIState
{
    private const float WIDTH = 400;
    private const float HEIGHT = 500;
    private const int EQUIPMENT_SLOTS = 6;
    private string[] HEADERS = { "Stats", "Inventory", "Skills" };

    private Rect windowDimensions;
    private int selection = 0;
    private int yOffset = 0;
    private Vector2 scrollViewVector;
    private Camera characterCamera;
    private GUITexture titsMcGee;

    private equipment toBeUsed;

    public CharacterUI(int id, UIController controller)
        : base(id, controller)
    {
        windowDimensions = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        scrollViewVector = Vector2.zero;
    }

    public override void Enter()
    {
        Controller.Camera.enabled = true;
        titsMcGee = null;
        toBeUsed = null;
    }

    public override void Exit()
    {
        Controller.Camera.enabled = false;
        titsMcGee = Controller.Camera.guiTexture;
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
        // Equipment slots.
        GUILayout.BeginArea(new Rect(5, 20, WIDTH, 300));
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        for (int i = 0; i < EQUIPMENT_SLOTS; i += 2)
        {
            GUILayout.BeginHorizontal();

            /*
            if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(50)))
            {
                Controller.Player.removeEquipment((equipSlots.slots)i);
            }
            */

            GUILayout.Space(WIDTH - 115);


            /*
            if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(50)))
            {
                Controller.Player.removeEquipment((equipSlots.slots)i);
            }
             */

            GUILayout.EndHorizontal();

            GUILayout.Space(50);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

        selection = GUI.Toolbar(new Rect(10, 300, WIDTH - 20, 50), selection, HEADERS);

        if (selection == 0)
            DrawStats();
        if (selection == 1)
            DrawInventory();
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
        int viewSize = Controller.Player.Inventory.Items.Count * 30;

        scrollViewVector = GUI.BeginScrollView(new Rect(10, 350, WIDTH - 10, 150), scrollViewVector,
            new Rect(0, 0, 10, viewSize));

        yOffset = 0;
        foreach (equipment item in Controller.Player.Inventory.Items)
        {
            
            /*
            if (GUI.Button(new Rect(0, yOffset, WIDTH - 30, 50), item.equipmentName, Controller.style))
            {
                toBeUsed = item;
            }
            */
            GUI.Box(new Rect(0, yOffset, WIDTH - 30, 22), item.equipmentName);

            yOffset += 24;
        }

        if(toBeUsed != null)
        {
            EquipItem(toBeUsed);
            toBeUsed = null;
        }

        GUI.EndScrollView();
    }

    void DrawSkills()
    {

    }

    void EquipItem(equipment item)
    {
        Controller.Player.addEquipment(item);
    }
}

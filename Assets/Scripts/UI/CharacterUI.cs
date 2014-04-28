using UnityEngine;
using System.Collections;

public class CharacterUI : UIState {
    private const float WIDTH = 400;
    private const float HEIGHT = 500;
    public const int EQUIPMENT_SLOTS = 6;
    private string[] HEADERS = { "Stats", "Inventory", "Skills" };

    private Rect windowDimensions;
    private int selection = 0;
    private int yOffset = 0;
    private Vector2 scrollViewVector;
    private Camera characterCamera;
    private GUITexture titsMcGee;
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

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("", Controller.Player.Inventory.EquippedItem(i)), 
                GUILayout.Width(50), GUILayout.Height(50)))
            {
                Controller.Player.removeEquipment((equipSlots.slots)i);
            }
            GUILayout.Label(GUI.tooltip, GUILayout.Width(100), GUILayout.Height(50));
            GUI.tooltip = null;
            GUILayout.EndHorizontal();

            GUILayout.Space(75);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("", Controller.Player.Inventory.EquippedItem(i+1)),
                GUILayout.Width(50), GUILayout.Height(50)))
            {
                Controller.Player.removeEquipment((equipSlots.slots)i+1);
            }
            GUILayout.Label(GUI.tooltip, GUILayout.Width(100), GUILayout.Height(50));
            GUI.tooltip = null;
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
            GUILayout.Space(50);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

        //DrawEquipSlots();

        selection = GUI.Toolbar(new Rect(10, 300, WIDTH - 20, 50), selection, HEADERS);

        if (selection == 0)
            DrawStats();
        if (selection == 1)
            DrawInventory();
    }

    /// <summary>
    /// In order to properly use tooltips each equipment slot needs to be created individually.
    /// </summary>
    void DrawEquipSlots()
    {
        GUILayout.BeginArea(new Rect(5, 20, WIDTH, 300));
        GUILayout.BeginVertical();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        // Drawing head slot.
        if (GUILayout.Button(new GUIContent("",
            Controller.Player.Inventory.EquippedItem((int)equipSlots.slots.Head)),
            GUILayout.Width(50), GUILayout.Height(50)))
        {
            Controller.Player.removeEquipment((equipSlots.slots.Head));
        }
        GUILayout.Label(GUI.tooltip, GUILayout.Width(50), GUILayout.Height(50));

        // Drawing chest slot.
        if (GUILayout.Button(new GUIContent("",
            Controller.Player.Inventory.EquippedItem((int)equipSlots.slots.Chest)),
            GUILayout.Width(50), GUILayout.Height(50)))
        {
            Controller.Player.removeEquipment((equipSlots.slots.Chest));
        }
        GUILayout.Label(GUI.tooltip, GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndHorizontal();
        GUILayout.Space(50);

        GUILayout.BeginHorizontal();
        // Drawing main hand slot.
        if (GUILayout.Button(new GUIContent("",
            Controller.Player.Inventory.EquippedItem((int)equipSlots.slots.Main)),
            GUILayout.Width(50), GUILayout.Height(50)))
        {
            Controller.Player.removeEquipment((equipSlots.slots.Main));
        }
        GUILayout.Label(GUI.tooltip, GUILayout.Width(50), GUILayout.Height(50));

        // Drawing off hand slot.
        if (GUILayout.Button(new GUIContent("",
            Controller.Player.Inventory.EquippedItem((int)equipSlots.slots.Off)),
            GUILayout.Width(50), GUILayout.Height(50)))
        {
            Controller.Player.removeEquipment((equipSlots.slots.Off));
        }
        GUILayout.Label(GUI.tooltip, GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndHorizontal();
        GUILayout.Space(50);

        GUILayout.BeginHorizontal();
        // Drawing legs slot.
        if (GUILayout.Button(new GUIContent("",
            Controller.Player.Inventory.EquippedItem((int)equipSlots.slots.Legs)),
            GUILayout.Width(50), GUILayout.Height(50)))
        {
            Controller.Player.removeEquipment((equipSlots.slots.Legs));
        }
        GUILayout.Label(GUI.tooltip, GUILayout.Width(50), GUILayout.Height(50));

        // Drawing feet slot.
        if (GUILayout.Button(new GUIContent("",
            Controller.Player.Inventory.EquippedItem((int)equipSlots.slots.Feet)),
            GUILayout.Width(50), GUILayout.Height(50)))
        {
            Controller.Player.removeEquipment((equipSlots.slots.Feet));
        }
        GUILayout.Label(GUI.tooltip, GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndHorizontal();
        GUILayout.Space(50);

        GUILayout.EndVertical();
        GUILayout.EndArea();
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
        int viewSize = Controller.Player.Inventory.Items.Count * 60;

        scrollViewVector = GUI.BeginScrollView(new Rect(10, 350, WIDTH - 10, 150), scrollViewVector,
            new Rect(0, 0, 10, viewSize));

        yOffset = 0;
        for (int I = 0; I < Controller.Player.Inventory.Items.Count; ++I )
        {
            equipment item = Controller.Player.Inventory.Items[I];
            if (GUI.Button(new Rect(0, yOffset, WIDTH - 30, 50), item.equipmentName, Controller.style))
            {
                Controller.Player.addEquipment(item);
            }
            yOffset += 60;
        }

        GUI.EndScrollView();
    }

    void DrawSkills()
    {

    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterUI : UIState
{
    private const float WIDTH = 400;
    private const float HEIGHT = 500;
    private const int EQUIPMENT_SLOTS = 6;
    private string[] HEADERS = { "Stats", "Inventory", "Skills" };

    private Rect windowDimensions;
    private int selection = 0;
    private int yOffset = 0;
    private int xOffset = 0;
    private int totalInvWidth = 0;
    private int rowLength = 8;
    private Dictionary<Rect, equipment> slotRects;

    private Vector2 scrollViewVector;
    private Camera characterCamera;
    private GUITexture titsMcGee;

    private equipment toBeUsed;


    private equipment hoverEquip;

    public CharacterUI(int id, UIController controller)
        : base(id, controller)
    {
        windowDimensions = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        scrollViewVector = Vector2.zero;

        totalInvWidth = Convert.ToInt32(WIDTH) - 62;
    }

    public override void Enter()
    {
        Controller.Camera.enabled = true;
        titsMcGee = null;
        toBeUsed = null;
        hoverEquip = null;

        Controller.DraggedEquip = null;
    }

    public override void Exit()
    {
        Controller.Camera.enabled = false;
        titsMcGee = Controller.Camera.guiTexture;
    }

    public override void Update()
    {
        if (Controller.DraggedEquip != null)
        {
            Debug.Log("Equipment: "+ Controller.DraggedEquip.equipmentName);
        }
    }

    public override void OnGui()
    {
        GUI.depth = 0;
        
        GUI.Window(0, windowDimensions, OnWindow, "Character");
        if (hoverEquip != null)
        {

            

            GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 250, 25), hoverEquip.equipmentName);

            
        }

        
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

            if (Controller.Player.EquippedEquip.ContainsKey(((equipSlots.slots)i)))
            {

                GUILayout.Box(new GUIContent(Controller.Player.EquippedEquip[((equipSlots.slots)i)].equipmentName), GUILayout.Width(50), GUILayout.Height(50));

                Drag(Controller.Player.EquippedEquip[((equipSlots.slots)i)], GUILayoutUtility.GetLastRect());
            }
            else
            {
                GUILayout.Box(new GUIContent(((equipSlots.slots)i).ToString()), GUILayout.Width(50), GUILayout.Height(50));
            }


            DropToEquip(GUILayoutUtility.GetLastRect(), i);

            

            GUILayout.Space(WIDTH - 115);


            if (Controller.Player.EquippedEquip.ContainsKey(((equipSlots.slots)i+1)))
            {

                GUILayout.Box(new GUIContent(Controller.Player.EquippedEquip[((equipSlots.slots)i+1)].equipmentName), GUILayout.Width(50), GUILayout.Height(50));

                Drag(Controller.Player.EquippedEquip[((equipSlots.slots)i+1)], GUILayoutUtility.GetLastRect());
            }
            else
            {
                GUILayout.Box(new GUIContent(((equipSlots.slots)i+1).ToString()), GUILayout.Width(50), GUILayout.Height(50));
            }


            DropToEquip(GUILayoutUtility.GetLastRect(), i+1);
            

            GUILayout.EndHorizontal();

            GUILayout.Space(50);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

        selection = GUI.Toolbar(new Rect(10, 300, WIDTH - 20, 50), selection, HEADERS);

        if (selection == 0)
            DrawStats();
        if (selection == 1)
        {

            int viewSize = (Controller.Player.Inventory.Max * ((totalInvWidth / rowLength) + 2)) / rowLength;

            Rect viewArea = new Rect(0, 0, 10, viewSize);

            scrollViewVector = GUI.BeginScrollView(new Rect(10, 350, WIDTH - 10, 150), scrollViewVector,
                viewArea);


            

            yOffset = 0;
            xOffset = 0;

            slotRects = new Dictionary<Rect, equipment>();
            

            for (int i = 0; i < Controller.Player.Inventory.Max; i++ )
            {
                if (i != 0 && i % 8 == 0)
                {
                    yOffset += (totalInvWidth / rowLength) + 2;
                    xOffset = 0;
                }

                Rect thisRect = new Rect(xOffset, yOffset, totalInvWidth / rowLength, totalInvWidth / rowLength);
                
                
                if (i < Controller.Player.Inventory.Items.Count)
                {
                    GUI.Box(thisRect, Controller.Player.Inventory.Items[i].equipmentName);

                    Drag(Controller.Player.Inventory.Items[i], thisRect);

                    slotRects.Add(thisRect, Controller.Player.Inventory.Items[i]);
                }
                else
                {
                    GUI.Box(thisRect, "");

                    DropToUnequip(thisRect);


                    slotRects.Add(thisRect, null);
                }

                

                xOffset += (totalInvWidth / rowLength) + 4;

            }

            Tooltip(slotRects);

            GUI.EndScrollView();
        }
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


        DropToUnequip(new Rect(10, 350, WIDTH - 10, 150));

        yOffset = 0;

        foreach (equipment item in Controller.Player.Inventory.Items)
        {
            
            
            GUI.Box(new Rect(0, yOffset, WIDTH - 30, 22), new GUIContent(item.equipmentName));

            Drag(item, new Rect(0, yOffset, WIDTH - 30, 22));

            yOffset += 24;
        }

        

        GUI.EndScrollView();
    }

    
    void Drag(equipment draggingEquip, Rect draggingRect)
    {
        //To Drag:
        //Check to see if the current event type is MouseDown and if the mouse position of the current event is inside the target rect. If it is, assign the target ability to a temporary variable that can be accessed from where you will be dropping the ability later.
        if (Event.current.type == EventType.MouseDown && draggingRect.Contains(Event.current.mousePosition) && Controller.DraggedEquip == null)
        {
            Controller.DraggedEquip = draggingEquip;
        }

    }

    
    void DropToEquip(Rect overRect, int slotIndex)
    {

        equipSlots.slots thisSlot = ((equipSlots.slots)slotIndex);

        //To Drop:
        //Check to see if the current event type is MouseUp and if the mouse position of the current event is inside the target rect. If it is, perform the necessary operation (in this case, replacing the currently slotted ability with the dragged ability) and set the temporary variable to null.
        if (Event.current.type == EventType.MouseUp && overRect.Contains(Event.current.mousePosition) && Controller.DraggedEquip != null)
        {

            if(thisSlot == Controller.DraggedEquip.validSlot)
            {

                if (Controller.Player.EquippedEquip.ContainsKey(thisSlot))
                {

                    Controller.Player.removeEquipment(thisSlot);

                    
                }

                Controller.Player.addEquipment(Controller.DraggedEquip);
            }

            Controller.DraggedEquip = null;
        }
    }

    
    void DropToUnequip(Rect overRect)
    {

        //To Drop:
        //Check to see if the current event type is MouseUp and if the mouse position of the current event is inside the target rect. If it is, perform the necessary operation (in this case, removing the currently slotted ability and adding it to the spellbook list) and set the temporary variable to null.
        if (Event.current.type == EventType.MouseUp && overRect.Contains(Event.current.mousePosition) && Controller.DraggedEquip != null)
        {


            if (Controller.Player.Inventory.Items.Count < Controller.Player.Inventory.Max)
            {
                Controller.Player.removeEquipment(Controller.DraggedEquip.validSlot);
            }

            Controller.DraggedEquip = null;
        }
    }

    void Tooltip(Dictionary<Rect, equipment> hoverRects)
    {
        
        Vector2 mPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);



        foreach (Rect hoverRect in hoverRects.Keys)
        {

            if (hoverRect.Contains(GUIUtility.ScreenToGUIPoint(mPos)) && Controller.DraggedEquip == null)
            {
                hoverEquip = hoverRects[hoverRect];
                return;
            }
            else
            {
                hoverEquip = null;
            }
        }
    }
}

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
    private equipment hoverEquipped;

    private equipment equipItem;
    private equipment deleteItem;

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
        hoverEquipped = null;
        equipItem = null;

        Controller.DraggedEquip = null;
    }

    public override void Exit()
    {
        base.Exit();

        Controller.Camera.enabled = false;
        titsMcGee = Controller.Camera.guiTexture;
    }

    public override void Update()
    {
        
    }

    public override void OnGui()
    {
        
        
        GUI.depth = 0;
        
        Rect windowRect = GUI.Window(0, windowDimensions, OnWindow, "Character");
        if (hoverEquip != null)
        {

            GUIContent thisContent = new GUIContent(
                hoverEquip.equipmentName + "\n"
                + "Health: " + hoverEquip.equipmentAttributes.Health.ToString() + "\n"
                + "Resource: " + hoverEquip.equipmentAttributes.Resource.ToString() + "\n"
                + "Power: " + hoverEquip.equipmentAttributes.Power.ToString() + "\n"
                + "Defense: " + hoverEquip.equipmentAttributes.Defense.ToString() + "\n"
                + "Damage: " + hoverEquip.equipmentAttributes.MinDamage.ToString() + " - " + hoverEquip.equipmentAttributes.MaxDamage.ToString() + "\n"
                + "Attack Speed: " + hoverEquip.equipmentAttributes.AttackSpeed.ToString() + "\n"
                + "Movement Speed: " + hoverEquip.equipmentAttributes.MovementSpeed.ToString() + "\n"
                );

            GUIStyle thisStyle = new GUIStyle("box");
            thisStyle.alignment = TextAnchor.UpperLeft;
            thisStyle.normal.textColor = Color.white;

            GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 250, 127), thisContent, thisStyle);

            
        }
        else if (hoverEquipped != null)
        {


            GUIContent thisContent = new GUIContent(
                hoverEquipped.equipmentName + "\n"
                + "Health: " + hoverEquipped.equipmentAttributes.Health.ToString() + "\n"
                + "Resource: " + hoverEquipped.equipmentAttributes.Resource.ToString() + "\n"
                + "Power: " + hoverEquipped.equipmentAttributes.Power.ToString() + "\n"
                + "Defense: " + hoverEquipped.equipmentAttributes.Defense.ToString() + "\n"
                + "Damage: " + hoverEquipped.equipmentAttributes.MinDamage.ToString() + " - " + hoverEquipped.equipmentAttributes.MaxDamage.ToString() + "\n"
                + "Attack Speed: " + hoverEquipped.equipmentAttributes.AttackSpeed.ToString() + "\n"
                + "Movement Speed: " + hoverEquipped.equipmentAttributes.MovementSpeed.ToString() + "\n"
                );

            GUIStyle thisStyle = new GUIStyle("box");
            thisStyle.alignment = TextAnchor.UpperLeft;
            thisStyle.normal.textColor = Color.white;

            GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 250, 127), thisContent, thisStyle);

            

        }
    }

    void OnWindow(int windowID)
    {

        #region Mouse in GUI check

        Vector2 mPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if (mPos.x > windowDimensions.x 
            && mPos.x < windowDimensions.width + windowDimensions.x
            && mPos.y > windowDimensions.y
            && mPos.y < windowDimensions.height + windowDimensions.y)
        {
            Controller.PlayerController.MouseOverGUI = true;
        }
        else
        {
            Controller.PlayerController.MouseOverGUI = false;
        }

        #endregion

        slotRects = new Dictionary<Rect, equipment>();

        yOffset = 30;

        // Equipment slots.
        GUILayout.BeginArea(new Rect(5, 20, WIDTH, 300));

        for (int i = 0; i < EQUIPMENT_SLOTS; i += 2)
        {
            

            Rect thisRect = new Rect(5, yOffset, 50, 50);

            if (Controller.Player.EquippedEquip.ContainsKey(((equipSlots.slots)i)))
            {
                
                GUI.Box(thisRect, new GUIContent(Controller.Player.EquippedEquip[((equipSlots.slots)i)].equipmentName));

                Drag(Controller.Player.EquippedEquip[((equipSlots.slots)i)], thisRect);


                slotRects.Add(thisRect, Controller.Player.EquippedEquip[((equipSlots.slots)i)]);

                if (thisRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp && Event.current.button == 1 && Event.current.shift == true)
                {
                    deleteItem = Controller.Player.EquippedEquip[((equipSlots.slots)i)];
                }
                else if (thisRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp && Event.current.button == 1)
                {
                    equipItem = Controller.Player.EquippedEquip[((equipSlots.slots)i)];
                }

            }
            else
            {
                GUI.Box(thisRect, new GUIContent(((equipSlots.slots)i).ToString()));
                slotRects.Add(thisRect, null);
            }


            DropToEquip(thisRect, i);

            

            //GUILayout.Space(WIDTH - 115);

            thisRect = new Rect((WIDTH - 65), yOffset, 50, 50);

            if (Controller.Player.EquippedEquip.ContainsKey(((equipSlots.slots)i+1)))
            {

                GUI.Box(thisRect, new GUIContent(Controller.Player.EquippedEquip[((equipSlots.slots)i+1)].equipmentName));

                Drag(Controller.Player.EquippedEquip[((equipSlots.slots)i+1)], thisRect);

                slotRects.Add(thisRect, Controller.Player.EquippedEquip[((equipSlots.slots)i + 1)]);

                if (thisRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp && Event.current.button == 1 && Event.current.shift == true)
                {
                    deleteItem = Controller.Player.EquippedEquip[((equipSlots.slots)i+1)];
                }
                else if (thisRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp && Event.current.button == 1)
                {
                    equipItem = Controller.Player.EquippedEquip[((equipSlots.slots)i+1)];
                }
            }
            else
            {
                GUI.Box(thisRect, new GUIContent(((equipSlots.slots)i+1).ToString()));
                slotRects.Add(thisRect, null);
            }


            DropToEquip(thisRect, i + 1);

            yOffset += 100;
            
        }

        if (equipItem != null)
        {
            RightClickUnEquip(equipItem);
            equipItem = null;
        }

        if (deleteItem != null)
        {
            DeleteItem(deleteItem);
            deleteItem = null;
        }


        


        TooltipEquipment(slotRects);
        GUILayout.EndArea();

        //selection = GUI.Toolbar(new Rect(10, 300, WIDTH - 20, 50), selection, HEADERS);

        #region Inventory Area

        slotRects = new Dictionary<Rect, equipment>();

        int viewSize = (Controller.Player.Inventory.Max * ((totalInvWidth / rowLength) + 2)) / rowLength;

        Rect viewArea = new Rect(0, 0, 10, viewSize);

        scrollViewVector = GUI.BeginScrollView(new Rect(10, 320, WIDTH - 10, 180), scrollViewVector,
            viewArea);


            

        yOffset = 0;
        xOffset = 0;

        
            

        for (int i = 0; i < Controller.Player.Inventory.Max; i++ )
        {
            if (i != 0 && i % rowLength == 0)
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


                if (thisRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp && Event.current.button == 1 && Event.current.shift == true)
                {
                    deleteItem = Controller.Player.Inventory.Items[i];
                }
                else if (thisRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp && Event.current.button == 1)
                {
                    equipItem = Controller.Player.Inventory.Items[i];
                }

            }
            else
            {
                GUI.Box(thisRect, "");

                DropToUnequip(thisRect);


                slotRects.Add(thisRect, null);
            }

                

            xOffset += (totalInvWidth / rowLength) + 4;

        }


        if (equipItem != null)
        {
            RightClickEquip(equipItem);
            equipItem = null;
        }

        if (deleteItem != null)
        {
            DeleteItem(deleteItem);
            deleteItem = null;
        }

        

        TooltipInventory(slotRects);

        GUI.EndScrollView();
        #endregion





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

    
    void Drag(equipment draggingEquip, Rect draggingRect)
    {
        //To Drag:
        //Check to see if the current event type is MouseDown and if the mouse position of the current event is inside the target rect. If it is, assign the target ability to a temporary variable that can be accessed from where you will be dropping the ability later.
        if (Event.current.type == EventType.MouseDown && draggingRect.Contains(Event.current.mousePosition) && Controller.DraggedEquip == null && Event.current.button == 0)
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
            hoverEquip = null;
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
            hoverEquip = null;
        }
    }

    void TooltipInventory(Dictionary<Rect, equipment> hoverRects)
    {
        
        Vector2 mPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);



        foreach (Rect hoverRect in hoverRects.Keys)
        {

            if (hoverRect.Contains(GUIUtility.ScreenToGUIPoint(mPos)) && Controller.DraggedEquip == null)
            {

                //Debug.Log("in rect: " + hoverRect.ToString());
                hoverEquip = hoverRects[hoverRect];
                return;
            }
            else
            {
                hoverEquip = null;
            }
        }
    }

    void TooltipEquipment(Dictionary<Rect, equipment> hoverRects)
    {

        Vector2 mPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);



        foreach (Rect hoverRect in hoverRects.Keys)
        {

            if (hoverRect.Contains(GUIUtility.ScreenToGUIPoint(mPos)) && Controller.DraggedEquip == null)
            {

                //Debug.Log("in rect: " + hoverRect.ToString());
                hoverEquipped = hoverRects[hoverRect];
                return;
            }
            else
            {
                hoverEquipped = null;
            }
        }
    }

    void RightClickEquip(equipment item)
    {
        if (Controller.Player.EquippedEquip.ContainsKey(item.validSlot))
        {
            Controller.Player.removeEquipment(item.validSlot);
        }

        Controller.Player.addEquipment(item);
    }

    void RightClickUnEquip(equipment item)
    {
        Controller.Player.removeEquipment(item.validSlot);
    }

    void DeleteItem(equipment item)
    {
        if (Controller.Player.EquippedEquip.ContainsKey(item.validSlot))
        {
            Controller.Player.removeEquipment(item.validSlot);
        }

        Controller.Player.Inventory.RemoveItem(item);
        
    }
}

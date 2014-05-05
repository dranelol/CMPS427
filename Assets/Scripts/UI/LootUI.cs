using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootUI : UIState
{

    private const float WIDTH = 200;
	private const float HEIGHT = 250;
    private Rect windowDimensions;

    private equipment lootItem;

    private float rowLength;

    private float totalInvWidth;

    private Vector2 scrollViewVector;

    private equipment hoverEquip;

    private Dictionary<Rect, equipment> lootRects;

    public LootUI(int id, UIController controller)
		: base(id, controller)
	{
        windowDimensions = new Rect(50, Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);

        totalInvWidth = WIDTH - 10;

        scrollViewVector = Vector2.zero;
	}
	public override void Enter ()
	{
		base.Enter();
        lootItem = null;
	}

	public override void Exit ()
	{
		base.Exit();
	}

	public override void Update () 
	{
        
	}

	public override void OnGui ()
	{
		GUI.depth = 0;

        GUI.Window(0, windowDimensions, OnWindow, "Loot");

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


        float yOffset = 0;

        lootRects = new Dictionary<Rect, equipment>();
        
        int viewSize = Controller.ChestInventory.Items.Count * 25;

        Rect viewArea = new Rect(0,0,10, viewSize);

        scrollViewVector = GUI.BeginScrollView(new Rect(5, 20, WIDTH - 10, HEIGHT - 50), scrollViewVector,
            viewArea);
        
        foreach (equipment item in Controller.ChestInventory.Items)
        {
            Rect thisRect = new Rect(0, yOffset, totalInvWidth, 25);
            
            GUI.Box(thisRect, item.equipmentName);

            if (thisRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp && Event.current.button == 1)
            {
                lootItem = item;
            }

            lootRects.Add(thisRect, item);

            yOffset += 27;
            
        }

        Tooltip(lootRects);

        if (lootItem != null)
        {
            Loot(lootItem);
            lootItem = null;
        }

        GUI.EndScrollView();

        Rect lootAllRect = new Rect(5, HEIGHT - 30, WIDTH - 10, 25);

        if (GUI.Button(lootAllRect, "Loot All"))
        {
            LootAll();
        }
	}

    void Loot(equipment thisItem)
    {

        Controller.ChestInventory.RemoveItem(thisItem);
        Controller.Player.Inventory.AddItem(thisItem);
    }

    void LootAll()
    {
        foreach (equipment item in Controller.ChestInventory.Items)
        {
            Controller.Player.Inventory.AddItem(item);
        }

        Controller.ChestInventory.Items.Clear();
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

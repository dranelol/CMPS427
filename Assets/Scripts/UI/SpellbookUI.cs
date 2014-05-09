using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpellbookUI : UIState
{
    private const float WIDTH = 400;
    private const float HEIGHT = 500;
    private Rect windowDimensions;

    private GUIStyle labelStyle;
   

    private Vector2 scrollViewVector;

    private Ability draggedAbility;

    private List<Ability> tempSpells;

    private Ability hoverInactiveSpell;
    private Ability hoverActiveSpell;

    private Dictionary<Rect, Ability> inactiveRects;
    private Dictionary<Rect, Ability> activeRects;

    public SpellbookUI(int id, UIController controller)
        : base(id, controller)
    {
        windowDimensions = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        scrollViewVector = Vector2.zero;

        draggedAbility = null;
        

        
    }
    public override void Enter()
    {
        base.Enter();

        
        hoverInactiveSpell = null;
        hoverActiveSpell = null;

        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {


    }

    public override void OnGui()
    {

        GUI.depth = 0;
        
        labelStyle = new GUIStyle("box");
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 12;
        labelStyle.normal.textColor = Color.white;
        GUI.Window(0, windowDimensions, OnWindow, "Spellbook");

        float tooltipWidth, tooltipHeight;
        string info;

        if (hoverInactiveSpell != null)
        {

            info = hoverInactiveSpell.Name + "\n"
                     + "Damage: " + hoverInactiveSpell.DamageMod.ToString() + "\n"
                     + "Cost: " + hoverInactiveSpell.ResourceCost.ToString() + "\n"
                     + "Range: " + hoverInactiveSpell.Range.ToString() + "\n"
                     + "Cooldown: " + hoverInactiveSpell.Cooldown.ToString();

            tooltipHeight = 83;
            tooltipWidth = 150;
            
            
            GUIContent thisContent = new GUIContent(info);

            GUIStyle thisStyle = new GUIStyle("box");
            thisStyle.alignment = TextAnchor.UpperLeft;
            thisStyle.normal.textColor = Color.white;

            GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, tooltipWidth, tooltipHeight), thisContent, thisStyle);
        }
        else if (hoverActiveSpell != null)
        {
            info = hoverActiveSpell.Name + "\n"
                     + "Damage: " + hoverActiveSpell.DamageMod.ToString() + "\n"
                     + "Cost: " + hoverActiveSpell.ResourceCost.ToString() + "\n"
                     + "Range: " + hoverActiveSpell.Range.ToString() + "\n"
                     + "Cooldown: " + hoverActiveSpell.Cooldown.ToString();

            tooltipHeight = 83;
            tooltipWidth = 150;


            GUIContent thisContent = new GUIContent(info);

            GUIStyle thisStyle = new GUIStyle("box");
            thisStyle.alignment = TextAnchor.UpperLeft;
            thisStyle.normal.textColor = Color.white;

            GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, tooltipWidth, tooltipHeight), thisContent, thisStyle);
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

        inactiveRects = new Dictionary<Rect, Ability>();
        activeRects = new Dictionary<Rect, Ability>();
        tempSpells = new List<Ability>();

        #region Spellbook

        for (int i = 1; i < 6; i++)
        {
            tempSpells.Add(Controller.Player.abilityManager.abilities[i]);
        }



        tempSpells = tempSpells.FindAll(delegate(Ability ab) { return ab != null; });

        //GUILayout.BeginArea(new Rect(0, 25, (WIDTH) / 2, HEIGHT - 25));

        

        Rect thisRect;

        float yOffset = 0;

        int totalSpells = Controller.PlayerController.SpellBook.Count + tempSpells.Count;


        int viewSize = (totalSpells + 1) * 28;

        Rect viewArea = new Rect(0, 0, 10, viewSize);

        scrollViewVector = GUI.BeginScrollView(new Rect(0, 25, (WIDTH) / 2, HEIGHT - 25), scrollViewVector,
            viewArea);


        for (int i = 0; i < totalSpells; i++)
        {
            thisRect = new Rect(5, yOffset, ((WIDTH) / 2) - 10, 25);

            if (i < Controller.PlayerController.SpellBook.Count)
            {
                
                GUI.Box(thisRect, Controller.PlayerController.SpellBook[i].Name);
                Drag(Controller.PlayerController.SpellBook[i], thisRect);
                inactiveRects.Add(new Rect(thisRect.x, thisRect.y+25, thisRect.width, thisRect.height), Controller.PlayerController.SpellBook[i]);
            }
            else
            {
                GUI.Box(thisRect, "Empty");
                DropActiveToInactive(thisRect);
            }

            

            yOffset += 28;
        }

        GUI.EndScrollView();
     
       // GUILayout.EndArea();

        SetInactiveHoverSpell(inactiveRects);

        #endregion

        #region Ability Slots

        //GUILayout.BeginArea(new Rect(WIDTH / 2, 25, (WIDTH) / 2, HEIGHT - 25));

       // GUILayout.BeginVertical();

        yOffset = 25;

        for (int i = 1; i<6 ;i++)
        {


            GUI.Label(new Rect(WIDTH / 2, yOffset, ((WIDTH) / 2) - 10, 20), NumberToKey(i));

            yOffset += 23;

            thisRect = new Rect((WIDTH / 2) + 5, yOffset, ((WIDTH) / 2) - 10, 50);

            //GUILayout.BeginHorizontal();
            //GUILayout.Space(5);
            if (Controller.Player.abilityManager.abilities[i] != null)
            {

                

                GUI.Box(thisRect, new GUIContent(Controller.Player.abilityManager.abilities[i].Name), labelStyle);




                DropInactiveToActive(thisRect, i);
                Drag(Controller.Player.abilityManager.abilities[i], thisRect);
                activeRects.Add(thisRect, Controller.Player.abilityManager.abilities[i]);

            }
            else
            {

                GUI.Box(thisRect, "Empty", labelStyle);
                
                
                DropInactiveToActive(thisRect, i);
            } 

            
            //GUILayout.EndHorizontal();

            yOffset += 50;
            
        }

       // GUILayout.EndVertical();

        //GUILayout.EndArea();

        SetActiveHoverSpell(activeRects);

        activeRects.Clear();
        inactiveRects.Clear();

        #endregion
    }

    /// <summary>
    /// Begins dragging an ability. Called every frame.
    /// </summary>
    /// <param name="draggingAbility">Ability to be dragged.</param>
    /// <param name="draggingRect">Target Rect where the click will need to be registered.</param>
    void Drag(Ability draggingAbility, Rect draggingRect)
    {
        //To Drag:
        //Check to see if the current event type is MouseDown and if the mouse position of the current event is inside the target rect. If it is, assign the target ability to a temporary variable that can be accessed from where you will be dropping the ability later.
        if (Event.current.type == EventType.MouseDown && draggingRect.Contains(Event.current.mousePosition) && draggedAbility == null)
        {
            draggedAbility = draggingAbility;
        }

    }

    /// <summary>
    /// Facilitates dropping an inactive ability into an active ability slot.
    /// </summary>
    /// <param name="overRect">Target Rect where the ability will be dropped.</param>
    /// <param name="slotIndex">The slot index</param>
    void DropInactiveToActive(Rect overRect, int slotIndex)
    {

        //To Drop:
        //Check to see if the current event type is MouseUp and if the mouse position of the current event is inside the target rect. If it is, perform the necessary operation (in this case, replacing the currently slotted ability with the dragged ability) and set the temporary variable to null.
        if (Event.current.type == EventType.MouseUp && overRect.Contains(Event.current.mousePosition) && draggedAbility != null)
        {


            if (Controller.Player.abilityManager.abilities[slotIndex] != null)
            {
                Controller.PlayerController.SpellBook.Add(Controller.Player.abilityManager.abilities[slotIndex]);
                Controller.Player.abilityManager.RemoveAbility(slotIndex);
            }



            Controller.Player.abilityManager.AddAbility(draggedAbility, slotIndex);

            Controller.PlayerController.SpellBook.Remove(draggedAbility);


            Controller.Player.abilityIndexDict[draggedAbility.ID] = slotIndex;

            draggedAbility = null;
        }
    }

    /// <summary>
    /// Facilitates dropping an active ability into spellbook.
    /// </summary>
    /// <param name="overRect"></param>
    void DropActiveToInactive(Rect overRect)
    {

        //To Drop:
        //Check to see if the current event type is MouseUp and if the mouse position of the current event is inside the target rect. If it is, perform the necessary operation (in this case, removing the currently slotted ability and adding it to the spellbook list) and set the temporary variable to null.
        if (Event.current.type == EventType.MouseUp && overRect.Contains(Event.current.mousePosition) && draggedAbility != null)
        {
            Controller.Player.abilityManager.RemoveAbility(Controller.Player.abilityIndexDict[draggedAbility.ID]);

            Controller.PlayerController.SpellBook.Add(draggedAbility);

            Controller.Player.abilityIndexDict[draggedAbility.ID] = -1;

            draggedAbility = null;
        }
    }


    /// <summary>
    /// Converts the slot index to their key binding.
    /// </summary>
    /// <param name="number">The slot index to convert.</param>
    /// <returns></returns>
    string NumberToKey(int number)
    {
        if (number == 1)
        {
            return "Right Mouse";
        }
        else if (number == 2)
        {
            return "Q";
        }
        else if (number == 3)
        {
            return "W";
        }
        else if (number == 4)
        {
            return "E";
        }
        else if (number == 5)
        {
            return "R";
        }
        else
        {
            return "";
        }
    }


    private void SetInactiveHoverSpell(Dictionary<Rect, Ability> hoverRects)
    {
        Vector2 mPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);



        foreach (var pair in hoverRects)
        {

            if (ButtonContains(pair.Key, GUIUtility.ScreenToGUIPoint(mPos)))
            {


                hoverInactiveSpell = pair.Value;

                return;
            }
            else
            {

                hoverInactiveSpell = null;
            }
        }
    }

    private void SetActiveHoverSpell(Dictionary<Rect, Ability> hoverRects)
    {
        Vector2 mPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);



        foreach (var pair in hoverRects)
        {

            if (ButtonContains(pair.Key, GUIUtility.ScreenToGUIPoint(mPos)))
            {


                hoverActiveSpell = pair.Value;

                return;
            }
            else
            {

                hoverActiveSpell = null;
            }
        }
    }

    private bool ButtonContains(Rect r, Vector2 mPos)
    {
        if (mPos.x > r.x
            && mPos.y > r.y
            && mPos.x < r.x + r.width
            && mPos.y < r.y + r.height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

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

        labelStyle = new GUIStyle("box");
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 12;
        labelStyle.normal.textColor = Color.white;
        GUI.Window(0, windowDimensions, OnWindow, "Spellbook");
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

        #region Spellbook



        GUILayout.BeginArea(new Rect(0, 25, (WIDTH) / 2, HEIGHT - 25));

        DropActiveToInactive(new Rect(0, 25, (WIDTH) / 2, HEIGHT - 25));
        

        GUILayout.BeginVertical();

        foreach (Ability item in Controller.PlayerController.SpellBook)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Box(new GUIContent(item.Name), GUILayout.Width(((WIDTH) / 2)-10), GUILayout.Height(50));

            Drag(item, GUILayoutUtility.GetLastRect());

            GUILayout.Space(5);
                
            
             
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

        

        GUILayout.EndArea();

        #endregion

        #region Ability Slots

        GUILayout.BeginArea(new Rect(WIDTH / 2, 25, (WIDTH) / 2, HEIGHT - 25));

        GUILayout.BeginVertical();

        for (int i = 1; i<6 ;i++)
        {


            GUILayout.Label(NumberToKey(i), GUILayout.Width(((WIDTH) / 2) - 10), GUILayout.Height(20));
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            if (Controller.Player.abilityManager.abilities[i] != null)
            {
                GUILayout.Box(new GUIContent(Controller.Player.abilityManager.abilities[i].Name), labelStyle,  GUILayout.Width(((WIDTH) / 2) - 10), GUILayout.Height(50));

                
                DropInactiveToActive(GUILayoutUtility.GetLastRect(), i);
                Drag(Controller.Player.abilityManager.abilities[i], GUILayoutUtility.GetLastRect());
            }
            else
            {
                GUILayout.Box(new GUIContent("Empty"), labelStyle, GUILayout.Width(((WIDTH) / 2) - 10), GUILayout.Height(50));
                DropInactiveToActive(GUILayoutUtility.GetLastRect(), i);
            } 

            GUILayout.Space(5);
            GUILayout.EndHorizontal();
            
        }

        GUILayout.EndVertical();

        GUILayout.EndArea();
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
}

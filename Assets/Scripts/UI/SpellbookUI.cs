using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpellbookUI : UIState
{
    private const float WIDTH = 400;
    private const float HEIGHT = 500;
    private Rect windowDim;

    private GUIStyle labelStyle;
   

    private Vector2 scrollViewVector;

    private Ability draggedAbility;

    public SpellbookUI(int id, UIController controller)
        : base(id, controller)
    {
        windowDim = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        scrollViewVector = Vector2.zero;

        draggedAbility = null;
        

        labelStyle = new GUIStyle();
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 14;
        labelStyle.normal.textColor = Color.white;
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
        if (draggedAbility != null)
        {
            Debug.Log("Ability: " + draggedAbility.Name);
        }

    }

    public override void OnGui()
    {
        GUI.Window(0, windowDim, OnWindow, "Spellbook");
    }

    void OnWindow(int windowID)
    {

        

        int viewSize = Controller.PlayerController.SpellBook.Count * 30;

        scrollViewVector = GUI.BeginScrollView(new Rect(5, 20, WIDTH, HEIGHT), scrollViewVector,
            new Rect(0, 0, 10, viewSize));

        GUILayout.BeginArea(new Rect(10, 25, WIDTH-10, HEIGHT-25));

        GUILayout.BeginVertical();

        foreach (Ability item in Controller.PlayerController.SpellBook)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box(new GUIContent(item.Name), GUILayout.Width(200), GUILayout.Height(50));
            
                
            Drag(item, GUILayoutUtility.GetLastRect());
             
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

        GUILayout.EndArea();

        GUI.EndScrollView();
       
    }

    void Drag(Ability draggingAbility, Rect draggingRect)
    {
        
        if(Event.current.type == EventType.MouseUp)
        {
            draggedAbility = null;
        }
        else if (Event.current.type == EventType.MouseDown && draggingRect.Contains(Event.current.mousePosition))
        {
            draggedAbility = draggingAbility;
        }

    }

    
    
}

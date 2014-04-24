using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TalentUI : UIState
{
    private const float WIDTH = 400;
    private const float HEIGHT = 500;
    private Rect windowDim;
    private List<Talent> mightTree;
    private List<Talent> magicTree;

    public TalentUI(int id, UIController controller)
        : base(id, controller)
    {
        windowDim = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        mightTree = Controller.PlayerController.TalentManager.MightTree.ToList<Talent>();
        magicTree = Controller.PlayerController.TalentManager.MagicTree.ToList<Talent>();
        

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
        GUI.Window(0, windowDim, OnWindow, "Talents");
    }

    void OnWindow(int windowID)
    {

        GUILayout.BeginArea(new Rect(5, 20, WIDTH / 2, HEIGHT));

        GUILayout.BeginVertical();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();



        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.EndArea();
       
    }
}

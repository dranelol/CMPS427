using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TalentUI : UIState
{
    private const float WIDTH = 800;
    private const float HEIGHT = 500;
    private Rect windowDim;
    private List<Talent> mightTree;
    private List<Talent> magicTree;
    private const int bufferSpace = 5;

    private GUIContent mightLabel;
    private GUIContent magicLabel;

    private GUIStyle titleStyle;
    private GUIStyle labelStyle;

    public TalentUI(int id, UIController controller)
        : base(id, controller)
    {
        windowDim = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        mightTree = Controller.PlayerController.TalentManager.MightTree.ToList<Talent>();
        magicTree = Controller.PlayerController.TalentManager.MagicTree.ToList<Talent>();

        

        titleStyle = new GUIStyle();
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 24;
        titleStyle.normal.textColor = Color.white;


        labelStyle = new GUIStyle();
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 10;
        labelStyle.normal.textColor = Color.white;

        mightLabel = new GUIContent("Might Tree");
        magicLabel = new GUIContent("Magic Tree");

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
  
        List<Talent> tempTalents = new List<Talent>();
        GUIContent tempTalentLabel = new GUIContent();
        int count = 0;
        float iconWidth = 0f;
        float iconHeight = 50f;

        GUILayout.BeginArea(new Rect(0,20,WIDTH/4, 20));

        GUILayout.BeginHorizontal();

        GUILayout.Label("Unused Talent Points: " + Controller.PlayerController.TalentManager.TalentPointPool.ToString(), GUILayout.Width(WIDTH / 4), GUILayout.Height(20));

        GUILayout.EndHorizontal();

        GUILayout.EndArea();

        #region Might Tree GUI

        GUILayout.BeginArea(new Rect(0, 40, (WIDTH / 2), HEIGHT));

        GUILayout.BeginVertical();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label(mightLabel, titleStyle, GUILayout.Width(WIDTH/4), GUILayout.Height(25));

        GUILayout.Label(new GUIContent(Controller.PlayerController.TalentManager.MightTreePoints.ToString()+" points"), titleStyle, GUILayout.Width(WIDTH / 4), GUILayout.Height(25));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        tempTalents = mightTree.FindAll(delegate(Talent tal) { return tal.Depth == count; });
        
        while(tempTalents.Count != 0)
        {

            GUILayout.Space(20);
            
            iconWidth = (((WIDTH/2)) / tempTalents.Count) - (bufferSpace * 2);

            GUILayout.BeginHorizontal();

            foreach (Talent t in tempTalents)
            {
                tempTalentLabel.text = t.CurrentPoints.ToString() + "/" + t.MaxPoints.ToString();
                
                GUILayout.Space(bufferSpace);
                GUILayout.BeginVertical();

                if (Controller.PlayerController.TalentManager.IsTalentActive(t) == true)
                {
                    if (GUILayout.Button(t.Name, GUILayout.Width(iconWidth), GUILayout.Height(iconHeight)))
                    {
                        if (Event.current.button == 0)
                        {
                            Controller.PlayerController.TalentManager.SpendPoint(t);
                        }
                        else if (Event.current.button == 1)
                        {
                            Controller.PlayerController.TalentManager.RemovePoint(t);
                        }
                    }
                }
                else
                {
                    GUI.enabled = false;
                    GUILayout.Button(t.Name, GUILayout.Width(iconWidth), GUILayout.Height(iconHeight));
                    GUI.enabled = true;
                }

                
                
                GUILayout.Label(tempTalentLabel, labelStyle, GUILayout.Width(iconWidth), GUILayout.Height(10));
                GUILayout.EndVertical();
                
            }
            GUILayout.Space(bufferSpace);
            GUILayout.EndHorizontal();

            count++;
            tempTalents = mightTree.FindAll(delegate(Talent tal) { return tal.Depth == count; });
        }

        

        GUILayout.EndVertical();

        GUILayout.EndArea();

        #endregion

        #region Magic Tree GUI

        count = 0;

        GUILayout.BeginArea(new Rect((WIDTH / 2), 40, WIDTH / 2, HEIGHT));

        GUILayout.BeginVertical();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label(magicLabel, titleStyle, GUILayout.Width(WIDTH / 4), GUILayout.Height(25));

        GUILayout.Label(new GUIContent(Controller.PlayerController.TalentManager.MagicTreePoints.ToString() + " points"), titleStyle, GUILayout.Width(WIDTH / 4), GUILayout.Height(25));

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        tempTalents = magicTree.FindAll(delegate(Talent tal) { return tal.Depth == count; });

        while (tempTalents.Count != 0)
        {

            

            GUILayout.Space(20);

            iconWidth = (((WIDTH / 2)) / tempTalents.Count) - (bufferSpace * 2);

            GUILayout.BeginHorizontal();

            foreach (Talent t in tempTalents)
            {
                tempTalentLabel.text = t.CurrentPoints.ToString() + "/" + t.MaxPoints.ToString();

                GUILayout.Space(bufferSpace);
                GUILayout.BeginVertical();


                if (Controller.PlayerController.TalentManager.IsTalentActive(t) == true)
                {
                    if (GUILayout.Button(t.Name, GUILayout.Width(iconWidth), GUILayout.Height(iconHeight)))
                    {
                        if (Event.current.button == 0)
                        {
                            Controller.PlayerController.TalentManager.SpendPoint(t);
                        }
                        else if (Event.current.button == 1)
                        {
                            Controller.PlayerController.TalentManager.RemovePoint(t);
                        }
                    }
                }
                else
                {
                    GUI.enabled = false;
                    GUILayout.Button(t.Name, GUILayout.Width(iconWidth), GUILayout.Height(iconHeight));
                    GUI.enabled = true;
                }



                GUILayout.Label(tempTalentLabel, labelStyle, GUILayout.Width(iconWidth), GUILayout.Height(10));
                GUILayout.EndVertical();
                

            }
            GUILayout.Space(bufferSpace);
            GUILayout.EndHorizontal();

            count++;
            tempTalents = magicTree.FindAll(delegate(Talent tal) { return tal.Depth == count; });
        }



        GUILayout.EndVertical();

        GUILayout.EndArea();

        #endregion
    }
}

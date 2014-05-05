using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TalentUI : UIState
{
    private const float WIDTH = 800;  //Total width of the window.
    private const float HEIGHT = 500; //Total height of the window.
    private Rect windowDimensions;
    private List<Talent> mightTree;  //List of the talents in the might tree.
    private List<Talent> magicTree;  //List of the talents in the magic tree.
    private const int bufferSpace = 5;  //Space between most GUI objects.

    private GUIContent mightLabel;      //The label for the might tree
    private GUIContent magicLabel;      //The label for the magic tree

    private GUIStyle titleStyle;        //The style for title
    private GUIStyle labelStyle;

    private bool applied;
    private int thisPool;

    private Dictionary<Talent, int> talentAllocation;

    private int tempMightTreePoints;
    private int tempMagicTreePoints;

    public TalentUI(int id, UIController controller)
        : base(id, controller)
    {
        windowDimensions = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        

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
        
        //Get the talent trees from the TalentManager and convert them to lists.
        mightTree = Controller.PlayerController.TalentManager.MightTree.ToList<Talent>();
        magicTree = Controller.PlayerController.TalentManager.MagicTree.ToList<Talent>();

        thisPool = Controller.PlayerController.TalentManager.TalentPointPool;

        tempMightTreePoints = Controller.PlayerController.TalentManager.MightTreePoints;
        tempMagicTreePoints = Controller.PlayerController.TalentManager.MagicTreePoints;

        InitTalentAllocation();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (Controller.PlayerController.TalentManager.TalentPointPool > 0)
        {
            applied = false;
        }
    }

    public override void OnGui()
    {
        GUI.Window(0, windowDimensions, OnWindow, "Talents");
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

        List<Talent> tempTalents = new List<Talent>();
        GUIContent tempTalentLabel = new GUIContent();
        int count = 0;
        float iconWidth = 0f;
        float iconHeight = 50f;

        GUILayout.BeginArea(new Rect(5,20,WIDTH, 20));

        GUILayout.BeginHorizontal();

        GUILayout.Label("Unused Talent Points: " + thisPool.ToString(), GUILayout.Width((WIDTH / 6)+20), GUILayout.Height(20));


        if (applied == false)
        {
            if (GUILayout.Button("Apply", GUILayout.Width(80), GUILayout.Height(20)))
            {
                ApplyChanges();
            }
        }
        else
        {
            GUI.enabled = false;
            GUILayout.Button("Apply", GUILayout.Width(80), GUILayout.Height(20));
            GUI.enabled = true;
        }

        GUILayout.EndHorizontal();

        GUILayout.EndArea();

        #region Might Tree GUI

        GUILayout.BeginArea(new Rect(0, 40, (WIDTH / 2), HEIGHT));

        GUILayout.BeginVertical();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label(mightLabel, titleStyle, GUILayout.Width((WIDTH / 6) - bufferSpace), GUILayout.Height(25));

        GUILayout.Label(new GUIContent(tempMightTreePoints.ToString() + " points"), titleStyle, GUILayout.Width((WIDTH / 6) - bufferSpace), GUILayout.Height(25));



        if (Controller.PlayerController.TalentManager.MightTreePoints > 0)
        {

            if (GUILayout.Button("RESPEC", GUILayout.Width((WIDTH / 6)-bufferSpace), GUILayout.Height(25)))
            {
                Respec("might");
            }
        }
        else
        {
            GUI.enabled = false;
            GUILayout.Button("RESPEC", GUILayout.Width((WIDTH / 6) - bufferSpace), GUILayout.Height(25));
            GUI.enabled = true;
        }




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
                tempTalentLabel.text = (t.CurrentPoints + talentAllocation[t]).ToString() + "/" + t.MaxPoints.ToString();
                
                GUILayout.Space(bufferSpace);
                GUILayout.BeginVertical();

                if (IsTalentActive(t) == true)
                {
                    if (GUILayout.Button(t.Name, GUILayout.Width(iconWidth), GUILayout.Height(iconHeight)))
                    {
                        if (Event.current.button == 0)
                        {
                            SpendPoint(t);
                        }
                        else if (Event.current.button == 1)
                        {
                            RemovePoint(t);
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
        GUILayout.Label(magicLabel, titleStyle, GUILayout.Width((WIDTH / 6) - bufferSpace), GUILayout.Height(25));

        GUILayout.Label(new GUIContent(tempMagicTreePoints.ToString() + " points"), titleStyle, GUILayout.Width((WIDTH / 6) - bufferSpace), GUILayout.Height(25));


        if (Controller.PlayerController.TalentManager.MagicTreePoints > 0)
        {

            if (GUILayout.Button("RESPEC", GUILayout.Width((WIDTH / 6) - bufferSpace), GUILayout.Height(25)))
            {
                Respec("magic");
            }
        }
        else
        {
            GUI.enabled = false;
            GUILayout.Button("RESPEC", GUILayout.Width((WIDTH / 6) - bufferSpace), GUILayout.Height(25));
            GUI.enabled = true;
        }

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
                tempTalentLabel.text = (t.CurrentPoints+ talentAllocation[t] ).ToString() + "/" + t.MaxPoints.ToString();

                GUILayout.Space(bufferSpace);
                GUILayout.BeginVertical();


                if (IsTalentActive(t) == true)
                {
                    if (GUILayout.Button(t.Name, GUILayout.Width(iconWidth), GUILayout.Height(iconHeight)))
                    {
                        if (Event.current.button == 0)
                        {
                            SpendPoint(t);
                        }
                        else if (Event.current.button == 1)
                        {
                            RemovePoint(t);
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

    private void ApplyChanges()
    {

        foreach (var pair in talentAllocation)
        {

            if(pair.Value > 0)
            {
                for(int i = 0; i < pair.Value; i++)
                {
                    Controller.PlayerController.TalentManager.SpendPoint(pair.Key);
                }
            }
        }
        
        
        Controller.PlayerController.TalentManager.MightTreePoints = tempMightTreePoints;
        Controller.PlayerController.TalentManager.MagicTreePoints = tempMagicTreePoints;
        
        Controller.PlayerController.TalentManager.TalentPointPool = thisPool;

        InitTalentAllocation();

        applied = true;
    }

    private bool IsTalentActive(Talent talent)
    {
        
        
        if (mightTree.Contains(talent) == true)
        {

            if (tempMightTreePoints >= talent.Depth * TalentManager.depthMultiplier)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (magicTree.Contains(talent) == true)
        {
            if (tempMagicTreePoints >= talent.Depth * TalentManager.depthMultiplier)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void SpendPoint(Talent talent)
    {
        if (thisPool > 0 && (talent.CurrentPoints + talentAllocation[talent]) < talent.MaxPoints && IsTalentActive(talent) == true)
        {

            thisPool--;
            talentAllocation[talent]++;

            if (mightTree.Contains(talent) == true)
            {
                tempMightTreePoints++;
            }
            else if (magicTree.Contains(talent) == true)
            {
                tempMagicTreePoints++;
            }
        }
    }

    private void RemovePoint(Talent talent)
    {
        if (talentAllocation[talent] > 0)
        {

            if (mightTree.Contains(talent) == true)
            {


                foreach (Talent t in mightTree)
                {
                    if (t.Depth > talent.Depth && (t.CurrentPoints + talentAllocation[t]) > 0)
                    {
                        if ((tempMightTreePoints - 1) <= t.Depth * TalentManager.depthMultiplier)
                        {
                            return;
                        }
                    }
                }

                tempMightTreePoints--;
            }
            else if (magicTree.Contains(talent) == true)
            {


                foreach (Talent t in magicTree)
                {
                    if (t.Depth > talent.Depth && (t.CurrentPoints + talentAllocation[t]) > 0)
                    {
                        if ((tempMagicTreePoints - 1) <= t.Depth * TalentManager.depthMultiplier)
                        {
                            return;
                        }
                    }
                }

                tempMagicTreePoints--;
            }

            thisPool++;
            talentAllocation[talent]--;
            
        }
    }

    private void InitTalentAllocation()
    {

        talentAllocation = new Dictionary<Talent, int>();
        
        foreach (Talent t in mightTree)
        {
            talentAllocation.Add(t, 0);
        }

        foreach (Talent t in magicTree)
        {
            talentAllocation.Add(t, 0);
        }
    }

    private void Respec(string tree)
    {
        if (tree == "might")
        {
            

            thisPool += tempMightTreePoints;
            tempMightTreePoints = 0;
        }
        else if (tree == "magic")
        {
            

            thisPool += tempMagicTreePoints;
            tempMagicTreePoints = 0;
        }

        Controller.PlayerController.TalentManager.Respec(tree);

        InitTalentAllocation();
    }
}

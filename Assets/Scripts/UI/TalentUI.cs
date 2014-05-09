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

    private Dictionary<Talent, int> talentMightAllocation;
    private Dictionary<Talent, int> talentMagicAllocation;

    private int tempMightTreePoints;
    private int tempMagicTreePoints;

    private Talent hoverTalentMight;
    private Talent hoverTalentMagic;

    private Dictionary<Rect, Talent> mightRects;
    private Dictionary<Rect, Talent> magicRects;

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

        InitMightTalentAllocation();
        InitMagicTalentAllocation();
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

        if (thisPool > 0 && applied == true)
        {
            applied = false;
        }

        GUI.Window(0, windowDimensions, OnWindow, "Talents");

        string info = "";
        int tooltipHeight = 0;
        int tooltipWidth = 0;

        if (hoverTalentMight != null)
        {

            

            if (hoverTalentMight.TalentAbility != null)
            {
                

                info = hoverTalentMight.TalentAbility.Name + "\n"
                     + "Damage: " + hoverTalentMight.TalentAbility.DamageMod.ToString() + "\n"
                     + "Cost: " + hoverTalentMight.TalentAbility.ResourceCost.ToString() + "\n"
                     + "Range: " + hoverTalentMight.TalentAbility.Range.ToString() + "\n"
                     + "Cooldown: " + hoverTalentMight.TalentAbility.Cooldown.ToString();

                tooltipHeight = 83;
                tooltipWidth = 150;

            }
            else
            {
                info = hoverTalentMight.Name + "\n" + hoverTalentMight.ReadableBonus();

                tooltipHeight = 40;
                tooltipWidth = 170;
            }


            GUIContent thisContent = new GUIContent(info);

            GUIStyle thisStyle = new GUIStyle("box");
            thisStyle.alignment = TextAnchor.UpperLeft;
            thisStyle.normal.textColor = Color.white;

            GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, tooltipWidth, tooltipHeight), thisContent, thisStyle);
        }
        else if (hoverTalentMagic != null)
        {

            if (hoverTalentMagic.TalentAbility != null)
            {


                info = hoverTalentMagic.TalentAbility.Name + "\n"
                     + "Damage: " + hoverTalentMagic.TalentAbility.DamageMod.ToString() + "\n"
                     + "Cost: " + hoverTalentMagic.TalentAbility.ResourceCost.ToString() + "\n"
                     + "Range: " + hoverTalentMagic.TalentAbility.Range.ToString() + "\n"
                     + "Cooldown: " + hoverTalentMagic.TalentAbility.Cooldown.ToString();

                tooltipHeight = 83;
                tooltipWidth = 150;
            }
            else
            {
                info = hoverTalentMagic.Name + "\n" + hoverTalentMagic.ReadableBonus();

                tooltipHeight = 40;
                tooltipWidth = 170;
            }


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

        List<Talent> tempTalents = new List<Talent>();
        GUIContent tempTalentLabel = new GUIContent();
        int count = 0;
        float iconWidth = 0f;
        float iconHeight = 30f;
        Rect thisRect = new Rect();
        mightRects = new Dictionary<Rect, Talent>();
        magicRects = new Dictionary<Rect, Talent>();

        GUILayout.BeginArea(new Rect(5,20,WIDTH, 20));

        

        GUI.Label(new Rect(0,0,(WIDTH / 6)+20,20), "Unused Talent Points: " + thisPool.ToString());

        //GUILayout.Label("Unused Talent Points: " + thisPool.ToString(), GUILayout.Width((WIDTH / 6)+20), GUILayout.Height(20));


        if (applied == false)
        {
            if (GUI.Button(new Rect((WIDTH / 6)+20,0,80,20),"Apply"))
            {
                ApplyChanges();
            }
        }
        else
        {
            GUI.enabled = false;
            GUI.Button(new Rect((WIDTH / 6) + 20, 0, 80, 20), "Apply");
            GUI.enabled = true;
        }

        

        GUILayout.EndArea();

        #region Might Tree GUI

        //GUI.BeginGroup(new Rect(0, 53, (WIDTH / 2), HEIGHT));



        GUI.Label(new Rect(0,53,(WIDTH / 6) - bufferSpace,25), mightLabel, titleStyle);

        

        GUI.Label(new Rect((WIDTH / 6) - bufferSpace,53,(WIDTH / 6) - bufferSpace,25), new GUIContent(tempMightTreePoints.ToString() + " points"), titleStyle);       
        
        



        if (Controller.PlayerController.TalentManager.MightTreePoints > 0)
        {

            if (GUI.Button(new Rect(((WIDTH / 6) - bufferSpace) * 2, 53, (WIDTH / 6) - bufferSpace, 25), "RESPEC"))
            {
                Respec("might");
            }
        }
        else
        {
            GUI.enabled = false;
            GUI.Button(new Rect(((WIDTH / 6) - bufferSpace) * 2, 53, (WIDTH / 6) - bufferSpace, 25), "RESPEC");
            GUI.enabled = true;
        }


        

        


        tempTalents = mightTree.FindAll(delegate(Talent tal) { return tal.Depth == count; });

        float xOffset = bufferSpace;
        float yOffset = 105;

        while(tempTalents.Count != 0)
        {
            
            
            iconWidth = (((WIDTH/2)) / tempTalents.Count) - (bufferSpace * 2);

            


            foreach (Talent t in tempTalents)
            {
                tempTalentLabel.text = (t.CurrentPoints + talentMightAllocation[t]).ToString() + "/" + t.MaxPoints.ToString();
                
                

                thisRect = new Rect(xOffset, yOffset, iconWidth, iconHeight);

                

                if (IsTalentActive(t) == true)
                {
                    if (GUI.Button(thisRect, t.Name))
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
                    GUI.Button(thisRect, t.Name);
                    GUI.enabled = true;
                }

                mightRects.Add(thisRect, t);

                GUI.Label(new Rect(xOffset, yOffset + iconHeight, iconWidth, 10), tempTalentLabel, labelStyle);
                

                xOffset += iconWidth + bufferSpace;

            }

            
            

            count++;
            tempTalents = mightTree.FindAll(delegate(Talent tal) { return tal.Depth == count; });

            xOffset = bufferSpace;
            yOffset += iconHeight + 22;
        }

        /*

        Vector2 mps = GUIUtility.ScreenToGUIPoint(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));

        Debug.Log("mps: "+ mps.ToString());
        */
        

        //GUI.EndGroup();

        #endregion

        
        SetHoverTalentMight(mightRects);


        #region Magic Tree GUI

        count = 0;

        //GUILayout.BeginArea(new Rect((WIDTH / 2), 54, WIDTH / 2, HEIGHT));

        GUI.Label(new Rect((WIDTH / 2), 54, (WIDTH / 6) - bufferSpace, 25), magicLabel, titleStyle);



        GUI.Label(new Rect((WIDTH / 2) + (WIDTH / 6) - bufferSpace, 54, (WIDTH / 6) - bufferSpace, 25), new GUIContent(tempMagicTreePoints.ToString() + " points"), titleStyle);

        



        if (Controller.PlayerController.TalentManager.MagicTreePoints > 0)
        {

            if (GUI.Button(new Rect((WIDTH / 2)+(((WIDTH / 6) - bufferSpace) * 2), 54, (WIDTH / 6) - bufferSpace, 25), "RESPEC"))
            {
                Respec("magic");
            }
        }
        else
        {
            GUI.enabled = false;
            GUI.Button(new Rect((WIDTH / 2) + (((WIDTH / 6) - bufferSpace) * 2), 54, (WIDTH / 6) - bufferSpace, 25), "RESPEC");
            GUI.enabled = true;
        }







        tempTalents = magicTree.FindAll(delegate(Talent tal) { return tal.Depth == count; });

        xOffset = (WIDTH / 2)+ bufferSpace;
        yOffset = 106;

        while (tempTalents.Count != 0)
        {
            

            iconWidth = (((WIDTH / 2)) / tempTalents.Count) - (bufferSpace * 2);

           




            foreach (Talent t in tempTalents)
            {
                tempTalentLabel.text = (t.CurrentPoints + talentMagicAllocation[t]).ToString() + "/" + t.MaxPoints.ToString();

                

                thisRect = new Rect(xOffset, yOffset, iconWidth, iconHeight);



                if (IsTalentActive(t) == true)
                {

                    if (GUI.Button(thisRect, t.Name))
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
                    GUI.Button(thisRect, t.Name);
                    GUI.enabled = true;
                }


                magicRects.Add(thisRect, t);

                GUI.Label(new Rect(xOffset, yOffset + iconHeight, iconWidth, 10), tempTalentLabel, labelStyle);
                

                xOffset += iconWidth + bufferSpace;

            }

            count++;
            tempTalents = magicTree.FindAll(delegate(Talent tal) { return tal.Depth == count; });

            xOffset = (WIDTH / 2) + bufferSpace;
            yOffset += iconHeight + 22;
        }

        //GUILayout.EndVertical();

        //GUILayout.EndArea();

        #endregion

        SetHoverTalentMagic(magicRects);

    }

    private void ApplyChanges()
    {

        foreach (var pair in talentMightAllocation)
        {
                    
            
            if(pair.Value > 0)
            {
                for(int i = 0; i < pair.Value; i++)
                {
                    Controller.PlayerController.TalentManager.SpendPoint(pair.Key);
                }
            }
        }

        foreach (var pair in talentMagicAllocation)
        {
            
            if (pair.Value > 0)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    Controller.PlayerController.TalentManager.SpendPoint(pair.Key);
                }
            }
        }


        tempMightTreePoints = Controller.PlayerController.TalentManager.MightTreePoints;
        tempMagicTreePoints = Controller.PlayerController.TalentManager.MagicTreePoints;

        thisPool = Controller.PlayerController.TalentManager.TalentPointPool;

        InitMightTalentAllocation();
        InitMagicTalentAllocation();

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
        if (thisPool > 0 && mightTree.Contains(talent) == true && (talent.CurrentPoints + talentMightAllocation[talent]) < talent.MaxPoints && IsTalentActive(talent) == true)
        {

            thisPool--;

            talentMightAllocation[talent]++;
            tempMightTreePoints++;
        }
        else if (thisPool > 0 && magicTree.Contains(talent) == true && (talent.CurrentPoints + talentMagicAllocation[talent]) < talent.MaxPoints && IsTalentActive(talent) == true)
        {
            thisPool--;

            talentMagicAllocation[talent]++;
            tempMagicTreePoints++;
        }
    }

    private void RemovePoint(Talent talent)
    {
        if (mightTree.Contains(talent) == true && talentMightAllocation[talent] > 0)
        {

            foreach (Talent t in mightTree)
            {
                if (t.Depth > talent.Depth && (t.CurrentPoints + talentMightAllocation[t]) > 0)
                {
                    if ((tempMightTreePoints - 1) <= t.Depth * TalentManager.depthMultiplier)
                    {
                        return;
                    }
                }
            }

            tempMightTreePoints--;
           

            thisPool++;
            talentMightAllocation[talent]--;
            
        }
        else if (magicTree.Contains(talent) == true && talentMagicAllocation[talent] > 0)
        {

            foreach (Talent t in magicTree)
            {
                if (t.Depth > talent.Depth && (t.CurrentPoints + talentMagicAllocation[t]) > 0)
                {
                    if ((tempMagicTreePoints - 1) <= t.Depth * TalentManager.depthMultiplier)
                    {
                        return;
                    }
                }
            }

            tempMagicTreePoints--;

            thisPool++;
            talentMagicAllocation[talent]--;

        }
    }

    private void InitMightTalentAllocation()
    {

        talentMightAllocation = new Dictionary<Talent, int>();
        
        foreach (Talent t in mightTree)
        {
            talentMightAllocation.Add(t, 0);
        }
    }

    private void InitMagicTalentAllocation()
    {

        talentMagicAllocation = new Dictionary<Talent, int>();
        
        foreach (Talent t in magicTree)
        {
            talentMagicAllocation.Add(t, 0);
        }
    }

    private void Respec(string tree)
    {
        Controller.PlayerController.TalentManager.Respec(tree);
        
        if (tree == "might")
        {

            tempMightTreePoints = Controller.PlayerController.TalentManager.MightTreePoints;

            thisPool = Controller.PlayerController.TalentManager.TalentPointPool;

            InitMightTalentAllocation();
        }
        else if (tree == "magic")
        {

            tempMagicTreePoints = Controller.PlayerController.TalentManager.MagicTreePoints;

            thisPool = Controller.PlayerController.TalentManager.TalentPointPool;

            InitMagicTalentAllocation();
        }  
    }

    private void SetHoverTalentMight(Dictionary<Rect, Talent> hoverRects)
    {

        
        Vector2 mPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        
        
        foreach (var pair in hoverRects)
        {

            if (ButtonContains(pair.Key, GUIUtility.ScreenToGUIPoint(mPos)))
            {

                
                hoverTalentMight = pair.Value;
                
                return;
            }
            else
            {
                
                hoverTalentMight = null;
            }
        }
        

    }

    private void SetHoverTalentMagic(Dictionary<Rect, Talent> hoverRects)
    {


        Vector2 mPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        foreach (var pair in hoverRects)
        {

            if (ButtonContains(pair.Key,GUIUtility.ScreenToGUIPoint(mPos)))
            {
                hoverTalentMagic = pair.Value;
                return;
            }
            else
            {
                hoverTalentMagic = null;
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

    private void Press(Talent t)
    {
        if (Event.current.button == 0 && Event.current.type == EventType.MouseUp)
        {
            SpendPoint(t);
        }
        else if (Event.current.button == 1 && Event.current.type == EventType.MouseUp)
        {
            RemovePoint(t);
        }
    }
}

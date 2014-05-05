using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AttributesUI : UIState
{
    private const float WIDTH = 400;
    private const float HEIGHT = 500;
    private Rect windowDimensions;

    private GUIStyle labelStyle;

    private float health, resource, power, defense, attackSpeed, movementSpeed;

    private bool applied;

    private int attrPoints;
    

    public AttributesUI(int id, UIController controller)
        : base(id, controller)
    {
        windowDimensions = new Rect(Screen.width - (WIDTH + 50), Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        


        labelStyle = new GUIStyle();
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 14;
        labelStyle.normal.textColor = Color.white;
    }
    public override void Enter()
    {
        base.Enter();
        health = Controller.Player.currentAtt.Health;
        resource = Controller.Player.currentAtt.Resource;
        power = Controller.Player.currentAtt.Power;
        defense = Controller.Player.currentAtt.Defense;
        attackSpeed = Controller.Player.currentAtt.AttackSpeed;
        movementSpeed = Controller.Player.currentAtt.MovementSpeed;

        attrPoints = Controller.Player.AttributePoints;

        if (attrPoints > 0)
        {
            applied = false;
        }
        else
        {
            applied = true;
        }
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
        GUI.Window(0, windowDimensions, OnWindow, "Attributes");
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

        float rowWidth = (WIDTH - 5) / 2;
        float rowHeight = 20f;

        float boxWidth = ((WIDTH - 5) / 4)-25;
        float buttonWidth = (((WIDTH - 5) / 8)-25);

        GUILayout.BeginArea(new Rect(5, 15, WIDTH, 35));

        if (applied == false)
        {
            GUILayout.Label(new GUIContent("Unused Skill Points: " + attrPoints.ToString()), labelStyle, GUILayout.Width(WIDTH), GUILayout.Height(35));
        }
        

        GUILayout.EndArea();
        
        GUILayout.BeginArea(new Rect(5, 50, WIDTH-5, HEIGHT-20));

        GUILayout.BeginVertical();

        #region Health

        GUILayout.BeginHorizontal();

        GUILayout.Label(new GUIContent("Health"), labelStyle, GUILayout.Width(rowWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (attrPoints > 0 && applied == false)
        {
            if (GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                AddPoints(ref health, 5);
            }
        }
        else if (attrPoints == 0 && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }


        GUILayout.Space(5);

        GUILayout.Box(new GUIContent(health.ToString()), GUILayout.Width(boxWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (health > Controller.Player.currentAtt.Health && applied == false)
        {
            if (GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                RemovePoints(ref health, 5);
            }
        }
        else if (health == Controller.Player.currentAtt.Health && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }

        GUILayout.EndHorizontal();

        #endregion

        GUILayout.Space(15);

        #region Resource

        GUILayout.BeginHorizontal();

        GUILayout.Label(new GUIContent("Resource"), labelStyle, GUILayout.Width(rowWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (attrPoints > 0 && applied == false)
        {
            if (GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                AddPoints(ref resource, 5);
            }
        }
        else if (attrPoints == 0 && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }


        GUILayout.Space(5);

        GUILayout.Box(new GUIContent(resource.ToString()), GUILayout.Width(boxWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (resource > Controller.Player.currentAtt.Resource && applied == false)
        {
            if (GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                RemovePoints(ref resource, 5);
            }
        }
        else if (resource == Controller.Player.currentAtt.Resource && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }

        GUILayout.EndHorizontal();

        #endregion

        GUILayout.Space(15);

        #region Power

        GUILayout.BeginHorizontal();

        GUILayout.Label(new GUIContent("Power"), labelStyle, GUILayout.Width(rowWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (attrPoints > 0 && applied == false)
        {
            if (GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                AddPoint(ref power);
            }
        }
        else if ( attrPoints == 0 && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }


        GUILayout.Space(5);

        GUILayout.Box(new GUIContent(power.ToString()), GUILayout.Width(boxWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (power > Controller.Player.currentAtt.Power && applied == false)
        {
            if (GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                RemovePoint(ref power);
            }
        }
        else if (power == Controller.Player.currentAtt.Power && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }

        GUILayout.EndHorizontal();

        #endregion

        GUILayout.Space(15);

        #region Defense

        GUILayout.BeginHorizontal();

        GUILayout.Label(new GUIContent("Defense"), labelStyle, GUILayout.Width(rowWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (attrPoints > 0 && applied == false)
        {
            if (GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                AddPoint(ref defense);
            }
        }
        else if (attrPoints == 0 && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }

        GUILayout.Space(5);

        GUILayout.Box(new GUIContent(defense.ToString()), GUILayout.Width(boxWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (defense > Controller.Player.currentAtt.Defense && applied == false)
        {
            if (GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                RemovePoint(ref defense);
            }
        }
        else if (defense == Controller.Player.currentAtt.Defense && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }

        GUILayout.EndHorizontal();

        #endregion

        GUILayout.Space(15);

        #region Attack Speed

        GUILayout.BeginHorizontal();

        GUILayout.Label(new GUIContent("Attack Speed"), labelStyle, GUILayout.Width(rowWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (attrPoints > 0 && applied == false)
        {
            if (GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                AddPoint(ref attackSpeed);
            }
        }
        else if (attrPoints == 0 && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }

        GUILayout.Space(5);

        GUILayout.Box(new GUIContent(attackSpeed.ToString()), GUILayout.Width(boxWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (attackSpeed > Controller.Player.currentAtt.AttackSpeed && applied == false)
        {
            if (GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                RemovePoint(ref attackSpeed);
            }
        }
        else if (attackSpeed == Controller.Player.currentAtt.AttackSpeed && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }

        GUILayout.EndHorizontal();

        #endregion

        GUILayout.Space(15);

        #region Movement Speed

        GUILayout.BeginHorizontal();

        GUILayout.Label(new GUIContent("Movement Speed"), labelStyle, GUILayout.Width(rowWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (attrPoints > 0 && applied == false)
        {
            if (GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                AddPoint(ref movementSpeed);
            }
        }
        else if (attrPoints == 0 && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("+", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }

        GUILayout.Space(5);

        GUILayout.Box(new GUIContent(movementSpeed.ToString()), GUILayout.Width(boxWidth), GUILayout.Height(rowHeight));

        GUILayout.Space(5);

        if (movementSpeed > Controller.Player.currentAtt.MovementSpeed && applied == false)
        {
            if (GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight)))
            {
                RemovePoint(ref movementSpeed);
            }
        }
        else if (movementSpeed == Controller.Player.currentAtt.MovementSpeed && applied == false)
        {
            GUI.enabled = false;
            GUILayout.Button("-", GUILayout.Width(buttonWidth), GUILayout.Height(rowHeight));
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Space(buttonWidth);
        }

        GUILayout.EndHorizontal();

        #endregion

        GUILayout.Space(30);

        GUILayout.BeginHorizontal();

        GUILayout.Space(rowWidth);

        if (applied == false)
        {
            if (GUILayout.Button("Apply Changes", GUILayout.Width(rowWidth-5), GUILayout.Height(rowHeight)))
            {
                ApplyChanges();
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.EndArea();
    }

    private void AddPoint(ref float attr)
    {
        if (attrPoints > 0)
        {
            attr++;
            attrPoints--;
        }
    }

    private void AddPoints(ref float attr, int points)
    {
        if (attrPoints > 0)
        {
            attr += points;
            attrPoints--;
        }
    }

    private void RemovePoint(ref float attr)
    {
        if (attrPoints < Controller.Player.AttributePoints)
        {
            attr--;
            attrPoints++;
        }
    }

    private void RemovePoints(ref float attr, int points)
    {
        if (attrPoints < Controller.Player.AttributePoints)
        {
            attr -= points;
            attrPoints++;
        }
    }

    private void ApplyChanges()
    {
        Controller.Player.currentAtt.Health = health;
        Controller.Player.currentAtt.Resource = resource;
        Controller.Player.currentAtt.Power = power;
        Controller.Player.currentAtt.Defense = defense;
        Controller.Player.currentAtt.AttackSpeed = attackSpeed;
        Controller.Player.currentAtt.MovementSpeed = movementSpeed;

        Controller.Player.AttributePoints = attrPoints;

        if (attrPoints <= 0)
        {
            applied = true;
        }
    }

    
}

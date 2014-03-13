using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour 
{
    public List<Ability> abilities;
    public List<float> activeCoolDowns;

    private float timeLeft;

    private Rect CDBox1;
    private Rect CDBox2;
    private Rect CDBox3;
    private Rect CDBox4;
    private GUIStyle CDBox;

	// Use this for initialization
    public void Awake()
    {
        abilities = new List<Ability>(6);
        activeCoolDowns = new List<float>(6);

        for (int i = 0; i < abilities.Capacity; i++)
        {
            abilities.Add(null);
            activeCoolDowns.Add(0.0f);
        }

        
        

        #region Cooldown GUI init

        CDBox1 = new Rect(Screen.width * .65f, Screen.height * .90f, Screen.width * .45f, Screen.height * .1f);
        CDBox2 = new Rect(Screen.width * .65f, Screen.height * .925f, Screen.width * .45f, Screen.height * .1f);
        CDBox3 = new Rect(Screen.width * .65f, Screen.height * .950f, Screen.width * .45f, Screen.height * .1f);
        CDBox4 = new Rect(Screen.width * .65f, Screen.height * .975f, Screen.width * .45f, Screen.height * .1f);
        CDBox = new GUIStyle();

        #endregion
    }


	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    public void AddAbility()
    {

    }

    public void UseAbility()
    {

    }

    #region ability cooldown GUI

    void OnGUI()
    {

        float timeLeft = 0;

        CDBox.normal.textColor = Color.white;

        if (activeCoolDowns[2] > Time.time)
        {
            timeLeft = activeCoolDowns[2] - Time.time;
        }
        else
        {
            timeLeft = 0;
        }

        GUI.Label(CDBox1, "Cleave CD Remaining: " + timeLeft.ToString("F") + "s", CDBox);

        if (activeCoolDowns[3] > Time.time)
        {
            timeLeft = activeCoolDowns[3] - Time.time;
        }
        else
        {
            timeLeft = 0;
        }

        GUI.Label(CDBox2, "Fus Ro Dah CD Remaining: " + timeLeft.ToString("F") + "s", CDBox);

        if (activeCoolDowns[4] > Time.time)
        {
            timeLeft = activeCoolDowns[4] - Time.time;
        }
        else
        {
            timeLeft = 0;
        }

        GUI.Label(CDBox3, "Hadouken CD Remaining: " + timeLeft.ToString("F") + "s", CDBox);

        if (activeCoolDowns[5] > Time.time)
        {
            timeLeft = activeCoolDowns[5] - Time.time;
        }
        else
        {
            timeLeft = 0;
        }



        GUI.Label(CDBox4, "Death Grip CD Remaining: " + timeLeft.ToString("F") + "s", CDBox);
    }

    #endregion
}

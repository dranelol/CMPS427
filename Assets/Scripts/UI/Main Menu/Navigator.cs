using UnityEngine;
using System.Collections;


public class Navigator : MonoBehaviour {
    private const int WIDTH = 200;
    private const int HEIGHT = 300;
	private const int INFO_WIDTH = 400;
    
	// Variable to orient the boxes and their contents
	private int MAIN_LEFT = (Screen.width - WIDTH) / 4 - WIDTH;
    private int MAIN_TOP = (Screen.height - HEIGHT) * 2 / 3;
	private int INFO_LEFT = (Screen.width - INFO_WIDTH) * 3/4 + WIDTH;
	private int INFO_TOP = (Screen.height - HEIGHT) / 3 - 100;
	private int CRED_LEFT = (Screen.width - WIDTH) * 3/4 + WIDTH;
	private int CRED_TOP = (Screen.height - HEIGHT) * 2 / 3 + 100;

	private bool showInfo = false;
	private bool showCredits = false;

	private GUIStyle centerText;

    void OnGUI()
    {
		centerText = new GUIStyle ("label");
		centerText.alignment = TextAnchor.MiddleCenter;

        GUI.Box(new Rect(MAIN_LEFT, CRED_TOP, WIDTH, HEIGHT),
            "OVER THE TOP");

        if (GUI.Button(new Rect(MAIN_LEFT + 20, CRED_TOP + 30, WIDTH - 40, 40), "Start Game"))
        {
            //Application.Quit();
            Application.LoadLevel("setup");
        }

		if (GUI.Button(new Rect(MAIN_LEFT + 20, CRED_TOP + HEIGHT - 200, WIDTH - 40, 40), 
		    "Game Info"))
		{
			// If Info Button pressed, alternate whether the info pane is to be shown
			showInfo = !showInfo;
			showCredits = false;
		}

		if (GUI.Button(new Rect(MAIN_LEFT + 20, CRED_TOP + HEIGHT - 130, WIDTH - 40, 40), 
		    "Credits"))
		{
			// If Credits Button pressed, alternate whether the credits pane is to be shown
			showCredits = !showCredits;
			showInfo = false;
		}

		if (GUI.Button(new Rect(MAIN_LEFT + 20, CRED_TOP + HEIGHT - 60, WIDTH - 40, 40), 
            "Quit")) // Sorry Ryan
        {
            Application.Quit();
        }

		if (showInfo)
		{
			drawInfo ();
		}
		if (showCredits)
		{
			drawCredits ();
		}
    }

	void drawInfo()
	{
		GUI.Box (new Rect (INFO_LEFT, CRED_TOP, INFO_WIDTH, HEIGHT), "GAME INFO");

		string infoString = "SPRING 2014 CMPS427 \n Over the Top \n A dungeon-crawler RPG featuring randomly generated items, enemies, and dungeons.";
		
		GUI.Label (new Rect (INFO_LEFT + 20, CRED_TOP + 30, INFO_WIDTH - 40, HEIGHT - 80), infoString, centerText);

		if (GUI.Button (new Rect (INFO_LEFT + 80, CRED_TOP + HEIGHT - 60, INFO_WIDTH - 160, 30), "Close"))
			showInfo = false;
	}

	void drawCredits()
	{
		GUI.Box (new Rect (CRED_LEFT, CRED_TOP, WIDTH, HEIGHT), "CREDITS");
		
		string creditsString = "Andrew Colvin\nRyan Durel\nDillon Davis\nJoe DeHart\nRyan Adair\nScott Roddy\nAugust Montalbano\nMatt Wallace";
		GUI.Label (new Rect (CRED_LEFT + 20, CRED_TOP + 30, WIDTH - 40, HEIGHT - 100), creditsString, centerText);
		
		if (GUI.Button (new Rect (CRED_LEFT + 40, CRED_TOP + HEIGHT - 60, WIDTH - 80, 30), "Close"))
			showCredits = false;
	}
}

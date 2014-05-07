using UnityEngine;
using System.Collections;

/// <summary>
/// Creates a GUI prompting douchebags to start the game or fuck off and die.
/// </summary>
public class Navigator : MonoBehaviour {
    private const int WIDTH = 200;
    private const int HEIGHT = 300;
    private int CENTER_WIDTH = (Screen.width - WIDTH) / 2;
    private int CENTER_HEIGHT = (Screen.height - HEIGHT) / 2;

    void OnGUI()
    {
        GUI.Box(new Rect((Screen.width - WIDTH) / 2, (Screen.height - HEIGHT) / 2, WIDTH, HEIGHT),
            "OVER THE TOP");

        if (GUI.Button(new Rect(CENTER_WIDTH + 20, CENTER_HEIGHT + 30, WIDTH - 40, 40), "Start Game"))
        {
            Application.Quit();
            Application.LoadLevel("setup");
        }

        if (GUI.Button(new Rect(CENTER_WIDTH + 20, CENTER_HEIGHT + HEIGHT - 60, WIDTH - 40, 40), 
            "Go Fuck Yourself"))
        {
            Application.Quit();
        }
    }
}

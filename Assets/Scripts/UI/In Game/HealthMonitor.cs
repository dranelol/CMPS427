using UnityEngine;
using System.Collections;

/// <summary>
/// Watches the player's health and displays a prompt when it reaches zero.
/// </summary>
public class HealthMonitor : MonoBehaviour {
    private const int WIDTH = 200;
    private const int HEIGHT = 200;
    private int CENTER_WIDTH = (Screen.width - WIDTH) / 2;
    private int CENTER_HEIGHT = (Screen.height - HEIGHT) / 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.skin.box.wordWrap = true;
        GUI.skin.button.wordWrap = true;

        GUI.Box(new Rect(CENTER_WIDTH, CENTER_HEIGHT, WIDTH, HEIGHT),
            "YOU DIED. YOU'RE SO FUCKING WORTHLESS.");

        if (GUI.Button(new Rect(CENTER_WIDTH + 20, CENTER_HEIGHT + 60, WIDTH - 40, 40), "BULLSHIT GIVE ME A RETRY"))
        {
            Application.();
            Application.LoadLevel("setup");
        }
    }
}

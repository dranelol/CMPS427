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
    private bool playerDead = false;
    private PlayerEntity player;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        playerDead = player.CurrentHP <= 0;
	}

    void OnGUI()
    {
        if (playerDead)
        {
            GUI.skin.box.wordWrap = true;
            GUI.skin.button.wordWrap = true;

            GUI.Box(new Rect(CENTER_WIDTH, CENTER_HEIGHT, WIDTH, HEIGHT),
                "YOU ARE DEAD");

            if (GUI.Button(new Rect(CENTER_WIDTH + 20, CENTER_HEIGHT + 60, WIDTH - 40, 40), "TRY AGAIN"))
            {
                player.Respawn();
            }

            if (GUI.Button(new Rect(CENTER_WIDTH + 20, CENTER_HEIGHT + HEIGHT - 60, WIDTH - 40, 40),
                "QUIT"))
            {
                Application.Quit();
            }
        }
    }
}

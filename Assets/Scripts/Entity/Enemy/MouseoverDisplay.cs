using UnityEngine;
using System.Collections;

public class MouseoverDisplay : MonoBehaviour {

	// Display Stuffs
	public string name;
	private float OffsetX = 20;
	private float OffsetY = -10;

	// Mouseover Bools to do stufs right
	private bool show = false;
	private bool mouseOver = false;

	// Health Bar Stuffs
	private GUIStyle barStyle;
	private GUIStyle blackTextStyle;
	private float barOffset = 20;
	private float barHeight = 15;
	private float barWidth = 100;
	private float barMax = 100;

	void Awake()
	{
		barStyle = new GUIStyle();
		Texture2D tex = new Texture2D (1, 1);
		tex.SetPixel (0, 0, Color.red);
		tex.Apply ();
		barStyle.normal.background = tex;

		blackTextStyle = new GUIStyle ();
		blackTextStyle.normal.textColor = Color.black;
	}

	void OnMouseOver()
	{
		show = true;
		mouseOver = true;
	}

	void OnMouseNotOver()
	{
		show = false;
	}

	void Update()
	{
		if (!mouseOver) OnMouseNotOver();
		mouseOver = false;
	}

	void OnGUI()
	{
		if (show) 
		{
			// Get da health numbahjs
			float currentHP = GetComponent<Entity>().CurrentHP;
			float maxHP = GetComponent<Entity> ().currentAtt.Health;

			// Health Bar - Calculate Size
			if (currentHP > maxHP) // If currentHP is left higher than the max because of reasons...
				barWidth = barMax;
			else if (currentHP >= 0) // If health is as it's supposed to be.
				barWidth = barMax * (currentHP / maxHP);
			else // Else, if health goes below zero for unknown reasons (I don't trust it)
				barWidth = 0;

			// Health Bar - Display
			Rect rectBar = new Rect (Input.mousePosition.x + OffsetX, 
			                      Screen.height - Input.mousePosition.y + OffsetY + barOffset, 
			                         barWidth, barHeight);
			GUI.Box (rectBar, new GUIContent(""), barStyle);

			// Name and Health Numbahs on top da bar
			Rect rectText = new Rect (Input.mousePosition.x + OffsetX, 
			                          Screen.height - Input.mousePosition.y + OffsetY, 150, 40);
			GUI.Label (rectText, name);
			GUI.Label (rectBar, " " + currentHP + "/" + maxHP, blackTextStyle);
		}
	}
}

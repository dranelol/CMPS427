using UnityEngine;
using System.Collections;

public class MouseoverDisplay : MonoBehaviour {

	public string name;
	private bool show = false;
	private bool mouseOver = false;
	public float OffsetX = 10;
	public float OffsetY = -10;

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
			float currentHP = GetComponent<Entity>().CurrentHP;
			float maxHP = GetComponent<Entity> ().currentAtt.Health;

			Rect temp = new Rect (Input.mousePosition.x + OffsetX, 
			                      Screen.height - Input.mousePosition.y + OffsetY, 150, 40);

			GUI.Label (temp, name + "\n" + currentHP + "/" + maxHP);
		}
	}
}

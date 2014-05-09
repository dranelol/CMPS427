using UnityEngine;
using System.Collections;

public class MouseHighlight : MonoBehaviour 
{
    public Shader defaultShader;
    public Shader highlight;
    private UIController uiController;

    void Awake()
    {
        defaultShader = Shader.Find("Diffuse");
        highlight = Shader.Find("Self-Illumin/Bumped Diffuse");

        uiController = GameObject.FindWithTag("UI Controller").GetComponent<UIController>();

    }
    void OnMouseEnter()
    {
        //Debug.Log("entering");
        renderer.material.shader = highlight;
        uiController.PlayerController.MouseOverChest = true;
    }

    void OnMouseOver()
    {
        //Debug.Log("asd");
    }

    void OnMouseExit()
    {
        //Debug.Log("exiting");
        renderer.material.shader = defaultShader;
        uiController.PlayerController.MouseOverChest = false;
    }
}

using UnityEngine;
using System.Collections;

public class MouseHighlight : MonoBehaviour 
{
    public Shader defaultShader;
    public Shader highlight;
    

    void Awake()
    {
        defaultShader = Shader.Find("Diffuse");
        highlight = Shader.Find("Self-Illumin/Bumped Diffuse");

    }
    void OnMouseEnter()
    {
        //Debug.Log("entering");
        renderer.material.shader = highlight;
    }

    void OnMouseOver()
    {
        //Debug.Log("asd");
    }

    void OnMouseExit()
    {
        //Debug.Log("exiting");
        renderer.material.shader = defaultShader;
    }
}

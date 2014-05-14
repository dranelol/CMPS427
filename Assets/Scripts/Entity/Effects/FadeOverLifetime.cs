using UnityEngine;
using System.Collections;
using System;

public class FadeOverLifetime : MonoBehaviour {

    public float lifeTime;
    private Renderer renderer;
    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // tinted materials
        try
        {
            if (renderer.material.GetColor("_TintColor").a > 0)
            {
                Color newColor = renderer.material.GetColor("_TintColor");
                newColor.a = newColor.a - (1.0f / lifeTime * Time.deltaTime);

                renderer.material.SetColor("_TintColor", newColor);
            }
        }

        catch (Exception e)
        {
            // normal color materials
            Debug.Log(e.ToString());
            if (renderer.material.GetColor("_Color").a > 0)
            {
                Color newColor = renderer.material.GetColor("_Color");
                newColor.a = newColor.a - (1.0f / lifeTime * Time.deltaTime);

                renderer.material.SetColor("_Color", newColor);
            }
        }
    }
}

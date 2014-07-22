using UnityEngine;
using System.Collections;
using System;

public class FadeProjector : MonoBehaviour
{

    public float lifeTime;
    private Projector projector;

    void Awake()
    {
        projector = GetComponent<Projector>();
    }

    void Update()
    {
        // tinted materials
        try
        {

            if (projector.material.GetColor("_TintColor").a > 0)
            {
                Color newColor = projector.material.GetColor("_TintColor");
                newColor.a = newColor.a - (1.0f / lifeTime * Time.deltaTime);

                projector.material.SetColor("_TintColor", newColor);
            }
        }

        catch (Exception e)
        {
            // normal color materials
            Debug.Log(e.ToString());
            if (projector.material.GetColor("_Color").a > 0)
            {
                Color newColor = projector.material.GetColor("_Color");
                newColor.a = newColor.a - (1.0f / lifeTime * Time.deltaTime);

                projector.material.SetColor("_Color", newColor);
            }
        }
    }
}

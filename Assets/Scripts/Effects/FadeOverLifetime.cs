using UnityEngine;
using System.Collections;

public class FadeOverLifetime : MonoBehaviour {

    public float lifeTime;
    private Renderer renderer;
    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (renderer.material.GetColor("_TintColor").a > 0)
        {
            Color newColor = renderer.material.GetColor("_TintColor");
            newColor.a = newColor.a - (1.0f/lifeTime * Time.deltaTime);

            renderer.material.SetColor("_TintColor", newColor);
        }
    }
}

using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour 
{
    private const float FLICKER_INTERVAL = 0.1f;
    private Light _light;

    void Awake()
    {
        _light = GetComponent<Light>();
        InvokeRepeating("FlickerLight", 0, FLICKER_INTERVAL); 
    }

    private void FlickerLight()
    {
        _light.range = Random.Range(45, 60);
    }
}

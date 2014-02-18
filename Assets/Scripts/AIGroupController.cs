using UnityEngine;
using System.Collections;

public class AIGroupController : MonoBehaviour {
    private const float baseResetDistance = 25;
    private const float groupBufferDistance = 1.5f;

    private Vector3 homePosition;
    private bool inCombat = false;
    private float resetDistance;

    void Awake()
    {
        CalculatePositions();
        homePosition = transform.position;
        resetDistance = baseResetDistance;
    }

    public void BeginCombat(GameObject target)
    {
        if (inCombat == false)
        {
            inCombat = true;

            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<AIController>().Threat(target);
            }
        }
    }

    public void EndCombat()
    {
        inCombat = false;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<AIController>().Reset();
        }

        resetDistance = baseResetDistance;
        CalculatePositions();
    }

    public void CalculatePositions()
    {
    }

    public float BaseResetDistance
    {
        get { return baseResetDistance; }
    }

    public bool InCombat
    {
        get { return inCombat; }
    }

    public float ResetDistance
    {
        get { return resetDistance; }
        set { resetDistance = Mathf.Max(resetDistance, value); }
    }

    public Vector3 HomePosition
    {
        get { return homePosition; }
    }
}

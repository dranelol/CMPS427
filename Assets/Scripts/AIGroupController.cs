using UnityEngine;
using System.Collections;

public class AIGroupController : MonoBehaviour {
    private const float baseResetDistance = 25;
    private const float groupBufferDistance = 1.5f;

    public bool orientationCentered = true;
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
        int childCount = transform.childCount;

        if (childCount == 1)
        {
            transform.GetChild(childCount - 1).GetComponent<AIController>().localHomePosition = homePosition;
        }
        
        else
        {
            float maxRadius = 0;

            foreach (Transform child in transform)
            {
                maxRadius = Mathf.Max(maxRadius, child.GetComponent<NavMeshAgent>().radius);
            }

            float adjustedBufferDistance = maxRadius * 2 + groupBufferDistance;

            if (childCount % 2 != 0)
            {
                if (orientationCentered)
                {
                    transform.GetChild(childCount - 1).GetComponent<AIController>().localHomePosition = homePosition;
                    childCount--;
                }
            }

            float angleBetween = 360 / (float)childCount;

        }
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

using UnityEngine;
using System.Collections.Generic;

public class AIGroupController : MonoBehaviour {
    private const float baseResetDistance = 40;
    private const float groupBufferDistance = 1.5f;

    private List<GameObject> MasterThreatTable;
    private Vector3 homePosition;
    private int GroupLevel;
    private float resetDistance;

    void Start()
    {
        MasterThreatTable = new List<GameObject>();
        homePosition = transform.position;
        GroupLevel = 5;
        resetDistance = baseResetDistance;
    }

    /// <summary>
    /// Apply threat to the entire group. Used when threat should be applied to anything targeting you, ie. heals.
    /// </summary>
    /// <param name="source">The source of threat.</param>
    /// <param name="magnitude">The magnitude of threat.</param>
    public void Threat(GameObject source, float magnitude = 0)
    {
        if (source.tag == "Player")
        {
            if (!MasterThreatTable.Contains(source))
            {
                MasterThreatTable.Add(source);
            }

            if (magnitude > 0)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.GetComponent<AIController>().Threat(source, magnitude);
                }
            }
        }
    }

    private bool TargetInRange(GameObject source)
    {
        if (source != null && CombatMath.DistanceLessThan(transform.position, source.transform.position, resetDistance))
        {
            return true;
        }

        else
        {
            MasterThreatTable.Remove(source);
            return false;
        }
    }

    /// <summary>
    /// Remove the target from all threat tables.
    /// </summary>
    /// <param name="source">The target to remove.</param>
    public void RemoveTarget(GameObject source)
    {
        if (MasterThreatTable.Remove(source))
        {
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<AIController>().RemoveTarget(source);
            }
        }
    }

    public int TargetCount
    {
        get { return MasterThreatTable.Count; }
    }

    public GameObject GetNewTarget()
    {
        if (MasterThreatTable.Count > 0)
        {
            return MasterThreatTable[0];
        }

        else
        {
            return null;
        }
    }

    public void ResetGroup()
    {
        resetDistance = baseResetDistance;
    }

    #region Getters and Setters

    public float ResetDistance
    {
        get { return resetDistance; }
    }

    public Vector3 HomePosition
    {
        get { return homePosition; }
    }

    #endregion
}

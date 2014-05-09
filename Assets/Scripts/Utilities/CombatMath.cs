using UnityEngine;
using System.Collections;

public class CombatMath : MonoBehaviour 
{
    public static Vector3 GetCenter(Transform entityTransform)
    {
        return entityTransform.transform.TransformPoint(entityTransform.GetComponent<CapsuleCollider>().center);
    }

    public static bool RayCast(Transform attacker, Transform defender, out RaycastHit hitInfo, float range = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers)
    {
        Vector3 origin = GetCenter(defender);
        Vector3 target = GetCenter(attacker);

        if (range != Mathf.Infinity)
        {
            if(attacker.GetComponent<MovementFSM>() != null)
            {
                range += attacker.GetComponent<MovementFSM>().Radius;
            }
        }

        return Physics.Raycast(origin, target - origin, out hitInfo, range, layerMask);
    }

    public static bool DistanceGreaterThan(Vector3 positionA, Vector3 positionB, float distance)
    {
        return (positionB - positionA).sqrMagnitude > Mathf.Pow(distance, 2f);
    }

    public static bool DistanceLessThan(Vector3 positionA, Vector3 positionB, float distance)
    {
        return (positionB - positionA).sqrMagnitude < Mathf.Pow(distance, 2f);
    }

    public static bool DistanceGreaterThanEqual(Vector3 positionA, Vector3 positionB, float distance)
    {
        return (positionB - positionA).sqrMagnitude >= Mathf.Pow(distance, 2f);
    }

    public static bool DistanceLessThanEqual(Vector3 positionA, Vector3 positionB, float distance)
    {
        return (positionB - positionA).sqrMagnitude <= Mathf.Pow(distance, 2f);
    }
}

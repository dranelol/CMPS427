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

    public static Vector3 ForwardRayCastToMouse(Vector3 origin)
    {
        int terrainMask = LayerMask.NameToLayer("Terrain");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayCastTarget;
        Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity, 1 << terrainMask);
        Vector3 vectorToMouse = rayCastTarget.point - origin;
        Vector3 forward = new Vector3(vectorToMouse.x, origin.y, vectorToMouse.z).normalized;

        return forward;
    }

    public static bool DistanceGreaterThan(Vector3 positionA, Vector3 positionB, float distance)
    {
        return (positionB - positionA).sqrMagnitude > distance * distance;
    }

    public static bool DistanceLessThan(Vector3 positionA, Vector3 positionB, float distance)
    {
        return (positionB - positionA).sqrMagnitude < distance * distance;
    }

    public static bool DistanceGreaterThanEqual(Vector3 positionA, Vector3 positionB, float distance)
    {
        return (positionB - positionA).sqrMagnitude >= distance * distance;
    }

    public static bool DistanceLessThanEqual(Vector3 positionA, Vector3 positionB, float distance)
    {
        return (positionB - positionA).sqrMagnitude <= distance * distance;
    }
}

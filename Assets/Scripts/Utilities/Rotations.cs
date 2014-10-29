using UnityEngine;
using System.Collections;

public class Rotations
{
    public static Vector3 RotateAboutX(Vector3 vectorToRotate, float angle)
    {
        Quaternion rotation = Quaternion.Euler(angle, 0, 0);
        return rotation * vectorToRotate;
    }
    public static Vector3 RotateAboutY(Vector3 vectorToRotate, float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        return rotation * vectorToRotate;
    }
    public static Vector3 RotateAboutZ(Vector3 vectorToRotate, float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        return rotation * vectorToRotate;
    }

    /// <summary>
    /// Determine the signed angle between two vectors, with normal 'n'
    /// as the rotation axis.
    /// </summary>

    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {

        return Mathf.Atan2(

            Vector3.Dot(n, Vector3.Cross(v1, v2)),

            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;

    }
}

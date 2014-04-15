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
}

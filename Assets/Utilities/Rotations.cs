using UnityEngine;
using System.Collections;

public class Rotations
{

    /// <summary>
    /// Rotates a vector about the x axis
    /// </summary>
    /// <param name="thisVector">Vector to rotate</param>
    /// <param name="angle">Angle to rotate by, in degrees</param>
    /// <returns></returns>
    public static Vector3 RotateAboutX(Vector3 thisVector, float angle)
    {
        Vector3 newVector = new Vector3(thisVector.x, thisVector.y, thisVector.z);
        newVector.y = (Mathf.Cos(Mathf.Deg2Rad * angle) * thisVector.y) - (Mathf.Sin(Mathf.Deg2Rad * angle) * thisVector.z);
        newVector.z = (Mathf.Cos(Mathf.Deg2Rad * angle) * thisVector.z) + (Mathf.Sin(Mathf.Deg2Rad * angle) * thisVector.y);
        return newVector;
    }


    public static Vector3 RotateAboutY(Vector3 thisVector, float angle)
    {
        Vector3 newVector = new Vector3(thisVector.x, thisVector.y, thisVector.z);
        newVector.x = (Mathf.Cos(Mathf.Deg2Rad * angle) * thisVector.x) + (Mathf.Sin(Mathf.Deg2Rad * angle) * thisVector.z);
        newVector.z = (Mathf.Cos(Mathf.Deg2Rad * angle) * thisVector.z) - (Mathf.Sin(Mathf.Deg2Rad * angle) * thisVector.x);
        return newVector;
    }

    public static Vector3 RotateAboutZ(Vector3 thisVector, float angle)
    {
        Vector3 newVector = new Vector3(thisVector.x, thisVector.y, thisVector.z);
        newVector.x = (Mathf.Cos(Mathf.Deg2Rad * angle) * thisVector.x) - (Mathf.Sin(Mathf.Deg2Rad * angle) * thisVector.y);
        newVector.y = (Mathf.Cos(Mathf.Deg2Rad * angle) * thisVector.y) + (Mathf.Sin(Mathf.Deg2Rad * angle) * thisVector.x);
        return newVector;
    }
}

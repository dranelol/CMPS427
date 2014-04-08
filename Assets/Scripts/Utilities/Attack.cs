using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour
{
    /// <summary>


    /// Completely removes the velocity from a rigidbody
    /// Note: This is used in most of the force-based attacks
    /// </summary>
    /// <param name="target">Target rigid body from which you are removing velocity</param>
    /// <param name="time">Time, in seconds, after which veloctiy is removed. Default=0</param>
    /// <returns></returns>
    public static IEnumerator RemovePhysics(Rigidbody target, float time=0.0f)
    {
        yield return new WaitForSeconds(time);


        if (target != null)
        {
            target.isKinematic = true;
        }


        yield break;
    }
}

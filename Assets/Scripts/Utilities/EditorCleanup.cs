using UnityEngine;
using System.Collections;

public class EditorCleanup : MonoBehaviour 
{
    void OnApplicationQuit()
    {
        DestroyImmediate(this.gameObject);

    }
}

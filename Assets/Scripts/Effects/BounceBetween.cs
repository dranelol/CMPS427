using UnityEngine;
using System.Collections;

public class BounceBetween : MonoBehaviour 
{
    public GameObject origin;
    public GameObject target;

    public int BounceAmount;

    public float WaitTime;
    
	void Awake () 
    {
	}
	
	void Update () 
    {
        StartCoroutine("bounce");
	}

    IEnumerable bounce()
    {
        for (int i = 0; i < BounceAmount;i++ )
        {
            transform.position = target.transform.position;

            yield return new WaitForSeconds(WaitTime);

            transform.position = origin.transform.position;

            yield return new WaitForSeconds(WaitTime);
        }

        yield return null;
    }
}

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
        StartCoroutine(invoker());
	}

    IEnumerator invoker()
    {
        for (int i = 0; i < BounceAmount; i++)
        {
            StartCoroutine(bounce());
            yield return new WaitForSeconds(WaitTime);
        }

        yield return null;
    }

    IEnumerator bounce()
    {
        if (transform.position == origin.transform.position)
        {
            while (transform.position != target.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.1f);
            }
        }

        else
        {
            while (transform.position != origin.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, origin.transform.position, 0.1f);
            }
        }
        

        yield return null;
    }
}

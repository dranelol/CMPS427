using UnityEngine;
using System.Collections;

public class AggroRadius : MonoBehaviour 
{
    private const float aggroRadius = 5;
    private AIGroupController group;
    private SphereCollider trigger;

	void Awake() 
    {
        group = transform.parent.parent.GetComponent<AIGroupController>();
        trigger = GetComponent<SphereCollider>();
	}

    void Start()
    {
        trigger.radius = transform.parent.GetComponent<NavMeshAgent>().radius * 10;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            group.Threat(other.gameObject, 1);
            trigger.enabled = false;
        }
        else if (other.tag == "Enemy"
            && transform.parent.GetComponent<AIController>().homeNodePosition != other.GetComponent<AIController>().homeNodePosition)
        {
            if (other.GetComponent<AIController>().Target != null)
            {
                group.Threat(other.GetComponent<AIController>().Target, 1);
            }
        }
    }

    public SphereCollider Trigger
    {
        get { return trigger; }
    }
}

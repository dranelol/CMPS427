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
        trigger.radius = aggroRadius;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            group.Threat(other.gameObject, 1);
            trigger.enabled = false;
        }
    }

    public SphereCollider Trigger
    {
        get { return trigger; }
    }
}

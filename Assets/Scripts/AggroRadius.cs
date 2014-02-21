using UnityEngine;
using System.Collections;

public class AggroRadius : MonoBehaviour 
{
    private const float defaultAggroRadius = 5;
    private AIGroupController EnemyGroup;
    private SphereCollider aggroTrigger;

	void Awake() 
    {
        EnemyGroup = transform.parent.parent.GetComponent<AIGroupController>();
        aggroTrigger = GetComponent<SphereCollider>();
	}

    void Start()
    {
        aggroTrigger.radius = defaultAggroRadius;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnemyGroup.Threat(other.gameObject, 1);
            aggroTrigger.enabled = false;
        }
    }

    public SphereCollider AggroTrigger
    {
        get { return aggroTrigger; }
    }
}

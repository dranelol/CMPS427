using UnityEngine;
using System.Collections;

public class AggroRadius : MonoBehaviour 
{
    private const float defaultAggroRadius = 5;
    private AIController EnemyAI;
    private SphereCollider aggroTrigger;

	void Awake() 
    {
        EnemyAI = transform.parent.gameObject.GetComponent<AIController>();
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
            // Do level calculations
            EnemyAI.Threat(other.gameObject);
        }
    }

    public SphereCollider AggroTrigger
    {
        get { return aggroTrigger; }
    }
}

using UnityEngine;
using System.Collections;

public class LootCrateDestroy : MonoBehaviour {


    private LootTrigger lootTrigger;

    void Awake()
    {
        lootTrigger = gameObject.GetComponentInChildren<LootTrigger>();
    }
	
	void Update () 
    {
        if (lootTrigger.SafeToDestroy == true)
        {
            GameObject.Destroy(gameObject);
        }
	}
}

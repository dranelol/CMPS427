using UnityEngine;
using System.Collections;

public class CombatTest : MonoBehaviour 
{

    public CombatFSM AttackFSM;

	// Use this for initialization
	void Start () 
    {
        AttackFSM = GetComponent<CombatFSM>();

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AttackFSM.Attack(10f);
        }
	}
}



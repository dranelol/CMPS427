using UnityEngine;
using System.Collections;

public class DefaultBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameObject.GetComponent<MoveFSM>().Transition(MoveFSM.MoveStates.idle);
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameObject.GetComponent<MoveFSM>().Transition(MoveFSM.MoveStates.move);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameObject.GetComponent<MoveFSM>().Transition(MoveFSM.MoveStates.run);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(gameObject.GetComponent<MoveFSM>().CurrentState.ToString());
        }
	}
}

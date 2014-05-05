using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public Trigger next;
	public GameObject triggerObject;
	public KeyCode triggerKey;
    public bool clickTrigger;
    
	public bool isActive = false;
	protected bool inRange = false;
	void Start () 
    {
		if(isActive) 
        {
            Activate();
        }
	}

	public virtual void Update () 
    {
        //Debug.Log(inRange.ToString());
		if(isActive == true 
        && inRange == true
        && clickTrigger == false
        &&(triggerKey == KeyCode.None || Input.GetKeyDown(triggerKey) == true))
		{
            SetOff ();
        }

        else if (isActive == true
        && inRange == true
        && clickTrigger == true
        && (Input.GetMouseButtonDown(1) == true))
        {
            SetOff();
        }
	}

	public virtual void Activate() 
    {
		Debug.Log (name.ToString()+" Active!");
		isActive = true;
	}

	public virtual void SetOff() 
    {
		isActive = false;
		Debug.Log (name.ToString()+" Triggered!");
		
		if(next != null) 
        {
            Debug.Log("Activating " + next.name.ToString());
            next.Activate();
        }
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			inRange = true;
			Debug.Log ("Collided!");
		}
	}

	public virtual void OnTriggerExit(Collider other)
	{
        if (other.gameObject.tag == "Player") 
        {
            inRange = false;
            Debug.Log("nocollided");
        }
    }

    

}

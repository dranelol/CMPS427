using UnityEngine;
using System.Collections;

public class LootTrigger : Trigger {

	public Shader defaultShader;
	public Shader highlight;
	public GameObject triggerTarget;

    bool inventoryOpened = false;

	void Start() 
    {
		defaultShader = Shader.Find("Diffuse");
		highlight = Shader.Find("Outlined/Silhouetted Diffuse");
		if(isActive) 
        {
            Activate();
        }
	}

	public override void Activate() 
    {
		base.Activate();
	}

	public override void SetOff()
    {
        Debug.Log("get dat loot");

        // bring up inventory for item

        // maybe change object's material?

        inventoryOpened = true;

		base.SetOff();
	}

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (inventoryOpened == true)
            {
                // close inventory if opened
                inventoryOpened = false;
            }
            
        }
    }

    void OnMouseEnter()
    {
        triggerObject.renderer.material.shader = highlight;
    }

    void OnMouseExit()
    {
        triggerObject.renderer.material.shader = defaultShader;
    }

    
}

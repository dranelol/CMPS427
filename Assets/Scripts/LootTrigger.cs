using UnityEngine;
using System.Collections;

public class LootTrigger : Trigger 
{

	public Shader defaultShader;
	public Shader highlight;
	public GameObject triggerTarget;

    private Inventory inventory;
    public Inventory Inventory
    {
        get { return inventory; }
    }

    private UIController uiController;

    private equipmentFactory ef;

    bool inventoryOpened = false;

	void Start() 
    {
        inventory = new Inventory();

        ef = new equipmentFactory();

        inventory.AddItem(ef.randomEquipment(1, 1, equipSlots.slots.Head));
        inventory.AddItem(ef.randomEquipment(1, 1, equipSlots.slots.Head));

        uiController = GameObject.FindWithTag("UI Controller").GetComponent<UIController>();

        defaultShader = Shader.Find("Diffuse");
		highlight = Shader.Find("Outlined/Silhouetted Diffuse");

		if(isActive) 
        {
            Activate();
        }
	}

    public override void Update()
    {
        base.Update();

        if (inventory.IsEmpty() == true)
        {
            inventoryOpened = false;
        }

        if (inventoryOpened == false)
        {
            if (uiController.GuiState == UIController.States.LOOT)
            {
                uiController.GuiState = UIController.States.INGAME;
            }
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
        if (uiController.GuiState == UIController.States.LOOT)
        {
            uiController.GuiState = UIController.States.INGAME;
        }
        else if (uiController.GuiState == UIController.States.INGAME)
        {

            uiController.SetInventory(inventory);
            uiController.GuiState = UIController.States.LOOT;
        }
        // maybe change object's material?

        inventoryOpened = true;

		base.SetOff();
	}

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
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
        Debug.Log("entering");
        triggerObject.renderer.material.shader = highlight;
    }

    void OnMouseOver()
    {
        Debug.Log("asd");
    }

    void OnMouseExit()
    {
        Debug.Log("exiting");
        triggerObject.renderer.material.shader = defaultShader;
    }

    
}

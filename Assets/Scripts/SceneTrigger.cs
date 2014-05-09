using UnityEngine;
using System.Collections;

public class SceneTrigger : Trigger
{

    public Shader defaultShader;
    public Shader highlight;
    public GameObject triggerTarget;
	public string destinationLevelName;

    bool inventoryOpened = false;
    void Awake()
    {
    }
    void Start()
    {
        defaultShader = Shader.Find("Diffuse");
        highlight = Shader.Find("Outlined/Silhouetted Diffuse");
        if (isActive)
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
        // level transition

		GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene = Application.loadedLevelName;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Moba_Camera>().settings.rotation.defualtRotation.y = 0;
		Application.LoadLevel(destinationLevelName);


//		if (Application.loadedLevel == 1)
//        {
//            Application.LoadLevel(2);
//            Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SpawnInParticles, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
//        
//        }
//
//        else
//        {
//            Application.LoadLevel(1);
//            Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SpawnInParticles, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
//        
//        }

        base.SetOff();
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }



    void OnMouseEnter()
    {
		//triggerObject.renderer.material.shader = highlight;
    }

    void OnMouseExit()
    {
        //triggerObject.renderer.material.shader = defaultShader;
    }


}

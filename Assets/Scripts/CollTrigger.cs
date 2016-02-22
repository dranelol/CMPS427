using UnityEngine;
using System.Collections;

public class CollTrigger : Trigger
{

    public Shader defaultShader;
    public Shader highlight;
    public GameObject triggerTarget;
    public int speed, rSpeed;
    public int waitTime;
    public GameObject destination;

    void Start()
    {
        defaultShader = Shader.Find("Diffuse");
        highlight = Shader.Find("Outlined/Silhouetted Diffuse");
        if (isActive) { Activate(); }
    }

    public override void Activate()
    {
        triggerObject.GetComponent<Renderer>().material.shader = highlight;
        triggerObject.GetComponent<Renderer>().material.color = Color.red;     // (1,0,0,1)
        Debug.Log(name.ToString() + " colActive!");
        base.Activate();
    }

    public override void SetOff()
    {

        Debug.Log("F is pressed");
        StartCoroutine("SplitUp");
        triggerObject.GetComponent<Renderer>().material.shader = defaultShader;
        triggerObject.GetComponent<Renderer>().material.color = Color.white;    // (1,1,1,1)

        base.SetOff();
    }

    public IEnumerator SplitUp()
    {
        Debug.Log("This is happening.");
        yield return new WaitForSeconds(waitTime);
        while (!isActive)
        {
            triggerTarget.transform.position = Vector3.Lerp(triggerTarget.transform.position, destination.transform.position, Time.deltaTime * speed);
            triggerTarget.transform.rotation = Quaternion.Lerp(triggerTarget.transform.rotation, destination.transform.rotation, Time.deltaTime * rSpeed);
            yield return null;
        }
    }
}

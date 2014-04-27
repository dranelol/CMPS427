using UnityEngine;
using System.Collections;

public class seekPoint : MonoBehaviour {

	public int speed, rSpeed;
	public int waitTime;
	public GameObject destination;
	public Trigger colTrigger;

	private bool fuck;
	// Use this for initialization
	void Start () {
		fuck = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!colTrigger.isActive && fuck) {
			StartCoroutine("SplitUp");
			fuck = false;
		}
	}

	public IEnumerator SplitUp() {
		Debug.Log ("This is happening.");
		yield return new WaitForSeconds(waitTime);
		while (!colTrigger.isActive) {
			transform.position = Vector3.Lerp(transform.position, destination.transform.position, Time.deltaTime * speed);
			transform.rotation = Quaternion.Lerp(transform.rotation,destination.transform.rotation, Time.deltaTime * rSpeed);
			yield return null;
		}
	}
}

using UnityEngine;
using System.Collections;

public class PlayerPreview : MonoBehaviour {
	public float rotationSpeed = 3.0f;

	private Vector3 rotationVector;
	private Animation animation;
	private string animationPrefix = "attack ";

	// Use this for initialization
	void Start () {
		rotationVector = new Vector3 (0, 1);
		animation = GetComponent<Animation> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (rotationVector * rotationSpeed * Time.deltaTime);

		if (!animation.isPlaying)
		{
			int attackNumber = Random.Range (1, 7);
			animation.Play (animationPrefix + attackNumber);
			animation.PlayQueued ("ready");
		}
	}
}

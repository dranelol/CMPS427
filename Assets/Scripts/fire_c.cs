using UnityEngine;
using System.Collections;

public class fire_c : MonoBehaviour {
	
	float t;
	float rnd=0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	t+=Time.deltaTime*10f;
		if (t>=1f){
			t=0f;
			
				rnd=Random.Range(.55f,.65f);
		}
		this.light.intensity+=(rnd-this.light.intensity)/5f;
	}
}

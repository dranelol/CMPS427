using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour {
	Material mat;
	Material mat1;
	string _name="DAY";
	// Use this for initialization
	void Start () {
	mat = Resources.Load("UFire/sky/DD") as Material;
		mat1 = Resources.Load("UFire/sky/MS") as Material;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 150, 100), _name)){
           if (_name=="DAY"){
			RenderSettings.skybox = new Material(mat);
			_name="NIGHT";
				GameObject.Find("sun").light.intensity=0.1f;
			}
			else { 
			RenderSettings.skybox = new Material(mat1);
			_name="DAY";
				GameObject.Find("sun").light.intensity=0f;
			}
        print (mat);
		}
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonSetUp : MonoBehaviour {

	// Use this for initialization



	void Start () {
		GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(transform.position);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Moba_Camera>().settings.rotation.defualtRotation.y = 180;

		//GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene="Dungeon2";

		GameObject[] GO = GameObject.FindGameObjectsWithTag("ExitTrigger");

		foreach(GameObject x in GO){

			x.GetComponent<SceneTrigger>().destinationLevelName = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene;
			
		}

		//if(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene==null) GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene="Dungeon2";
	}
	
	// Update is called once per frame
	void Update () {

	}


}

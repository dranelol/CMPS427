using UnityEngine;
using System.Collections;

public class portalmanager : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if (Application.loadedLevelName == "ForestOverworld")
        {
            if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene == "OverworldBaseCamp")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(GameObject.Find("OverworldBaseCampEntrance").transform.position);
            }
            else if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene == "floorPlanDemo")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(GameObject.Find("DungeonEntrance").transform.position);
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(GameObject.Find("DefaultEntrance").transform.position);
            }
        }
        if (Application.loadedLevelName == "OverworldBaseCamp")
        {
            if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene == "ForestOverworld")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(GameObject.Find("ForestOverworldEntrance").transform.position);
            }
            else if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene == "floorPlanDemo")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(GameObject.Find("DungeonEntrance").transform.position);
            }
            else if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene == "OverworldDesert")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(GameObject.Find("OverworldDesertEntrance").transform.position);
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(GameObject.Find("DefaultEntrance").transform.position);
            }

        }
        if (Application.loadedLevelName == "OverworldDesert")
        {
            if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene == "floorPlanDemo")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(GameObject.Find("DungeonEntrance").transform.position);
            }
            else if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().previousScene == "OverworldBaseCamp")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(GameObject.Find("OverworldBaseCampEntrance").transform.position);
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementFSM>().Warp(GameObject.Find("DefaultEntrance").transform.position);
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
	



	}
}

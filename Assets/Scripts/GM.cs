using UnityEngine;
using System.Collections;

public class GM : MonoBehaviour {

    public equipmentFactory EquipmentFact;

	// Use this for initialization
	void Start () {
        EquipmentFact = new equipmentFactory();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

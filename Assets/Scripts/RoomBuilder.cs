using UnityEngine;
using System.Collections;

public class RoomBuilder : MonoBehaviour {

	public Vector3[] possibleObjectPositions;

	public GameObject[] possibleObjects;

    public GameObject enemyNode;

	public float[] choiceWeights;

	public int maxObjectCount = 3;

	float weightSum = 0.0f;

	//Joe changes 
	public int roomID;
	public cellType myType;

	// Use this for initialization
	void Start () 
    {

		foreach(float weight in choiceWeights)
		{weightSum += weight;}

		InitRoom();
	}

	void InitRoom() 
    {
        

		foreach(Vector3 pos in possibleObjectPositions)
		{
			float curWeight = Random.Range(0.0f,weightSum);
			int i = 0;
			while(i < choiceWeights.Length && curWeight > choiceWeights[i])
			{
				curWeight -= choiceWeights[i];
				++i;
			}

			if(i < possibleObjects.Length && possibleObjects[i] != null)
			{
				GameObject obj = GameObject.Instantiate(possibleObjects[i]) as GameObject;
                obj.transform.parent = transform;
				obj.transform.position = this.transform.position+pos;

				--maxObjectCount;
				if(maxObjectCount == 0) {break;}
			}
		}

        if (Random.RandomRange(0, 100) % 25 == 0)
        {
            GameObject.Instantiate(enemyNode, transform.position, Quaternion.identity);
        }
	}

}

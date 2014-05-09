using UnityEngine;
using System.Collections;

public class RoomBuilder : MonoBehaviour {

	public Vector3[] possibleObjectPositions;

	public GameObject[] possibleObjects;

    public GameObject enemyNode;

	public float[] choiceWeights;

	public int[,] choiceWeightRange;

	public int[] overallChoiceRange;

	public int maxObjectCount = 3;

	public int itemChoice;

	int numChoices;

	float weightSum = 0.0f;

	//Joe changes 
	public int roomID;
	public cellType myType;

	// Use this for initialization
	void Start () 
    {
		numChoices = possibleObjects.Length;
		choiceWeightRange = new int[numChoices,2];

		for(int k = 0; k < numChoices; k++){
			if(k == 0){
				choiceWeightRange[0,0] = 0;
				choiceWeightRange[0,1] = (int)choiceWeights[0] -1;
			}
			else{
				choiceWeightRange[k,0] = choiceWeightRange[k-1,1]+1;
				choiceWeightRange[k,1] = choiceWeightRange[k-1,1]+(int)choiceWeights[k];
			}

		}

		overallChoiceRange = new int[2]{0,0};

		foreach(int x in choiceWeights){
			overallChoiceRange[1] += (x);
		}
		overallChoiceRange[1]--;
		InitRoom();
	}

	void InitRoom() 
    {
        

		foreach(Vector3 pos in possibleObjectPositions)
		{
			int i = 0;
			itemChoice = EvaluateChoice(Random.Range(overallChoiceRange[0], overallChoiceRange[1]));


			int numItems = Random.Range(0,maxObjectCount);

			if(numItems > 0 && possibleObjects[itemChoice] != null)
			{
				GameObject obj = GameObject.Instantiate(possibleObjects[itemChoice]) as GameObject;
				obj.transform.parent = gameObject.transform;

				obj.transform.localPosition = pos;


				numItems--;
				if(numItems == 0) {break;}
			}
		}

        if (Random.RandomRange(0, 100) % 10 == 0&& myType != cellType.empty)

        {
            GameObject.Instantiate(enemyNode, transform.position, Quaternion.identity);
        }
	}

	public int EvaluateChoice(int randNum){
		for(int i =0; i < choiceWeightRange.Length; i++){
			if(choiceWeightRange[i,0] <= randNum && randNum <= choiceWeightRange[i,1])return i;  
		}
		return 0;
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum cellType{
	empty = 0,
	topLeft = 1,
	top = 2,
	topRight = 3,
	left = 4,
	center = 5,
	right = 6,
	bottomLeft = 7,
	bottom = 8,
	bottomRight = 9,
	fourPillars = 10,
	centerPillar = 11,
	rightHallEnd = 12,
	leftHallEnd = 13,
	topHallEnd = 14,
	bottomHallEnd = 15,
	horizontalHall = 16,
	verticalHall = 17
};

public class FloorBuilder : MonoBehaviour {

	public Vector2 buildArea;
	int w;
	int h;

	// Weights used to select rooms based on the amount
	// of free cells around them (1, 2, 3, and 4, respectively)
	public int[] roomWeights = new int[4]{1,1,1,1};
	public int hallsWeight = 1;
	public int cornersWeight = 1;

	public int[] rooms;// number of free cell adjacent, not counting cells outside the building area

	public int desiredRoomCount; // desired number of cells

	public GameObject room;
	public GameObject Center;// single cell prefab
	public GameObject Empty;
	public GameObject FourPillars;
	public GameObject BottomLeft;
	public GameObject BottomRight;
	public GameObject Bottom;
	public GameObject CenterPillar;
	public GameObject Left;
	public GameObject Right;
	public GameObject TopLeft;
	public GameObject TopRight;
	public GameObject Top;
	public GameObject RightHallEnd;
	public GameObject LeftHallEnd;
	public GameObject TopHallEnd;
	public GameObject BottomHallEnd;
	public GameObject HorizontalHall;
	public GameObject VerticalHall;


	public int[] roomList;


	public cellType[] cellTypes;
	Vector3 [] cellPositions;

	// Use this for initialization
	void Start () 
    {
		roomList = new int[desiredRoomCount];
		if(roomWeights.Length < 4)// clamp minimum array size to 4
		{roomWeights = new int[4]{1,1,1,1};}

		w = (int)buildArea.x;//max num cells wide
		h = (int)buildArea.y;// max num cells deep
		cellTypes = new cellType[w*h];
		cellPositions = new Vector3[w*h];
		buildFloorplan ();
		EvaluateTiles();
	}
	
	void buildFloorplan()
	{

		rooms = new int[w*h];// stores cell types


		for(int r = 0; r < desiredRoomCount; ++r) roomList[r] = -1;// initialize all temp room ids to -1
		for(int r = 0; r < w*h; ++r) rooms[r] = -1;// initialize all room ids to -1

		int x = w/2;// middle of area x value
		int y = h/2;// middle of area y value

		rooms[y*w+x] = 4;// set center tile to have 4 available adjacent cells
		roomList[0] = y*w+x;
		int weightSum = roomWeights[3];
		Debug.Log(weightSum);
		int roomCount = 1;

		while(roomCount < desiredRoomCount && weightSum > 0)
		{
			// Selects a room using the above weights
			int curWeight = Random.Range(0,weightSum);
			int i = 0;
			while (roomList[i] == -1 || rooms[roomList[i]] < 1  || curWeight >= roomWeights[rooms[roomList[i]]-1])
			{

				if(roomList[i] >= 0 && rooms[roomList[i]] > 0) {curWeight -= roomWeights[rooms[roomList[i]]-1];}
				++i;
			}

			int newRoom = pickNewRoom (roomList[i], ref weightSum);// pick a new cell to initiate

			if(newRoom != -1 && rooms[newRoom] > 0)
			{
				if(rooms[roomList[i]] == 0) {roomList[i] = newRoom;}
				else 
				{
					int k = 0;
					while(k < roomList.Length && roomList[k] != -1) ++k;
					if(k < roomList.Length) {roomList[k] = newRoom;}
				}
			} else if(rooms[roomList[i]] == 0) {roomList[i] = -1;}// 

			if(newRoom != -1) {++roomCount;}
		}
		float roomWidth = room.transform.localScale.x;
		float roomHeight = room.transform.localScale.z;
		int count = 0;

		for(int j = 0; j < h; ++j)
		{
			for(int i = 0; i < w; ++i)
			{
				if(rooms[j*w+i] >= 0) 
				{
					cellPositions[(h-j-1)*w+i] = Vector3.right*(roomWidth*((i-w/2)+.5f))+Vector3.forward*(roomHeight*((j-h/2)+.5f));
					cellTypes[(h-j-1)*w+i] = cellType.center;
					++count;
				}
				else {
					cellPositions[(h-j-1)*w+i] = Vector3.right*(roomWidth*((i-w/2)+.5f))+Vector3.forward*(roomHeight*((j-h/2)+.5f));
					cellTypes[(h-j-1)*w+i] = cellType.empty;
				}
			}
		}
		Debug.Log (count.ToString()+" cells created!");
	}

	// picks a new room to be in the maze
	int pickNewRoom(int roomIndex, ref int weightSum)
	{
		int adjMax = 0;
		if(roomIndex%w > 0 && rooms[roomIndex-1] < 0) {++adjMax;}
		if(roomIndex%w < h && rooms[roomIndex+1] < 0) {++adjMax;}
		if(roomIndex-w >= 0 && rooms[roomIndex-w] < 0) {++adjMax;}
		if(roomIndex+w < w*h && rooms[roomIndex+w] < 0) {++adjMax;}

		int adj = Random.Range(0,adjMax);

		if(roomIndex%w > 0 && rooms[roomIndex-1] < 0)
		{
			if(adj == 0)// this is a center floor tile
			{
				SetRoom (roomIndex-1, ref weightSum);
				return roomIndex-1;
			}
		} else {adj += 1;}

		if(roomIndex%w < h-1 && rooms[roomIndex+1] < 0)
		{
			if(adj == 1)// This is a flat wall tile
			{
				SetRoom (roomIndex+1, ref weightSum);
				return roomIndex+1;
			}
		} else {adj += 1;}

		if(roomIndex-w >= 0 && rooms[roomIndex-w] < 0)
		{
			if(adj == 2)// this can be either a corner or hallway tile
			{
				SetRoom(roomIndex-w, ref weightSum);
				return roomIndex-w;
			}
		} else {adj += 1;}

		if(roomIndex+w < w*h && rooms[roomIndex+w] < 0)
		{
			if(adj == 3)// this is a hallway end tile
			{
				SetRoom(roomIndex+w, ref weightSum);
				return roomIndex+w;
			}
		}
		return -1;
	}

	// Adds the room [roomIndex] to the floorplan, adjusting
	// the value of weightSum accordingly
	void SetRoom(int roomIndex, ref int weightSum)
	{
		int adjCount = 0;
		bool topEmpty = false;
		bool bottomEmpty = false;
		bool leftEmpty = false;
		bool rightEmpty = false;

		// check space to the left
		if(roomIndex%w > 0) // if not a left border tile
		{
			if (rooms[roomIndex-1] < 0) {
				++adjCount;
				leftEmpty = true;
			}// If no there is currently no room to the left, increase adj free space count.

			else // If not, adjust the weightSum to reflect the room to the left having one less adjacent free space itself.
			{
				--rooms[roomIndex-1];
				weightSum -= roomWeights[rooms[roomIndex-1]];
				if(rooms[roomIndex-1] > 0) {weightSum += roomWeights[rooms[roomIndex-1]-1];}
			}
		}

		// check space to the right
		if(roomIndex%w < w-1) 
		{
			if (rooms[roomIndex+1] < 0) {
				++adjCount;
				rightEmpty = true;
			}
			else 
			{
				--rooms[roomIndex+1];
				weightSum -= roomWeights[rooms[roomIndex+1]];
				if(rooms[roomIndex+1] > 0) {weightSum += roomWeights[rooms[roomIndex+1]-1];}
			}
		}

		// check space above
		if(roomIndex-w >= 0) 
		{
			if (rooms[roomIndex-w] < 0) {
				++adjCount;
				topEmpty = true;
			}
			else 
			{
				--rooms[roomIndex-w];
				weightSum -= roomWeights[rooms[roomIndex-w]];
				if(rooms[roomIndex-w] > 0) {weightSum += roomWeights[rooms[roomIndex-w]-1];}
			}
		}

		// check space below
		if(roomIndex+w < w*h) 
		{
			if (rooms[roomIndex+w] < 0) {
				++adjCount;
				bottomEmpty = true;
			}
			else 
			{
				--rooms[roomIndex+w];
				weightSum -= roomWeights[rooms[roomIndex+w]];
				if(rooms[roomIndex+w] > 0) {weightSum += roomWeights[rooms[roomIndex+w]-1];}
			}
		}

		if((topEmpty && bottomEmpty) || (leftEmpty && rightEmpty)){
			// it is possible to but a hall here, choose hall or corner for this tile
			// choosing hall means preceeding in the same direction
			// choosing corner means going in a different direction when connecting the next tile to this one.
			int minCornerRange = 0;
			int maxCornerRange = cornersWeight-1; 
			int minHallRange = cornersWeight;
			int maxHallRange = cornersWeight + hallsWeight-1;
			
			int choice = Random.Range(0,maxHallRange);//use a weighted random selector to decide
			// here we need to implement a structure to store some kind of path info
			// maybe another parallel array storing the next direction to take precident from that tile
			// will need to adjust where it picks the next tile
			// need dynamic order or checking next tile, maybe
		}

		rooms[roomIndex] = adjCount;
		if(adjCount > 0) {weightSum += roomWeights[adjCount-1];}
	}

	void EvaluateTiles(){
		float roomWidth = room.transform.localScale.x;
		float roomHeight = room.transform.localScale.z;


		for(int i = 0; i < w*h; i++){
			bool top = false;
			bool left = false;
			bool right = false;
			bool bottom = false;

			// Top wall test
			if( i - w >= 0){
				//print ("in top test " + i);
				if(cellTypes[i-w] == cellType.empty && cellTypes[i] != cellType.empty){
					top = true;
				}
				else top = false;
			}
			else{// cell on top row, needs a top wall
				if(cellTypes[i] != cellType.empty)top = true;
				//print("in top test " + i + " this on in top row");
			}

			// bottom wall test
			if( i + w <= (h*w-1)){
				//print ("in bottom test " + i);
				if(cellTypes[i+w] == cellType.empty && cellTypes[i] != cellType.empty){
					bottom = true;
				}
				else bottom = false;
			}
			else{// cell on bottom row, needs a bottom wall
				if(cellTypes[i] != cellType.empty)bottom = true;
				//print("in bottom test " + i + " this on in bottom row");
			}

			// Left Wall test
			if( i % w != 0){
				//print ("in left test " + i);
				if(cellTypes[i-1] == cellType.empty && cellTypes[i] != cellType.empty){
					left = true;
				}
				else left = false;
			}
			else{// cell on bottom row, needs a bottom wall
				if(cellTypes[i] != cellType.empty)left = true;
				//print("in left test " + i + " this on in left column");
			}
			// Right Wall test
			if( i % w != (w-1)){
				//print ("in right test " + i);
				if(cellTypes[i+1] == cellType.empty && cellTypes[i] != cellType.empty){
					right = true;

				}
				else right = false;
			}
			else{// cell on bottom row, needs a bottom wall
				if(cellTypes[i] != cellType.empty)right = true;
				//print("in right test " + i + " this on in right column");
			}

			// set final tile type
			if(top && left && !right && !bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.topLeft;
			if(top && !left && !right && !bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.top;
			if(top && !left && right && !bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.topRight;
			if(!top && left && !right && !bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.left;
			if(!top && !left && !right && !bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.center;
			if(!top && !left && right && !bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.right;
			if(!top && left && !right && bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.bottomLeft;
			if(!top && !left && !right && bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.bottom;
			if(!top && !left && right && bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.bottomRight;
			if(top && !left && !right && bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.horizontalHall;
			if(!top && left && right && !bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.verticalHall;
			if(top && !left && right && bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.rightHallEnd;
			if(top && left && !right && bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.leftHallEnd;
			if(top && left && right && !bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.topHallEnd;
			if(!top && left && right && bottom && cellTypes[i] != cellType.empty) cellTypes[i] = cellType.bottomHallEnd;

		}




        GameObject cellParent = new GameObject("mapCells");


		for(int j = 0; j < w*h; j++)
		{
			//GameObject cell = GameObject.Instantiate(Center) as GameObject;
			//cell.transform.position = Vector3.right*(roomWidth*((i-w/2)+.5f))+Vector3.forward*(roomHeight*((j-h/2)+.5f));
			//cell.GetComponent<RoomBuilder>().roomID = j; 
			//cell.GetComponent<RoomBuilder>().myType = RoomBuilder.cellType.center;
			GameObject cell;
			if(cellTypes[j] == cellType.center){
				if((cellTypes[j-(int)buildArea.x] == cellType.center &&
				   cellTypes[j+(int)buildArea.x] == cellType.center) ||
			       (cellTypes[j+1] == cellType.center &&
			       cellTypes[j-1] == cellType.center)){
						cellTypes[j] = cellType.centerPillar;
				   		cell = GameObject.Instantiate(CenterPillar, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation ) as GameObject;
				}
				else{
					cell = GameObject.Instantiate(Center, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation ) as GameObject;
					// need top left pillar
					if(cellTypes[j-(int)buildArea.x] == cellType.left ||
					   cellTypes[j-(int)buildArea.x] == cellType.topLeft ||
					   cellTypes[j-(int)buildArea.x] == cellType.verticalHall||
					   cellTypes[j-1] == cellType.top ||
					   cellTypes[j-1] == cellType.topLeft ||
					   cellTypes[j-1] == cellType.horizontalHall){
						foreach (Transform child in cell.transform)
						{
							if(child.gameObject.name =="TLPillar")child.gameObject.SetActive(true);
						} 

					}
					// need top right pillar
					if(cellTypes[j-(int)buildArea.x] == cellType.right ||
					   cellTypes[j-(int)buildArea.x] == cellType.topRight ||
					   cellTypes[j-(int)buildArea.x] == cellType.verticalHall||
					   cellTypes[j+1] == cellType.top ||
					   cellTypes[j+1] == cellType.topRight ||
					   cellTypes[j+1] == cellType.horizontalHall){
						foreach (Transform child in cell.transform)
						{
							if(child.gameObject.name =="TRPillar")child.gameObject.SetActive(true);
						} 
						
					}
					// need bottom left pillar
					if(cellTypes[j+(int)buildArea.x] == cellType.left ||
					   cellTypes[j+(int)buildArea.x] == cellType.bottomLeft ||
					   cellTypes[j+(int)buildArea.x] == cellType.verticalHall||
					   cellTypes[j-1] == cellType.bottom ||
					   cellTypes[j-1] == cellType.bottomLeft ||
					   cellTypes[j-1] == cellType.horizontalHall){
						foreach (Transform child in cell.transform)
						{
							if(child.gameObject.name =="BLPillar")child.gameObject.SetActive(true);
						} 
						
					}
					// need bottom right pillar
					if(cellTypes[j+(int)buildArea.x] == cellType.right ||
					   cellTypes[j+(int)buildArea.x] == cellType.bottomRight ||
					   cellTypes[j+(int)buildArea.x] == cellType.verticalHall||
					   cellTypes[j+1] == cellType.bottom ||
					   cellTypes[j+1] == cellType.bottomRight ||
					   cellTypes[j+1] == cellType.horizontalHall){
						foreach (Transform child in cell.transform)
						{
							if(child.gameObject.name =="BRPillar")child.gameObject.SetActive(true);
						} 
						
					}
				}
			}
			else if(cellTypes[j] == cellType.topLeft){
				cell = GameObject.Instantiate(TopLeft, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
				if(cellTypes[j+(int)buildArea.x] == cellType.right ||
				   cellTypes[j+(int)buildArea.x] == cellType.bottomRight ||
				   cellTypes[j+(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j+1] == cellType.bottom ||
				   cellTypes[j+1] == cellType.bottomRight ||
				   cellTypes[j+1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="BRPillar")child.gameObject.SetActive(true);
					} 
					
				}
			}
			else if(cellTypes[j] == cellType.top){
				cell = GameObject.Instantiate(Top, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
				if(cellTypes[j+(int)buildArea.x] == cellType.left ||
				   cellTypes[j+(int)buildArea.x] == cellType.bottomLeft ||
				   cellTypes[j+(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j-1] == cellType.bottom ||
				   cellTypes[j-1] == cellType.bottomLeft ||
				   cellTypes[j-1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="BLPillar")child.gameObject.SetActive(true);
					} 
					
				}
				if(cellTypes[j+(int)buildArea.x] == cellType.right ||
				   cellTypes[j+(int)buildArea.x] == cellType.bottomRight ||
				   cellTypes[j+(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j+1] == cellType.bottom ||
				   cellTypes[j+1] == cellType.bottomRight ||
				   cellTypes[j+1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="BRPillar")child.gameObject.SetActive(true);
					} 
					
				}
			}
			else if(cellTypes[j] == cellType.topRight){
				cell = GameObject.Instantiate(TopRight, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
				if(cellTypes[j+(int)buildArea.x] == cellType.left ||
				   cellTypes[j+(int)buildArea.x] == cellType.bottomLeft ||
				   cellTypes[j+(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j-1] == cellType.bottom ||
				   cellTypes[j-1] == cellType.bottomLeft ||
				   cellTypes[j-1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="BLPillar")child.gameObject.SetActive(true);
					} 
					
				}
			}
			else if(cellTypes[j] == cellType.left){
				cell = GameObject.Instantiate(Left, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
				if(cellTypes[j-(int)buildArea.x] == cellType.right ||
				   cellTypes[j-(int)buildArea.x] == cellType.topRight ||
				   cellTypes[j-(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j+1] == cellType.top ||
				   cellTypes[j+1] == cellType.topRight ||
				   cellTypes[j+1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="TRPillar")child.gameObject.SetActive(true);
					} 
					
				}
				if(cellTypes[j+(int)buildArea.x] == cellType.right ||
				   cellTypes[j+(int)buildArea.x] == cellType.bottomRight ||
				   cellTypes[j+(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j+1] == cellType.bottom ||
				   cellTypes[j+1] == cellType.bottomRight ||
				   cellTypes[j+1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="BRPillar")child.gameObject.SetActive(true);
					} 
					
				}
			}
			else if(cellTypes[j] == cellType.right){
				cell = GameObject.Instantiate(Right, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
				if(cellTypes[j-(int)buildArea.x] == cellType.left ||
				   cellTypes[j-(int)buildArea.x] == cellType.topLeft ||
				   cellTypes[j-(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j-1] == cellType.top ||
				   cellTypes[j-1] == cellType.topLeft ||
				   cellTypes[j-1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="TLPillar")child.gameObject.SetActive(true);
					} 
					
				}
				if(cellTypes[j+(int)buildArea.x] == cellType.left ||
				   cellTypes[j+(int)buildArea.x] == cellType.bottomLeft ||
				   cellTypes[j+(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j-1] == cellType.bottom ||
				   cellTypes[j-1] == cellType.bottomLeft ||
				   cellTypes[j-1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="BLPillar")child.gameObject.SetActive(true);
					} 
					
				}
			}
			else if(cellTypes[j] == cellType.bottomLeft){
				cell = GameObject.Instantiate(BottomLeft, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
				if(cellTypes[j-(int)buildArea.x] == cellType.right ||
				   cellTypes[j-(int)buildArea.x] == cellType.topRight ||
				   cellTypes[j-(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j+1] == cellType.top ||
				   cellTypes[j+1] == cellType.topRight ||
				   cellTypes[j+1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="TRPillar")child.gameObject.SetActive(true);
					} 
					
				}
			}
			else if(cellTypes[j] == cellType.bottom){
				cell = GameObject.Instantiate(Bottom, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
				if(cellTypes[j-(int)buildArea.x] == cellType.left ||
				   cellTypes[j-(int)buildArea.x] == cellType.topLeft ||
				   cellTypes[j-(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j-1] == cellType.top ||
				   cellTypes[j-1] == cellType.topLeft ||
				   cellTypes[j-1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="TLPillar")child.gameObject.SetActive(true);
					} 
					
				}
				if(cellTypes[j-(int)buildArea.x] == cellType.right ||
				   cellTypes[j-(int)buildArea.x] == cellType.topRight ||
				   cellTypes[j-(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j+1] == cellType.top ||
				   cellTypes[j+1] == cellType.topRight ||
				   cellTypes[j+1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="TRPillar")child.gameObject.SetActive(true);
					} 
					
				}
			}
			else if(cellTypes[j] == cellType.bottomRight){
				cell = GameObject.Instantiate(BottomRight, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
				if(cellTypes[j-(int)buildArea.x] == cellType.left ||
				   cellTypes[j-(int)buildArea.x] == cellType.topLeft ||
				   cellTypes[j-(int)buildArea.x] == cellType.verticalHall||
				   cellTypes[j-1] == cellType.top ||
				   cellTypes[j-1] == cellType.topLeft ||
				   cellTypes[j-1] == cellType.horizontalHall){
					foreach (Transform child in cell.transform)
					{
						if(child.gameObject.name =="TLPillar")child.gameObject.SetActive(true);
					} 
					
				}
			}
			else if(cellTypes[j] == cellType.horizontalHall){
				cell = GameObject.Instantiate(HorizontalHall, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
			}
			else if(cellTypes[j] == cellType.verticalHall){
				cell = GameObject.Instantiate(VerticalHall, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
			}
			else if(cellTypes[j] == cellType.rightHallEnd){
				cell = GameObject.Instantiate(RightHallEnd, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
			}
			else if(cellTypes[j] == cellType.leftHallEnd){
				cell = GameObject.Instantiate(LeftHallEnd, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
			}
			else if(cellTypes[j] == cellType.topHallEnd){
				cell = GameObject.Instantiate(TopHallEnd, Vector3.right*(roomWidth*(j%w-(w/2)+.5f))+Vector3.forward*(roomHeight*((((h*w)-j-1))/w)-(5*w)+5)+ Vector3.down *.25f,transform.rotation) as GameObject;
			}
			else if (cellTypes[j] == cellType.bottomHallEnd){
				cell = GameObject.Instantiate(BottomHallEnd, Vector3.right * (roomWidth * (j % w - (w / 2) + .5f)) + Vector3.forward * (roomHeight * ((((h * w) - j - 1)) / w) - (5 * w) + 5) + Vector3.down * .25f, transform.rotation) as GameObject;
			}
			else
            {
                //cell = null;
				cell = GameObject.Instantiate(Empty, Vector3.right * (roomWidth * (j % w - (w / 2) + .5f)) + Vector3.forward * (roomHeight * ((((h * w) - j - 1)) / w) - (5 * w) + 5) + Vector3.down * .25f, transform.rotation) as GameObject;
            }
			//cell.transform.position = Vector3.right*(roomWidth*(((j%w)/2)+.5f))+Vector3.forward*(roomHeight*(((j/w)/2)+.5f));
			//cell.GetComponent<RoomBuilder>().roomID = j; 
			//cell.GetComponent<RoomBuilder>().myType = cellTypes[j];
			//else cell = GameObject.Instantiate(Empty, Vector3.right*(roomWidth*(j%w+.5f))+Vector3.forward*(roomHeight*(j/w +.5f)),Quaternion.identity) as GameObject;

            if (cell != null)
            {
                cell.transform.parent = cellParent.transform;
            }
			//cell.GetComponent<RoomBuilder>().myType = cellTypes[j];
        }

	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGeneratorJimi : MonoBehaviour {
	private int doorPos;
	private Vector3 dirX = new Vector3 (1, 0, 0);
	private Vector3 dirZ = new Vector3 (0, 0, 1);
	private Vector3 origin;
	public int minLength = 10;
	public int maxLength = 20;
	public int wallHeight = 5;
	public int numberOfRooms = 3;
	public int trapAmount = 30;

	public int dustLeft = 0;

	public List<Material> wallMaterial = new List<Material> ();

	public GameObject item;
	public Material floorMat;
	public GameObject trapTrigger;

	float endTimer = 0;
	float endTime = 3f;

	void Start(){
//		Tile tileTemp = new Tile ("tile_floor", 100, 1, 100, Vector3.zero, floorMat);
//		tileTemp.instantiateTile (tileTemp);
		GenerateUnicornDungeon ();
	}

	void Update(){
		GameObject.Find ("UnicornDustGatheredText").GetComponent<Text> ().text = "Dust left: " + dustLeft;
		if (dustLeft == 0) {
			if (endTimer < endTime) {
				endTimer += Time.deltaTime;
			} else {
				GameController.Instance.ChangeGameState (GameController.GAMESTATE_MAP);
			}
		}
	}


	public void GenerateWall(bool hasDoor, Vector3 origin, int length, int height, Vector3 direction) {
		
		if (hasDoor == true) {
			doorPos = Random.Range (1, (length - 1));
		} else {
			doorPos = 100;
		}
		for (int i = 0; i < length; i++) {
			if(i != doorPos && i != doorPos + 1 && i != doorPos + 2) {
			Tile tileTemp = new Tile ("wall_temp", 1, 1, 1, new Vector3(direction.x * i + origin.x, 0, direction.z * i + origin.z), wallMaterial [Random.Range (0, 2)]);
			tileTemp.instantiateTile (tileTemp);
				for (int j = 1; j < height; j++) {
					if (i != doorPos && i != doorPos + 1 && i != doorPos + 2) {
						Tile tileTemp3 = new Tile ("wall_temp", 1, 1, 1, new Vector3(direction.x * i + origin.x, j, direction.z * i + origin.z), wallMaterial [Random.Range (0, 2)]);
						tileTemp3.instantiateTile (tileTemp3);
					}
				}
			}
		}
	}

	public void GenerateRoom(bool doors, Vector3 origin, int wallLengthX, int wallLengthZ, int height) {
		GenerateWall (doors, origin, wallLengthX, height, dirX);
		GenerateWall (doors, origin, wallLengthZ, height, dirZ);
		GenerateWall (doors, new Vector3(origin.x + wallLengthX, origin.y, origin.z), wallLengthX, height, dirZ);
		GenerateWall (doors, new Vector3(origin.x, origin.y, origin.z + wallLengthZ), wallLengthX, height, dirX);
	}

	public void GenerateDungeon() {
		int dungWidth = numberOfRooms;
		int dungLenght = numberOfRooms;

		for (int w = 0; w < dungWidth; w++) {
			for (int l = 0; l < dungLenght; l++) {
				GenerateRoom (true, new Vector3 (w * 15, 0, l * 15), Random.Range (minLength, maxLength), Random.Range (minLength, maxLength), wallHeight);
			}
		}
	}

	void GenerateTrappedFloor(int trapAmount, int levelWidth, int levelLenght) {

		int additionalEdge = 40;
		Vector3 pos = new Vector3(-levelWidth - 1 - additionalEdge, 0, -levelLenght - 1 - additionalEdge);
		int trapCount = trapAmount;
		for (int w = 0; w < levelWidth + additionalEdge; w++) {
			for (int l = 0; l < levelLenght + additionalEdge; l++) {
				float chance = Random.Range (0f, 1f); // 1 because levelWidth and levelLenght are only half so 4x is whole map and divided by 4 because tiles are 2x2, doh
				if (chance < (trapAmount / ((levelWidth + additionalEdge) * (levelLenght + additionalEdge) * 1f)) && trapCount > 0) {
					GameObject tempTrap = Instantiate (trapTrigger, new Vector3 (pos.x, -2, pos.z), Quaternion.identity);
					tempTrap.transform.localScale = new Vector3 (2, 1, 2);
					trapCount--;
				} else {
					Tile tileTemp = new Tile ("tile_floor", 2, 1, 2, pos, floorMat);
					tileTemp.instantiateTile (tileTemp);
				}
				pos += new Vector3 (0, 0, 2);
			}
			pos += new Vector3 (2, 0, 0);
			pos = new Vector3 (pos.x, 0, -levelLenght - 1 - additionalEdge);
		}


	}

	/// <summary>
	/// Generates the unicorn dungeon.
	/// </summary>
	public void GenerateUnicornDungeon(){
		// Number of rooms
		int dungWidth = numberOfRooms;
		int dungLenght = numberOfRooms;
		// For calculations
		int roomSize = minLength + maxLength / 2;
		int levelWidth = dungWidth * roomSize / 2;
		int levelLenght = dungLenght * roomSize / 2;

		// Add trapped floor
		GenerateTrappedFloor(trapAmount, levelWidth, levelLenght);

		// Add the rooms
		for (int w = 0; w < dungWidth; w++) {
			for (int l = 0; l < dungLenght; l++) {
				GenerateRoom (true, new Vector3 (w * roomSize + w * 1 - levelWidth, 0, l * roomSize - levelLenght), Random.Range (minLength, maxLength), Random.Range (minLength, maxLength), wallHeight);
			}
		}

		// Create border walls
		GenerateRoom (false, new Vector3 (-(levelWidth + 2), 0, -(levelLenght + 2)), dungWidth * roomSize + 4, dungLenght * roomSize + 4, wallHeight);

		// Add towers surrounding the level outside the border wall
		int towers = 20;
		for (int i = 0; i < towers; i++) {
			while (true) {
				int axis = Random.Range (0, 2);
				Vector3 pos = new Vector3 (Random.Range (-levelWidth - 20, levelWidth + 20), 0, Random.Range (-levelLenght - 20, levelLenght + 20));
				if ((axis == 0 && !(pos.x < levelWidth + 10 && pos.x > -levelWidth - 10)) || (axis == 1 && !(pos.z < levelLenght + 10 && pos.z > -levelLenght - 10))) {
					GenerateRoom (false, pos, Random.Range (5, 10), Random.Range (5, 10), wallHeight * 3);
					break;
				}
			}
		}

		// Add Unicorn Dust items
		AddItems ("Unicorn Dust", levelWidth, levelLenght);
	}

	/// <summary>
	/// Adds items to the level. 
	/// </summary>
	/// <param name="itemName">Item name.</param>
	/// <param name="assetRangeX">Range to spawn in from middle of level on the x axis.</param>
	/// <param name="assetRangeZ">Range to spawn in from middle of level on the z axis.</param>
	void AddItems(string itemName, float assetRangeX, float assetRangeZ){
		int itemCount = 8;
		for (int i = 0; i < itemCount; i++) {
			while (true) {
				Vector3 position = new Vector3 (Random.Range (-assetRangeX, assetRangeX), 1, Random.Range (-assetRangeZ, assetRangeZ));
				if (!Physics.Raycast (position, Vector3.up, 1f)) {
					GameObject temp = Instantiate (item, position, Quaternion.identity);
					temp.GetComponent<Item> ().GenerateItem (itemName);
					dustLeft++;
					break;
				}
			}
		}
	}
}

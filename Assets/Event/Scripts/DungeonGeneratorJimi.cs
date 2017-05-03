using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorJimi : MonoBehaviour {
	private int doorPos;
	private Vector3 dirX = new Vector3 (1, 0, 0);
	private Vector3 dirZ = new Vector3 (0, 0, 1);
	private Vector3 origin;
	public int minLength = 10;
	public int maxLength = 20;
	public int wallHeight = 5;
	public int numberOfRooms = 1;

	public List<Material> wallMaterial = new List<Material> ();

	public GameObject item;
	public Material floorMat;

	void Start(){
		GenerateUnicornDungeon ();

		Tile tileTemp = new Tile ("tile_floor", 100, 1, 100, Vector3.zero, floorMat);
		tileTemp.instantiateTile (tileTemp);
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
		int dungWidth = 3;
		int dungLenght = 3;

		for (int w = 0; w < dungWidth; w++) {
			for (int l = 0; l < dungLenght; l++) {
				GenerateRoom (true, new Vector3 (w * 15, 0, l * 15), Random.Range (minLength, maxLength), Random.Range (minLength, maxLength), wallHeight);
			}
		}
	}

	public void GenerateUnicornDungeon(){
		int dungWidth = 3;
		int dungLenght = 3;
		int roomSize = 15;
		int levelWidth = dungWidth * roomSize / 2;
		int levelLenght = dungLenght * roomSize / 2;

		for (int w = 0; w < dungWidth; w++) {
			for (int l = 0; l < dungLenght; l++) {
				GenerateRoom (true, new Vector3 (w * roomSize - levelWidth, 0, l * roomSize - levelLenght), Random.Range (minLength, maxLength), Random.Range (minLength, maxLength), wallHeight);
			}
		}

		int towers = 20;
		for (int i = 0; i < towers; i++) {
			while (true) {
				Vector3 pos = new Vector3 (Random.Range (-levelWidth - 20, levelWidth + 20), 0, Random.Range (-levelLenght - 20, levelLenght + 20));
				if (!((pos.x < levelWidth + 10 && pos.x > -levelWidth - 10) || (pos.z < levelLenght + 10 && pos.z > -levelLenght - 10))) {
					GenerateRoom (false, pos, Random.Range (minLength / 2, maxLength / 2), Random.Range (minLength / 2, maxLength / 2), wallHeight * 3);
					break;
				}
			}
		}

		GenerateRoom (false, new Vector3 (-(levelWidth + 5), 0, -(levelLenght + 5)), dungWidth * roomSize + 10, dungLenght * roomSize + 10, wallHeight);
		AddItems ("Unicorn Dust", levelWidth, levelLenght);
	}

	void AddItems(string itemName, float assetRangeX, float assetRangeZ){
		int itemCount = 8;
		for (int i = 0; i < itemCount; i++) {
			while (true) {
				Vector3 position = new Vector3 (Random.Range (-assetRangeX, assetRangeX), 1, Random.Range (-assetRangeZ, assetRangeZ));
				if (!Physics.Raycast (position, Vector3.up, 1f)) {
					GameObject temp = Instantiate (item, position, Quaternion.identity);
					temp.GetComponent<Item> ().GenerateItem (itemName);
					break;
				}
			}
		}
	}
}

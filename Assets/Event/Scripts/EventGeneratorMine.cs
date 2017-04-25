using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGeneratorMine : MonoBehaviour {

	public Dictionary<string, bool> directions = new Dictionary<string, bool>();
	List<string> keyList = new List<string> ();


	public List<Material> wallMaterial = new List<Material> ();

	private int numberOfDoors;
	private int doorPos;

	public int originX;
	public int originZ;

	public int roomMinZ = 15;
	public int roomMaxZ = 20;

	public int roomMinX = 15;
	public int roomMaxX = 20;

	public int roomY = 5;

	void Start() {
		directions.Add ("North", false);
		directions.Add ("South", false);
		directions.Add ("East", false);
		directions.Add ("West", false);
		keyList = new List<string>(directions.Keys);
	}

	/*
	public void GenerateWall(bool hasDoor, Vector3 origin, int length, int height, Vector3 direction) {
		int doorPos;
		if (hasDoor == true) {
			doorPos = Random.Range (1, (length - 1));
		}
		for (int i = 0; i < length; i++) {
			Tile tileTemp = new Tile("walltemp", 1,1,1, new Vector3(origin.x * direction.x + i, 0, origin.z * direction.z + i), wallMaterial [Random.Range (0, 2)]);
			tileTemp.instantiateTile (tileTemp);
		}
	}
	*/



	public void GenerateRoom() {


		//DOORS
		//Randomize number of doors
		numberOfDoors = Random.Range(1,4);

		//Randomize which directions has doors
		for (int i = 0; i < numberOfDoors; i++) {
			string randomKey = keyList[Random.Range(0, keyList.Count)];
				directions [randomKey] = true;
		}
			



		//WALLS
		//Randomize room size and origin
		int roomZ = Random.Range(roomMinZ, roomMaxZ);
		Debug.Log (roomZ);
		int roomX = Random.Range(roomMinX, roomMaxX);
		Debug.Log (roomX);
		originX = Random.Range (-10, 10);
		originZ = Random.Range (-10, 10);




		//GENERATE X WALLS EAST
		for (int i = 0; i < roomX; i++) {
				
				Tile tileTemp = new Tile ("wall_temp", 1, 1, 1, new Vector3 (originX + i, 0, originZ), wallMaterial [Random.Range (0, 2)]);
				tileTemp.instantiateTile (tileTemp);
				//GENERATE OPPOSITE WALL WEST
				Tile tileTemp2 = new Tile ("wall_temp", 1, 1, 1, new Vector3 (originX + i, 0, originZ + roomZ), wallMaterial [Random.Range (0, 2)]);
				tileTemp2.instantiateTile (tileTemp2);
				for (int j = 1; j < roomY; j++) {
					//STACK CUBES

					Tile tileTemp3 = new Tile ("wall_temp", 1, 1, 1, new Vector3 (originX + i, j, originZ), wallMaterial [Random.Range (0, 2)]);
					tileTemp3.instantiateTile (tileTemp3);
			
					//STACK CUBES ON OPPOSITE WALL
					Tile tileTemp4 = new Tile ("wall_temp", 1, 1, 1, new Vector3 (originX + i, j, originZ + roomZ), wallMaterial [Random.Range (0, 2)]);
					tileTemp4.instantiateTile (tileTemp4);
				

				}
		}

		//GENERATE Z WALLS SOUTH
		for (int i = 0; i < roomZ; i++) {
			Tile tileTemp = new Tile ("wall_temp", 1, 1, 1, new Vector3 (originX, 0, originZ + i), wallMaterial [Random.Range (0, 2)]);
			tileTemp.instantiateTile (tileTemp);
			//GENERATE OPPOSITE WALL NORTH
			Tile tileTemp2 = new Tile ("wall_temp", 1, 1, 1, new Vector3 (roomX + originX, 0, originZ + i), wallMaterial [Random.Range (0, 2)]);
			tileTemp.instantiateTile (tileTemp2);
			for (int j = 1; j < roomY; j++) {
				//STACK CUBES
				Tile tileTemp3 = new Tile ("wall_temp", 1, 1, 1, new Vector3 (originX, j, originZ + i), wallMaterial [Random.Range (0, 2)]);
				tileTemp.instantiateTile (tileTemp3);
				//STACK CUBES ON OPPOSITE WALL
				Tile tileTemp4 = new Tile ("wall_temp", 1, 1, 1, new Vector3 (roomX + originX, j, originZ + i), wallMaterial [Random.Range (0, 2)]);
				tileTemp.instantiateTile (tileTemp4);

			}
		}

	


	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {
	private int doorPos;
	private Vector3 dirX = new Vector3 (1, 0, 0);
	private Vector3 dirZ = new Vector3 (0, 0, 1);
	private Vector3 origin;
	public int minLength = 10;
	public int maxLength = 20;
	public int height = 5;

	public List<Material> wallMaterial = new List<Material> ();


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

	public void GenerateRoom(int numberOfDoors) {
		origin = new Vector3(Random.Range(0, 10), 0, Random.Range(0,10));
		int wallLengthX = Random.Range (minLength, maxLength);
		int wallLengthZ = Random.Range (minLength, maxLength);

		GenerateWall (true, origin, wallLengthX, height, dirX);
		GenerateWall (true, origin, wallLengthZ, height, dirZ);
		GenerateWall (true, new Vector3(origin.x + wallLengthX, origin.y, origin.z), wallLengthX, height, dirZ);
		GenerateWall (true, new Vector3(origin.x, origin.y, origin.z + wallLengthZ), wallLengthX, height, dirX);
	}

	public void GenerateDungeon() {
		GenerateRoom (3);
	}

}

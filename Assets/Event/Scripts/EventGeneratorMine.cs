using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGeneratorMine : MonoBehaviour {

	public List<Material> wallMaterial = new List<Material> ();

	public int originX;
	public int originZ;

	public int roomMinZ = 15;
	public int roomMaxZ = 20;

	public int roomMinX = 15;
	public int roomMaxX = 20;

	public int roomY = 5;





	public void GenerateRoom() {

		int roomZ = Random.Range(roomMinZ, roomMaxZ);
		Debug.Log (roomZ);
		int roomX = Random.Range(roomMinX, roomMaxX);
		Debug.Log (roomX);
		originX = Random.Range (-10, 10);
		originZ = Random.Range (-10, 10);



		//GENERATE X WALL
		for (int i = 0; i < roomX; i++) {
			Tile tileTemp = new Tile ("wall_" + i, 1, 1, 1, new Vector3 (originX + i, 0, originZ), wallMaterial [Random.Range (0, 2)]);
			tileTemp.instantiateTile (tileTemp);
			for (int j = 1; j < roomY; j++) {
				Tile tileTemp2 = new Tile ("wall_" + i, 1, 1, 1, new Vector3 (originX + i, j, originZ), wallMaterial [Random.Range (0, 2)]);
				tileTemp.instantiateTile (tileTemp2);

			}
		}
		//GENERATE OPPOSITE X WALL
		for (int i = 0; i < roomX; i++) {
			Tile tileTemp = new Tile ("wall_" + i, 1, 1, 1, new Vector3 (originX + i, 0, originZ + roomZ), wallMaterial [Random.Range (0, 2)]);
			tileTemp.instantiateTile (tileTemp);
			for (int j = 1; j < roomY; j++) {
				Tile tileTemp2 = new Tile ("wall_" + i, 1, 1, 1, new Vector3 (originX + i, j, originZ + roomZ), wallMaterial [Random.Range (0, 2)]);
				tileTemp.instantiateTile (tileTemp2);

			}
		}
		//GENERATE Z WALL
		for (int i = 0; i < roomZ; i++) {
			Tile tileTemp = new Tile ("wall_" + i, 1, 1, 1, new Vector3 (originX, 0, originZ + i), wallMaterial [Random.Range (0, 2)]);
			tileTemp.instantiateTile (tileTemp);
			for (int j = 1; j < roomY; j++) {
				Tile tileTemp2 = new Tile ("wall_" + i, 1, 1, 1, new Vector3 (originX, j, originZ + i), wallMaterial [Random.Range (0, 2)]);
				tileTemp.instantiateTile (tileTemp2);

			}
		}
		//GENERATE OPPOSITE Z WALL
		for (int i = 0; i < roomZ; i++) {
			Tile tileTemp = new Tile ("wall_" + i, 1, 1, 1, new Vector3 (roomX + originX, 0, originZ + i), wallMaterial [Random.Range (0, 2)]);
			tileTemp.instantiateTile (tileTemp);
			for (int j = 1; j < roomY; j++) {
				Tile tileTemp2 = new Tile ("wall_" + i, 1, 1, 1, new Vector3 (roomX + originX, j, originZ + i), wallMaterial [Random.Range (0, 2)]);
				tileTemp.instantiateTile (tileTemp2);

			}
		}



	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorBiomes : MonoBehaviour {

	GameObject tileForest1;
	GameObject tileForest2;
	GameObject tileWater1;
	GameObject tileMountain1;
	GameObject tileDesert1;
	GameObject tileDesert2;

	int tileMask;

	public int levelSize = 100;
	public int waterWidth = 20;
	public GameObject[,] tilemap;
	Dictionary<string, GameObject> tileTypeDict;
	Transform tileHolder;
	List<GameObject> terrainSprites;

	List<string> biomeList = new List<string> ();

	void Awake(){
		tileHolder = GameObject.Find("TileHolder").transform;

		tileMask = LayerMask.GetMask ("TileMask");

		biomeList.Add ("Forest");
		biomeList.Add ("Mountains");
		biomeList.Add ("Desert");
		biomeList.Add ("Beach");
	}

	public void GenerateBiomesMap(){
		foreach (GameObject tile in tilemap) {
			Destroy (tile);
		}
		foreach (GameObject sprite in terrainSprites) {
			Destroy (sprite);
		}
		terrainSprites.Clear ();
		GetComponent<Clouds> ().DestroyClouds ();

		List<GameObject> openTiles = new List<GameObject> ();

		Vector3 location = new Vector3 (-levelSize / 2 + 0.5f, 0, -levelSize / 2 + 0.5f);
		// Place initial tiles
		for (int w = 0; w < levelSize; w++) {
			for (int h = 0; h < levelSize; h++) {
				int boundary = levelSize / 2 - waterWidth;
				if ((location.x > boundary || location.x < -boundary) || (location.z > boundary || location.z < -boundary)) {
					tilemap [w, h] = Instantiate (tileTypeDict ["tile_water1"], location, Quaternion.Euler (90, 0, 0), tileHolder);
				} else {
					tilemap [w, h] = Instantiate (tileTypeDict ["tile_land1"], location, Quaternion.Euler (90, 0, 0), tileHolder);
					openTiles.Add (tilemap [w, h]);
				}

				location += new Vector3 (1, 0, 0);
			}
			location = new Vector3 (-levelSize / 2 + 0.5f, 0, location.z);
			location += new Vector3 (0, 0, 1);
		}

		//		int mountainBiomes = levelSize / 20;
		//		int forestBiomes = levelSize / 10;
		//		float mountainChance = 0.1f;
		//		float forestChance = 0.2f;
		//		for (int w = 0; w < levelSize; w++) {
		//			for (int h = 0; h < levelSize; h++) {
		//				foreach (GameObject openTile in openTiles) {
		//					if (tilemap [w, h].gameObject.Equals (openTile)) {
		//						float chance = Random.Range (0, 100);
		//						if (chance < mountainChance && mountainBiomes > 0) {
		//							mountainBiomes--;
		//							Vector3 temp = tilemap [w, h].transform.position;
		//							Destroy (tilemap [w, h]);
		//							tilemap [w, h] = Instantiate (tileTypeDict ["tile_mountain1"], temp, Quaternion.Euler (90, 0, 0), tileHolder);
		//						} else if (chance >= mountainChance && chance < (mountainChance + forestChance) && forestBiomes > 0) {
		//							forestBiomes--;
		//							Vector3 temp = tilemap [w, h].transform.position;
		//							Destroy (tilemap [w, h]);
		//							tilemap [w, h] = Instantiate (tileTypeDict ["tile_land2"], temp, Quaternion.Euler (90, 0, 0), tileHolder);
		//						}
		//					}
		//				}
		//			}
		//		}
	}

	void GenerateMountainsBiome(int size){

	}

	void GenerateForestBiome(int size){

	}
}

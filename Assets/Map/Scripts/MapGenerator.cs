using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour {

	GameObject tileForest1;
	GameObject tileForest2;
	GameObject tileWater1;
	GameObject tileMountain1;
	GameObject spriteMountain1;
	GameObject spriteForest1;
	public int levelSize = 100;
	public GameObject[,] tilemap;
	List<GameObject> tileTypeList;
	Transform tileHolder;
	List<GameObject> terrainSprites;

	// Instance for singleton class, meaning there is only one of this object ever
	public static MapGenerator Instance = null;
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);
		} else if (Instance != this) {
			Destroy (gameObject);
		}
		// References
		tileHolder = GameObject.FindGameObjectWithTag ("TileHolder").transform;

		spriteMountain1 = Resources.Load ("Prefabs/sprite_mountain1") as GameObject;
		spriteForest1 = Resources.Load ("Prefabs/sprite_forest1") as GameObject;

		tileForest1 = Resources.Load ("Prefabs/Tile_forest1") as GameObject;
		tileForest2 = Resources.Load ("Prefabs/Tile_forest2") as GameObject;
		tileWater1 = Resources.Load ("Prefabs/Tile_water1") as GameObject;
		tileMountain1 = Resources.Load ("Prefabs/Tile_mountain1") as GameObject;

		terrainSprites = new List<GameObject> ();

		tilemap = new GameObject[levelSize, levelSize];
		tileTypeList = new List<GameObject> ();
		tileTypeList.Add (tileForest1);
		tileTypeList.Add (tileForest2);
		tileTypeList.Add (tileWater1);
		tileTypeList.Add (tileMountain1);
	}

	// Generates new map, destroys all old tiles and slaps down new ones, calls Smoothing() and PlaceSprites()
	public void GenerateMap () {
		foreach (GameObject tile in tilemap) {
			Destroy (tile);
		}
		foreach (GameObject sprite in terrainSprites) {
			Destroy (sprite);
		}
		terrainSprites.Clear ();
		GetComponent<Clouds> ().DestroyClouds ();

		int startAreaSize = 3;
		Vector3 location = new Vector3 (-levelSize / 2 + 0.5f, 0, -levelSize / 2 + 0.5f);

		for (int w = 0; w < levelSize; w++) {
			for (int h = 0; h < levelSize; h++) {
				int boundary = levelSize / 2 - 20;
				if (location.x < startAreaSize && location.x > -startAreaSize && location.z < startAreaSize && location.z > -startAreaSize) {
					tilemap [w, h] = Instantiate (tileTypeList [Random.Range (0, 2)], location, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if ((location.x > boundary || location.x < -boundary) || (location.z > boundary || location.z < -boundary)) {
					tilemap [w, h] = Instantiate (tileTypeList [2], location, Quaternion.Euler (90, 0, 0), tileHolder);
				} else {
					tilemap [w, h] = Instantiate (tileTypeList [Random.Range (0, 4)], location, Quaternion.Euler (90, 0, 0), tileHolder);

				}

				location += new Vector3 (1, 0, 0);
			}
			location = new Vector3 (-levelSize / 2 + 0.5f, 0, location.z);
			location += new Vector3 (0, 0, 1);

		}
		Smoothing ();
		PlaceSprites ();
		GetComponent<Clouds> ().PlaceClouds ();
	}

	// Looks at each tile and it's neighbours and if lots of one tile type surrounds the tile, changes to that tile type
	// Smooths the map's look, more uniform areas
	void Smoothing () {
		for (int w = 1; w < tilemap.GetLength (0) - 1; w++) {
			for (int h = 1; h < tilemap.GetLength (1) - 1; h++) {
				// Make a list of neighbouring tiles
				List<GameObject> neighbours = new List<GameObject> ();

				neighbours.Add (tilemap [w - 1, h - 1]);
				neighbours.Add (tilemap [w + 1, h + 1]);
				neighbours.Add (tilemap [w - 1, h + 1]);
				neighbours.Add (tilemap [w, h - 1]);
				neighbours.Add (tilemap [w + 1, h]);
				neighbours.Add (tilemap [w - 1, h]);
				neighbours.Add (tilemap [w, h + 1]);
				neighbours.Add (tilemap [w + 1, h - 1]);
				neighbours.Add (tilemap [w, h]);

				int waterTiles = 0;
				int mountainTiles = 0;
				int land1Tiles = 0;
				int land2Tiles = 0;

				// Count the amount of each tile type in neighbours
				foreach (GameObject tile in neighbours) {
					if (tile.tag == "tile_land1") {
						land1Tiles++;
					} else if (tile.tag == "tile_land2") {
						land2Tiles++;
					} else if (tile.tag == "tile_water") {
						waterTiles++;
					} else if (tile.tag == "tile_mountain") {
						mountainTiles++;
					}
				}

				// Depending on tile type counts change the current tile
				Vector3 temp = tilemap [w, h].transform.position;

				if (mountainTiles > 4) {
					Destroy (tilemap [w, h]);
					tilemap [w, h] = Instantiate (tileTypeList [3], temp, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if (waterTiles > 4) {
					Destroy (tilemap [w, h]);
					tilemap [w, h] = Instantiate (tileTypeList [2], temp, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if (land1Tiles > 4) {
					Destroy (tilemap [w, h]);
					tilemap [w, h] = Instantiate (tileTypeList [0], temp, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if (land2Tiles > 4) {
					Destroy (tilemap [w, h]);
					tilemap [w, h] = Instantiate (tileTypeList [1], temp, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if (tilemap [w, h].tag == "tile_water" && waterTiles < 4) {
					Destroy (tilemap [w, h]);
					if (land1Tiles > land2Tiles && land1Tiles > mountainTiles) {
						tilemap [w, h] = Instantiate (tileTypeList [0], temp, Quaternion.Euler (90, 0, 0), tileHolder);
					} else if (land2Tiles > land1Tiles && land2Tiles > mountainTiles) {
						tilemap [w, h] = Instantiate (tileTypeList [1], temp, Quaternion.Euler (90, 0, 0), tileHolder);
					} else {
						tilemap [w, h] = Instantiate (tileTypeList [3], temp, Quaternion.Euler (90, 0, 0), tileHolder);
					}
				}
			}
		}
	}

	// Places upright mountain sprites 
	void PlaceSprites(){
		for (int w = 1; w < tilemap.GetLength (0) - 1; w++) {
			for (int h = 1; h < tilemap.GetLength (1) - 1; h++) {
				if (tilemap [w, h].tag == "tile_mountain") {
					Vector3 location = new Vector3 (tilemap [w, h].transform.position.x, 0, tilemap [w, h].transform.position.z + Random.Range (-0.4f, 0.4f));
					GameObject tempMountain = Instantiate (spriteMountain1, location, Quaternion.identity, tileHolder);
					tempMountain.transform.localScale = new Vector3 (Random.Range (1f, 3f), Random.Range (2f, 3f), 1f);
					terrainSprites.Add (tempMountain);
				} else if (tilemap [w, h].tag == "tile_land1" || tilemap [w, h].tag == "tile_land2") {
					Vector3 location = new Vector3 (tilemap [w, h].transform.position.x, 0, tilemap [w, h].transform.position.z + Random.Range (-0.4f, 0.4f));
					GameObject tempForest = Instantiate (spriteForest1, location, Quaternion.identity, tileHolder);
					tempForest.transform.localScale = new Vector3 (Random.Range(1f, 1.5f), Random.Range(2f, 4f), 1f);
					terrainSprites.Add (tempForest);
				}
			}
		}
	}

//	// Old
//	void GenerateMapBasic () {
//		Vector3 location = new Vector3 (-levelSize / 2 + 0.5f, 0, -levelSize / 2 + 0.5f);
//		for (int w = 0; w < levelSize; w++) {
//			for (int h = 0; h < levelSize; h++) {
//				tilemap [w, h] = Instantiate (tileTypeList [Random.Range (0, 4)], location, Quaternion.Euler (90, 0, 0));
//
//				//				int randomTile = Random.Range (1, 5);
//				//				switch (randomTile){
//				//					case 1:
//				//						tilemap [w, h] = Instantiate (tileSand1, location, Quaternion.Euler (90, 0, 0));
//				//						break;
//				//					case 2:
//				//						tilemap [w, h] = Instantiate (tileSand2, location, Quaternion.Euler (90, 0, 0));
//				//						break;
//				//					case 3:
//				//						tilemap [w, h] = Instantiate (tileWater1, location, Quaternion.Euler (90, 0, 0));
//				//						break;
//				//					case 4:
//				//						tilemap [w, h] = Instantiate (tileMountain1, location, Quaternion.Euler (90, 0, 0));
//				//						break;
//				//					default:
//				//						Debug.Log ("Random tile = " + randomTile);
//				//						break;
//				//				}
//				location += new Vector3 (1, 0, 0);
//			}
//			location = new Vector3 (-levelSize / 2 + 0.5f, 0, location.z);
//			location += new Vector3 (0, 0, 1);
//
//		}
//
//
//	}
}

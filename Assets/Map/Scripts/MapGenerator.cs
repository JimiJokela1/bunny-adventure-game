using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour {

	GameObject tileSand1;
	GameObject tileSand2;
	GameObject tileWater1;
	GameObject tileMountain1;
	GameObject spriteMountain1;
	public int levelSize = 10;
	public GameObject[,] tilemap;
	List<GameObject> tileTypeList;
	Transform tileHolder;

	// Instance for singleton class, meaning there is only one of this object ever
	public static MapGenerator Instance = null;
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	
		tileHolder = GameObject.FindGameObjectWithTag ("TileHolder").transform;

		spriteMountain1 = Resources.Load ("Prefabs/sprite_mountain1") as GameObject;

		tileSand1 = Resources.Load ("Prefabs/Tile_sand1") as GameObject;
		tileSand2 = Resources.Load ("Prefabs/Tile_sand2") as GameObject;
		tileWater1 = Resources.Load ("Prefabs/Tile_water1") as GameObject;
		tileMountain1 = Resources.Load ("Prefabs/Tile_mountain1") as GameObject;

		tilemap = new GameObject[levelSize, levelSize];
		tileTypeList = new List<GameObject> ();
		tileTypeList.Add (tileSand1);
		tileTypeList.Add (tileSand2);
		tileTypeList.Add (tileWater1);
		tileTypeList.Add (tileMountain1);
	}

	public void GenerateMap () {
		foreach (GameObject tile in tilemap) {
			Destroy (tile);
		}

		int startAreaSize = 3;
		Vector3 location = new Vector3 (-levelSize / 2 + 0.5f, 0, -levelSize / 2 + 0.5f);

		for (int w = 0; w < levelSize; w++) {
			for (int h = 0; h < levelSize; h++) {
				if (location.x < startAreaSize && location.x > -startAreaSize && location.z < startAreaSize && location.z > -startAreaSize) {
					tilemap [w, h] = Instantiate (tileTypeList [Random.Range (0, 2)], location, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if ((location.x > 40 || location.x < -40) || (location.z > 40 || location.z < -40)) {
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
	}

	// Looks at each tile and it's neighbours and if lots of one tile type surrounds the tile, changes to that tile type
	// Smooths the map's look, more uniform areas
	void Smoothing () {
		for (int w = 1; w < tilemap.GetLength (0) - 1; w++) {
			for (int h = 1; h < tilemap.GetLength (1) - 1; h++) {
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

	void PlaceSprites(){
		for (int w = 1; w < tilemap.GetLength (0) - 1; w++) {
			for (int h = 1; h < tilemap.GetLength (1) - 1; h++) {
				if (tilemap [w, h].tag == "tile_mountain") {
					Vector3 location = new Vector3 (tilemap [w, h].transform.position.x, 0, tilemap [w, h].transform.position.z + Random.Range (-0.4f, 0.4f));
					GameObject tempMountain = Instantiate (spriteMountain1, location, Quaternion.identity, tileHolder);
					tempMountain.transform.localScale = new Vector3 (Random.Range (1, 4), Random.Range (2, 4), 0);
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

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour {

	GameObject tileForest1;
	GameObject tileForest2;
	GameObject tileWater1;
	GameObject tileMountain1;
	GameObject tileDesert1;
	GameObject tileDesert2;

	GameObject spriteMountain1;
	GameObject spriteForest1;

	GameObject randomEvent;
	GameObject storyEvent;
	int tileMask;

	GameObject randomEventMarker;
	GameObject friendMarker;
	GameObject questMarker;

	public int levelSize = 100;
	public int waterWidth = 20;
	public GameObject[,] tilemap;
	Dictionary<string, GameObject> tileTypeDict;
	Transform tileHolder;
	List<GameObject> terrainSprites;
	List<GameObject> eventList;

	public int randomEventAmount = 100;
	Transform eventHolder;
	Transform questEventHolder;


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
		tileHolder = GameObject.Find("TileHolder").transform;
		eventHolder = GameObject.Find ("EventHolder").transform;
		questEventHolder = GameObject.Find ("QuestEventHolder").transform;
//		while (true) {
//			if (TileHolder.Instance != null) {
//				tileHolder = TileHolder.Instance.gameObject.transform;
//				break;
//			}
//		}
//		while (true) {
//			if (EventHolder.Instance != null) {
//				eventHolder = EventHolder.Instance.gameObject.transform;
//				break;
//			}
//		}
//		while (true) {
//			if (QuestEventHolder.Instance != null) {
//				questEventHolder = QuestEventHolder.Instance.gameObject.transform;
//				break;
//			}
//		}

		spriteMountain1 = Resources.Load ("Prefabs/sprite_mountain1") as GameObject;
		spriteForest1 = Resources.Load ("Prefabs/sprite_forest1") as GameObject;

		tileForest1 = Resources.Load ("Prefabs/Tile_forest1") as GameObject;
		tileForest2 = Resources.Load ("Prefabs/Tile_forest2") as GameObject;
		tileWater1 = Resources.Load ("Prefabs/Tile_water1") as GameObject;
		tileMountain1 = Resources.Load ("Prefabs/Tile_mountain1") as GameObject;
		tileDesert1 = Resources.Load ("Prefabs/Tile_desert1") as GameObject;
		tileDesert2 = Resources.Load ("Prefabs/Tile_desert2") as GameObject;

		randomEvent = Resources.Load ("Prefabs/RandomEvent") as GameObject;
		storyEvent = Resources.Load ("Prefabs/StoryEvent") as GameObject;

		randomEventMarker = Resources.Load ("Prefabs/randomEventMarker") as GameObject;
		questMarker = Resources.Load ("Prefabs/questLocationMarker") as GameObject;
		friendMarker = Resources.Load ("Prefabs/friendHouseMarker") as GameObject;

		terrainSprites = new List<GameObject> ();
		eventList = new List<GameObject> ();

		tilemap = new GameObject[levelSize, levelSize];
		tileTypeDict = new Dictionary<string, GameObject> ();
		tileTypeDict.Add ("tile_land1", tileForest1);
		tileTypeDict.Add ("tile_land2", tileForest2);
		tileTypeDict.Add ("tile_water1", tileWater1);
		tileTypeDict.Add ("tile_mountain1", tileMountain1);
		tileTypeDict.Add ("tile_desert1", tileDesert1);
		tileTypeDict.Add ("tile_desert2", tileDesert2);

		tileMask = LayerMask.GetMask ("TileMask");

	}

	/// <summary>
	/// Generates new map, destroys all old tiles and slaps down new ones, calls Smoothing(), PlaceSprites(), PlaceEvents(), PlaceClouds().
	/// </summary>
	public void GenerateMap () {
		foreach (GameObject tile in tilemap) {
			Destroy (tile);
		}
		foreach (GameObject sprite in terrainSprites) {
			Destroy (sprite);
		}
		terrainSprites.Clear ();
		foreach (GameObject eventObject in eventList) {
			Destroy (eventObject);
		}
		eventList.Clear ();
		GetComponent<Clouds> ().DestroyClouds ();

		int startAreaSize = 3;
		Vector3 location = new Vector3 (-levelSize / 2 + 0.5f, 0, -levelSize / 2 + 0.5f);
		// Place all tiles
		for (int w = 0; w < levelSize; w++) {
			for (int h = 0; h < levelSize; h++) {
				int boundary = levelSize / 2 - waterWidth;
				if (location.x < startAreaSize && location.x > -startAreaSize && location.z < startAreaSize && location.z > -startAreaSize) {
					tilemap [w, h] = Instantiate (RandomTile("tile_land1", "tile_land2", "tile_water1"), location, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if ((location.x > boundary || location.x < -boundary) || (location.z > boundary || location.z < -boundary)) {
					tilemap [w, h] = Instantiate (tileTypeDict ["tile_water1"], location, Quaternion.Euler (90, 0, 0), tileHolder);
				} else {
					tilemap [w, h] = Instantiate (RandomTile(), location, Quaternion.Euler (90, 0, 0), tileHolder);

				}

				location += new Vector3 (1, 0, 0);
			}
			location = new Vector3 (-levelSize / 2 + 0.5f, 0, location.z);
			location += new Vector3 (0, 0, 1);

		}
		Smoothing ();
		PlaceSprites ();
		PlaceEvents (randomEventAmount);
		GetComponent<Clouds> ().PlaceClouds ();
//		Debug.Log (tilemap [50, 50].tag);
	}

	GameObject RandomTile(string choice1 = null, string choice2 = null, string choice3 = null, string choice4 = null){
		bool allChoices = false;
		if (choice1 == null && choice2 == null && choice3 == null && choice4 == null) {
			allChoices = true;
		}
		while (true) {
			int choice = Random.Range (0, tileTypeDict.Count);
			switch (choice) {
				case 0:
					if (!allChoices && choice1 != null) {
						return tileTypeDict [choice1];
					} else {
						return tileTypeDict ["tile_land1"];
					}
				case 1:
					if (!allChoices && choice2 != null) {
						return tileTypeDict [choice2];
					} else {
						return tileTypeDict ["tile_land2"];
					}
				case 2:
					if (!allChoices && choice3 != null) {
						return tileTypeDict [choice3];
					} else {
						return tileTypeDict ["tile_water1"];
					}
				case 3:
					if (!allChoices && choice4 != null) {
						return tileTypeDict [choice4];
					} else {
						return tileTypeDict ["tile_mountain1"];
					}

				case 4:
//				return tileTypeDict ["tile_land1"];
					break;
				default:
					break;
			}
		}
	}

	/// <summary>
	/// Looks at each tile and it's neighbours. If lots of one tile type surrounds the tile, changes to that tile type.
	/// Smooths the map's look, resulting in more uniform areas.
	/// </summary>
	public void Smoothing () {
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
					} else if (tile.tag == "tile_water1") {
						waterTiles++;
					} else if (tile.tag == "tile_mountain1") {
						mountainTiles++;
					}
				}

				// Depending on tile type counts change the current tile
				Vector3 temp = tilemap [w, h].transform.position;

				if (mountainTiles > 4) {
					Destroy (tilemap [w, h]);
					tilemap [w, h] = Instantiate (tileTypeDict ["tile_mountain1"], temp, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if (waterTiles > 4) {
					Destroy (tilemap [w, h]);
					tilemap [w, h] = Instantiate (tileTypeDict ["tile_water1"], temp, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if (land1Tiles > 4) {
					Destroy (tilemap [w, h]);
					tilemap [w, h] = Instantiate (tileTypeDict ["tile_land1"], temp, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if (land2Tiles > 4) {
					Destroy (tilemap [w, h]);
					tilemap [w, h] = Instantiate (tileTypeDict ["tile_land2"], temp, Quaternion.Euler (90, 0, 0), tileHolder);
				} else if (tilemap [w, h].tag == "tile_water1" && waterTiles < 4) {
					Destroy (tilemap [w, h]);
					if (land1Tiles > land2Tiles && land1Tiles > mountainTiles) {
						tilemap [w, h] = Instantiate (tileTypeDict ["tile_land1"], temp, Quaternion.Euler (90, 0, 0), tileHolder);
					} else if (land2Tiles > land1Tiles && land2Tiles > mountainTiles) {
						tilemap [w, h] = Instantiate (tileTypeDict ["tile_land2"], temp, Quaternion.Euler (90, 0, 0), tileHolder);
					} else {
						tilemap [w, h] = Instantiate (tileTypeDict ["tile_mountain1"], temp, Quaternion.Euler (90, 0, 0), tileHolder);
					}
				}
			}
		}
	}

	/// <summary>
	/// Places upright mountain and forest sprites 
	/// </summary>
	void PlaceSprites(){
		for (int w = 1; w < tilemap.GetLength (0) - 1; w++) {
			for (int h = 1; h < tilemap.GetLength (1) - 1; h++) {
				if (tilemap [w, h].tag == "tile_mountain1") {
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

	/// <summary>
	/// Places random events randomly on map and hides them.
	/// </summary>
	/// <param name="amount">Amount of random events to spawn.</param>
	void PlaceEvents(int amount){
		for (int i = 0; i < randomEventAmount; i++) {
			bool success = false;
			while (success == false) {
				float halfLevelSize = levelSize / 2;
				Vector3 location = new Vector3 (Random.Range (-halfLevelSize + waterWidth, halfLevelSize - waterWidth), 0.5f, Random.Range (-halfLevelSize + waterWidth, halfLevelSize - waterWidth));
				if (Vector3.Distance (location, Vector3.zero) > 8f) {
					RaycastHit hit;
					if (Physics.Raycast (location, Vector3.down, out hit, 100f, tileMask)) {
						string tileType = hit.collider.tag;
						if (tileType != "tile_water1") {
							GameObject temp = Instantiate (randomEventMarker, location, Quaternion.Euler(-90f, 0f, Random.Range(-35f, 35f)), eventHolder);
							eventList.Add (temp);
							success = true;
						}
					}
				}
			}
		}

		// place panda and courthouse
		Vector3 pandaPos;
		Vector3 courthousePos;
		float landEdge = (levelSize - waterWidth * 2) / 2 + .5f;
		float closeEdge = (levelSize - waterWidth * 2) / 3 + .5f;
		int tries = 100;
		while (tries > 0){
			int randomSide = Random.Range (0, 4);
			if (randomSide == 0) {
				pandaPos = new Vector3 (Random.Range (-landEdge, landEdge), .5f, Random.Range(closeEdge, landEdge));
				courthousePos = new Vector3 (Random.Range (-closeEdge / 2, closeEdge / 2), .5f, -closeEdge / 2);
			} else if (randomSide == 1) {
				pandaPos = new Vector3 (Random.Range(closeEdge, landEdge), .5f, Random.Range (-landEdge, landEdge));
				courthousePos = new Vector3 (-closeEdge / 2, .5f, Random.Range (-closeEdge / 2, closeEdge / 2));
			} else if (randomSide == 2) {
				pandaPos = new Vector3 (Random.Range (-landEdge, landEdge), .5f, -Random.Range(closeEdge, landEdge));
				courthousePos = new Vector3 (Random.Range (-closeEdge / 2, closeEdge / 2), .5f, closeEdge / 2);
			} else {
				pandaPos = new Vector3 (-Random.Range(closeEdge, landEdge), .5f, Random.Range (-landEdge, landEdge));
				courthousePos = new Vector3 (closeEdge / 2, .5f, Random.Range (-closeEdge / 2, closeEdge / 2));
			}
			RaycastHit hit;
			if (Physics.Raycast (pandaPos, Vector3.down, out hit, 100f, tileMask)) {
				if (hit.collider.tag == "tile_mountain1") {
					Debug.Log ("tries: " + tries);
					// panda
					GameObject pandaEvent = Instantiate (questMarker, pandaPos, Quaternion.Euler(-90f, 0f, Random.Range(-35f, 35f)), questEventHolder);
					pandaEvent.GetComponent<EventTriggerer> ().storyEventName = "panda";
					pandaEvent.GetComponent<SphereCollider> ().radius = 15f;

					// courthouse 
					GameObject courthouseEvent = Instantiate (questMarker, courthousePos, Quaternion.Euler(-90f, 0f, Random.Range(-35f, 35f)), questEventHolder);
					courthouseEvent.GetComponent<EventTriggerer> ().storyEventName = "courthouse";

					Debug.Log ("Placed panda and courthouse");
					break;
				}
			}
			tries--;
		}

		// place quests
		Vector3 owlPos = new Vector3(Random.Range(-closeEdge / 2, closeEdge / 2), .5f, Random.Range(-closeEdge / 2, closeEdge / 2));
		Vector3 unicornPos = new Vector3(Random.Range(-closeEdge, closeEdge), .5f, Random.Range(-closeEdge, closeEdge));
		Vector3 guineapigPos = new Vector3(Random.Range(-closeEdge / 2, closeEdge / 2), .5f, Random.Range(-closeEdge / 2, closeEdge / 2));
		Vector3 centaurPos = new Vector3(Random.Range(-closeEdge, closeEdge), .5f, Random.Range(-closeEdge, closeEdge));

		GameObject owlEvent = Instantiate (friendMarker, owlPos, Quaternion.Euler(-90f, 0f, Random.Range(-35f, 35f)), questEventHolder);
		owlEvent.GetComponent<EventTriggerer> ().storyEventName = "owl";
		owlEvent.GetComponent<EventTriggerer> ().MakeAlwaysHidden ();
		GameObject unicornEvent = Instantiate (questMarker, unicornPos, Quaternion.Euler(-90f, 0f, Random.Range(-35f, 35f)), questEventHolder);
		unicornEvent.GetComponent<EventTriggerer> ().storyEventName = "unicorn";
		unicornEvent.GetComponent<EventTriggerer> ().MakeAlwaysHidden ();
		GameObject guineapigEvent = Instantiate (friendMarker, guineapigPos, Quaternion.Euler(-90f, 0f, Random.Range(-35f, 35f)), questEventHolder);
		guineapigEvent.GetComponent<EventTriggerer> ().storyEventName = "david";
		guineapigEvent.GetComponent<EventTriggerer> ().MakeAlwaysHidden ();
		GameObject centaurEvent = Instantiate (questMarker, centaurPos, Quaternion.Euler(-90f, 0f, Random.Range(-35f, 35f)), questEventHolder);
		centaurEvent.GetComponent<EventTriggerer> ().storyEventName = "centaur";
		centaurEvent.GetComponent<EventTriggerer> ().MakeAlwaysHidden ();

		unicornEvent.SetActive (false);
		centaurEvent.SetActive (false);


	}

	public void LoadMap(string[] tiletags){
		Vector3 location = new Vector3 (-levelSize / 2 + 0.5f, 0, -levelSize / 2 + 0.5f);
		for (int w = 0; w < levelSize; w++) {
			for (int h = 0; h < levelSize; h++) {
				tilemap[w, h] = Instantiate (tileTypeDict[tiletags[w * 100 + h]], location, Quaternion.Euler (90, 0, 0), tileHolder);
				location += new Vector3 (1, 0, 0);
			}
			location = new Vector3 (-levelSize / 2 + 0.5f, 0, location.z);
			location += new Vector3 (0, 0, 1);
		}
		PlaceSprites ();
		PlaceEvents (randomEventAmount);
		GetComponent<Clouds> ().PlaceClouds ();
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

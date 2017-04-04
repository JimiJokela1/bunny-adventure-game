﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventGenerator2 : MonoBehaviour{

	public LayerMask layerMask;

	public List<GameObject> objectList = new List<GameObject>();

	public List<Material> terrainMaterial = new List<Material> ();

	public List<Vector3> terrainPosition = new List<Vector3> ();

	public int assetRangeX = 15;
	public int assetRangeZ = 15;
	public int numberOfAssets = 10;
	public int rayRange = 10;


	// Use this for initialization
	void Start () {

		terrainPosition.Add (new Vector3(0, 0, 0));
		terrainPosition.Add (new Vector3(10, 0, 0));
		terrainPosition.Add (new Vector3(-10, 0, 0));
		terrainPosition.Add (new Vector3(0, 0, 10));
		terrainPosition.Add (new Vector3(0, 0, -10));
		terrainPosition.Add (new Vector3(10, 0, -10));
		terrainPosition.Add (new Vector3 (-10, 0, 10));
		terrainPosition.Add (new Vector3(10, 0, 10));
		terrainPosition.Add (new Vector3(-10, 0, -10));



	}
		

	public void GenerateEvent() {
	
		//Generate tiles
		for (int i = 0; i < terrainPosition.Count; i++) {
			Tile tileTemp = new Tile("tile_" + i, 10, 1, 10, terrainPosition[i], terrainMaterial[Random.Range(0, 3)]);
			tileTemp.instantiateTile (tileTemp);
		}
		//Generate trees & rocks
		int j = 0;
		while(j < numberOfAssets) {
			GameObject asset = objectList [Random.Range (0, 3)];
			Vector3 position = new Vector3 (Random.Range (-assetRangeX, assetRangeX), 0, Random.Range (-assetRangeZ, assetRangeZ));
			}

		}

	public void GenerateNPC() {
		string name = "velho";
		Sprite portrait1 = Resources.Load ("velho") as Sprite;

	
		}
			

}

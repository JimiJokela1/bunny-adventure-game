using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventGenerator : MonoBehaviour {

	public List<GameObject> terrainList = new List<GameObject>();

	public List<Vector3> terrainPosition = new List<Vector3> ();

	public List<GameObject> assetList = new List<GameObject> ();

	private float rndMax = 15f;
	private float rndMin = -15f;



	// POSITIONS

	Vector3 pos1 = new Vector3(0, 0, 0);
	Vector3 pos2 = new Vector3(10, 0, 0);
	Vector3 pos3 = new Vector3(-10, 0, 0);
	Vector3 pos4 = new Vector3(0, 0, 10);
	Vector3 pos5 = new Vector3(0, 0, -10);
	Vector3 pos6 = new Vector3(10, 0, -10);
	Vector3 pos7 = new Vector3 (-10, 0, 10);
	Vector3 pos8 = new Vector3(10, 0, 10);
	Vector3 pos9 = new Vector3(-10, 0, -10);

	 
	// Use this for initialization
	void Start () {
		terrainPosition.Add (pos1);
		terrainPosition.Add (pos2);
		terrainPosition.Add (pos3);
		terrainPosition.Add (pos4);
		terrainPosition.Add (pos5);
		terrainPosition.Add (pos6);
		terrainPosition.Add (pos7);
		terrainPosition.Add (pos8);
		terrainPosition.Add (pos9);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		


	public void generateEvent() {
		//RANDOMIZE TERRAINLIST
		for (int i = 0; i < terrainList.Count; i++) {
			GameObject temp = terrainList[i];
			int randomIndex = Random.Range(i, terrainList.Count);
			terrainList[i] = terrainList[randomIndex];
			terrainList[randomIndex] = temp;
		}

		//INSTANTIATE TERRAIN ELEMENTS
		for (int i = 0; i < terrainList.Count; i++) {
			Instantiate (terrainList [i], terrainPosition[i], Quaternion.identity);

		}

		//INSTANTIATE TREES
		for (int i = 0; i < assetList.Count; i++) {
			Instantiate (assetList [i], new Vector3 (Random.Range (rndMin, rndMax), 2.5f, Random.Range (rndMin, rndMax)), Quaternion.Euler(-90, 0, 0));
		}


		
	}


}




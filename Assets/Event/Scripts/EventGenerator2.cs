using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventGenerator2 : MonoBehaviour{



	public NPC npc;
	public LayerMask layerMask;
	//list of environmental assets
	public List<GameObject> objectList = new List<GameObject>();
	//templist 
	public List<GameObject> tempList = new List<GameObject> ();
	//materials for tiles
	public List<Material> terrainMaterial = new List<Material> ();
	//positions for tiles
	public List<Vector3> terrainPosition = new List<Vector3> ();
	//position range for environmetal objects
	public int assetRangeX = 15;
	public int assetRangeZ = 15;
	//how many environmental objects will be in the scene
	public int numberOfAssets = 10;
	//Range for the raycast
	public int rayRange = 10;

	public GameObject item;
	//List for instanciated objects
	public List<GameObject> assetList = new List<GameObject> ();
	public GameObject shrubbery;
	public GameObject knightsWhoSayNi;

	// Use this for initialization
	void Start () {
		npc = GameObject.FindObjectOfType (typeof(NPC)) as NPC;
		//Add positions where to instanciate tiles
		terrainPosition.Add (new Vector3(0, 0, 0));
		terrainPosition.Add (new Vector3(10, 0, 0));
		terrainPosition.Add (new Vector3(-10, 0, 0));
		terrainPosition.Add (new Vector3(0, 0, 10));
		terrainPosition.Add (new Vector3(0, 0, -10));
		terrainPosition.Add (new Vector3(10, 0, -10));
		terrainPosition.Add (new Vector3 (-10, 0, 10));
		terrainPosition.Add (new Vector3(10, 0, 10));
		terrainPosition.Add (new Vector3(-10, 0, -10));
		

		GenerateEvent ();
	}
		

	public void GenerateEvent() {


	
		foreach (GameObject o in tempList)
			Destroy (o);

		//Generate tiles
		for (int i = 0; i < terrainPosition.Count; i++) {
			Tile tileTemp = new Tile ("tile_" + i, 10, 1, 10, terrainPosition [i], terrainMaterial [Random.Range (0, 3)]);
			tileTemp.instantiateTile (tileTemp);
		}
		//Generate trees & rocks

		int j = 0;
		while(j < numberOfAssets) {
			GameObject asset = objectList [Random.Range (0, 3)];
			Vector3 position = new Vector3 (Random.Range (-assetRangeX, assetRangeX), 0, Random.Range (-assetRangeZ, assetRangeZ));

//			Debug.DrawLine (position, new Vector3 (position.x, 10, position.z), Color.green, 60f);
			//Cast a ray from position +10.y, if it hits collider, randomize new position
			if (Physics.Raycast (new Vector3 (position.x, 10, position.z), -Vector3.up, 5f) == false) {
				Debug.Log ("Ei osunut");
				GameObject temp = Instantiate (asset, position, Quaternion.identity);
				if (temp.GetComponent<Rigidbody> ()) {
					temp.GetComponent<Rigidbody> ().centerOfMass = new Vector3 (0f, -3f, 0f);
				}
				assetList.Add (temp);
				j++;
			} else {
				j++;
			}
		}
		SpawnKnights ();
		GenerateShrubbery (15f, 15f);
		AddItems ();
	}

	void SpawnKnights() {
		if (Random.value > .9f) {
			Vector3 position = new Vector3(Random.Range (-assetRangeX, assetRangeX), 1, Random.Range (-assetRangeZ, assetRangeZ));
			if (!Physics.Raycast (position, Vector3.up, 1f)) {
				GameObject temp = Instantiate (knightsWhoSayNi, position, Quaternion.identity);
			}
		}
	}

	void GenerateShrubbery(float assetRangeX, float assetRangeZ){
		int itemCount = Random.Range(1,3);
		for (int i = 0; i < itemCount; i++) {
			Vector3 position = new Vector3 (Random.Range (-assetRangeX, assetRangeX), 1, Random.Range (-assetRangeZ, assetRangeZ));
			if (!Physics.Raycast (position, Vector3.up, 1f)) {
				GameObject temp = Instantiate (shrubbery, position, Quaternion.identity);
			}
		}
	}

	/// <summary>
	/// Adds the items.
	/// </summary>
	void AddItems(){
		int itemCount = 0;
		for (int i = 0; i < itemCount; i++) {
			Vector3 position = new Vector3 (Random.Range (-assetRangeX, assetRangeX), 1, Random.Range (-assetRangeZ, assetRangeZ));
			GameObject temp = Instantiate (item, position, Quaternion.identity);
			temp.GetComponent<Item> ().GenerateItem ("Ankka");
		}
	}

}
	

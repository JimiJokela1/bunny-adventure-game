using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGenerator3 : MonoBehaviour{


	public List<Material> terrainMaterial = new List<Material> ();

	public int tileX = 5;
	public int tileZ = 5;
	public int tileY = 1;

	// Use this for initialization
	void Start () {


	}

	public void GenerateEvent() {
		Tile tileOrigin = new Tile ("tileOrigin", tileX, tileY, tileZ, new Vector3 (0, 0, 0), terrainMaterial [0]);
		tileOrigin.instantiateTile (tileOrigin);


			
	}

}

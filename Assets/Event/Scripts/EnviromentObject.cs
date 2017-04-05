using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentObject {

	public GameObject environmentalObject;
	public int sizeX;
	public int sizeZ;
	public int sizeY;
	public Vector3 position;
	public Material material;


	public EnviromentObject(GameObject environmentalObject, Vector3 position, int sizeX, int sizeZ, int sizeY, Material material) {
		this.environmentalObject = environmentalObject;
		this.position = position;
		this.sizeX = sizeX;
		this.sizeZ = sizeZ;
		this.sizeY = sizeY;
		this.material = material;
	}

	public void instantiateEnvironmentObject(EnviromentObject asset) {
		
	
	}

}

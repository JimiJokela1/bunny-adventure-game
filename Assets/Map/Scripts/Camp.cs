using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour {

	// TODO
	// Buttons for camping activities including:
	// - inventory?
	// - clickable objects that make little sounds or particle effects like fire stoking
	// Camp sprite and campfire particle system prefab

	GameObject inventory;

	void Start(){
		inventory = GameObject.Find ("Inventory");
		inventory.SetActive (false);
	}

	public void SetUpCamp(){
		// Activate all the things
		Camera.main.GetComponent<CameraController>().CampZoom();
	}

	public void PackUpCamp(){
		// Deactivate all the things
		Camera.main.GetComponent<CameraController>().Unzoom();
	}

	void OnInventoryButtonClick(){
		inventory.SetActive (true);
	}
}

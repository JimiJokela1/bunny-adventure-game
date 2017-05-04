using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour {

	// TODO
	// Buttons for camping activities including:
	// - inventory?
	// - clickable objects that make little sounds or particle effects like fire stoking

	// Camp sprite and campfire particle system prefab
	GameObject campFire;
	GameObject campFireTemp;
	Transform player;

	void Start(){
//		inventory = GameObject.Find ("Inventory");
//		inventory.SetActive (false);
		campFire = Resources.Load("Prefabs/FireParticles") as GameObject;
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	public void SetUpCamp(){
		// Activate all the things
		Camera.main.GetComponent<CameraController>().CampZoom();
		GameController.Instance.GetComponent<Inventory> ().ShowInv ();
		GameController.Instance.timeScale = 0.3f;
		campFireTemp = Instantiate (campFire, new Vector3 (player.position.x, 0.5f, player.position.z - 1f), Quaternion.identity);
			
	}

	public void PackUpCamp(){
		// Deactivate all the things
		Camera.main.GetComponent<CameraController> ().Unzoom ();
		GameController.Instance.GetComponent<Inventory> ().HideInv ();
		GameController.Instance.timeScale = 1f;
		if (campFireTemp != null) {
			Destroy (campFireTemp);
		}
	}

	void OnInventoryButtonClick(){
//		inventory.SetActive (true);
	}
}

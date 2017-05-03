using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	string itemName;

	public void GenerateItem(string itemName) {
		this.itemName = itemName;
		if (itemName != "Unicorn Dust") {
			GetComponentInChildren<ParticleSystem> ().gameObject.SetActive (false);
		}
	}


	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.name == "EventPlayer") {
			Debug.Log ("Picked up item" + itemName);
//			GameController.Instance.AddItemToInventory (itemName);
			PlayerController.Instance.AddToInv (itemName);
			Destroy (gameObject);
		}
	}
}

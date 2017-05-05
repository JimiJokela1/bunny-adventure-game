using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armorTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		GameController.Instance.GetComponent<Inventory> ().AddToInv ("davids armor");
		Destroy (gameObject);
	}
}

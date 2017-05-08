using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armorTrigger : MonoBehaviour {
	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other) {
		GameController.Instance.GetComponent<Inventory> ().AddToInv ("davids armor");
		Destroy (gameObject);
	}
}

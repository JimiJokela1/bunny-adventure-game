using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrubberyTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		GameController.Instance.GetComponent<Inventory> ().AddToInv ("shrubbery");
		Destroy (gameObject);
	}
}

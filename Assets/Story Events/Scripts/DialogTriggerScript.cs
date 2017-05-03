using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTriggerScript : MonoBehaviour {

	public DialogueController diagController;
	string name;

	void OnTriggerEnter(Collider other) {
		Debug.Log ("dial trigger osu" + name);
		name = this.gameObject.name;
		diagController.DialogueTriggered (name);
	}
}

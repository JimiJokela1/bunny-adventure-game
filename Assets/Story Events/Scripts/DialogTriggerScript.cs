using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTriggerScript : MonoBehaviour {

	public DialogueController diagController;
	string name;

	void OnTriggerEnter(Collider other) {
		name = this.gameObject.name;
		Debug.Log ("dialogue trigger osu " + name);
		diagController.DialogueTriggered (name);
	}
}

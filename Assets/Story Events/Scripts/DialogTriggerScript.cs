using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTriggerScript : MonoBehaviour {

	public DialogueController diagController;
	string name;
	void Start() {
		diagController = FindObjectOfType<DialogueController> ();
	}
	/// <summary>
	/// Raises the trigger enter event.
	/// Passes the name of NPC to dialoguecontroller
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other) {
		name = this.gameObject.name;
		Debug.Log ("dialogue trigger osu " + name);
		diagController.DialogueTriggered (name);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript2 : MonoBehaviour {

	public EventController eventController;

	void OnTriggerEnter(Collider other) {
		Debug.Log ("osu");
		eventController.TeleportPlayer (new Vector3(96,0.5f,-82));
	}
}

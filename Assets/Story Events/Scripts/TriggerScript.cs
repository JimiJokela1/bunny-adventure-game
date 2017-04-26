using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {

	public EventController eventController;

	void OnTriggerEnter(Collider other) {
		Debug.Log ("osu");
		eventController.TeleportPlayer (new Vector3(0,0.5f,0));
	}
}

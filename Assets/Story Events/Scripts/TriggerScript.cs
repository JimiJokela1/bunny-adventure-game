using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {

	public EventController eventController;

	void OnTriggerEnter(Collider other) {
		Debug.Log ("ontrigger osu");
		eventController.TriggerEvent ();
	}
}

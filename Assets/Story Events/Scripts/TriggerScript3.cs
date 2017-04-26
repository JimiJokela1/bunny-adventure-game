using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript3 : MonoBehaviour {

	public EventController eventController;

	void OnTriggerEnter(Collider other) {
		Debug.Log ("JARKKO EVENT");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {

	public EventController eventcontroller;
	GameObject player;

	void Start() {
		player = GameObject.Find ("EventPlayer");
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("ontrigger osu");
		eventcontroller.TriggerEvent ();

	}
}

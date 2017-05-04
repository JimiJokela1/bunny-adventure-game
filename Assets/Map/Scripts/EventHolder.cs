using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds random events so we can reactivate them
public class EventHolder : MonoBehaviour {

	// Instance for singleton class, meaning there is only one of this object ever
	public static EventHolder Instance = null;
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}
}

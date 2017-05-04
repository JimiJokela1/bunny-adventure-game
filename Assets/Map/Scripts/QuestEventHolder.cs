using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds uest events so we can reactivate events
public class QuestEventHolder : MonoBehaviour {

	// Instance for singleton class, meaning there is only one of this object ever
	public static QuestEventHolder Instance = null;
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}
}

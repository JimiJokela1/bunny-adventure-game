﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds map tiles so we can return to the same map after an event
public class TileHolder : MonoBehaviour {

	// Instance for singleton class, meaning there is only one of this object ever
	public static TileHolder Instance = null;
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}
}

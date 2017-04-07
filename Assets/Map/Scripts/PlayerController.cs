using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Instance for singleton class, meaning there is only one of this object ever
	public static PlayerController Instance = null;
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	MapPlayer mapPlayer;

	void Start(){
		mapPlayer = gameObject.GetComponent<MapPlayer> ();
	}

	void Update(){
		if (GameController.Instance.GetGameState () == GameController.GAMESTATE_MAP) {
			mapPlayer.HandleControls ();
		} else if (GameController.Instance.GetGameState () == GameController.GAMESTATE_EVENT) {
			// Event controls
		}
	}

	void FixedUpdate(){

	}
}

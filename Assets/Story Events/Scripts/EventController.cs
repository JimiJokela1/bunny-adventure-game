using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour {

	public int sceneProgress = 1; //keeps track of scene progress
	public GameObject player;
	public Camera eventCamera;
	Vector3[] locationsArray = new [] {new Vector3(0, 0.5f, 0), new Vector3(96,0.5f,-82)};


	public void TriggerEvent() {
		switch (sceneProgress) {
		case 1:
			Debug.Log ("triggerevent osu");
			TeleportPlayer (locationsArray [0]);
			sceneProgress += 1;
			break;
		case 2:
			TeleportPlayer (locationsArray [1]);
			sceneProgress += 1;
			break;
		default:
			break;
		}
	}

	public void TeleportPlayer(Vector3 position) {
		player.transform.position = position;
	}

}

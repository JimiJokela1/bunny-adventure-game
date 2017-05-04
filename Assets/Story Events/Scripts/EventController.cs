using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventController : MonoBehaviour {

	int sceneProgress = 0; //keeps track of scene progress
	GameObject player;
	public Camera eventCamera;
	public GameObject[] teleportTargets;
	void Start() {
		player = GameObject.Find ("EventPlayer");
		teleportTargets = GameObject.FindGameObjectsWithTag ("teleport").OrderBy (go => go.name).ToArray ();

		foreach (GameObject target in teleportTargets) {
			Debug.Log (target.name);
		}
		Debug.Log ("sceneprogress" + sceneProgress);

	}


	public void TriggerEvent() {
		player.transform.position = teleportTargets[sceneProgress].transform.position;
		Debug.Log (sceneProgress);
		sceneProgress++;
	}

	public void TeleportPlayer(Vector3 position) {
		player.transform.position = position;
	}

	public void CompleteEvent(int progressID){
		PlayerController.Instance.AddToProgress (progressID);
	}
}

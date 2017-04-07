using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventTriggerer : MonoBehaviour {

	string tileType;

	GameObject player;
	GameObject flag;

	float randomEventChance = 1f;
	int tileMask;

	float triggerTimer;
	float triggerTime = 3f;
	bool triggered = false;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		tileMask = LayerMask.GetMask ("TileMask");
		flag = Resources.Load ("Prefabs/flag") as GameObject;
	}

	void Update(){
		if (triggered) {
			if (triggerTimer < triggerTime) {
				triggerTimer += Time.deltaTime;
			} else {
				triggerTimer = 0f;
				triggered = false;
				GameController.Instance.StartEvent (tileType);
			}
		}
	}

	void FixedUpdate () {
//		if (Random.Range (0f, 100f) < randomEventChance) {
//			RaycastHit hit;
//			if (Physics.Raycast (player.transform.position, Vector3.down, out hit, 100f, tileMask)) {
//				tileType = hit.collider.tag;
//				GameController.Instance.ChangeGameState (GameController.GAMESTATE_EVENT);
//			}
//		}
	}

	public void TriggerEvent() {
		RaycastHit hit;
		if (Physics.Raycast (player.transform.position, Vector3.down, out hit, 100f, tileMask)) {
			tileType = hit.collider.tag;
			Instantiate (flag, hit.point, Quaternion.identity);
			triggered = true;
			player.GetComponent<MapPlayer> ().StopMoving ();
			Camera.main.GetComponent<CameraController> ().EventZoom ();
		}
	}
}

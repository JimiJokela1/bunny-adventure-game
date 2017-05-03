using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventTriggerer : MonoBehaviour {

	string tileType;
	public string eventType;
	public string storyEventName;
	float triggerDistance = 0.5f;

	GameObject player;
	GameObject flag;

	int tileMask;

//	public float randomEventChance = 1f;
//	float randomEventTimer = 0f;
//	float randomEventTime = 1f;

	float triggerTimer = 0f;
	float triggerTime = 3f;
	public bool triggered;
	bool visible = false;
	bool testOverride = false;

	void Start () {
		if (eventType == null) {
			eventType = "RandomEvent";
		}
		player = GameObject.FindGameObjectWithTag ("Player");
		tileMask = LayerMask.GetMask ("TileMask");
		flag = Resources.Load ("Prefabs/flag") as GameObject;
		GetComponent<MeshRenderer> ().sortingLayerName = "EventLayer";
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
	}

	void Update(){
		if (visible || testOverride) {
			if (!triggered && Vector3.Distance (transform.position, player.transform.position) < triggerDistance) {
				Debug.Log ("trigger");
				if (eventType == "RandomEvent") {
					TriggerRandomEvent ();
				} else if (eventType == "StoryEvent") {
					TriggerStoryEvent ();
				}
			}
			if (triggered) {
				// After a timer, during which camera zooms, begin event by calling ChangeGameState
				if (triggerTimer < triggerTime) {
					triggerTimer += Time.deltaTime;
				} else {
					if (eventType == "RandomEvent") {
						GameController.Instance.ChangeGameState (GameController.GAMESTATE_RANDOMEVENT, this);
					} else if (eventType == "StoryEvent") {
						GameController.Instance.ChangeGameState (GameController.GAMESTATE_STORYEVENT, this);
					}
					gameObject.SetActive (false);
				}
			}
		}
	}

	/// <summary>
	/// Tests event trigger when button clicked.
	/// </summary>
	public void TestTriggerEvent(){
		RaycastHit hit;
		if (Physics.Raycast (player.transform.position, Vector3.down, out hit, 100f, tileMask)) {
			tileType = hit.collider.tag;
			Instantiate (flag, hit.point, Quaternion.identity);
			triggered = true;
			testOverride = true;
			player.GetComponent<MapPlayer> ().StopMoving ();
			Camera.main.GetComponent<CameraController> ().EventZoom ();
			GameController.Instance.StopTime ();
		}
	}

	/// <summary>
	/// Triggers a random event, checks the tile under.
	/// </summary>
	void TriggerRandomEvent() {
		RaycastHit hit;
		if (Physics.Raycast (player.transform.position, Vector3.down, out hit, 100f, tileMask)) {
			tileType = hit.collider.tag;
			Instantiate (flag, hit.point, Quaternion.identity);
			triggered = true;
			player.GetComponent<MapPlayer> ().StopMoving ();
			Camera.main.GetComponent<CameraController> ().EventZoom ();
			GameController.Instance.StopTime ();
		}
	}

	/// <summary>
	/// Triggers a story event, checks the tile under.
	/// </summary>
	void TriggerStoryEvent(){
		RaycastHit hit;
		if (Physics.Raycast (player.transform.position, Vector3.down, out hit, 100f, tileMask)) {
			tileType = hit.collider.tag;
			Instantiate (flag, hit.point, Quaternion.identity);
			triggered = true;
			player.GetComponent<MapPlayer> ().StopMoving ();
			Camera.main.GetComponent<CameraController> ().EventZoom ();
			GameController.Instance.StopTime ();
		}
	}

	// 
	/// <summary>
	/// Returns the type of the map tile under the player.
	/// </summary>
	/// <returns>The map tile type for event generation.</returns>
	public string GetEventTileType(){
		RaycastHit hit;
		if (Physics.Raycast (player.transform.position, Vector3.down, out hit, 100f, tileMask)) {
			tileType = hit.collider.tag;
			return tileType;
		}
		return "error";
	}

	void OnTriggerEnter(){
		visible = true;
		GetComponent<MeshRenderer> ().enabled = true;
	}

	void OnTriggerExit(){
		visible = false;
		GetComponent<MeshRenderer> ().enabled = false;
	}
//	void FixedUpdate () {
//		if (player.GetComponent<MapPlayer> ().moving) {
//			if (randomEventTimer > randomEventTime) {
//
//				if (Random.Range (0f, 100f) < randomEventChance) {
//					RaycastHit hit;
//					if (Physics.Raycast (player.transform.position, Vector3.down, out hit, 100f, tileMask)) {
//						tileType = hit.collider.tag;
//						TriggerRandomEvent ();
//					}
//				}
//				randomEventTimer = 0f;
//			} else {
//				randomEventTimer += Time.fixedDeltaTime;
//			}
//		}
//	}

}

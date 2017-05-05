using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		progress = new List<int> ();
		mapPlayer = gameObject.GetComponent<MapPlayer> ();
	}

	MapPlayer mapPlayer;

	public const int TUTORIAL = 0;
	public const int COURTHOUSE_FIRST = 1;
	public const int UNICORN = 2;
	public const int CENTAUR = 3;
	public const int ALLQUESTSDONE = 4;
	public const int PANDA = 5;
	public const int COURTHOUSE_FINAL = 6;
	public const int DIPUTS_QUEST = 7;
	public const int DAVID_QUEST = 8;

	public List<int> progress;
//	public List<string> charactersMet;
//	public List<string> questsDone;

	void Update(){
		if (GameController.Instance.GetGameState () == GameController.GAMESTATE_MAP) {
			mapPlayer.HandleControls ();
		} else if (GameController.Instance.GetGameState () == GameController.GAMESTATE_RANDOMEVENT) {
			// Event controls
		}
	}

	public void AddToInv(string itemName){
		GameController.Instance.GetComponent<Inventory> ().AddToInv (itemName);
	}

	public void AddToProgress(int newProgress){
		progress.Add (newProgress);
		if (newProgress == TUTORIAL) {
			foreach (GameObject o in GameObject.FindGameObjectsWithTag("QuestEventTrigger")) {
				if (o.GetComponent<EventTriggerer> ().storyEventName == "courthouse") {
					o.GetComponent<EventTriggerer> ().MakeAlwaysVisible ();
				}
			}
		} else if (newProgress == COURTHOUSE_FIRST) {
			foreach (GameObject o in GameObject.FindGameObjectsWithTag("QuestEventTrigger")) {
				if (o.GetComponent<EventTriggerer> ().storyEventName == "owl") {
					o.GetComponent<EventTriggerer> ().MakeAlwaysVisible ();
				} else if (o.GetComponent<EventTriggerer> ().storyEventName == "david") {
					o.GetComponent<EventTriggerer> ().MakeAlwaysVisible ();
				}
			}
		} else if (newProgress == UNICORN) {
			foreach (GameObject o in GameObject.FindGameObjectsWithTag("QuestEventTrigger")) {
				if (o.GetComponent<EventTriggerer> ().storyEventName == "unicorn") {
					o.GetComponent<EventTriggerer> ().MakeAlwaysHidden ();
				}
			}
		} else if (newProgress == CENTAUR) {
			foreach (GameObject o in GameObject.FindGameObjectsWithTag("QuestEventTrigger")) {
				if (o.GetComponent<EventTriggerer> ().storyEventName == "centaur") {
					o.GetComponent<EventTriggerer> ().MakeAlwaysHidden ();
				}
			}
		} else if (newProgress == ALLQUESTSDONE) {
			foreach (GameObject o in GameObject.FindGameObjectsWithTag("QuestEventTrigger")) {
				if (o.GetComponent<EventTriggerer> ().storyEventName == "panda") {
					o.GetComponent<EventTriggerer> ().MakeAlwaysVisible ();
				}
			}
		}
	}
}

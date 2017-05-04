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
	}

	MapPlayer mapPlayer;

	public const int TUTORIAL = 0;
	public const int COURTHOUSE_FIRST = 1;
	public const int UNICORN = 2;
	public const int CENTAUR = 3;
	public const int PANDA = 4;
	public const int COURTHOUSE_FINAL = 5;


	public List<int> progress;
//	public List<string> charactersMet;
//	public List<string> questsDone;

	void Start(){
		mapPlayer = gameObject.GetComponent<MapPlayer> ();
//		charactersMet = new List<string> ();
	}

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
}

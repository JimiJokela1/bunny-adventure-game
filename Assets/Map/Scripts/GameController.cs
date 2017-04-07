using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	// Instance for singleton class, meaning there is only one of this object ever
	public static GameController Instance = null;
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	public int gameState;
	public const int GAMESTATE_START = 0;
	public const int GAMESTATE_MAP = 1;
	public const int GAMESTATE_EVENT = 2;
	public const int GAMESTATE_EXIT = 3;

	public string eventTileType;
	GameObject player;

	Button generateButton; // for testing
	Button eventTestButton;

	void Start(){
		generateButton = GameObject.Find ("GenerateButton").GetComponent<Button> ();
		generateButton.onClick.AddListener (()=> onGenerateButtonClick());
		eventTestButton = GameObject.Find ("EventTestButton").GetComponent<Button> ();
		eventTestButton.onClick.AddListener (()=> onEventTestButtonClick());
		player = GameObject.FindGameObjectWithTag ("Player");
		ChangeGameState (GAMESTATE_START);
	}

	public void ChangeGameState (int newGameState)
	{
		int oldGameState = gameState;
		gameState = newGameState;
		switch (gameState) {
			case GAMESTATE_START:
				MapGenerator.Instance.GenerateMap ();
				ChangeGameState (GAMESTATE_MAP);
				break;

			case GAMESTATE_MAP:
				GameObject.FindGameObjectWithTag ("MapCanvas").SetActive (true);
				player.SetActive (true);
				Debug.ClearDeveloperConsole ();
				TileHolder.Instance.gameObject.SetActive (true);
				if (SceneManager.GetActiveScene ().name !=  "tilemap") {
					SceneManager.LoadScene ("tilemap");
				}
				break;

			case GAMESTATE_EVENT:
				if (oldGameState == GAMESTATE_MAP) {
					player.GetComponent<MapPlayer> ().StopMoving ();
					GameObject.FindGameObjectWithTag ("MapCanvas").SetActive (false);
					player.SetActive (false);
				}
				TileHolder.Instance.gameObject.SetActive (false);
					if (SceneManager.GetActiveScene ().name !=  "EventGeneratorScene") {
						SceneManager.LoadScene ("EventGeneratorScene");
					}
				break;

			default:
				break;
		}
	}

	public int GetGameState(){
		return gameState;
	}

	public void ActivateDialogue(){

	}

	public void DeActivateDialogue(){

	}

	void onGenerateButtonClick () {
		MapGenerator.Instance.GenerateMap ();
	}

	void onEventTestButtonClick(){
		gameObject.GetComponentInChildren<EventTriggerer> ().TriggerEvent ();
	}

	public void StartEvent(string tileType){
		eventTileType = tileType;
		ChangeGameState (GameController.GAMESTATE_EVENT);
	}

	public void GetRandomNumber(int min, int max){

	}
}

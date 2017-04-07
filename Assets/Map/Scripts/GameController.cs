using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	int gameState;
	public const int GAMESTATE_START = 0;
	public const int GAMESTATE_MAP = 1;
	public const int GAMESTATE_EVENT = 2;
	public const int GAMESTATE_DIALOGUE = 3;
	public const int GAMESTATE_EXIT = 4;

	void Start(){
		ChangeGameState (GAMESTATE_START);
	}

	public void ChangeGameState (int NewGameState)
	{
		gameState = NewGameState;
		switch (gameState) {
			case GAMESTATE_START:
				MapGenerator.Instance.GenerateMap ();
				break;

			case GAMESTATE_MAP:
				Debug.ClearDeveloperConsole ();
				TileHolder.Instance.gameObject.SetActive (true);
				SceneManager.LoadScene ("tilemap");
				break;

			case GAMESTATE_EVENT:
				TileHolder.Instance.gameObject.SetActive (false);
				SceneManager.LoadScene ("EventGeneratorScene");
				break;

			case GAMESTATE_DIALOGUE:
				break;

			default:
				break;
		}
	}

	public void GetRandomNumber(int min, int max){

	}
}

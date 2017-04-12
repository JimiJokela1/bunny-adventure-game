﻿using System.Collections;
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

	// Game state determines what scene, objects, ui, controls are active... ideally
	public int gameState;
	public const int GAMESTATE_START = 0;
	public const int GAMESTATE_MAP = 1;
	public const int GAMESTATE_EVENT = 2;
	public const int GAMESTATE_EXIT = 3;

	public string eventTileType;
	GameObject player;
	EventTriggerer eventTriggerer;
	string weatherState = "clear";
	Light directionalLight;
	float lightIntensityTarget;
	public float lightChangeRate = 0.3f;
	public bool testStorm = false;

	public List<GameObject> mapCanvasObjects;
	public int oldGameState;
	Button generateButton; // for testing
	Button eventTestButton;
	Button spawnCloudsButton;
	Button returnToMapButton;

	void Start(){
		eventTriggerer = GetComponentInChildren<EventTriggerer> ();

		// TEST BUTTONS
		generateButton = GameObject.Find ("GenerateButton").GetComponent<Button> ();
		generateButton.onClick.AddListener (()=> onGenerateButtonClick());
		eventTestButton = GameObject.Find ("EventTestButton").GetComponent<Button> ();
		eventTestButton.onClick.AddListener (()=> onEventTestButtonClick());
		spawnCloudsButton = GameObject.Find ("SpawnCloudsButton").GetComponent<Button> ();
		spawnCloudsButton.onClick.AddListener (() => onSpawnCloudsButtonClick ());
		returnToMapButton = GameObject.Find ("ReturnToMapButton").GetComponent<Button> ();
		returnToMapButton.onClick.AddListener (() => GetComponent<ReturnToMap>().ToMap());

		directionalLight = GameObject.Find ("Directional Light").GetComponent<Light> ();

		mapCanvasObjects = new List<GameObject> ();
		mapCanvasObjects.Add (generateButton.gameObject);
		mapCanvasObjects.Add (eventTestButton.gameObject);
		mapCanvasObjects.Add (spawnCloudsButton.gameObject);

		player = GameObject.FindGameObjectWithTag ("Player");
		ChangeGameState (GAMESTATE_START);
	}

	void Update(){
		ChangeLight ();
		RandomWeather ();
	}

	public void ChangeGameState (int newGameState)
	{
		oldGameState = gameState;
		gameState = newGameState;
		switch (gameState) {
			case GAMESTATE_START:
				MapGenerator.Instance.GenerateMap ();
				ChangeGameState (GAMESTATE_MAP);
				break;

			case GAMESTATE_MAP:
				returnToMapButton.gameObject.SetActive (false);
				foreach (GameObject o in mapCanvasObjects) {
					o.SetActive (true);
				}
				player.SetActive (true);
				Debug.ClearDeveloperConsole ();
				TileHolder.Instance.gameObject.SetActive (true);
				if (SceneManager.GetActiveScene ().name != "tilemap") {
					SceneManager.LoadScene ("tilemap");
				}

				directionalLight = GameObject.Find ("Directional Light").GetComponent<Light> ();
				break;

			case GAMESTATE_EVENT:
				returnToMapButton.gameObject.SetActive (true);
				eventTileType = eventTriggerer.GetEventTileType ();
				if (oldGameState == GAMESTATE_MAP) {
					player.GetComponent<MapPlayer> ().StopMoving ();
					foreach (GameObject o in mapCanvasObjects) {
						o.SetActive (false);
					}
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

	void onSpawnCloudsButtonClick(){
		MapGenerator.Instance.GetComponent<Clouds> ().DestroyClouds ();
		MapGenerator.Instance.GetComponent<Clouds> ().PlaceClouds ();
	}


	public void GetRandomNumber(int min, int max){

	}

	public void AddItemToInventory(string itemName){
		player.GetComponent<PlayerController> ().AddToInv (itemName);
	}

	void SetWeather(string weather){
		if (weather == "clear") {
			weatherState = weather;
			lightIntensityTarget = 1.35f;
		} else if (weather == "storm") {
			weatherState = weather;
			lightIntensityTarget = 0.1f;
		}
	}

	void ChangeLight(){
		if (weatherState == "clear") {
			if (directionalLight.intensity < lightIntensityTarget) {
				directionalLight.intensity += lightChangeRate * Time.deltaTime;
			}
		} else if (weatherState == "storm") {
			if (directionalLight.intensity > lightIntensityTarget) {
				directionalLight.intensity -= lightChangeRate * Time.deltaTime;
			}
		}
	}

	void RandomWeather(){
		if (testStorm) {
			SetWeather ("storm");
		} else {
			SetWeather ("clear");
		}

//		if (Random.Range (0f, 100f) < (5f * Time.deltaTime)) {
//			if (weatherState == "clear") {
//				SetWeather ("storm");
//			} else if (weatherState == "storm") {
//				SetWeather ("clear");
//			}
//		}
	}
}

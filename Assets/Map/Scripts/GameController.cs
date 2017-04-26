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

	// Game state determines what scene, objects, ui, controls are active... ideally
	public int gameState;
	public int oldGameState;
	public const int GAMESTATE_START = 0;
	public const int GAMESTATE_MAP = 1;
	public const int GAMESTATE_RANDOMEVENT = 2;
	public const int GAMESTATE_STORYEVENT = 3;
	public const int GAMESTATE_EXIT = 4;

	public string eventTileType;
	GameObject player;
	EventTriggerer eventTriggerer;
	ParticleSystem rainParticles;
	public string weatherState = "clear";

	Light directionalLight;
	float lightIntensityTarget;
	public float lightChangeRate = 0.3f;
	float daytimeLightIntensity = 1.25f;
	float nighttimeLightIntensity = 0.2f;
	float stormLightIntensity = 0.6f;
	float clearLightIntensity = 1f;
	public float timeOfDay = 8f;
	public float timeScale = 1f;

	public bool testStorm = false; // for testing
	Button generateButton; // for testing
	Button eventTestButton; // for testing
	Button spawnCloudsButton; // for testing
	Button returnToMapButton;
	Button umbrellaButton;
	Button campButton;
	public List<GameObject> mapCanvasObjects; // List for canvas objects that should be active only on map
	public bool mouseOverButton = false; // tells if mouse pointer is over a button

	void Start(){
//		eventTriggerer = GetComponentInChildren<EventTriggerer> ();

		// TEST BUTTONS
		generateButton = GameObject.Find ("GenerateButton").GetComponent<Button> ();
		generateButton.onClick.AddListener (()=> onGenerateButtonClick());
		eventTestButton = GameObject.Find ("EventTestButton").GetComponent<Button> ();
		eventTestButton.onClick.AddListener (()=> onEventTestButtonClick());
		spawnCloudsButton = GameObject.Find ("SpawnCloudsButton").GetComponent<Button> ();
		spawnCloudsButton.onClick.AddListener (() => onSpawnCloudsButtonClick ());

		// Gameplay buttons
		returnToMapButton = GameObject.Find ("ReturnToMapButton").GetComponent<Button> ();
		returnToMapButton.onClick.AddListener (() => GetComponent<ReturnToMap>().ToMap());
		umbrellaButton = GameObject.Find ("UmbrellaButton").GetComponent<Button> ();
		umbrellaButton.onClick.AddListener (() => onUmbrellaButtonClick ());
		umbrellaButton.gameObject.SetActive (false);
		campButton = GameObject.Find ("CampButton").GetComponent<Button> ();
		campButton.onClick.AddListener (() => onCampButtonClick ());

		directionalLight = GameObject.Find ("Directional Light").GetComponent<Light> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		rainParticles = GameObject.Find ("RainSystem").GetComponent<ParticleSystem> ();
		rainParticles.gameObject.SetActive (false);

		mapCanvasObjects = new List<GameObject> ();
		mapCanvasObjects.Add (generateButton.gameObject);
		mapCanvasObjects.Add (eventTestButton.gameObject);
		mapCanvasObjects.Add (spawnCloudsButton.gameObject);
		mapCanvasObjects.Add (umbrellaButton.gameObject);
		mapCanvasObjects.Add (campButton.gameObject);


		ChangeGameState (GAMESTATE_START);
	}

	void Update(){
		SkyLightUpdate ();
		ChangeLight ();
		RandomWeather ();

		if (gameState == GAMESTATE_MAP) {
			if (player.GetComponent<MapPlayer> ().camping == false) {
				timeOfDay += Time.deltaTime * timeScale;
				if (timeOfDay >= 24f) {
					timeOfDay -= 24f;
				} 
			} else {
				timeOfDay += Time.deltaTime * timeScale * 0.3f;
				if (timeOfDay >= 24f) {
					timeOfDay -= 24f;
				} 
			}
		}
	}

	public void ChangeGameState (int newGameState, EventTriggerer eventTriggerer = null)
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
				SetWeather (weatherState);
				directionalLight.gameObject.SetActive (true);
				player.SetActive (true);
				Debug.ClearDeveloperConsole ();
				TileHolder.Instance.gameObject.SetActive (true);
				EventHolder.Instance.gameObject.SetActive (true);
				if (SceneManager.GetActiveScene ().name != "tilemap") {
					SceneManager.LoadScene ("tilemap");
				}

				break;

			case GAMESTATE_RANDOMEVENT:
				if (eventTriggerer != null) {
					returnToMapButton.gameObject.SetActive (true);
					eventTileType = eventTriggerer.GetEventTileType ();
					if (oldGameState == GAMESTATE_MAP) {
						player.GetComponent<MapPlayer> ().StopMoving ();
						foreach (GameObject o in mapCanvasObjects) {
							o.SetActive (false);
						}
						directionalLight.gameObject.SetActive (false);
						player.SetActive (false);
					}

					TileHolder.Instance.gameObject.SetActive (false);
					EventHolder.Instance.gameObject.SetActive (false);
					if (SceneManager.GetActiveScene ().name != "EventGeneratorScene") {
						SceneManager.LoadScene ("EventGeneratorScene");
					}
				}
				break;

			case GAMESTATE_STORYEVENT:

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
//		gameObject.GetComponentInChildren<EventTriggerer> ().TriggerRandomEvent ();
	}

	void onSpawnCloudsButtonClick(){
		MapGenerator.Instance.GetComponent<Clouds> ().DestroyClouds ();
		MapGenerator.Instance.GetComponent<Clouds> ().PlaceClouds ();
	}

	void onUmbrellaButtonClick(){
		player.GetComponent<MapPlayer> ().umbrella = true;
	}

	void onCampButtonClick(){
		if (player.GetComponent<MapPlayer> ().camping == false) {
			player.GetComponent<MapPlayer> ().camping = true;
			player.GetComponent<MapPlayer> ().StopMoving ();
			campButton.gameObject.GetComponentInChildren<Text> ().text = "Pack up";
			player.GetComponent<Camp> ().SetUpCamp ();
		} else if (player.GetComponent<MapPlayer> ().camping == true) {
			player.GetComponent<MapPlayer> ().camping = false;
			campButton.gameObject.GetComponentInChildren<Text> ().text = "Set up camp";
			player.GetComponent<Camp> ().PackUpCamp ();
		}
	}

	public void AddItemToInventory(string itemName){
		player.GetComponent<PlayerController> ().AddToInv (itemName);
	}

	void SetWeather(string weather){
		if (weather == "clear") {
			weatherState = weather;
			umbrellaButton.gameObject.SetActive (false);
			rainParticles.gameObject.SetActive (false);
			player.GetComponent<MapPlayer> ().umbrella = false;
			MapGenerator.Instance.GetComponent<Clouds> ().RemoveStormClouds ();
		} else if (weather == "storm") {
			weatherState = weather;
			umbrellaButton.gameObject.SetActive (true);
			rainParticles.gameObject.SetActive (true);
			MapGenerator.Instance.GetComponent<Clouds> ().AddStormClouds (100);
		}
	}

	void SkyLightUpdate(){
		if (timeOfDay < 12f) {
			lightIntensityTarget = Mathf.Lerp(nighttimeLightIntensity, daytimeLightIntensity, timeOfDay / 12f);
		} else if (timeOfDay > 12f) {
			lightIntensityTarget = Mathf.Lerp(daytimeLightIntensity, nighttimeLightIntensity, (timeOfDay - 12f) / 12f);
		}

		if (weatherState == "clear") {
			lightIntensityTarget = lightIntensityTarget * clearLightIntensity;
		} else if (weatherState == "storm") {
			lightIntensityTarget = lightIntensityTarget * stormLightIntensity;
		}
	}

	void ChangeLight(){
		if (directionalLight != null) {
			if (directionalLight.intensity < lightIntensityTarget) {
				directionalLight.intensity += lightChangeRate * Time.deltaTime;
			}
			if (directionalLight.intensity > lightIntensityTarget) {
				directionalLight.intensity -= lightChangeRate * Time.deltaTime;
			}
		}
	}

	void RandomWeather(){
		if (testStorm && weatherState != "storm") {
			SetWeather ("storm");
		} else if (!testStorm && weatherState != "clear") {
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

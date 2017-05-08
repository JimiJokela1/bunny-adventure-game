using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

	// Game state determines what scene, objects, ui, controls are active
	public int gameState;
	public int oldGameState;
	public const int GAMESTATE_START = 0;
	public const int GAMESTATE_MAP = 1;
	public const int GAMESTATE_RANDOMEVENT = 2;
	public const int GAMESTATE_STORYEVENT = 3;
	public const int GAMESTATE_EXIT = 4;

	float gameStateChangeTimer = 0f;
	float gameStateChangeTime = 1f;
	bool changingGameState = false;

	public string eventTileType;
	GameObject player;
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
	bool timePaused = true;
	public float randomStormChance = 5f;

	float stormTimer = 0f;
	float stormTime = 5f;

	public bool testStorm = false; // for testing
	Button generateButton; // for testing
	Button eventTestButton; // for testing
	Button spawnCloudsButton; // for testing
	Button smoothMapButton; // for testing
	Button saveButton; // works I guess
	Button quitButton;
	Button returnToMapButton;
	Button umbrellaButton;
	Button campButton;
	InputField console;
	Dropdown commandDropdown;

	public List<GameObject> mapCanvasObjects; // List for canvas objects that should be active only on map
	public bool mouseOverButton = false; // tells if mouse pointer is over a button

	string fileName;

	void Start(){
		fileName = Application.persistentDataPath + "/savedata.dat";

//		eventTriggerer = GetComponentInChildren<EventTriggerer> ();

		// TEST BUTTONS
		generateButton = GameObject.Find ("GenerateButton").GetComponent<Button> ();
		generateButton.onClick.AddListener (()=> onGenerateButtonClick());
		eventTestButton = GameObject.Find ("EventTestButton").GetComponent<Button> ();
		eventTestButton.onClick.AddListener (()=> onEventTestButtonClick());
		spawnCloudsButton = GameObject.Find ("SpawnCloudsButton").GetComponent<Button> ();
		spawnCloudsButton.onClick.AddListener (() => onSpawnCloudsButtonClick ());
		smoothMapButton = GameObject.Find ("SmoothMapButton").GetComponent<Button> ();
		smoothMapButton.onClick.AddListener (() => MapGenerator.Instance.Smoothing ());

		saveButton = GameObject.Find ("SaveButton").GetComponent<Button> ();
		saveButton.onClick.AddListener (() => SaveGame ());
		quitButton = GameObject.Find ("QuitButton").GetComponent<Button> ();
		quitButton.onClick.AddListener (() => QuitGame ());
		console = GameObject.Find ("Console").GetComponent<InputField> ();
		console.onEndEdit.AddListener ((value) => ConsoleInput(value));
		console.gameObject.SetActive (false);

//		commandDropdown = GameObject.Find ("CommandDropdown").GetComponent<Dropdown> ();
//		commandDropdown.onValueChanged.AddListener (delegate {
//			CommandDropdownHandler(commandDropdown);
//		});


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
		mapCanvasObjects.Add (umbrellaButton.gameObject);
		mapCanvasObjects.Add (campButton.gameObject);
		mapCanvasObjects.Add (saveButton.gameObject);

//		mapCanvasObjects.Add (generateButton.gameObject);
//		mapCanvasObjects.Add (eventTestButton.gameObject);
//		mapCanvasObjects.Add (spawnCloudsButton.gameObject);
//		mapCanvasObjects.Add (smoothMapButton.gameObject);

		generateButton.gameObject.SetActive (false);
		spawnCloudsButton.gameObject.SetActive (false);
		smoothMapButton.gameObject.SetActive (false);
		eventTestButton.gameObject.SetActive (false);

		ChangeGameState (GAMESTATE_START);
	}

	void Update(){
		if (changingGameState) {
			if (gameStateChangeTimer < gameStateChangeTime) {
				gameStateChangeTimer += Time.deltaTime;
			} else {
				gameStateChangeTimer = 0f;
				changingGameState = false;
				ChangeGameState (GAMESTATE_STORYEVENT, scene: "scene_beginning");
			}
		} else {
			HandleConsole ();

			SkyLightUpdate ();
			ChangeLight ();
			RandomWeather ();

			if (gameState == GAMESTATE_MAP && !timePaused) {
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
	}

	/// <summary>
	/// Changes the state of the game.
	/// </summary>
	/// <param name="newGameState">New game state: see constants.</param>
	/// <param name="scene">Scene to change to, used for console commands only.</param>
	/// <param name="eventTriggerer">If an EventTriggerer is changing the game state.</param>
	public void ChangeGameState (int newGameState, string scene = null, EventTriggerer eventTriggerer = null)
	{
		oldGameState = gameState;
		gameState = newGameState;
		switch (gameState) {
			case GAMESTATE_START:
				if (!LoadGame ()) {
					MapGenerator.Instance.GenerateMap ();
					changingGameState = true;
				}

				ChangeGameState (GAMESTATE_MAP);

				break;

			case GAMESTATE_MAP:
				// Reactivate stuff
				Debug.Log ("Active scene: " + SceneManager.GetActiveScene ().name);
				if (SceneManager.GetActiveScene ().name != "scene_tilemap") {
					Debug.Log ("Loading scene: scene_tilemap");
					SceneManager.LoadScene ("scene_tilemap");
				}
				returnToMapButton.gameObject.SetActive (false);
				foreach (GameObject o in mapCanvasObjects) {
					o.SetActive (true);
				}
				SetWeather (weatherState);
				directionalLight.gameObject.SetActive (true);
				TileHolder.Instance.gameObject.SetActive (true);
				EventHolder.Instance.gameObject.SetActive (true);
				player.SetActive (true);
				player.GetComponent<MapPlayer> ().StopMoving ();
				player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				foreach (EventTriggerer eve in QuestEventHolder.Instance.GetComponentsInChildren<EventTriggerer>()) {
					eve.ShowAfterEvent ();
				}
				ResumeTime ();
				break;

			case GAMESTATE_RANDOMEVENT:
				console.gameObject.SetActive (false);
				if (eventTriggerer != null) {
					eventTileType = eventTriggerer.GetEventTileType ();
				}
				returnToMapButton.gameObject.SetActive (true);
				if (oldGameState == GAMESTATE_MAP) {
					HideMapStuff ();
				}
				if (SceneManager.GetActiveScene ().name != "scene_random") {
					Debug.Log ("Loading scene: " + scene);
					SceneManager.LoadScene ("scene_random");
				}
				break;

			case GAMESTATE_STORYEVENT:
				
				console.gameObject.SetActive (false);

				if (eventTriggerer != null) { // if call came from an event triggerer on the map
					string eventName = eventTriggerer.storyEventName;
					// if changing from map, disable map stuff that will not be destroyed, but need to be hidden
					if (oldGameState == GAMESTATE_MAP) {
						HideMapStuff ();
					}
					returnToMapButton.gameObject.SetActive (true);

					if (eventName == "owl") {
						if (GetComponent<Inventory>().GetInv().FindAll(FindUnicornDust).Count == 8){
							eventName += "2";
						}
					}
					if (eventName == "david") {
						if (GetComponent<Inventory> ().GetInv ().Contains ("davids armor")) {
							eventName += "2";
						}
					}
					if (eventName == "courthouse") {
						if (PlayerController.Instance.progress.Contains (PlayerController.PANDA)) {
							eventName += "2";
						}
					}
					Debug.Log ("Loading scene: scene_" + eventName);
					SceneManager.LoadScene ("scene_" + eventName);

				} else if (scene != null) { // if call came from console command
					// if changing to tilemap or random event, there's a special case in ChangeGameState()
					if (scene == "scene_tilemap") {
						ChangeGameState (GAMESTATE_MAP);
						break;
					} else if (scene == "scene_random") {
						ChangeGameState (GAMESTATE_RANDOMEVENT);
						break;
					}

					// if changing from map, disable map stuff that will not be destroyed, but need to be hidden
					if (oldGameState == GAMESTATE_MAP) {
						HideMapStuff ();
					}

					returnToMapButton.gameObject.SetActive (true);
					Debug.Log ("Loading scene: " + scene);
					SceneManager.LoadScene (scene);
				}
				break;
				
			default:
				break;
		}
	}

	/// <summary>
	/// Hides map stuff.
	/// </summary>
	void HideMapStuff(){
		player.GetComponent<MapPlayer> ().StopMoving ();
		foreach (GameObject o in mapCanvasObjects) {
			o.SetActive (false);
		}
		directionalLight.gameObject.SetActive (false);
		player.SetActive (false);
		TileHolder.Instance.gameObject.SetActive (false);
		EventHolder.Instance.gameObject.SetActive (false);
		foreach (EventTriggerer eve in QuestEventHolder.Instance.GetComponentsInChildren<EventTriggerer>()) {
			eve.HideDuringEvent ();
		}
	}

	/// <summary>
	/// Activates and deactivates the command console.
	/// </summary>
	void HandleConsole() {
		if (Input.GetKeyDown (KeyCode.K) && !console.gameObject.activeSelf) {
			console.gameObject.SetActive (true);
			console.interactable = true;
			console.ActivateInputField ();
			if (gameState == GAMESTATE_RANDOMEVENT || gameState == GAMESTATE_STORYEVENT) {
				GameObject.Find ("EventPlayer").GetComponent<EventPlayer> ().canMove = false;
			}
		} else if (Input.GetKeyDown (KeyCode.K) && console.gameObject.activeSelf && !console.isFocused) {
			console.gameObject.SetActive (false);
			if (gameState == GAMESTATE_RANDOMEVENT || gameState == GAMESTATE_STORYEVENT) {
				GameObject.Find ("EventPlayer").GetComponent<EventPlayer> ().canMove = false;
			}
		}
	}

	void CommandDropdownHandler(Dropdown dropdown){
		Debug.Log (dropdown.value);
	}

	/// <summary>
	/// Handles console input.
	/// </summary>
	/// <param name="command">Command. Name of scene loads that scene: beginning, courthouse, owl, david, unicorn, centaur, etc.</param>
	public void ConsoleInput(string command) {
		Debug.Log ("Console: " + command);

		switch (command) {
			case "test":
				Debug.Log ("test command");
				break;
			case "skipscene":
				if (SceneManager.GetActiveScene ().name != "scene_tilemap") {
					switch (SceneManager.GetActiveScene ().name) {
						case "scene_beginning":
							PlayerController.Instance.AddToProgress (PlayerController.TUTORIAL);
							break;
						case "scene_owl":
							PlayerController.Instance.AddToProgress (PlayerController.DIPUTS_QUEST);
							break;
						case "scene_owl2":
							PlayerController.Instance.AddToProgress (PlayerController.UNICORN);
							break;
						case "scene_david":
							PlayerController.Instance.AddToProgress (PlayerController.DAVID_QUEST);
							break;
						case "scene_david2":
							PlayerController.Instance.AddToProgress (PlayerController.CENTAUR);
							break;
						case "scene_courthouse":
							PlayerController.Instance.AddToProgress (PlayerController.COURTHOUSE_FIRST);
							break;
						case "scene_panda":
							PlayerController.Instance.AddToProgress (PlayerController.PANDA);
							break;
						case "scene_courthousefinal":
							PlayerController.Instance.AddToProgress (PlayerController.COURTHOUSE_FINAL);
							break;
						default:
							break;
					}
					ChangeGameState (GAMESTATE_MAP);
				}
				break;
			default:
				if (SceneManager.GetActiveScene ().name != "scene_" + command && SceneListCheck.Has ("scene_" + command)) {
					ChangeGameState (GAMESTATE_STORYEVENT, "scene_" + command);
				} else {
					Debug.Log ("Invalid scene name: \"scene_" + command + "\"");
				}
				break;
		}
		console.ActivateInputField ();
	}

	/// <summary>
	/// Gets the state of the game.
	/// </summary>
	/// <returns>The game state.</returns>
	public int GetGameState(){
		return gameState;
	}

	/// <summary>
	/// Explicit predicate delegate to find item "Unicorn Dust".
	/// </summary>
	/// <returns><c>true</c>, if unicorn dust was found, <c>false</c> otherwise.</returns>
	/// <param name="itemName">Item name.</param>
	private static bool FindUnicornDust(string itemName){
		if (itemName == "Unicorn Dust") {
			return true;
		} else {
			return false;
		}
	}

	void onGenerateButtonClick () {
		MapGenerator.Instance.GenerateMap ();
	}

	void onEventTestButtonClick(){
		GameObject.FindGameObjectWithTag("EventTrigger").GetComponentInChildren<EventTriggerer> ().TestTriggerEvent ();
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
//			campButton.gameObject.GetComponentInChildren<Text> ().text = "Pack up";
			player.GetComponent<Camp> ().SetUpCamp ();
		} else if (player.GetComponent<MapPlayer> ().camping == true) {
			player.GetComponent<MapPlayer> ().camping = false;
//			campButton.gameObject.GetComponentInChildren<Text> ().text = "Set up camp";
			player.GetComponent<Camp> ().PackUpCamp ();
		}
	}

	/// <summary>
	/// Sets the weather.
	/// </summary>
	/// <param name="weather">A weather state value: "clear" or "storm".</param>
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
			stormTime = Random.Range (5f, 15f);
		}
	}

	/// <summary>
	/// Updates skylight intensity target value based on time of day and weather.
	/// </summary>
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

	/// <summary>
	/// Changes the skylight intensity each frame at a speed set by lightChangeRate.
	/// </summary>
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

	/// <summary>
	/// Sets the weather to storm randomly.
	/// </summary>
	void RandomWeather(){
//		if (testStorm && weatherState != "storm") {
//			SetWeather ("storm");
//		} else if (!testStorm && weatherState != "clear") {
//			SetWeather ("clear");
//		}

		if (Random.Range (0f, 100f) < (randomStormChance * Time.deltaTime)) {
			if (weatherState == "clear") {
				SetWeather ("storm");
			} 
		}
		if (weatherState == "storm" && stormTimer < stormTime) {
			stormTimer += Time.deltaTime * timeScale;
		} else if (weatherState == "storm") {
			SetWeather ("clear");
			stormTimer = 0f;
		}

	}

	public void StopTime(){
		timePaused = true;
	}

	public void ResumeTime(){
		timePaused = false;
	}

	// If saving data as strings
//	void SaveGame(){
//		SaveData saveData = new SaveData ();
//		saveData.progress = player.GetComponent<PlayerController> ().progress;
//		saveData.charactersMet = player.GetComponent<PlayerController> ().charactersMet;
//
//		for(int w = 0; w < MapGenerator.Instance.tilemap.GetLength(0); w++){
//			for(int h = 0; h < MapGenerator.Instance.tilemap.GetLength(1); h++){
//				saveData.tileTypes[w * 100 + h] = MapGenerator.Instance.tilemap[w, h].tag;
//			}
//		}
//
//		saveData.playerPosition[0] = player.transform.position.x;
//		saveData.playerPosition[1] = player.transform.position.y;
//		saveData.playerPosition[2] = player.transform.position.z;
//		saveData.inventory = GetComponent<Inventory> ().GetInv ();
//
//		int lineCount = 104;
//		string[] saveDataLines = new string[lineCount];
//
//		for (int i = 0; i < saveData.tileTypes.Length; i++) {
//			int line = i / 100;
//			saveDataLines [line] += saveData.tileTypes [i];
//			if (i % 100 != 99) {
//				saveDataLines [line] += ",";
//			}
//		}
//		saveDataLines [101] = "";
//		foreach (string word in saveData.progress) {
//			saveDataLines [101] += word + ";";
//		}
//		saveDataLines [101] += "*";
//		foreach (string word in saveData.charactersMet) {
//			saveDataLines [101] += word + ";";
//		}
//		saveDataLines [101] += "*";
//		foreach (string word in saveData.inventory) {
//			saveDataLines [101] += word + ";";
//		}
//		saveDataLines [101] += "*";
//		for (int i = 0; i < saveData.playerPosition.Length; i++) {
//			saveDataLines [101] += saveData.playerPosition [i] + ";";
//		}
//		saveDataLines [101] += "*";
//		File.WriteAllLines (fileName, saveDataLines);
//	}

	// If loading data as strings
//	bool LoadGame(){
//		if (File.Exists (fileName)) {
//			SaveData saveData = new SaveData ();
//			string[] saveDataLines = File.ReadAllLines (fileName);
//			player.GetComponent<PlayerController> ().progress = saveData.progress;
//			player.GetComponent<PlayerController> ().charactersMet = saveData.charactersMet;
//
//			for (int i = 0; i < saveDataLines.Length - 1; i++) {
//				Debug.Log ("Lines: " + saveDataLines.Length);
//				char[] separators = {','};
//				string[] split = saveDataLines[i].Split (separators);
//				Debug.Log ("Line: " + i + ", Split lenght: " + split.Length);
//				if (split.Length == 100) {
//					for (int tile = 0; tile < split.Length; tile++) {
//						if (i == 99) {
//							Debug.Log ("tile: " + tile);
//						}
//						saveData.tileTypes [tile + i * 100] = split [tile];
//					}
//				} else {
//					char[] separatorsLists = {'*'};
//					string[] splitLists = saveDataLines[i].Split (separators);
//					for (int list = 0; list < splitLists.Length; list++) {
//						char[] separatorsWords = {';'};
//						string[] splitWords = splitLists[list].Split (separators);
//						for (int word = 0; word < splitWords.Length; word++) {
//							Debug.Log (splitWords [word] + " word: " + word);
//							if (splitWords [word] != "") {
//								if (list == 0) {
//									saveData.progress.Add (splitWords [word]);
//								} else if (list == 1) {
//									saveData.charactersMet.Add (splitWords [word]);
//								} else if (list == 2) {
//									saveData.inventory.Add (splitWords [word]);
//								} else if (list == 3) {
//									float.TryParse (splitWords [word], out saveData.playerPosition [word]);
//									Debug.Log (saveData.playerPosition [word]);
//								}
//							}
//						}
//					}
//				}
//			}
//
//			MapGenerator.Instance.LoadMap(saveData.tileTypes);
//
//
//			Vector3 pos = new Vector3(saveData.playerPosition[0], saveData.playerPosition[1], saveData.playerPosition[2]);
//			player.transform.position = pos;
//			GetComponent<Inventory> ().LoadInv (saveData.inventory);
//			return true;
//		} else {
//			return false;
//		}
//	}

	/// <summary>
	/// Saves the game.
	/// </summary>
	void SaveGame(){
		try {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/savedata.dat", FileMode.OpenOrCreate);

			SaveData saveData = new SaveData ();
			saveData.progress = player.GetComponent<PlayerController> ().progress;

			for(int w = 0; w < MapGenerator.Instance.tilemap.GetLength(0); w++){
				for(int h = 0; h < MapGenerator.Instance.tilemap.GetLength(1); h++){
					saveData.tileTypes[w * 100 + h] = MapGenerator.Instance.tilemap[w, h].tag;
				}
			}

			foreach(EventTriggerer eventTriggerer in QuestEventHolder.Instance.GetComponentsInChildren<EventTriggerer>()){
				float[] pos = new []{eventTriggerer.transform.position.x, eventTriggerer.transform.position.y, eventTriggerer.transform.position.z};
				saveData.storyEventLocations.Add(eventTriggerer.storyEventName, pos);
			}

			saveData.playerPosition[0] = player.transform.position.x;
			saveData.playerPosition[1] = player.transform.position.y;
			saveData.playerPosition[2] = player.transform.position.z;
			saveData.inventory = GetComponent<Inventory> ().GetInv ();

			bf.Serialize (file, saveData);
			file.Close ();
		} catch (FileLoadException e) {
			Debug.Log (e.ToString ());
		}
	}

	/// <summary>
	/// Loads saved game.
	/// </summary>
	/// <returns><c>true</c>, if game was loaded, <c>false</c> otherwise.</returns>
	bool LoadGame(){
		try {
			if (File.Exists (Application.persistentDataPath + "/savedata.dat")) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/savedata.dat", FileMode.Open);
				file.Position = 0;
				SaveData saveData = (SaveData)bf.Deserialize (file);
				file.Close ();

				player.GetComponent<PlayerController> ().progress = saveData.progress;

				MapGenerator.Instance.LoadMap(saveData.tileTypes, saveData.storyEventLocations);


				Vector3 pos = new Vector3(saveData.playerPosition[0], saveData.playerPosition[1], saveData.playerPosition[2]);
				player.transform.position = pos;
				foreach(string item in saveData.inventory){
					Debug.Log(item);
				}
				GetComponent<Inventory> ().LoadInv (saveData.inventory);
				return true;
			} else {
				return false;
			}
		} catch (FileLoadException e) {
			Debug.Log (e.ToString ());
			return false;
		}
	}

	void QuitGame(){
		Application.Quit ();
	}
}

/// <summary>
/// Class that stores save data including map tile tags, player position, inventory by serializing data with binary formatter.
/// </summary>
[System.Serializable]
class SaveData
{
	public List<int> progress;
	public string[] tileTypes;
	public float[] playerPosition;
	public List<string> inventory;
	public Dictionary<string, float[]> storyEventLocations;

	public SaveData(){
		progress = new List<int> ();
		inventory = new List<string> ();
		playerPosition = new float[3];
		storyEventLocations = new Dictionary<string, float[]> ();
		tileTypes = new string[MapGenerator.Instance.levelSize * MapGenerator.Instance.levelSize];
	}
}

// If saving data as strings
//class SaveData
//{
//	public string[] tileTypes;
//	public List<string> progress;
//	public List<string> charactersMet;
//	public float[] playerPosition;
//	public List<string> inventory;
//
//	public SaveData(){
//		progress = new List<string> ();
//		charactersMet = new List<string> ();
//		inventory = new List<string> ();
//		playerPosition = new float[3];
//		tileTypes = new string[MapGenerator.Instance.levelSize * MapGenerator.Instance.levelSize];
//	}
//}

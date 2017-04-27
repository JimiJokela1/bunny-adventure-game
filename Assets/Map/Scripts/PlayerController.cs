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

//	List<string> inventory;
//	GameObject inventoryBackground;
//	GameObject inventoryItemPrefab;

	List<string> progress; // ??
	List<string> charactersMet;

	void Start(){
		mapPlayer = gameObject.GetComponent<MapPlayer> ();
//		inventory = new List<string> ();
		charactersMet = new List<string> ();
//		inventoryBackground = GameObject.Find ("InventoryBackground");
//		inventoryBackground.SetActive (false);
//		inventoryItemPrefab = Resources.Load ("Prefabs/InventoryItem") as GameObject;
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
//
//	public List<string> GetInv(){
//		return inventory;
//	}
//
//	public void ShowInv(){
//		inventoryBackground.SetActive (true);
//		Vector2 location = new Vector2(50, -100);
//		foreach (string item in inventory) {
//			GameObject tempItem = Instantiate (inventoryItemPrefab, inventoryBackground.transform);
//			tempItem.GetComponent<RectTransform> ().localScale = Vector3.one;
//			tempItem.GetComponent<RectTransform> ().localPosition = Vector3.zero;
//			tempItem.GetComponent<RectTransform> ().anchoredPosition = location;
////			tempItem.transform.SetParent(inventoryBackground.transform);
//
//			location += new Vector2 (150, 0);
//			if (location.x > inventoryBackground.GetComponent<RectTransform>().rect.width - 100) {
//				location = new Vector2 (50, location.y - 100);
//			}
//		}
//	}
}

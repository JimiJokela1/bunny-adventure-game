using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	List<string> inventory;
	GameObject inventoryBackground;
	GameObject inventoryItemPrefab;
	List<GameObject> invItems;

	void Awake(){
		inventory = new List<string> ();
		inventoryBackground = GameObject.Find ("InventoryBackground");
		inventoryBackground.SetActive (false);
		inventoryItemPrefab = Resources.Load ("Prefabs/InventoryItem") as GameObject;
		invItems = new List<GameObject> ();
	}

	void Start () {
//		AddToInv ("item");
//		AddToInv ("item");
//		AddToInv ("item");
//		AddToInv ("item");
//		AddToInv ("item");
//		AddToInv ("item");
//		AddToInv ("item");
	}

	public void AddToInv(string itemName){
		inventory.Add (itemName);
	}

	/// <summary>
	/// Gets the inventory.
	/// </summary>
	/// <returns>The inventory List<string>.</returns>
	public List<string> GetInv(){
		return inventory;
	}

	/// <summary>
	/// Loads the inventory from a save file.
	/// </summary>
	/// <param name="inv">Inv.</param>
	public void LoadInv(List<string> inv){
		foreach (string item in inv) {
			if (item != null) {
				AddToInv (item);
			}
		}
	}

	/// <summary>
	/// Shows the inventory and lists the items.
	/// </summary>
	public void ShowInv(){
		inventoryBackground.SetActive (true);
		Vector2 location = new Vector2(50, -100);
		foreach (string item in inventory) {
			GameObject tempItem = Instantiate (inventoryItemPrefab, inventoryBackground.transform);
			tempItem.GetComponent<RectTransform> ().localScale = Vector3.one;
			tempItem.GetComponent<RectTransform> ().localPosition = Vector3.zero;
			tempItem.GetComponent<RectTransform> ().anchoredPosition = location;
			tempItem.GetComponent<Text> ().text = item;
			//			tempItem.transform.SetParent(inventoryBackground.transform);

			location += new Vector2 (250, 0);
			if (location.x > inventoryBackground.GetComponent<RectTransform>().rect.width - 100) {
				location = new Vector2 (50, location.y - 100);
			}
			invItems.Add (tempItem);
		}
	}

	/// <summary>
	/// Hides the inventory.
	/// </summary>
	public void HideInv(){
		foreach (GameObject invItem in invItems) {
			Destroy (invItem);
		}
		invItems.Clear ();
		inventoryBackground.SetActive (false);
	}
}

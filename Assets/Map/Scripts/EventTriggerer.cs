using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventTriggerer : MonoBehaviour {

	string tileType;

	GameObject player;

	public Button testButton;

	float randomEventChance = 1f;
	int tileMask;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		tileMask = LayerMask.GetMask ("TileMask");

	}


	void Update () {
//		if (Random.Range (0f, 100f) < randomEventChance) {
//			RaycastHit hit;
//			if (Physics.Raycast (player.transform.position, Vector3.down, out hit, 100f, tileMask)) {
//				tileType = hit.collider.tag;
//				GameController.Instance.ChangeGameState (GameController.GAMESTATE_EVENT);
//			}
//		}
	}

	public void test() {
		RaycastHit hit;
		if (Physics.Raycast (player.transform.position, Vector3.down, out hit, 100f, tileMask)) {
			tileType = hit.collider.tag;
			GameController.Instance.ChangeGameState (GameController.GAMESTATE_EVENT);

		}
	}
}

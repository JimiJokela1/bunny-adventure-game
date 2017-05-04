using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		Debug.Log ("EXIT TRIGGER");
		GameController.Instance.ChangeGameState (GameController.GAMESTATE_MAP);
	}
}

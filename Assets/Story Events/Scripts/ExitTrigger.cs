using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{

	void OnTriggerEnter (Collider other)
	{
		Debug.Log ("EXIT TRIGGER");
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

		GameController.Instance.ChangeGameState (GameController.GAMESTATE_MAP);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReturnToMap : MonoBehaviour {

	public void ToMap(){
		GameController.Instance.ChangeGameState (GameController.GAMESTATE_MAP);

	}
}

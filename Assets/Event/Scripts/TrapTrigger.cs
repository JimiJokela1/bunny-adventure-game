using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour {

	EventPlayer eventPlayer;

	void Start(){
		eventPlayer = GameObject.Find ("EventPlayer").GetComponent<EventPlayer>();
	}

	void OnTriggerEnter(){
		eventPlayer.FallInTrap ();
	}
}

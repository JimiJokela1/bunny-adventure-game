using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour {

	public GameObject player;
	public Camera eventCamera;
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		

	public void TeleportPlayer(Vector3 position) {
		player.transform.position = position;
	}

}

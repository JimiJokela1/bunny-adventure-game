﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls movement on map
public class MapPlayer : MonoBehaviour{

	Rigidbody rigidBody;
	bool controlCooldown = false;
	float controlCooldownTimer;
	float controlCooldownTime = 0.5f;
	public bool moving = false;

	public float movementSpeed = 1f;
	float originalMovementSpeed;
	float slowMovementSpeed;
	public bool umbrella = false;
	Vector3 randomAcceleration;
	Vector3 movement = Vector3.zero;
	Vector3 finalDestination;

	float derailTimer = 0;
	float randomDerailTime = 0;

	int tileMask;
	string tileType;
	GameObject flag;
	GameObject destinationFlag;

	void Start () {
		originalMovementSpeed = movementSpeed;
		slowMovementSpeed = originalMovementSpeed / 2f;
		flag = Resources.Load ("Prefabs/flag") as GameObject;
		rigidBody = GetComponent<Rigidbody> ();
		tileMask = LayerMask.GetMask ("TileMask");
		InvokeRepeating ("CheckTileUnder", 0, 0.5f);
	}

	void Update () {
		// Timer so one click doesn't register many times
		if (controlCooldown) {
			if (controlCooldownTimer < controlCooldownTime) {
				controlCooldownTimer += Time.deltaTime;
			} else {
				controlCooldown = false;
				controlCooldownTimer = 0f;
			}
		}
		UpdateSpeed ();
	}

	void FixedUpdate () {
		if (GameController.Instance.GetGameState () == GameController.GAMESTATE_MAP) {
			Moving ();
		}
	}

	// Check if clicked on map
	public void HandleControls () {
		if (Input.GetAxis ("Action1") > 0.1f && GameController.Instance.mouseOverButton == false) {
			if (controlCooldown == false && moving == false) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 100f, LayerMask.GetMask ("FloorLayer"))) {
					Debug.Log ("generating path");
					Vector3 destination = hit.point;
					GeneratePath (destination);
					controlCooldown = true;
				}
			} else if (controlCooldown == false && moving == true) {
				StopMoving ();
				controlCooldown = true;
			}
		}
	}

	// Start moving
	void GeneratePath (Vector3 destination){
		randomAcceleration = Vector3.ClampMagnitude(new Vector3 (Random.Range (-1f, 1f), 0, Random.Range (-1f, 1f)), 1);
		finalDestination = destination;
		moving = true;
		randomDerailTime = Random.Range (0.1f, 1f);
		destinationFlag = Instantiate (flag, finalDestination, Quaternion.identity);
	}

	void Move (Vector3 movement) {
		rigidBody.MovePosition (transform.position + movement);
	}

	public void StopMoving(){
		moving = false;
		if (destinationFlag != null) {
			Destroy (destinationFlag);
		}
		Debug.Log ("stopped");
	}

	// Movement
	void Moving() {
		if (moving) {
			Move (movement);

			movement += Vector3.ClampMagnitude (finalDestination - transform.position, movementSpeed * Time.fixedDeltaTime / 8);
			movement += Vector3.ClampMagnitude (randomAcceleration, movementSpeed * Time.fixedDeltaTime / 10);
			movement = Vector3.ClampMagnitude (movement, movementSpeed * Time.fixedDeltaTime);

			if (derailTimer < randomDerailTime) {
				derailTimer += Time.fixedDeltaTime;
			} else {
				randomAcceleration = Vector3.ClampMagnitude (new Vector3 (Random.Range (-1f, 1f), 0, Random.Range (-1f, 1f)), 1);
				derailTimer = 0;
				randomDerailTime = Random.Range (0.2f, 0.6f);
			}

			if (Vector3.Distance(transform.position, finalDestination) < 0.6f){
				StopMoving ();
			}
		}
	}

	void CheckTileUnder(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit, 100f, tileMask)) {
			tileType = hit.collider.tag;
			if (tileType == "tile_mountain") {
				movementSpeed = slowMovementSpeed;
			} else if (tileType == "tile_water") {
				movementSpeed = slowMovementSpeed / 2f;
			} else {
				movementSpeed = originalMovementSpeed;
			}
		}
	}

	void UpdateSpeed(){
		if (GameController.Instance.weatherState == "storm" && umbrella == false) {
			movementSpeed = slowMovementSpeed / 2f;
		} else if (tileType == "tile_mountain") {
			movementSpeed = slowMovementSpeed;
		} else if (tileType == "tile_water") {
			movementSpeed = slowMovementSpeed / 2f;
		} else {
			movementSpeed = originalMovementSpeed;
		}
	}


//	//------------------------------------------------------------
//	// OOOOOOLD
//	float progress = 0;
//	float distanceToTarget;
//	int pathDetail;
//	public float stepLenght;
//	Vector3 stepDestination;
//	Vector3 firstMidDestination;
//	Vector3 secondMidDestination;
//	//	Vector3 startLocation;
//
//	void Moving () {
//		if (moving) {
//			Move (stepDestination);
//
//			if (progress < (distanceToTarget / 3)) {
//				stepDestination = Vector3.ClampMagnitude (Vector3.RotateTowards (stepDestination, firstMidDestination - transform.position, Mathf.PI / pathDetail * 5, 0f) * 100, stepLenght);
//			} else if (progress >= (distanceToTarget / 3) && progress < (distanceToTarget * 2 / 3)) {
//				stepDestination = Vector3.ClampMagnitude (Vector3.RotateTowards (stepDestination, secondMidDestination - transform.position, Mathf.PI / pathDetail * 5, 0f) * 100, stepLenght);
//			} else {
//				stepDestination = Vector3.ClampMagnitude (Vector3.RotateTowards (stepDestination, finalDestination - transform.position, Mathf.PI / pathDetail * 5, 0f) * 100, stepLenght);
//			}
//			if (Vector3.Distance (transform.position, finalDestination) < 0.6f) {
//				moving = false;
//				Debug.Log ("stopped");
//			}
//			progress += stepLenght;
//
//			//			Debug.Log ("step");
//		}
//	}
//
//	void GeneratePath (Vector3 destination) {
//		distanceToTarget = Vector3.Distance (transform.position, destination);
//		pathDetail = (int)(distanceToTarget * 100);
//		stepLenght = 0.05f;
//		firstMidDestination = Vector3.RotateTowards (Vector3.ClampMagnitude (destination, distanceToTarget / 3), RandomDir (), Mathf.PI / distanceToTarget, 0f);
//		secondMidDestination = Vector3.RotateTowards (Vector3.ClampMagnitude (destination, distanceToTarget * 2 / 3), RandomDir (), Mathf.PI / distanceToTarget, 0f);
//		finalDestination = destination;
//		stepDestination = Vector3.ClampMagnitude (Vector3.RotateTowards (destination, firstMidDestination, Mathf.PI / pathDetail * 5, 0f), stepLenght);
//		progress = 0;
//		startLocation = transform.position;
//		moving = true;
//	}
//
//	void GeneratePath2(Vector3 destination) {
//		distanceToTarget = Vector3.Distance (transform.position, destination);
//		pathDetail = (int)(distanceToTarget * 100);
//		stepLenght = 0.05f;
//		Vector3 travel = destination - transform.position;
//		firstMidDestination = Vector3.ClampMagnitude (Quaternion.AngleAxis (RandomPlusOrMinus() * 30f, Vector3.up) * travel, distanceToTarget / 3);
//		secondMidDestination = Vector3.ClampMagnitude (Quaternion.AngleAxis (RandomPlusOrMinus() * 15f, Vector3.up) * travel, distanceToTarget * 2 / 3);
//		finalDestination = destination;
//		stepDestination = Vector3.ClampMagnitude (Vector3.RotateTowards (travel, firstMidDestination, Mathf.PI / pathDetail * 5, 0f), stepLenght);
//		progress = 0;
//		//		startLocation = transform.position;
//		moving = true;
//
//	}
//
//	int RandomPlusOrMinus(){
//		int random = Random.Range (0, 2);
//
//		if (random == 0) {
//			return -1;
//		} else {
//			return 1;
//		}
//	}
//
//	// return left or right
//	Vector3 RandomDir () {
//		int dir = Random.Range (0, 2);
//		if (dir == 0) {
//			return Vector3.left;
//		} else {
//			return Vector3.right;
//		}
//	}
}

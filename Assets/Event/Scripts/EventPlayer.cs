using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlayer : MonoBehaviour {

	Rigidbody rigidBody;
	bool controlCooldown = false;
	float controlCooldownTimer;
	float controlCooldownTime = 0.5f;
	public bool moving = false;
	bool jumping = false;

	float horizontalMove;
	float verticalMove;

	public float speed = 3f;
	public float jumpStrenght = 3f;

	Vector3 movement;

	int floorMask;

	void Start(){
		rigidBody = GetComponent<Rigidbody> ();
		floorMask = LayerMask.GetMask ("EventFloorMask");
	}

	void Update(){
		horizontalMove = Input.GetAxis ("Horizontal");
		verticalMove = Input.GetAxis ("Vertical");
	}

	void FixedUpdate(){
//		Vector3 move = new Vector3 (horizontalMove * Time.fixedDeltaTime, 0, verticalMove * Time.fixedDeltaTime);
//		move = move * speed;
//		if (jumping == false) {
//			rigidBody.AddForce (move, ForceMode.Impulse);
//		} else {
//			rigidBody.AddForce (move * 0.2f, ForceMode.Impulse);
//		}
//		move = move.normalized;
//		transform.forward.Set (move.x, move.y, move.z);
//		Quaternion.
//		rigidBody.MoveRotation(
		Move();
		Turning ();
		if (controlCooldown == true) {
			if (controlCooldownTimer > controlCooldownTime) {
				controlCooldown = false;
				controlCooldownTimer = 0f;
			} else {
				controlCooldownTimer += Time.fixedDeltaTime;
			}
		}
//		Explode ();
		LedgeJump ();
//		Jump ();
		if (transform.position.y < -5f) {
			rigidBody.velocity = Vector3.zero;
			rigidBody.angularVelocity = Vector3.zero;
			rigidBody.MoveRotation (Quaternion.identity);
			transform.position = new Vector3 (0f, 1.1f, 0f);
		}
	}

	// Move player according to keyboard input
	void Move() {
		movement.Set (horizontalMove, 0f, verticalMove);
		movement = movement.normalized * speed * Time.fixedDeltaTime;

		rigidBody.MovePosition (transform.position + movement);
		if (movement != Vector3.zero) {
			Quaternion newRotation = Quaternion.LookRotation (movement);
			rigidBody.MoveRotation (newRotation);
		}
	}

	// Turn player object towards mouse position
	void Turning(){ 
		if (movement == Vector3.zero) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

			RaycastHit floorHit;

			if (Physics.Raycast (camRay, out floorHit, 100f, floorMask)) {
				Vector3 playerToMouse = floorHit.point - transform.position;

				playerToMouse.y = 0f;

				Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
				rigidBody.MoveRotation (newRotation);
			}
		}
	}

	void Jump(){
		if (Physics.Raycast (transform.position, Vector3.down, transform.localScale.y * 0.7f)) {
			if (Input.GetAxis ("Jump") > 0.1f && controlCooldown == false) {

				rigidBody.AddForce (jumpStrenght * Vector3.up, ForceMode.Impulse);
				controlCooldown = true;
				jumping = true;
			} else {
				jumping = false;
			}
		} else {
			jumping = true;
		}
	}

	void LedgeJump(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.forward, out hit,  1f)) {
			if (Input.GetAxis ("Jump") > 0.1f && controlCooldown == false) {
				if (hit.collider.transform.position.y + hit.collider.transform.localScale.y / 2 - transform.position.y < 3f) {
					rigidBody.AddForce (jumpStrenght * Vector3.up + Vector3.forward, ForceMode.Impulse);
					controlCooldown = true;
				}
			}
		}
	}

	void Explode(){
		if (Input.GetAxis ("Jump") > 0.1f && controlCooldown == false) {
			foreach (GameObject o in GameObject.Find("EventGenerator2").GetComponent<EventGenerator2>().assetList) {
				if (o.GetComponent<Rigidbody> ()) {
					o.GetComponent<Rigidbody> ().AddExplosionForce (300f, transform.position, 50f);
				}
			}
		}
	}
}

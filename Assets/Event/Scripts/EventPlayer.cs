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

	void Start(){
		rigidBody = GetComponent<Rigidbody> ();

	}

	void Update(){
		horizontalMove = Input.GetAxis ("Horizontal");
		verticalMove = Input.GetAxis ("Vertical");
	}

	void FixedUpdate(){
		Vector3 move = new Vector3 (horizontalMove * Time.fixedDeltaTime, 0, verticalMove * Time.fixedDeltaTime);
		move = move * speed;
		if (jumping == false) {
			rigidBody.AddForce (move, ForceMode.Impulse);
		} else {
			rigidBody.AddForce (move * 0.2f, ForceMode.Impulse);
		}
//		move = move.normalized;
//		transform.forward.Set (move.x, move.y, move.z);
//		Quaternion.
//		rigidBody.MoveRotation(
		if (controlCooldown == true) {
			if (controlCooldownTimer > controlCooldownTime) {
				controlCooldown = false;
				controlCooldownTimer = 0f;
			} else {
				controlCooldownTimer += Time.fixedDeltaTime;
			}
		}
//		LedgeJump ();
		Jump ();
		if (transform.position.y < -5f) {
			rigidBody.velocity = Vector3.zero;
			rigidBody.angularVelocity = Vector3.zero;
			transform.rotation.eulerAngles.Set (0f, 0f, 0f);
			transform.position = new Vector3 (0f, 1.1f, 0f);
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
}

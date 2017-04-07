using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlayer : MonoBehaviour {

	Rigidbody rigidBody;
	bool controlCooldown = false;
	float controlCooldownTimer;
	float controlCooldownTime = 1.0f;
	public bool moving = false;

	float horizontalMove;
	float verticalMove;

	public float speed = 3f;

	void Start(){
		rigidBody = GetComponent<Rigidbody> ();

	}

	void FixedUpdate(){
		horizontalMove = Input.GetAxis ("Horizontal");
		verticalMove = Input.GetAxis ("Vertical");

		Vector3 move = new Vector3 (horizontalMove * Time.fixedDeltaTime, 0, verticalMove * Time.fixedDeltaTime);
		move = move * speed;
		rigidBody.AddForce (move, ForceMode.Impulse);
	}

}

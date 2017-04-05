using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private Transform target;
	public float smoothing = 5f; // Camera follow speed

	Vector3 offset;

	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		offset = transform.position - target.position; // Calculate initial offset
	}

	void FixedUpdate () {
		Vector3 targetCamPos = target.position + offset;

		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Transform target;
	public float smoothing = 5f; // Camera follow speed
	string mode;

	Vector3 followOffset;
	Vector3 eventZoomOffset;
	Vector3 targetCamPos;

	void Start () {
		mode = "Follow";
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		followOffset = transform.position - target.position; // Calculate initial offset
		eventZoomOffset = followOffset / 10;

		transform.position = target.position + followOffset;
	}

	void FixedUpdate () {
		if (mode == "Follow") {
			targetCamPos = target.position + followOffset;
		}
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);
	} 

	public void EventZoom(){
		mode = "EventZoom";
		targetCamPos = target.position + eventZoomOffset;
	}
}

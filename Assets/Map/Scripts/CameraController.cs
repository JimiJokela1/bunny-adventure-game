using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Transform target;
	public float smoothing = 5f; // Camera follow speed
	string mode;

	static Vector3 followOffset = Vector3.zero;
	Vector3 eventZoomOffset;
	Vector3 targetCamPos;

	void Awake () {
		mode = "Follow";
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		if (followOffset == Vector3.zero) {
			followOffset = transform.position - target.position; // Calculate initial offset
		}
		eventZoomOffset = followOffset / 10;

		transform.position = target.position + followOffset;
	}

	void FixedUpdate () {
		// Check if camera is orthographic or perspective and move it accordingly, specifically zoom is different
		if (!GetComponent<Camera> ().orthographic) {
			if (mode == "Follow") {
				targetCamPos = target.position + followOffset;
			}
			transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);
		} else {
			if (mode == "Follow") {
				targetCamPos = target.position + followOffset;
				transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);
			} else {
				if (GetComponent<Camera> ().orthographicSize > 1.5) {
					GetComponent<Camera> ().orthographicSize += -Time.fixedDeltaTime * 10f;
				}
			}
		}
	} 

	public void EventZoom(){
		mode = "EventZoom";
		targetCamPos = target.position + eventZoomOffset;
	}
}

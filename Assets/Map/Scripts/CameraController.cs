using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Transform target;
	public float smoothing = 5f; // Camera follow speed
	public float zoomSpeed = 10f;
	string mode;

	static Vector3 followOffset = Vector3.zero;
	Vector3 eventZoomOffset;
	Vector3 campZoomOffset;
	Vector3 targetCamPos;

	void Awake () {
		mode = "Follow";
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		if (followOffset == Vector3.zero) {
			followOffset = transform.position - target.position; // Calculate initial offset
		}
		eventZoomOffset = followOffset / 10f;
		campZoomOffset = followOffset / 6f;

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
				if (GetComponent<Camera> ().orthographicSize < 10f) {
					GetComponent<Camera> ().orthographicSize += Time.fixedDeltaTime * zoomSpeed;
				}
				targetCamPos = target.position + followOffset;
				transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);
			} else if (mode == "EventZoom") {
				if (GetComponent<Camera> ().orthographicSize > 1.5f) {
					GetComponent<Camera> ().orthographicSize -= Time.fixedDeltaTime * zoomSpeed;
				}
			} else if (mode == "CampZoom") {
				if (GetComponent<Camera> ().orthographicSize > 4.5f) {
					GetComponent<Camera> ().orthographicSize -= Time.fixedDeltaTime * zoomSpeed;
				}
			}
		}
	} 

	public void EventZoom(){
		mode = "EventZoom";
		targetCamPos = target.position + eventZoomOffset;
	}

	public void CampZoom(){
		mode = "CampZoom";
		targetCamPos = target.position + campZoomOffset;
	}

	public void Unzoom(){
		mode = "Follow";
	}
}

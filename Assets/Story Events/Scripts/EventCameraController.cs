using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCameraController : MonoBehaviour {	
	private Transform target;
	public float smoothing = 5f; // Camera follow speed
	public float zoomSpeed = 10f;
	float fadeSpeed = 15f;
	static Vector3 followOffset = Vector3.zero;
	Vector3 eventZoomOffset;
	Vector3 campZoomOffset;
	Vector3 targetCamPos;
	string mode;
	void Awake () {
		target = GameObject.Find ("EventPlayer").transform;
		if (followOffset == Vector3.zero) {
			followOffset = transform.position - target.position; // Calculate initial offset
		}
		eventZoomOffset = followOffset / 10f;
		campZoomOffset = followOffset / 6f;
		mode = "FadeIn";
		GetComponent<Camera> ().nearClipPlane = 50;
		transform.position = target.position + followOffset;
	}
	void FixedUpdate () {
		if (mode == "FadeIn") {
			if (GetComponent<Camera> ().nearClipPlane > 0) {
				GetComponent<Camera> ().nearClipPlane -= Time.fixedDeltaTime * fadeSpeed;
			} else {
				mode = "Follow";
			}
		} else if (mode == "Follow") {
			targetCamPos = target.position + followOffset;
			transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);
		}
	}
	public void ZoomIn () {
		GetComponent<Camera> ().orthographicSize = 4;
	}
	public void ZoomOut () {
		GetComponent<Camera> ().orthographicSize = 6;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Transform target;
	public float smoothing = 5f; // Camera follow speed
	public float zoomSpeed = 10f;
	string mode;

	static Vector3 followOffset = Vector3.zero;
	public Vector3 eventZoomOffset;
	public Vector3 campZoomOffset;
	Vector3 targetCamPos;
	float eventZoomCameraSize;
	float campZoomCameraSize;

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
				if (GetComponent<Camera> ().orthographicSize > eventZoomCameraSize) {
					GetComponent<Camera> ().orthographicSize -= Time.fixedDeltaTime * zoomSpeed;
				}
			} else if (mode == "CampZoom") {
				if (GetComponent<Camera> ().orthographicSize > campZoomCameraSize) {
					GetComponent<Camera> ().orthographicSize -= Time.fixedDeltaTime * zoomSpeed;
				}
			}
		}
	} 


	/// <summary>
	/// Zooms down to triggered event.
	/// </summary>
	/// <param name="offset">Offset from player to zoom to in perspective mode.</param>
	/// <param name="cameraSize">Camera size to zoom to in orthographic mode.</param>
	public void EventZoom(Vector3 offset, float cameraSize = 1.5f){
		mode = "EventZoom";
		targetCamPos = target.position + offset;
		eventZoomCameraSize = cameraSize;
	}

	/// <summary>
	/// Zooms down to triggered event.
	/// </summary>
	/// <param name="offset">Offset from player to zoom to in perspective mode.</param>
	/// <param name="cameraSize">Camera size to zoom to in orthographic mode.</param>
	public void EventZoom(float cameraSize = 1.5f){
		mode = "EventZoom";
		targetCamPos = target.position + eventZoomOffset;
		eventZoomCameraSize = cameraSize;
	}

	/// <summary>
	/// Zooms down to camp.
	/// </summary>
	/// <param name="offset">Offset from player to zoom to in perspective mode.</param>
	/// <param name="cameraSize">Camera size to zoom to in orthographic mode.</param>
	public void CampZoom(Vector3 offset, float cameraSize = 4.5f){
		mode = "CampZoom";
		targetCamPos = target.position + offset;
		campZoomCameraSize = cameraSize;
	}

	/// <summary>
	/// Zooms down to camp.
	/// </summary>
	/// <param name="offset">Offset from player to zoom to in perspective mode.</param>
	/// <param name="cameraSize">Camera size to zoom to in orthographic mode.</param>
	public void CampZoom(float cameraSize = 4.5f){
		mode = "CampZoom";
		targetCamPos = target.position + campZoomOffset;
		campZoomCameraSize = cameraSize;
	}

	public void Unzoom(){
		mode = "Follow";
	}
}

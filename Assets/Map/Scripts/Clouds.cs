﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour {

	public int cloudAmount = 100;
	GameObject cloud1;
	GameObject cloud2;
	GameObject cloud3;
	GameObject cloud4;
	GameObject cloud5;

	GameObject[] cloudTypes;
	Transform tileHolder;

	List<GameObject> clouds;
	public List<GameObject> stormClouds;
	Vector3[] winds;
	public int levelSize;

	public bool fadingStormCloudsIn = false;
	public bool fadingStormCloudsOut = false;
	float cloudFadeSpeed = 1f;
	public float stormCloudAlpha = 0f;

	void Awake(){
		tileHolder = GameObject.FindGameObjectWithTag ("TileHolder").transform;

		cloud1 = Resources.Load ("Prefabs/cloud1") as GameObject;
		cloud2 = Resources.Load ("Prefabs/cloud2") as GameObject;
		cloud3 = Resources.Load ("Prefabs/cloud3") as GameObject;
		cloud4 = Resources.Load ("Prefabs/cloud4") as GameObject;
		cloud5 = Resources.Load ("Prefabs/cloud5") as GameObject;

		cloudTypes = new GameObject[5];
		cloudTypes [0] = cloud1;
		cloudTypes [1] = cloud2;
		cloudTypes [2] = cloud3;
		cloudTypes [3] = cloud4;
		cloudTypes [4] = cloud5;

		clouds = new List<GameObject> ();
		stormClouds = new List<GameObject> ();
		winds = new Vector3[10];
		for(int wind = 0; wind < winds.Length; wind++){
			winds[wind] = new Vector3 (Random.Range(0.3f, 1.5f), 0, Random.Range(-0.3f, -1.5f));
		}
		levelSize = MapGenerator.Instance.levelSize / 2;
	}

	void Update(){
		if (GameController.Instance.GetGameState () == GameController.GAMESTATE_MAP) {
			MoveClouds ();
			ResetClouds ();

			foreach (GameObject stormCloud in stormClouds) {
				if (stormCloud != null) {
					stormCloud.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, stormCloudAlpha);
				}
			}

			if (stormCloudAlpha > 0f && fadingStormCloudsOut) {
				stormCloudAlpha -= cloudFadeSpeed * Time.deltaTime;
			}
			if (stormCloudAlpha < 1f && fadingStormCloudsIn) {
				stormCloudAlpha += cloudFadeSpeed * Time.deltaTime;
			}
			if (stormCloudAlpha <= 0f && fadingStormCloudsOut == true) {
				fadingStormCloudsOut = false;
				stormClouds.Clear ();
			}
			if (stormCloudAlpha >= 1f && fadingStormCloudsIn) {
				fadingStormCloudsIn = false;
			}
		}
	}

	/// <summary>
	/// Places the initial clouds.
	/// </summary>
	public void PlaceClouds(){
		for (int i = 0; i < cloudAmount; i++) {
			Vector3 location = new Vector3 (Random.Range (-levelSize, levelSize), 10, Random.Range (-levelSize, levelSize));
			GameObject tempCloud = Instantiate (cloudTypes [Random.Range (0, cloudTypes.Length)], location, Quaternion.Euler (90, Random.Range(0, 360), 0), tileHolder);
			clouds.Add (tempCloud);
		}
	}

	/// <summary>
	/// Destroys all of the clouds.
	/// </summary>
	public void DestroyClouds(){
		foreach (GameObject cloud in clouds) {
			Destroy (cloud);
		}
		foreach (GameObject cloud in stormClouds) {
			Destroy (cloud);
		}
		clouds.Clear ();
		stormClouds.Clear ();
	}

	/// <summary>
	/// Moves the clouds with one of the random winds vector3s.
	/// </summary>
	void MoveClouds(){
		foreach (GameObject cloud in clouds) {
			cloud.transform.position += winds[Random.Range(0, winds.Length)] * Time.deltaTime * GameController.Instance.timeScale;
		}
		foreach (GameObject cloud in stormClouds) {
			cloud.transform.position += winds[Random.Range(0, winds.Length)] * Time.deltaTime * GameController.Instance.timeScale;
		}
	}

	/// <summary>
	/// Resets the clouds that reach the end of the map to the other side of the map.
	/// </summary>
	void ResetClouds(){
		foreach (GameObject cloud in clouds) {
			if (cloud.transform.position.x > levelSize || cloud.transform.position.z < -levelSize) {
				Vector3 pos = cloud.transform.position;
				pos = new Vector3 (-pos.x, pos.y, -pos.z);
				cloud.transform.position = pos;
			}
		}
		foreach (GameObject cloud in stormClouds) {
//			if (cloud.transform.position.x > player.transform.position.x + 30 || cloud.transform.position.z < player.transform.position.z - 30) {
//				Vector3 pos = cloud.transform.position;
//				pos = new Vector3 (-pos.x, pos.y, -pos.z);
//				cloud.transform.position = pos;
//			}
			if (cloud.transform.position.x > levelSize || cloud.transform.position.z < -levelSize) {
				Vector3 pos = cloud.transform.position;
				pos = new Vector3 (-pos.x, pos.y, -pos.z);
				cloud.transform.position = pos;
			}
		}
	}

	/// <summary>
	/// Adds more clouds.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void AddClouds (int amount){
		for (int i = 0; i < amount; i++) {
			int randomSide = Random.Range (0, 2);
			Vector3 location;
			if (randomSide == 0) {
				location = new Vector3 (Random.Range (-levelSize, levelSize), 10, levelSize);
			} else {
				location = new Vector3 (-levelSize, 10, Random.Range (-levelSize, levelSize));
			}
			GameObject tempCloud = Instantiate (cloudTypes [Random.Range (0, cloudTypes.Length)], location, Quaternion.Euler (90, Random.Range(0, 360), 0), tileHolder);
			clouds.Add (tempCloud);
		}
	}

	/// <summary>
	/// Adds storm clouds.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void AddStormClouds (int amount){
		for (int i = 0; i < amount; i++) {
//			Vector3 location = new Vector3 (Random.Range (-30, 30), 10, Random.Range (-30, 30));
//			location = player.transform.position + location;
			Vector3 location = new Vector3(Random.Range(-levelSize, levelSize), 10, Random.Range(-levelSize, levelSize));

			GameObject tempCloud = Instantiate (cloudTypes [Random.Range (0, cloudTypes.Length)], location, Quaternion.Euler (90, Random.Range(0, 360), 0), tileHolder);
			stormClouds.Add (tempCloud);
			tempCloud.GetComponent<SpriteRenderer> ().color -= new Color (0, 0, 0, 255);
		}
		fadingStormCloudsIn = true;
	}

	/// <summary>
	/// Starts fading out storm clouds.
	/// </summary>
	public void RemoveStormClouds(){
		fadingStormCloudsOut = true;
	}
}

using System.Collections;
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
	Vector3[] winds;
	public int levelSize;

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
		}
	}

	public void PlaceClouds(){
		
		for (int i = 0; i < cloudAmount; i++) {
			Vector3 location = new Vector3 (Random.Range (-levelSize, levelSize), 10, Random.Range (-levelSize, levelSize));
			GameObject tempCloud = Instantiate (cloudTypes [Random.Range (0, cloudTypes.Length)], location, Quaternion.Euler (90, Random.Range(0, 360), 0), tileHolder);
			clouds.Add (tempCloud);
		}
	}

	public void DestroyClouds(){
		foreach (GameObject cloud in clouds) {
			Destroy (cloud);
		}
		clouds.Clear ();
	}

	void MoveClouds(){
		foreach (GameObject cloud in clouds) {
			cloud.transform.position += winds[Random.Range(0, winds.Length)] * Time.deltaTime;
		}
	}

	void ResetClouds(){
		foreach (GameObject cloud in clouds) {
			if (cloud.transform.position.x > levelSize || cloud.transform.position.z < -levelSize) {
				Vector3 pos = cloud.transform.position;
				pos = new Vector3 (-pos.x, pos.y, -pos.z);
				cloud.transform.position = pos;
			}
		}
	}
}

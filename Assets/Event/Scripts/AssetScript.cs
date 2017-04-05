using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetScript : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
		Debug.Log ("osu johonki");
		if (col.gameObject.tag == "asset") {
			Debug.Log ("osu");
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NPC : MonoBehaviour {

	public Text textField;
	public Image imageField;
	private string name;

	public void GenerateNPC(string name, Sprite sprite, Text text) {
		this.name = name;
		this.imageField.sprite = sprite;
		this.textField.text = "SADSAD";

	}


}

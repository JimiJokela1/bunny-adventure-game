using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NPC : MonoBehaviour {

	public CanvasGroup canvasGroup;
	public Text textField;
	public Image imageField;
	private string name;

	public void GenerateNPC(string name, string text) {
		name = name;
		imageField.sprite = Resources.Load<Sprite>("velho");
		textField.text = "text";

	}

	public void hideUI() {
		canvasGroup.alpha = 0f;
	}

	public void showUI() {
		canvasGroup.alpha = 1f;
	}

	public void setFields() {
		textField.text = "ULIULI";
	}


}

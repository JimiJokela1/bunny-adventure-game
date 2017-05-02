using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NPC : MonoBehaviour {

	CanvasGroup canvasGroup;
	Text textField;
	Image imageField;
	public string name;
	TextAsset dialogueText;
	public string[] dialogueLines;


	void Start() {
		textField = GameObject.Find("TextFieldNPC").GetComponent<Text>();
		imageField = GameObject.Find("ImageFieldNPC").GetComponent<Image>();

	}



	public void GenerateNPC(string name) {
		this.name = name;
		textField.text = name;
		imageField.sprite = Resources.Load<Sprite> (name);
		dialogueText = Resources.Load<TextAsset> (name + "Dialogue");
		//TESTIÄ
		dialogueLines = (dialogueText.text.Split ('\n'));
		textField.text = dialogueLines [0];
	}


	public void hideUI() {
		canvasGroup.alpha = 0f;
	}

	public void showUI() {
		canvasGroup.alpha = 1f;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueController : MonoBehaviour {

	Text textFieldNPC;
	Image imageFieldNPC;
	Text textFieldPlayer;
	Image imageFieldPlayer;
	TextAsset playerText;
	TextAsset npcText;
	public string [] dialogueNPC;
	public string [] dialoguePlayer;
	bool playerOrNPC = false; //If true, player speaks 
	bool isDialogueOn = false; //If true, clicking anywhere makes the dialogue go forward
	int lineNumberNPC = 0;
	int lineNumberPlayer = 0;
	public Button ButtonDOption1;
	public Button ButtonDOption2;
	public string npcName;

	void Start(){
		textFieldNPC = GameObject.Find("TextFieldNPC").GetComponent<Text>();
		imageFieldNPC = GameObject.Find("ImageFieldNPC").GetComponent<Image>();
		textFieldPlayer = GameObject.Find("TextFieldPlayer").GetComponent<Text>();
		imageFieldPlayer = GameObject.Find("ImageFieldPlayer").GetComponent<Image>();
		npcName = GameObject.Find ("NPC").GetComponent<NPC> ().name;
		ButtonDOption1 = (Button)GameObject.Find ("ButtonDOption1").GetComponent<Button> ();
		ButtonDOption2 = (Button)GameObject.Find ("ButtonDOption2").GetComponent<Button> ();
		//ButtonDOption1.gameObject.SetActive(false);
		//ButtonDOption2.gameObject.SetActive (false);
	}

	void Update(){
		Debug.Log ("upadate");
		if (Input.GetMouseButtonDown (0) && isDialogueOn == true) {
			ShowDialogue ();
			Debug.Log ("Pressed");
		}

	}
	/// <summary>
	/// Shows the dialogue.
	/// </summary>
	public void ShowDialogue () {
		if (playerOrNPC == false && lineNumberNPC < dialogueNPC.Length) {
			textFieldNPC.text = dialogueNPC [lineNumberNPC];
			lineNumberNPC++;
			playerOrNPC = true;
		} else if (lineNumberNPC == dialogueNPC.Length && lineNumberPlayer == dialoguePlayer.Length) {
			textFieldPlayer.text = "";
			textFieldNPC.text = "";
		}
		else if (lineNumberPlayer < dialoguePlayer.Length){
			textFieldPlayer.text = dialoguePlayer [lineNumberPlayer];
			lineNumberPlayer++;
			playerOrNPC = false;
		}
	}

	public void ShowButtons (){
		
	}
	/// <summary>
	/// Destroyes both buttons and loads dialogue 1
	/// </summary>
	public void DestroyButtonOne(){
		//Sets both buttons unactive
		ButtonDOption1.gameObject.SetActive(false);
		ButtonDOption2.gameObject.SetActive (false);
		//dialogueOneOrTwo = true;
		npcText = Resources.Load<TextAsset> ("velhoDialogue1");
		dialogueNPC = (npcText.text.Split ('\n'));
		playerText = Resources.Load<TextAsset> ("velhoPlayerDialogue1");
		dialoguePlayer = (playerText.text.Split ('\n'));;
		isDialogueOn = true;
		textFieldPlayer.text = dialoguePlayer [0];
		lineNumberPlayer++;
	}
	/// <summary>
	/// Destroyes both buttons and loads dialogue 2 
	/// </summary>
	public void DestroyButtonTwo(){
		ButtonDOption2.gameObject.SetActive(false);
		ButtonDOption1.gameObject.SetActive (false);
		//dialogueOneOrTwo = false;
		npcText = Resources.Load<TextAsset> ("velhoDialogue2");
		dialogueNPC = (npcText.text.Split ('\n'));
		playerText = Resources.Load<TextAsset> ("velhoPlayerDialogue2");
		dialoguePlayer = (playerText.text.Split ('\n'));
		isDialogueOn = true;
		textFieldPlayer.text = dialoguePlayer [0];
		lineNumberPlayer++;
	}
}
	


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueController : MonoBehaviour
{

	public EventPlayer eventPlayer;
	Text textFieldNPC;
	Image imageFieldNPC;
	Text textFieldPlayer;
	Image imageFieldPlayer;
	TextAsset playerText;
	TextAsset npcText;
	public string[] dialogueNPC;
	public string[] dialoguePlayer;
	bool playerTurn = false;
	//If true, player speaks
	bool isDialogueOn = false;
	//If true, clicking anywhere makes the dialogue go forward
	int lineNumberNPC = 0;
	int lineNumberPlayer = 0;
	public Button ButtonDOption1;
	public Button ButtonDOption2;
	private string activeNPC;
	TextAsset options;
	string[] optionsArray;


	void Start ()
	{
		textFieldNPC = GameObject.Find ("TextFieldNPC").GetComponent<Text> ();
		imageFieldNPC = GameObject.Find ("ImageFieldNPC").GetComponent<Image> ();
		textFieldPlayer = GameObject.Find ("TextFieldPlayer").GetComponent<Text> ();
		imageFieldPlayer = GameObject.Find ("ImageFieldPlayer").GetComponent<Image> ();
		ButtonDOption1 = (Button)GameObject.Find ("ButtonDOption1").GetComponent<Button> ();
		ButtonDOption2 = (Button)GameObject.Find ("ButtonDOption2").GetComponent<Button> ();
		//ButtonDOption1.GetComponentInChildren<Text>().text = "Scream";
		//ButtonDOption2.GetComponentInChildren<Text>().text = "Hide";
		//ButtonDOption1.gameObject.SetActive(false);
		//ButtonDOption2.gameObject.SetActive (false);
		HideUI ();
	}

	void Update ()
	{
		//Debug.Log ("upadate");
		if (Input.GetMouseButtonDown (0) && isDialogueOn == true) {
			ProgressDialogue ();
			Debug.Log ("Pressed");
		}

	}



	public void DialogueTriggered (string name)
	{
		activeNPC = name;
		lineNumberNPC = 0;
		lineNumberPlayer = 0;
		Debug.Log (name + "dialogue");
		eventPlayer.canMove = false;
		ShowUI ();
		imageFieldPlayer.sprite = Resources.Load<Sprite> ("bunny");
		imageFieldNPC.sprite = Resources.Load<Sprite> (name);
		npcText = Resources.Load<TextAsset> (name + "_npc");
		dialogueNPC = (npcText.text.Split ('\n'));
		playerText = Resources.Load<TextAsset> (name + "_player");
		dialoguePlayer = (playerText.text.Split ('\n'));
		isDialogueOn = true;
		playerTurn = true;
	}

	public void OptionTriggered (string name)
	{
		lineNumberNPC = 0;
		lineNumberPlayer = 0;
		eventPlayer.canMove = false;
		npcText = Resources.Load<TextAsset> (name + "_npc");
		dialogueNPC = (npcText.text.Split ('\n'));
		playerText = Resources.Load<TextAsset> (name + "_player");
		dialoguePlayer = (playerText.text.Split ('\n'));
		isDialogueOn = true;
		playerTurn = true;

	}


	/// <summary>
	/// Shows the dialogue.
	/// </summary>
	public void ProgressDialogue ()
	{
		
		if (lineNumberPlayer < dialoguePlayer.Length) {
			Debug.Log (lineNumberPlayer + "   " + dialoguePlayer [lineNumberPlayer]);
			if (dialoguePlayer [lineNumberPlayer].Equals("!")) {
				Debug.Log ("option trigger");
				options = Resources.Load<TextAsset> (activeNPC + "_options");
				optionsArray = (options.text.Split ('\n'));
				ButtonDOption1.GetComponentInChildren<Text> ().text = optionsArray [0];
				ButtonDOption2.GetComponentInChildren<Text> ().text = optionsArray [1];
				ShowButtons ();

			}
		}
		//NPC PUHUU
		if (playerTurn == false && lineNumberNPC < dialogueNPC.Length) {
			//Debug.Log (lineNumberNPC + "   " + dialogueNPC [lineNumberNPC]);
			textFieldPlayer.text = "";
			textFieldNPC.text = dialogueNPC [lineNumberNPC];
			lineNumberNPC++;
			if (lineNumberPlayer < dialoguePlayer.Length) {
				playerTurn = true;
			}

		} else if (lineNumberNPC == dialogueNPC.Length && lineNumberPlayer == dialoguePlayer.Length) { //DIALOGI LOPPUU
			textFieldPlayer.text = "";
			textFieldNPC.text = "";
			imageFieldNPC.gameObject.SetActive (false);
			imageFieldPlayer.gameObject.SetActive (false);
			isDialogueOn = false;
			eventPlayer.canMove = true;

		} else if (lineNumberPlayer < dialoguePlayer.Length) {  //PELAAJA PUHUU
			//Debug.Log (lineNumberPlayer + "   " + dialoguePlayer [lineNumberPlayer]);
			textFieldNPC.text = "";
			textFieldPlayer.text = dialoguePlayer [lineNumberPlayer];
			lineNumberPlayer++;
			if (lineNumberNPC < dialogueNPC.Length) {
				playerTurn = false;
			}
		

		}

	}


	public void HideUI ()
	{
		ButtonDOption1.gameObject.SetActive (false);
		ButtonDOption2.gameObject.SetActive (false);
		imageFieldNPC.gameObject.SetActive (false);
		imageFieldPlayer.gameObject.SetActive (false);

	}

	public void ShowUI ()
	{
		imageFieldNPC.gameObject.SetActive (true);
		imageFieldPlayer.gameObject.SetActive (true);
	}
	//hide option buttons
	public void HideButtons ()
	{
		ButtonDOption1.gameObject.SetActive (false);
		ButtonDOption2.gameObject.SetActive (false);
	}
	//show option buttons
	public void ShowButtons ()
	{
		ButtonDOption1.gameObject.SetActive (true);
		ButtonDOption2.gameObject.SetActive (true);
	}

	public void OptionA ()
	{
		Debug.Log ("kutsu " + activeNPC + "a");
		OptionTriggered (activeNPC + "a");
		HideButtons ();
	}

	public void OptionB ()
	{
		Debug.Log ("kutsu " + activeNPC + "b");
		OptionTriggered (activeNPC + "b");
		HideButtons ();
	}

	/// <summary>
	/// Destroyes both buttons and loads dialogue 1
	/// </summary>
	/*
	public void DestroyButtonOne(){
		//Sets both buttons unactive
		ButtonDOption1.gameObject.SetActive(false);
		ButtonDOption2.gameObject.SetActive (false);
		//dialogueOneOrTwo = true;
		npcText = Resources.Load<TextAsset> ("velhoDialogue1");
		dialogueNPC = (npcText.text.Split ('\n'));
		playerText = Resources.Load<TextAsset> ("velhoPlayerDialogue1");
		dialoguePlayer = (playerText.text.Split ('\n'));
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
	*/
}
	


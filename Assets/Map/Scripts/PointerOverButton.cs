using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerOverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public void OnPointerEnter(PointerEventData eventData){
		GameController.Instance.mouseOverButton = true;
	}

	public void OnPointerExit(PointerEventData eventData){
		GameController.Instance.mouseOverButton = false;
	}
}

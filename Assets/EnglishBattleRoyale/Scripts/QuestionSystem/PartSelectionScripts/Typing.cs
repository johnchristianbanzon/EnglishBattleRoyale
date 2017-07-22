using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Typing : MonoBehaviour, ISelection
{
	public void DeploySelectionType(string questionAnswer){
		gameObject.SetActive (true);
	}

	public void ShuffleSelection(){
	
	}

	public void OnSelect(){
		//gameObject.transform.parent.GetComponent<getSelectedObject> ().GetSelectedObject (selectedButton.gameObject);
		QuestionSystemController.Instance.partAnswerController.SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
	}
}

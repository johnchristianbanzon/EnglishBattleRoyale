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


	public void RemoveSelection(int hintIndex){

	}


	public void OnSelect(){
		QuestionSystemController.Instance.partAnswer.
		SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
	}
}

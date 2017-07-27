using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Typing : MonoBehaviour, ISelection
{
	public void ShowSelectionType (string questionAnswer,Action<List<GameObject>> onSelectCallBack){
		gameObject.SetActive (true);
	}

	public void ShowCorrectAnswer(){
		
	}

	public void HideSelectionType(){
		gameObject.SetActive (false);
	}

	public void ShowSelectionHint(int hintIndex){

	}


	public void OnSelect(){
		QuestionSystemController.Instance.partAnswer.fillAnswer.
		SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
	}
}

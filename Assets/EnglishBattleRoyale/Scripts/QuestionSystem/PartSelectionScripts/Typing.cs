using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
public class Typing : MonoBehaviour, ISelection
{
	private string questionAnswer;
	public void ShowSelectionType (string questionAnswer,Action<List<GameObject>> onSelectCallBack){
		this.questionAnswer = questionAnswer;
		gameObject.SetActive (true);
	}

	public void ShowCorrectAnswer(){
		
	}

	public void HideSelectionHint(){

	}

	public void HideSelectionType(){
		gameObject.SetActive (false);
	}

	public void ShowSelectionHint(int hintIndex, GameObject correctAnswerContainer){
		correctAnswerContainer.GetComponent<Button> ().enabled = false;
		correctAnswerContainer.GetComponentInChildren<Text> ().text = questionAnswer [hintIndex].ToString ();
		TweenFacade.TweenScaleToLarge (correctAnswerContainer.transform, Vector3.one, 0.3f);
		correctAnswerContainer.GetComponent<Image> ().color = new Color (255 / 255, 102 / 255f, 51 / 255f);
	}

	public void OnSelect(){
		QuestionSystemController.Instance.partAnswer.fillAnswer.
		SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoAnswerType : MonoBehaviour,IAnswer {
	public GameObject correctAnswerContainer;

	public void CheckAnswerFromSelection(string selectedAnswer, string questionAnswer){
		Debug.Log (selectedAnswer + "/" + questionAnswer);
		if(selectedAnswer.Equals(questionAnswer)){
			QuestionSystemController.Instance.CheckAnswer (true);
			CorrectSpecialEffect (QuestionSystemController.Instance.selectionType);
		}
	}

	public void DeployAnswerType(){
		gameObject.SetActive (true);
	}

	public void CorrectSpecialEffect(ISelection selectionType){
		
		/*
		switch (selectionType) {
		case QuestionSystemEnums.SelectionType.ChangeOrder:
			correctAnswerContainer.SetActive (true);
			TweenFacade.TweenScaleToSmall (correctAnswerContainer.transform, new Vector3 (0.03f, correctAnswerContainer.transform.localScale.y, correctAnswerContainer.transform.localScale.z), 0.02f);
			List<GameObject> answerButtons = QuestionSystemController.Instance.correctAnswerButtons;
			foreach (GameObject button in answerButtons) {
				button.SetActive (false);
				//TweenController.TweenMoveTo (button.transform,new Vector2(correctAnswerContainer.transform.localPosition.x,button.transform.localPosition.y),0.2f);
			}
			correctAnswerContainer.GetComponentInChildren<Text> ().text = QuestionSystemController.Instance.partSelectionController.changeOrderController.questionAnswer;
			Invoke ("AfterScalingTween", 0.05f);
			break;
		}
		*/
	}

	public void AfterScalingTween(){
		TweenFacade.TweenScaleToLarge (correctAnswerContainer.transform,Vector3.one,0.5f);

	}
}

using UnityEngine;
using System;
public class NoAnswerType : MonoBehaviour,IAnswer {
	public GameObject correctAnswerContainer;
	private Action<bool> onHintResult;

	public void CheckAnswerFromSelection(string selectedAnswer, string questionAnswer){
		Debug.Log (selectedAnswer + "/" + questionAnswer);
		if(selectedAnswer.Equals(questionAnswer)){
			QuestionSystemController.Instance.CheckAnswer (true);
		}
	}

	public void OnClickHint (int hintIndex,Action<bool> onHintResult){
		this.onHintResult = onHintResult;
		QuestionSystemController.Instance.selectionType.ShowSelectionHint (hintIndex,null);

	}

	public void ClearHint (){
		foreach (Transform hint in correctAnswerContainer.transform) {
			GameObject.Destroy(hint.gameObject);
		}
	}

	public void DeployAnswerType(){
		gameObject.SetActive (true);
	}

}

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
public class ShowAnswer : MonoBehaviour,IAnswer
{
	public GameObject showLetterView;
	private Action<bool> onHintResult;
	private string questionAnswer;
	private bool hasInitHints = false;
	private List<GameObject> hintContainers = new List<GameObject>();

	public void DeployAnswerType ()
	{
		this.gameObject.SetActive (true);
		questionAnswer = QuestionSystemController.Instance.questionAnswer;
	}

	public void CheckAnswerFromSelection(string selectedAnswer, string questionAnswer){
		if(selectedAnswer.Equals(questionAnswer)){
			QuestionSystemController.Instance.CheckAnswer (true);
			onHintResult.Invoke (true);
			ClearLettersInView ();
		}
	}

	public void OnClickHint (int hintIndex, Action<bool> onHintResult)
	{
		this.onHintResult = onHintResult;
		InitHints ();
		QuestionSystemController.Instance.selectionType.ShowSelectionHint (hintIndex, hintContainers[hintIndex]);

	}

	private void InitHints(){
		if (!hasInitHints) {
			hasInitHints = true;
			for (int i = 0; i < QuestionSystemController.Instance.correctAnswerButtons.Count; i++) {
				GameObject letterPrefab = SystemResourceController.Instance.LoadPrefab ("Input-UI",QuestionSystemController.Instance.partAnswer.showAnswer.showLetterView);
				letterPrefab.GetComponentInChildren<Text> ().text = QuestionSystemController.Instance.correctAnswerButtons [i].GetComponentInChildren<Text> ().text;	
				hintContainers.Add (letterPrefab);
				letterPrefab.GetComponentInChildren<Text> ().enabled = false;
				
			}
		}
	}

	public void ShowLetterInView (GameObject selectedLetter)
	{
		GameObject letterPrefab = SystemResourceController.Instance.LoadPrefab ("Input-UI", showLetterView);
		letterPrefab.GetComponentInChildren<Text> ().text = selectedLetter.GetComponentInChildren<Text> ().text;
		TweenFacade.TweenScaleToLarge (letterPrefab.transform, Vector3.one, 0.3f);
	}

	public void ClearHint ()
	{
		foreach (Transform hint in showLetterView.transform) {
			GameObject.Destroy (hint.gameObject);
		}
	}

	public void ClearLettersInView ()
	{

		foreach (Transform letter in showLetterView.transform) {
			GameObject.Destroy (letter.gameObject);
		}
	}

}

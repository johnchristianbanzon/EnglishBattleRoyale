using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ShowAnswer : MonoBehaviour,IAnswer
{
	public GameObject showLetterView;
	private Action<bool> onHintResult;
	private bool hasInitHints = false;
	public List<GameObject> hintContainers = new List<GameObject> ();
	public string questionAnswer;

	public void DeployAnswerType ()
	{
		this.gameObject.SetActive (true);
		hintContainers.Clear ();
		selectedIndex = 0;
		hasInitHints = false;
		questionAnswer = QuestionSystemController.Instance.questionAnswer;
	}

	public void CheckAnswerFromSelection (string selectedAnswer, string questionAnswer)
	{
		if (selectedAnswer.Equals (questionAnswer)) {
			QuestionSystemController.Instance.CheckAnswer (true);
			QuestionSystemController.Instance.selectionType.ShowCorrectAnswer (true);
			onHintResult.Invoke (true);
			ClearLettersInView ();
		}
	}

	public void OnClickHint (int hintIndex, Action<bool> onHintResult)
	{
		if (QuestionSystemController.Instance.questionHint.hasHintAvailable) {
			selectedIndex = 0;
			this.onHintResult = onHintResult;
			InitHints ();
			QuestionSystemController.Instance.selectionType.ShowSelectionHint (hintIndex, hintContainers [hintIndex]);
		}
	}

	public void InitHints ()
	{
		if (!hasInitHints) {
			hasInitHints = true;
			for (int i = 0; i < QuestionSystemController.Instance.correctAnswerButtons.Count; i++) {
				GameObject letterPrefab = SystemResourceController.Instance.LoadPrefab ("Input-UI", QuestionSystemController.Instance.partAnswer.showAnswer.showLetterView);
				letterPrefab.GetComponentInChildren<Image> ().color = new Color32 (80,135,223,255);
				letterPrefab.GetComponentInChildren<Text> ().text = "_";
				hintContainers.Add (letterPrefab);
			}
		}
	}

	public int selectedIndex = 0;
	public void ShowLetterInView (GameObject selectedLetter)
	{
		if (hasInitHints) {
			if (selectedIndex < questionAnswer.Length) {
				if (hintContainers [selectedIndex].GetComponent<Button> ().interactable) {
					hintContainers [selectedIndex].GetComponentInChildren<Text> ().text = selectedLetter.GetComponentInChildren<Text> ().text;
					hintContainers [selectedIndex].GetComponentInChildren<Image> ().color = new Color32 (255, 255, 255, 255);

				} 
				TweenFacade.TweenScaleToLarge (hintContainers [selectedIndex].transform, Vector3.one, 0.3f);
			}
		} else {
			GameObject letterPrefab = SystemResourceController.Instance.LoadPrefab ("Input-UI", showLetterView);
			letterPrefab.GetComponentInChildren<Text> ().text = selectedLetter.GetComponentInChildren<Text> ().text;
			TweenFacade.TweenScaleToLarge (letterPrefab.transform, Vector3.one, 0.3f);
		}
		selectedIndex++;
	}

	public void ClearHint ()
	{
		foreach (Transform hint in showLetterView.transform) {
			GameObject.Destroy (hint.gameObject);
		}
	}

	public void ClearLettersInView ()
	{
		selectedIndex = 0;
		foreach (Transform letter in showLetterView.transform) {
			if (!hasInitHints) {
				GameObject.Destroy (letter.gameObject);
			} else {
				if (letter.GetComponent<Button> ().interactable) {
					letter.GetComponentInChildren<Image> ().color = new Color32 (80,135,223,255);
					letter.GetComponentInChildren<Text> ().text = "_";
				}
			}
		}
	}

}

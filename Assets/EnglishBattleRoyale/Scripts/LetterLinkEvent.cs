using System;
using UnityEngine;
using UnityEngine.UI;

public class LetterLinkEvent : MonoBehaviour
{	
	public LetterLink letterlink;
	private static bool startSelection = false;
	private static string writtenAnswer;
	private Color selectedColor = new Color (36f / 255, 189f / 255f, 88f / 255f);
	private Color defaultColor = new Color32 (255,255,255,255);


	public void OnBeginDrag (GameObject currentSelectedLetter)
	{
		startSelection = true;
		writtenAnswer = "";
		OnDragSelection (currentSelectedLetter);

	}

	public void ShowCorrectAnswer(bool isAnswerCorrect){
		Color containerColor = new Color ();
		if (isAnswerCorrect) {
			containerColor = new Color32 (36, 189, 88, 255);
		} else {
			containerColor = new Color32 (255, 100, 100, 255);
		}
		if (letterlink.questionAnswer.Contains (GetComponentInChildren<Text>().text)) {
			GetComponent<Image> ().color = containerColor;
		}
	}

	public void OnDragSelection (GameObject currentSelectedLetter)
	{
		if (startSelection && (currentSelectedLetter.GetComponent<Image> ().color != selectedColor)) {
			writtenAnswer += currentSelectedLetter.GetComponentInChildren<Text> ().text;
			QuestionSystemController.Instance.partAnswer.showAnswer.ShowLetterInView (currentSelectedLetter);
			currentSelectedLetter.GetComponent<Image> ().color = selectedColor;
		}
	}


	public void OnEndDrag ()
	{
		if (letterlink.questionAnswer == writtenAnswer) {
			startSelection = false;

			QuestionSystemController.Instance.CheckAnswer (true);
		} else {
			ClearSelection ();
		}
	}

	public void ClearSelection ()
	{
		writtenAnswer = "";
		startSelection = false;
		QuestionSystemController.Instance.partAnswer.showAnswer.ClearLettersInView ();
		foreach (LetterLinkEvent selection in letterlink.connectLetterButtons) {
			selection.GetComponent<Image> ().color = defaultColor;
		}
	}
}



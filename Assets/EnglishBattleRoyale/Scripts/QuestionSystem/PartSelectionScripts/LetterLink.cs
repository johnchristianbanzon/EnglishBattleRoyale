using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LetterLink : MonoBehaviour ,ISelection
{
	public GameObject[] connectLetterButtons = new GameObject[9];
	public List<GameObject> selectedLetters = new List<GameObject> ();
	private bool startSelection = false;
	private string writtenAnswer;
	private string questionAnswer;
	private int noHintUsed;
	private Color selectedColor = new Color (36f / 255, 189f / 255f, 88f / 255f);
	private Color defaultColor = new Color (94f / 255, 255f / 255f, 148f / 255f);
	private int noOfContainersSelected = 0;

	public void ShowCorrectAnswer ()
	{
		
	}

	public void OnBeginDrag (GameObject currentSelectedLetter)
	{
		startSelection = true;
		noOfContainersSelected = noHintUsed;
		OnDragSelection (currentSelectedLetter);
	}

	public void HideSelectionType ()
	{
		//gameObject.SetActive (false);
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
		noOfContainersSelected = 0;
		if (questionAnswer == writtenAnswer) {
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
		foreach (GameObject selection in connectLetterButtons) {
			selection.GetComponent<Image> ().color = defaultColor;
		}
	}

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{
		//HINT STILL IN PLANNING
	}

	public void HideSelectionHint(){
	
	}

	public void ShowSelectionType (string questionAnswer, Action<List<GameObject>> onSelectCallBack)
	{
		this.questionAnswer = questionAnswer;
		ShuffleSelection ();
		gameObject.SetActive (true);
	}

	/// <summary>
	/// Shuffles The Selection in the Letter Link, Randomizes Starting point first and then goes to the next viable 
	/// letter index in the selectableIndex List depending on the number. 
	/// While loop is used to ensure to repeating index is selected. 
	/// </summary>
	public void ShuffleSelection ()
	{
		ClearSelection ();
		List<GameObject> correctAnswerButtons = new List<GameObject> ();
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		List<List<int>> selectableIndex = new List<List<int>> {
			new List<int>{ 0 }, new List<int>{ 2, 4 }, new List<int>{ 1, 3, 5 }, new List<int>{ 2, 6 },
			new List<int>{ 1, 5, 7 }, new List<int>{ 2, 4, 6, 8 }, new List<int>{ 3, 5, 9 },
			new List<int>{ 4, 8 }, new List<int>{ 7, 5, 9 }, new List<int>{ 8, 6 }
		};
		int selectionIndex = UnityEngine.Random.Range (1, connectLetterButtons.Length + 1);
		List<int> numbersDone = new List<int> ();
		for (int i = 0; i < connectLetterButtons.Length; i++) {
			connectLetterButtons [i].GetComponentInChildren<Text> ().text = alphabet [UnityEngine.Random.Range (0, alphabet.Length)].ToString ();
		}
		int whileindex = 0;
		for (int i = 0; i < questionAnswer.Length; i++) {
			int randomizedSelection = UnityEngine.Random.Range (0, selectableIndex [selectionIndex].Count);
			connectLetterButtons [selectionIndex - 1].GetComponentInChildren<Text> ().text = questionAnswer [i].ToString ();
			while (numbersDone.Contains (selectableIndex [selectionIndex] [randomizedSelection])) {
				randomizedSelection = UnityEngine.Random.Range (0, selectableIndex [selectionIndex].Count);
				if (whileindex > 100) {
					break;
				}
				whileindex++;
			}
			numbersDone.Add (selectionIndex);
			correctAnswerButtons.Add (connectLetterButtons [selectionIndex - 1]);
			selectionIndex = selectableIndex [selectionIndex] [randomizedSelection];
		}
		QuestionSystemController.Instance.correctAnswerButtons = correctAnswerButtons;
	}

}

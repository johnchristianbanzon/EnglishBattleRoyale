using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LetterLink : MonoBehaviour {
	public GameObject[] connectLetterButtons = new GameObject[9];
	public List<GameObject> correctAnswerButtons = new List<GameObject>();
	private bool startSelection = false;
	private string writtenAnswer;
	private string questionAnswer;

	public void OnbeginDrag(GameObject currentSelectedLetter){
		startSelection = true;
		OnDragSelection (currentSelectedLetter);
	}

	public void OnDragSelection(GameObject currentSelectedLetter){
		if (startSelection && (currentSelectedLetter.GetComponent<Image> ().color != new Color (36f / 255, 189f / 255f, 88f / 255f))) {
			currentSelectedLetter.GetComponent<Image> ().color = new Color (36f / 255, 189f / 255f, 88f / 255f);
			writtenAnswer += currentSelectedLetter.GetComponentInChildren<Text> ().text;
			QuestionSystemController.Instance.answerController.showAnswer.ShowLetterInView (currentSelectedLetter);
		}
	}
	public void OnEndDrag(){
		if (questionAnswer == writtenAnswer) {
			startSelection = false;
			QuestionSystemController.Instance.CheckAnswer (true);

		} else {
			clearSelection ();
		}

	}
	public void clearSelection(){
		writtenAnswer = "";
		startSelection = false;
		QuestionSystemController.Instance.answerController.showAnswer.ClearLettersInView ();
		foreach (GameObject selection in connectLetterButtons) {
			selection.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
		}
	}
	public void ConnectLetterShuffle(string answer){
		clearSelection ();
		questionAnswer = answer;
		correctAnswerButtons.Clear ();
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		List<List<int>> selectableIndex = new List<List<int>> {
			new List<int>{0}, new List<int>{2,4},new List<int>{1,3,5},new List<int>{2,6},
			new List<int>{1,5,7},new List<int>{2,4,6,8},new List<int>{3,5,9},
			new List<int>{4,8},new List<int>{7,5,9},new List<int>{8,6}
		};
		int selectionIndex = UnityEngine.Random.Range(1,connectLetterButtons.Length+1);
		List<int> numbersDone = new List<int>();
		for (int i = 0; i < connectLetterButtons.Length; i++) {
			connectLetterButtons [i].GetComponentInChildren<Text> ().text = alphabet [UnityEngine.Random.Range (0, alphabet.Length)].ToString ();
		}
		int whileindex = 0;
		for (int i = 0; i < answer.Length; i++) {
			int randomizedSelection = UnityEngine.Random.Range (0, selectableIndex [selectionIndex].Count);
			connectLetterButtons [selectionIndex-1].GetComponentInChildren<Text> ().text = answer [i].ToString();
			while (numbersDone.Contains (selectableIndex[selectionIndex][randomizedSelection])) {
				randomizedSelection = UnityEngine.Random.Range (0, selectableIndex [selectionIndex].Count);
				if (whileindex > 100) {
					break;
				}
				whileindex++;
			}
			numbersDone.Add (selectionIndex);
			correctAnswerButtons.Add (connectLetterButtons [selectionIndex - 1]);
			selectionIndex = selectableIndex[selectionIndex][randomizedSelection];
		}
		QuestionSystemController.Instance.correctAnswerButtons = correctAnswerButtons;
	}

}

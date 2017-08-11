using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectLetterEvent : MonoBehaviour
{
	public SelectLetter selectLetter;
	public Text letter;
	public bool isSelected = false;
	public int selectionIndex = 0; 
	public int correctAnswerIndex;
	public bool isCorrect = false;

	public void Init (bool isCorrect, int letterIndex)
	{
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		if (isCorrect) {
			letter.text = selectLetter.questionAnswer [letterIndex].ToString ();
			correctAnswerIndex = letterIndex;
			this.isCorrect = isCorrect;
		} else {
			int randomizeLetterIndex = Random.Range (0, alphabet.Length);
			while (selectLetter.questionAnswer.Contains (alphabet [randomizeLetterIndex].ToString())) {
				randomizeLetterIndex = Random.Range (0, alphabet.Length);
			}
			letter.text = alphabet [randomizeLetterIndex].ToString ();
		}
		gameObject.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
		isSelected = false;
		gameObject.GetComponent<Button> ().interactable = true;
		gameObject.GetComponent<EventTrigger> ().enabled = true;
	}

	public void OnSelectLetter (GameObject selectedLetter)
	{
		if (!isSelected) {
			selectLetter.fillAnswer.ShowSelectedLetter (selectedLetter);
		} else {
			ReturnSelectedLetter ();
		}
		if (!selectLetter.fillAnswer.isFull) {
			isSelected = !isSelected;
		}
	}

	public void ReturnSelectedLetter(){
//		selectLetter.fillAnswer.InitContainer (transform.GetSiblingIndex());
		isSelected = false;
		transform.SetParent(selectLetter.transform);
	}
}

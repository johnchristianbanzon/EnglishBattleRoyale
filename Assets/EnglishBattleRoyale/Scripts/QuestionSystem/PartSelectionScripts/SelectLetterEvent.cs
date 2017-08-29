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
	public int correctAnswerIndex;
	public bool isCorrect = false;
	public GameObject containerReplacement;
	public int containerIndex;

	public void Init (bool isCorrect, int letterIndex)
	{
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		if (isCorrect) {
			letter.text = selectLetter.questionAnswer [letterIndex].ToString ();
			correctAnswerIndex = letterIndex;
			this.isCorrect = isCorrect;
		} else {
			int randomizeLetterIndex = Random.Range (0, alphabet.Length);
			while (selectLetter.questionAnswer.Contains (alphabet [randomizeLetterIndex].ToString ())) {
				randomizeLetterIndex = Random.Range (0, alphabet.Length);
			}
			letter.text = alphabet [randomizeLetterIndex].ToString ();
		}
		Destroy (containerReplacement);
		containerIndex = transform.GetSiblingIndex ();
//		gameObject.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
		gameObject.GetComponent<Image> ().color = Color.white;
		isSelected = false;
		gameObject.GetComponent<Button> ().interactable = true;
		gameObject.GetComponent<EventTrigger> ().enabled = true;
	}

	public void OnSelectLetter (GameObject selectedLetter)
	{
		SystemSoundController.Instance.PlaySFX ("SFX_ClickButton");
		selectLetter.fillAnswer.CheckAnswerHolder ();
		if (!isSelected) {
			if (!selectLetter.fillAnswer.isFull) {
				InstantiateHiddenContaner (selectedLetter.transform.GetSiblingIndex ());
				selectLetter.fillAnswer.ShowSelectedLetter (selectedLetter);
				isSelected = true;
			}
		} else {
			ReturnSelectedLetter (selectedLetter);
			isSelected = false;
		}
	}

	public void InstantiateHiddenContaner (int index)
	{
		containerReplacement = SystemResourceController.Instance.LoadPrefab ("Input-UI", selectLetter.gameObject);
		containerReplacement.GetComponent<Image> ().enabled = false;
		containerReplacement.transform.SetSiblingIndex (index);
	}

	public void ReturnSelectedLetter (GameObject selectedLetter)
	{
		if (isSelected) {
			Destroy (containerReplacement);
		}
		isSelected = false;
		transform.SetParent (selectLetter.transform);
		transform.SetSiblingIndex (containerIndex);
	}

	public void ShowCorrectAnswer (bool isAnswerCorrect)
	{
		Color selectionColor = new Color ();
		if (isAnswerCorrect) {
			selectionColor = new Color32 (255, 223, 0, 255);
		} else {
			
			selectionColor = new Color32 (255, 100, 100, 255);
		}
		GetComponent<Image> ().color = selectionColor;
		GameObject answerContainer = selectLetter.fillAnswer.answerContainers [correctAnswerIndex];
		answerContainer.GetComponent<Image> ().color = selectionColor;
	}

}

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
	public bool hasHint = false;
	public GameObject hintSpecialEffectObject;
	public bool isButtonsClickable = true;

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
		Destroy (hintSpecialEffectObject);
		containerIndex = transform.GetSiblingIndex ();
//		gameObject.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
		gameObject.GetComponent<Image> ().color = Color.white;
		isSelected = false;
		hasHint = false;
		gameObject.GetComponent<Button> ().interactable = true;
		gameObject.GetComponent<EventTrigger> ().enabled = true;
	}

	public void OnSelectLetter (GameObject selectedLetter)
	{
		SystemSoundController.Instance.PlaySFX ("SFX_ClickButton");
		selectLetter.fillAnswer.CheckAnswerHolder ();
		if (isButtonsClickable) {
			if (!isSelected) {
				if (!selectLetter.fillAnswer.isFull) {
					if (!CheckHints (selectedLetter)) {
						InstantiateHiddenContaner (selectedLetter.transform.GetSiblingIndex ());
						selectLetter.fillAnswer.ShowSelectedLetter (selectedLetter);
						isSelected = true;
					} else {
						TweenFacade.TweenShakePosition (selectedLetter.transform, 0.5f, 15.0f, 20, 90f);
						isButtonsClickable = false;
						Invoke ("ButtonClickable", 0.5f);
					}
				}
			} else {
				ReturnSelectedLetter (selectedLetter);
				isSelected = false;
			}
		}
	}

	private void ButtonClickable(){
		isButtonsClickable = true;
	}

	private bool CheckHints(GameObject selectedLetter){
		for (int i = 0; i < selectLetter.selectionButtons.Length; i++) {
			if (selectLetter.selectionButtons [i].hasHint && !selectLetter.selectionButtons[i].isSelected) {
				if (selectedLetter.GetComponent<SelectLetterEvent> ().isCorrect && 
					selectedLetter.GetComponent<SelectLetterEvent>().letter.text==selectLetter.questionAnswer[selectLetter.fillAnswer.CheckAnswerHolder()].ToString()) {
				
				} else {
					GameObject correctAnswerObject = selectLetter.correctContainers[selectLetter.fillAnswer.CheckAnswerHolder()].gameObject;
					TweenFacade.TweenJumpTo (correctAnswerObject.transform, new Vector2 (correctAnswerObject.transform.localPosition.x, correctAnswerObject
						.transform.localPosition.y), 50f, 1, 0.4f, 0);
					return true;
				}
			}
		}
		return false;
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
			if (!isSelected && isCorrect) {
//				Invoke ("ShowEachLetter", (float)((0.2) + (0.1 * correctAnswerIndex)));
				ShowEachLetter();
			} 
		}
		GetComponent<Image> ().color = selectionColor;
		GameObject answerContainer = selectLetter.fillAnswer.answerContainers [correctAnswerIndex];
		answerContainer.GetComponent<Image> ().color = selectionColor;
	}

	public void ShowEachLetter(){
//		OnSelectLetter (gameObject);
//		selectLetter.ShowSelectionHint(0,null);
		QuestionSystemController.Instance.questionHint.OnClick();

	}
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class WordChoice : MonoBehaviour, ISelection
{
	public bool justAnswered = false;
	private string questionAnswer = "";
	private int numberOfCorrectAnswer = 2;
	public WordChoiceEvent[] selectionButtons = new WordChoiceEvent[4];
	private List<GameObject> answerClicked = new List<GameObject> ();
	private List<GameObject> answerButtons = new List<GameObject> ();
	private List<string> answerString = new List<string> ();
	private GameObject selectedObject;
	private int hintIndex = 0;

	private bool isAnswerCorrect;

	private void CheckAnswer (string answerClicked1, string answerClicked2)
	{
		if (answerString.Contains (answerClicked1) && answerString.Contains (answerClicked2)) {
			isAnswerCorrect = true;
		} else {
			isAnswerCorrect = false;
		}
		ShowCorrectAnswer (isAnswerCorrect);
		Invoke ("CheckIfCorrect", 0.3f);
	}

	List<GameObject> popUpSelectionList = new List<GameObject> ();

	public GameObject ShowSelectionPopUp ()
	{
		SystemSoundController.Instance.PlaySFX ("SFX_WordChoice");
		GameObject selectionPopUp = SystemResourceController.Instance.LoadPrefab ("PopUpWordChoice", SystemPopupController.Instance.popUp);
		gameObject.SetActive (true);
		popUpSelectionList = new List<GameObject> ();
		for (int i = 0; i < selectionPopUp.transform.childCount; i++) {
			popUpSelectionList.Add (selectionPopUp.transform.GetChild (i).gameObject);
		}
		if (popUpSelectionList.Count > 1) {
			TweenFacade.TweenJumpTo (popUpSelectionList [0].transform, popUpSelectionList [0].transform.localPosition
			, 30f, 1, 0.5f, 0);
			TweenFacade.TweenJumpTo (popUpSelectionList [1].transform, popUpSelectionList [1].transform.localPosition
				, 30f, 1, 0.5f, 0.5f);
			InvokeRepeating ("ChangeSelectionColor",0.5f,0.5f);
//			StartCoroutine (ChangeSelectionColor (popUpSelectionList));
		}
		return selectionPopUp;
	}

	private int indexer = 0;

	private void ChangeSelectionColor ()
	{
		if (indexer < 2) {
			if (indexer.Equals (0)) {
				popUpSelectionList [indexer].GetComponent<Image> ().color = new Color32 (255, 223, 0, 255);
			} else {
				popUpSelectionList [indexer].GetComponent<Image> ().color = new Color32 (255, 100, 100, 255);
			}
		} else {
			popUpSelectionList [0].GetComponent<Image> ().color = new Color32 (255, 223, 0, 255);
			popUpSelectionList [1].GetComponent<Image> ().color = new Color32 (255, 223, 0, 255);
			CancelInvoke ();
		}
		indexer++;
	}

	public void ShowCorrectAnswer (bool isAnswerCorrect)
	{
		Color answerColor = new Color ();
		if (isAnswerCorrect) {
			answerColor = new Color32 (255, 223, 0, 255);
		} else {
			answerColor = new Color32 (255, 100, 100, 255);
		}

		for (int i = 0; i < answerClicked.Count; i++) {
			answerClicked [i].GetComponent<Image> ().color = answerColor;
			if (!isAnswerCorrect) {
				TweenFacade.TweenShakePosition (answerClicked [i].transform, 0.3f, 30.0f, 90, 90f);
			}
		}
	}

	public void HideSelectionHint ()
	{

	}

	public void CheckAnswerClicked(){
		bool allAnswerCorrect = true;
		for (int i = 0; i < selectionButtons.Length; i++) {
			selectionButtons[i].GetComponent<Image> ().color = Color.white;
			if (selectionButtons [i].isSelected && !selectionButtons[i].isCorrect) {
				allAnswerCorrect = false;
			}
			selectionButtons [i].isSelected = false;
		}
		if (allAnswerCorrect) {
			QuestionSystemController.Instance.CheckAnswer (true);
		} else {
			QuestionSystemController.Instance.CheckAnswer (false);
		}
	}

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{ 
		/*
		if (hintIndex < numberOfCorrectAnswer) {
			for (int i = 0; i < answerButtons.Count; i++) {
				if (answerButtons [i].GetComponent<Image> ().color != Color.gray &&
				    answerString.Contains (answerButtons [i].GetComponentInChildren<Text> ().text)) {
					answerButtons [i].GetComponent<Button> ().interactable = false;
					OnClickSelection (answerButtons [i].gameObject);
					break;
				}
			}

			answerButtons [answerClicked.Count].GetComponent<Button> ().interactable = false;
			OnClickSelection (answerButtons [answerClicked.Count].gameObject);
			this.hintIndex++;
		}*/
	}

	public void HideSelectionType ()
	{
		gameObject.SetActive (false);
	}

	private void CheckIfCorrect ()
	{
		if (isAnswerCorrect) {
			QuestionSystemController.Instance.selectionType.ShowCorrectAnswer (true);
			QuestionSystemController.Instance.CheckAnswer (true);
		} else {
			for (int i = 0; i < answerClicked.Count; i++) {
				if (answerClicked [i].GetComponent<Button> ().interactable) {
					answerClicked [i].GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
					if (hintIndex > 0) {
						answerClicked.Remove (answerClicked [i]);
					}
				}
			}
			if (hintIndex.Equals (0)) {
				answerClicked.Clear ();
			}
		}
	}

	public void ShowSelectionType (string questionAnswer, Action<List<GameObject>> onSelectCallBack)
	{
		this.gameObject.SetActive (true);
		this.questionAnswer = questionAnswer;
		answerClicked.Clear ();
		ShuffleSelection ();
	}

	public void ShuffleSelection ()
	{
		answerButtons.Clear ();
		answerString.Clear ();
		answerClicked.Clear ();
		hintIndex = 0;
		int numberOfAnswers = 2;
		List <int> randomList = new List<int> ();
		string[] temp = questionAnswer.Split ('/');
		for (int i = 0; i < selectionButtons.Length; i++) {
			int randomNum = UnityEngine.Random.Range (0, 4); 
			while (randomList.Contains (randomNum)) {
				randomNum = UnityEngine.Random.Range (0, selectionButtons.Length);
			}
			randomList.Add (randomNum);
			if (i < numberOfAnswers) {
				selectionButtons [randomNum].Init (true, temp [i].ToUpper ());
				answerButtons.Add (selectionButtons [randomNum].gameObject);
			} else {
				selectionButtons [randomNum].Init (false, temp [i].ToUpper ());

			}
			selectionButtons [randomNum].GetComponent<Button> ().interactable = true;
		}
		QuestionSystemController.Instance.correctAnswerButtons = answerButtons;
		answerString.Add (answerButtons [0].GetComponentInChildren<Text> ().text.ToUpper ());
		answerString.Add (answerButtons [1].GetComponentInChildren<Text> ().text.ToUpper ());
	}
}

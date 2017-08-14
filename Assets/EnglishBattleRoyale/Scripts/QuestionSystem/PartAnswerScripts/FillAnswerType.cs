using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FillAnswerType : MonoBehaviour,IAnswer
{

	public List<GameObject> answerContainers = new List<GameObject> ();
	public GameObject[] selectionIdentifier = new GameObject[12];
	public GameObject outviewContent;
	private string questionAnswer;
	public int answerIndex = 0;
	List <int> hintIndexRandomList = new List<int> ();
	public GameObject clearButton;
	public bool isFull = false;

	public void DeployAnswerType ()
	{
		gameObject.SetActive (true);
		hintIndexRandomList.Clear ();
		questionAnswer = QuestionSystemController.Instance.questionAnswer;
		PopulateContainer ();
	}

	public void ClearAnswerContainers ()
	{
		for (int i = 0; i < answerContainers.Count; i++) {
			if (answerContainers [i].transform.childCount > 0) {
				if (answerContainers [i].GetComponentInChildren<SelectLetterEvent> () != null) {
					answerContainers [i].GetComponentInChildren<SelectLetterEvent> ().ReturnSelectedLetter (answerContainers [i]);
				}
			} 
		}
	}

	public void ClearHint ()
	{
		for (int i = 0; i < answerContainers.Count; i++) {
			if (answerContainers [i].transform.childCount > 0) {
				if (answerContainers [i].GetComponentInChildren<SelectLetterEvent> () != null) {
					answerContainers [i].GetComponentInChildren<SelectLetterEvent> ().ReturnSelectedLetter (answerContainers [i]);
				}
			} 
			Destroy (answerContainers [i]);
		}
	}

	public void OnClickHint (int hintIndex, Action<bool> onHintResult)
	{
		CheckAnswerHolder ();
		QuestionSystemController.Instance.selectionType.ShowSelectionHint (hintIndex, answerContainers [answerIndex]);
		CheckAnswer ();
	}

	public void PopulateContainer ()
	{
		answerContainers.Clear ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject answerPrefab = SystemResourceController.Instance.LoadPrefab ("AnswerContainer", outviewContent);
			answerPrefab.name = "output" + (i + 1);
			answerContainers.Add (answerPrefab);
			answerPrefab.GetComponent<Button> ().onClick.AddListener (() => {
				OnAnswerClick (answerPrefab.GetComponent<Button> ());
			});
		}
		QuestionSystemController.Instance.correctAnswerButtons = answerContainers;
	}

	public void OnAnswerClick (Button answerButton)
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		if (string.IsNullOrEmpty (answerButton.transform.GetComponentInChildren<Text> ().text)) {
			TweenFacade.TweenShakePosition (answerButton.transform, 0.5f, 15.0f, 50, 90f);
		} else {
			Destroy (answerButton.transform.GetChild (0).gameObject);
			CheckAnswerHolder ();
		}
	}

	public void SelectionLetterGot (GameObject selectedObject)
	{
		CheckAnswerHolder ();
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		if (string.IsNullOrEmpty (selectedObject.GetComponentInChildren<Text> ().text)) {
			TweenFacade.TweenShakePosition (selectedObject.transform, 1.0f, 30.0f, 50, 90f);
		} else {
			if (!isFull) {
				GameObject container = SystemResourceController.Instance.LoadPrefab ("Input-UI", answerContainers [answerIndex]);
				selectionIdentifier [answerIndex] = selectedObject.gameObject;
				hintIndexRandomList.Add (answerIndex);
				container.GetComponentInChildren<Text> ().text = selectedObject.GetComponentInChildren<Text> ().text;
				container.GetComponent<Image> ().raycastTarget = false;
				CheckAnswer ();
			}
		}
	}

	public void ShowSelectedLetter (GameObject selectedObject)
	{
		if (!isFull) {
			if (questionAnswer.Length > answerIndex) {
				selectedObject.transform.parent = answerContainers [answerIndex].transform;
				CheckAnswer ();
			}
		}
		CheckAnswerHolder ();
	}

	public string GetAnswerWritten ()
	{
		string answerWrote = "";
		for (int i = 0; i < questionAnswer.Length; i++) {
			
			if (answerContainers [i].transform.childCount > 0) {
				answerWrote += answerContainers [i].transform.GetChild (0).GetComponentInChildren<Text> ().text;
			}
		}
		return answerWrote;
	}

	public void CheckAnswer ()
	{
		GetAnswerWritten ();
		string answerWrote = GetAnswerWritten ();
		if (answerWrote.Length.Equals (questionAnswer.Length)) {
			if (answerWrote.ToUpper ().Equals (questionAnswer.ToUpper ())) {
				QuestionSystemController.Instance.selectionType.ShowCorrectAnswer (true);
				QuestionSystemController.Instance.CheckAnswer (true);
			} else {
				QuestionSystemController.Instance.CheckAnswer (false);
			}
		}
	}

	public void InitContainer (int positionIndex)
	{
		GameObject emptyContainer = SystemResourceController.Instance.LoadPrefab ("Input-UI", outviewContent);
		emptyContainer.transform.SetSiblingIndex (positionIndex);
		answerContainers [positionIndex] = emptyContainer;
	}

	public GameObject GetSelectionIdentifier (int index)
	{
		GameObject objectIdentifier = selectionIdentifier [index];
		return objectIdentifier;
	}

	public int CheckAnswerHolder ()
	{
		isFull = true;
		foreach (Transform answerContainer in outviewContent.transform) {
			if (answerContainer.childCount.Equals (0)) {
				answerIndex = answerContainer.GetSiblingIndex ();
				isFull = false;
				break;
			}
		}
		return answerIndex;
	}
}

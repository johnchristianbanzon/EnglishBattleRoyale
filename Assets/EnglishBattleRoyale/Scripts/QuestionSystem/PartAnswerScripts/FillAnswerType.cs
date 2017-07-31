using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FillAnswerType : MonoBehaviour,IAnswer {
	
	public List<GameObject> answerContainers = new List<GameObject>();
	public GameObject[] selectionIdentifier = new GameObject[12];
	public GameObject outviewContent;
	private string questionAnswer;
	private int answerIndex = 0;
	Action<bool> onHintResult;
	List <int> hintIndexRandomList = new List<int> ();
	public string answerWrote = "";

	public void DeployAnswerType(){
		gameObject.SetActive (true);
		hintIndexRandomList.Clear ();
		this.questionAnswer = QuestionSystemController.Instance.questionAnswer;
		PopulateContainer ();
	}

	public void ClearHint (){
		foreach (Transform hint in outviewContent.transform) {
			GameObject.Destroy(hint.gameObject);
		}
	}

	public void OnClickHint (int hintIndex,Action<bool> onHintResult){
		this.onHintResult = onHintResult;
		CheckAnswerHolder ();
		int randomizedHintIndex = 0;
		int whileCounter = 0;
		randomizedHintIndex = UnityEngine.Random.Range (0, questionAnswer.Length);
		while (hintIndexRandomList.Contains (randomizedHintIndex) &&
			questionAnswer[randomizedHintIndex].ToString().Equals(answerContainers[randomizedHintIndex].GetComponentInChildren<Text>().text)) {
			randomizedHintIndex = UnityEngine.Random.Range (0, questionAnswer.Length);
			if (whileCounter > 100) {
				break;
			}
			whileCounter++;
		}
			
		hintIndexRandomList.Add (randomizedHintIndex);
		Debug.Log (randomizedHintIndex);
		QuestionSystemController.Instance.selectionType.ShowSelectionHint (randomizedHintIndex,answerContainers[randomizedHintIndex]);

		CheckAnswer ();
	}

	public void PopulateContainer(){
		answerContainers.Clear ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject answerPrefab = SystemResourceController.Instance.LoadPrefab ("Input-UI",outviewContent);
			answerPrefab.name = "output" + (i+1);
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
		string answerclicked = "";
		if (string.IsNullOrEmpty (answerButton.transform.GetComponentInChildren<Text>().text)) {
			TweenFacade.TweenShakePosition (answerButton.transform, 0.5f, 15.0f, 50, 90f);
		} else {
			for (int i = 0; i < answerContainers.Count; i++) {
				if (answerButton.name.Equals ("output" + (i+1))) {
					answerclicked = answerContainers [i].transform.GetChild (0).GetComponent<Text> ().text;
					hintIndexRandomList.Remove (i);
					answerContainers [i].GetComponentInChildren<Text>().text = "";
					GetSelectionIdentifier (i).SetActive(true);

				}
			}
			CheckAnswerHolder ();
		}
	}

	public void SelectionLetterGot(GameObject selectedObject){
		CheckAnswerHolder ();
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		if (string.IsNullOrEmpty (selectedObject.GetComponentInChildren<Text>().text)) {
			TweenFacade.TweenShakePosition (selectedObject.transform, 1.0f, 30.0f, 50, 90f);
		} else {
			selectionIdentifier[answerIndex] = selectedObject.gameObject;
			hintIndexRandomList.Add (answerIndex);
			answerContainers [answerIndex].GetComponentInChildren<Text>().text 
			= selectedObject.GetComponentInChildren<Text>().text;
			CheckAnswer ();
		}
	}

	public void GetAnswerWritten(){
		answerWrote = "";
		for (int j = 0; j < questionAnswer.Length; j++) {
			answerWrote += answerContainers [j].transform.GetChild (0).GetComponent<Text> ().text;
		}
	}

	public void CheckAnswer(){
		GetAnswerWritten ();
		if (answerWrote.Length.Equals (questionAnswer.Length)) {
			
			if (answerWrote.ToUpper ().Equals (questionAnswer.ToUpper ())) {
				QuestionSystemController.Instance.CheckAnswer (true);
				onHintResult.Invoke (true);
			} else {
				QuestionSystemController.Instance.CheckAnswer (false);
			}
		}
	}

	public GameObject GetSelectionIdentifier(int index){
		GameObject objectIdentifier = selectionIdentifier[index];
		return objectIdentifier;
	}

	public void CheckAnswerHolder ()
	{
		for (int j = 0; j < answerContainers.Count; j++) {
			GameObject findEmpty = answerContainers [j].transform.GetChild (0).gameObject;
			if (string.IsNullOrEmpty (findEmpty.GetComponent<Text> ().text)) {
				answerIndex = j;
				break;
			}
		}

	}
}

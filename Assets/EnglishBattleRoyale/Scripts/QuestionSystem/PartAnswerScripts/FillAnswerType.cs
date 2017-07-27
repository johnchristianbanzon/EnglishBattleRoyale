using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FillAnswerType : MonoBehaviour,IAnswer {
	
	public List<GameObject> answerContainers = new List<GameObject>();
	private GameObject[] selectionIdentifier = new GameObject[12];
	public GameObject outviewContent;
	private string questionAnswer;
	private int answerIndex = 0;

	public void DeployAnswerType(){
		gameObject.SetActive (true);
		this.questionAnswer = QuestionSystemController.Instance.questionAnswer;
		PopulateContainer ();
	}

	public void OnClickHint (int hintCounter){
		GameObject letterHint = answerContainers [hintCounter];
		letterHint.GetComponentInChildren<Text> ().text = questionAnswer [hintCounter].ToString ();
		letterHint.GetComponent<Button> ().enabled = false;
		TweenFacade.TweenScaleToLarge (letterHint.transform, Vector3.one, 0.3f);
		letterHint.GetComponent<Image> ().color = new Color (255 / 255, 102 / 255f, 51 / 255f);
		CheckAnswer ();
		answerIndex++;
	}

	public void PopulateContainer(){
		
		foreach (Transform child in outviewContent.transform) {
				GameObject.Destroy(child.gameObject);
		}
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
			Debug.Log (answerButton.name);
			for (int i = 0; i < answerContainers.Count; i++) {
				if (answerButton.name.Equals ("output" + (i+1))) {
					answerclicked = answerContainers [i].transform.GetChild (0).GetComponent<Text> ().text;
					answerContainers [i].GetComponentInChildren<Text>().text = "";
					GetSelectionIdentifier (i).GetComponentInChildren<Text>().text = answerclicked;
//					GetSelectionIdentifier (i).SetActive(true);
//					answerIdentifier [i]

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
			answerContainers [answerIndex].GetComponentInChildren<Text>().text 
			= selectedObject.GetComponentInChildren<Text>().text;
			CheckAnswer ();
		}
	}

	public void CheckAnswer(){
		string answerWrote = "";
		for (int j = 0; j < questionAnswer.Length; j++) {
			answerWrote += answerContainers [j].transform.GetChild (0).GetComponent<Text> ().text;
		}
		if (answerWrote.Length.Equals (questionAnswer.Length)) {
			if (answerWrote.ToUpper ().Equals (questionAnswer.ToUpper ())) {
				QuestionSystemController.Instance.CheckAnswer(true);
			} else {
				QuestionSystemController.Instance.CheckAnswer(false);
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

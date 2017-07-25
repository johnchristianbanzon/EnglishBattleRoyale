using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartAnswerController : MonoBehaviour {
	private List<GameObject> answerContainer = new List<GameObject>();
	private GameObject[] selectionIdentifier = new GameObject[12];
	private QuestionSystemEnums.AnswerType answerType;
	private int answerIndex = 0;
	private string questionAnswer = "";
	public FillAnswerType fillAnswer;
	public NoAnswerType noAnswer;
	public ShowAnswer showAnswer;

	public void SelectionLetterGot(GameObject selectedObject){
		CheckAnswerHolder ();
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		if (string.IsNullOrEmpty (selectedObject.GetComponentInChildren<Text>().text)) {
			TweenFacade.TweenShakePosition (selectedObject.transform, 1.0f, 30.0f, 50, 90f);
		} else {
			selectionIdentifier[answerIndex] = selectedObject.gameObject;
			answerContainer [answerIndex].GetComponentInChildren<Text>().text 
			= selectedObject.GetComponentInChildren<Text>().text;
			CheckAnswer ();
		}
	}

	public void CheckAnswer(){
		string answerWrote = "";
		for (int j = 0; j < questionAnswer.Length; j++) {
			answerWrote += answerContainer [j].transform.GetChild (0).GetComponent<Text> ().text;
		}
			
		if (answerWrote.Length.Equals (questionAnswer.Length)) {
			if (answerWrote.ToUpper ().Equals (questionAnswer.ToUpper ())) {
				//CheckAnswer (true);
				QuestionSystemController.Instance.CheckAnswer(true);
			} else {
				//CheckAnswer (false);
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
		for (int j = 0; j < answerContainer.Count; j++) {
			GameObject findEmpty = answerContainer [j].transform.GetChild (0).gameObject;
			if (string.IsNullOrEmpty (findEmpty.GetComponent<Text> ().text)) {
				answerIndex = j;
				break;
			}
		}

	}

	public void DeployAnswerType(IAnswer answerType){
		answerType.DeployAnswerType ();
	}

}

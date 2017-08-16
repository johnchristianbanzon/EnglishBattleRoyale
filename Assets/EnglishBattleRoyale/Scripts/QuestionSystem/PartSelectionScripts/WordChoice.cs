using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class WordChoice : MonoBehaviour, ISelection
{
	public bool justAnswered = false;
	private string questionAnswer = "";
	private int numberOfCorrectAnswer = 2;
	public GameObject[] selectionButtons = new GameObject[4];
	private List<GameObject> answerClicked = new List<GameObject> ();
	private List<GameObject> answerButtons = new List<GameObject> ();
	private List<string> answerString = new List<string> ();
	private GameObject selectedObject;
	private int hintIndex = 0;

	public void OnClickSelection (GameObject clickedButton)
	{
		selectedObject = clickedButton;
		Debug.Log (answerClicked.Count);
		if (!justAnswered) {
			AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
			GameObject wordClicked = clickedButton;
			string wordClickedString = wordClicked.GetComponentInChildren<Text> ().text;
			if (wordClicked.GetComponent<Image> ().color == Color.gray) {
				wordClicked.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
				answerClicked.Remove (wordClicked);
			} else {
				wordClicked.GetComponent<Image> ().color = Color.gray;
				answerClicked.Add (wordClicked);
				if (answerClicked.Count.Equals (2)) {
					string answerClicked1 = answerClicked [0].GetComponentInChildren<Text> ().text.ToUpper ();
					string answerClicked2 = answerClicked [1].GetComponentInChildren<Text> ().text.ToUpper ();
					CheckAnswer (answerClicked1, answerClicked2);
				}
			}
		}
	}

	private bool isAnswerCorrect;
	private void CheckAnswer (string answerClicked1, string answerClicked2)
	{
		if (answerString.Contains (answerClicked1) && answerString.Contains (answerClicked2)) {
			isAnswerCorrect = true;
		} else {
			isAnswerCorrect = false;
		}
		ShowCorrectAnswer (isAnswerCorrect);
		Invoke ("CheckIfCorrect",0.3f);
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

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{ 
		if (hintIndex < numberOfCorrectAnswer) {
			answerButtons [answerClicked.Count].GetComponent<Button> ().interactable = false;
			OnClickSelection (answerButtons [answerClicked.Count].gameObject);
			this.hintIndex++;
		}
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
				answerClicked [i].GetComponent<Image> ().color =  new Color (94f / 255, 255f / 255f, 148f / 255f);
			}
			answerClicked.Clear ();
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
				selectionButtons [randomNum].GetComponentInChildren<Text> ().text = temp [i].ToString ().ToUpper ();
				answerButtons.Add (selectionButtons [randomNum]);
			} else {
				selectionButtons [randomNum].GetComponentInChildren<Text> ().text = temp [i].ToUpper ();
			}
			selectionButtons [randomNum].GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
			selectionButtons [randomNum].GetComponent<Button> ().interactable = true;
		}
		QuestionSystemController.Instance.correctAnswerButtons = answerButtons;
		answerString.Add (answerButtons [0].GetComponentInChildren<Text> ().text.ToUpper ());
		answerString.Add (answerButtons [1].GetComponentInChildren<Text> ().text.ToUpper ());
	}
}

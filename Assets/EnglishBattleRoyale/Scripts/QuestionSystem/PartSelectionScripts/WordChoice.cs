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
				if (answerClicked.Count >= 2) {
					string answerClicked1 = answerClicked [0].GetComponentInChildren<Text> ().text.ToUpper ();
					string answerClicked2 = answerClicked [1].GetComponentInChildren<Text> ().text.ToUpper ();
					CheckIfCorrect (answerClicked1, answerClicked2);
				}
			}
		}
	}

	public void ShowCorrectAnswer ()
	{

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

	private void CheckIfCorrect (string answerClicked1, string answerClicked2)
	{
		if (answerString.Contains (answerClicked1) && answerString.Contains (answerClicked2)) {
			QuestionSystemController.Instance.CheckAnswer (true);
			answerClicked.Clear ();
		} else {
			QuestionSystemController.Instance.CheckAnswer (false);
			if (hintIndex > 0) {
				answerClicked.Remove (selectedObject);
			} else {
				answerClicked.Clear ();
			}
			for (int i = 0; i < selectionButtons.Length; i++) {
				if (selectionButtons [i].GetComponent<Button> ().interactable) {
					selectionButtons [i].GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
				}
			}
		}
	}

	public void ShowSelectionType (string questionAnswer, Action<List<GameObject>> onSelectCallBack)
	{
		this.gameObject.SetActive (true);
		this.questionAnswer = questionAnswer;
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

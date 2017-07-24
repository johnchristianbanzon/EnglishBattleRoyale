using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WordChoice : MonoBehaviour, ISelection
{
	public bool justAnswered = false;
	private string questionAnswer = "";
	public GameObject[] selectionButtons = new GameObject[4];
	private List<GameObject> answerClicked = new List<GameObject> ();
	private List<GameObject> answerButtons = new List<GameObject> ();
	private List<string> answerString = new List<string>();

	public void OnClickSelection ()
	{
		if (!justAnswered) {
			AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
			GameObject wordClicked = EventSystem.current.currentSelectedGameObject;
			string wordClickedString = wordClicked.transform.GetChild (0).GetComponent<Text> ().text;
			if (wordClicked.GetComponent<Image> ().color == Color.gray) {
				wordClicked.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
				answerClicked.Remove (wordClicked);

			} else {
				wordClicked.GetComponent<Image> ().color = Color.gray;
				answerClicked.Add (wordClicked);
				if (answerClicked.Count == 2){
					string answerClicked1 = answerClicked [0].transform.GetChild(0).GetComponent<Text>().text.ToUpper();
					string answerClicked2 = answerClicked [1].transform.GetChild(0).GetComponent<Text>().text.ToUpper();
					CheckIfCorrect (answerClicked1,answerClicked2);
				}
			}
	}
	}
	private void CheckIfCorrect(string answerClicked1, string answerClicked2){
		answerClicked.Clear ();

		if (answerString.Contains (answerClicked1) && answerString.Contains (answerClicked2)) {
			QuestionSystemController.Instance.CheckAnswer(true);
		} else {
			QuestionSystemController.Instance.CheckAnswer(false);
		}
	}
	public void DeploySelectionType(string questionAnswer){
		this.gameObject.SetActive (true);
		this.questionAnswer = questionAnswer;
	}
	public void ShuffleSelection ()
	{
		
		answerButtons.Clear ();
		answerString.Clear ();
		int numberOfAnswers = 2;
		List <int> randomList = new List<int> ();
		string[] temp = questionAnswer.Split ('/');
		int whileindex = 0;
		for (int i = 0; i < selectionButtons.Length; i++) {
			int randomnum = UnityEngine.Random.Range (0, 4); 
			while (randomList.Contains (randomnum)) {
				randomnum = UnityEngine.Random.Range (0, selectionButtons.Length);
				whileindex++;
			}
			randomList.Add (randomnum);
			string wrongChoiceGot = QuestionBuilder.GetRandomChoices ();
		
			if (i < numberOfAnswers) {
				selectionButtons [randomnum].GetComponentInChildren<Text> ().text = temp [i].ToString ().ToUpper ();
				answerButtons.Add (selectionButtons [randomnum]);

			} else {
				selectionButtons [randomnum].GetComponentInChildren<Text> ().text = wrongChoiceGot;
			}
			selectionButtons[randomnum].GetComponent<Image> ().color = new Color(94f/255,255f/255f,148f/255f);
		}
		QuestionSystemController.Instance.correctAnswerButtons = answerButtons;
		answerString.Add (answerButtons [0].GetComponentInChildren<Text> ().text.ToUpper ());
		answerString.Add (answerButtons [1].GetComponentInChildren<Text> ().text.ToUpper ());
	}
}

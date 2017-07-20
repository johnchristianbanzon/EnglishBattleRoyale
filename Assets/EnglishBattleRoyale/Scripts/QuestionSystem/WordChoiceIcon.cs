using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WordChoiceIcon : BaseQuestion, IQuestion
{

	private bool justAnswered = false;
	private List<GameObject> answerClicked = new List<GameObject>();
	private string answer1 = "";
	private string answer2 = "";
	public GameObject gpText;
	public Text questionType;
	public Text questionText;
	new public GameObject[] selectionButtons = new GameObject[4];

	public void Activate (Action<int,int> Result)
	{
//		QuestionBuilder.PopulateQuestion ("wordchoice",gameObject);
		currentRound = 1;
		correctAnswers = 0;
		NextQuestion ();
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;
	}
		
	public void NextQuestion ()
	{
		LoadQuestion (QuestionSystemEnums.QuestionType.Synonym);
		SelectionInit ();
	}
		
	public void TweenCallBack(){
		gpText.transform.DOScale (new Vector3(1,1,1),1.0f);
		gpText.GetComponent<Text> ().text = " ";

	}

	public void OnFinishQuestion(){
		TweenCallBack ();
		justAnswered = false;
		QuestionController qc = new QuestionController();
		hasSkippedQuestion = false;
		Clear ();
		currentRound = currentRound + 1;
		NextQuestion ();
		qc.Returner (delegate {
			qc.onFinishQuestion = true;
		}, currentRound, correctAnswers);
		if (currentRound == 4) {
			Clear ();
		}
	}

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
					CheckIfCorrect(answerClicked1,answerClicked2);
					}		
				}
			}
	}
		
	private void CheckIfCorrect(string answerClicked1, string answerClicked2){
		foreach (GameObject g in selectionButtons) {
			g.GetComponent<Image> ().color = new Color(94f/255,255f/255f,148f/255f);
		}
		if ((answerClicked1.Equals (answer1) || answerClicked1.Equals (answer2)) &&
		   (answerClicked2.Equals (answer1) || answerClicked2.Equals (answer2))) {
			CheckAnswer (true);
		} else {

			CheckAnswer (false);
		}
	}

	public void SelectionInit ()
	{
		answerButtons.Clear ();
		int numberOfAnswers = 2;
		List <int> randomList = new List<int>();
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
			selectionButtons [randomnum].transform.GetChild (0).GetComponent<Text> ().text = 
				i < numberOfAnswers ? temp [i].ToString ().ToUpper () : 
				wrongChoiceGot;
			if (i < numberOfAnswers) {
				answerButtons.Add (selectionButtons[randomnum]);
			}
		}
		answer1 = answerButtons [0].GetComponentInChildren<Text> ().text.ToUpper ();
		answer2 = answerButtons [1].GetComponentInChildren<Text> ().text.ToUpper ();
	}

	public void Clear ()
	{
		answerClicked.Clear ();
		foreach (GameObject g in selectionButtons) {
			g.GetComponent<Image> ().color = new Color(94f/255,255f/255f,148f/255f);
		}
	}
}

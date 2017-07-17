using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ChangeOrderIcon : BaseQuestion, IQuestion
{
	
	public GameObject inputPrefab;
	public GameObject outputPrefab;
	public GameObject answerContent;
	public GameObject selectionContent;
	public GameObject gpText;

	private QuestionSystemEnums.QuestionType[] questionTypes = new QuestionSystemEnums.QuestionType[3]{
		QuestionSystemEnums.QuestionType.Antonym,
		QuestionSystemEnums.QuestionType.Synonym,
		QuestionSystemEnums.QuestionType.Definition
	};

	public void Activate (Action<int,int> Result)
	{
		//QuestionBuilder.PopulateQuestion ("SelectChangeTyping",gameObject);
		currentRound = 1;
		correctAnswers = 0;
		NextQuestion ();
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;

	}

	public void NextQuestion ()
	{
		LoadQuestion (questionTypes[UnityEngine.Random.Range(0,questionTypes.Length)]);
		PopulateSelectionHolder (gameObject, inputPrefab, selectionContent);
		PopulateAnswerHolder (gameObject, inputPrefab, answerContent);
		ShuffleAlgo ();
	}

	public void OnFinishQuestion ()
	{
		TweenCallBack ();
		hasSkippedQuestion = false;
		Clear ();
		QuestionController.Instance.Stoptimer = true;
		answerIndex = 1;
		currentRound = currentRound + 1;

		NextQuestion ();
		QuestionController.Instance.Returner (delegate {
			QuestionController.Instance.onFinishQuestion = true;
		}, currentRound, correctAnswers);
		if (currentRound == 4) {
			Clear ();
		}
	}

	public void TweenCallBack ()
	{
		TweenLogic.TweenTextScale (gpText.transform, Vector3.one, 1.0f);
		gpText.GetComponent<Text> ().text = " ";
	}

	public void ShuffleAlgo ()
	{
		List<int> RandomExist = new List<int> ();
		string temp = questionAnswer;

		int letterno = 0;
		int randomnum = 0;      
		for (int z = 0; z < temp.Length; z++) {
			randomnum = UnityEngine.Random.Range (0, questionAnswer.Length);        
			int whileindex = 0;
			while (true) {
				if (whileindex > 100) {
					break;
				}
				bool index = RandomExist.Contains (randomnum);
				if (index) {
					randomnum = UnityEngine.Random.Range (0, questionAnswer.Length);
				} else {
					break;
				}
				whileindex++;
			}
			selectionButtons [letterno].transform.GetChild (0).GetComponent<Text> ().text = 
				temp [randomnum].ToString ().ToUpper ();
			RandomExist.Add (randomnum);
			letterno = letterno + 1;
		}
		string answerGot = "";
		foreach(GameObject g in selectionButtons){
			answerGot += g.GetComponentInChildren<Text> ().text;
		}
		if (answerGot == questionAnswer) {
			ShuffleAlgo ();
		}

	}

	public void Clear ()
	{

		answerIndex = 1;
		foreach (GameObject i in selectionButtons) {
			Destroy (i);
		}
		foreach (GameObject o in answerButtons) {
			Destroy (o);
		}
		gameObject.transform.GetChild (0).GetComponent<Text> ().text = "";
	}
}

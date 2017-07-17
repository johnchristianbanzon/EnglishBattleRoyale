using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class SelectLetterIcon :  BaseQuestion , IQuestion
{
	public GameObject inputPrefab;
	public GameObject gpText;
	new  public GameObject[] selectionButtons = new GameObject[12];
	public GameObject answerContent;

	private QuestionSystemEnums.QuestionType[] questionTypes = new QuestionSystemEnums.QuestionType[3]{
		QuestionSystemEnums.QuestionType.Antonym,
		QuestionSystemEnums.QuestionType.Synonym,
		QuestionSystemEnums.QuestionType.Definition
	};

	public void Activate (Action<int,int> result)
	{
		currentRound = 1;
		correctAnswers = 0;
		NextQuestion ();
		QuestionController.Instance.OnResult = result;
	}
		
	public void NextQuestion ()
	{
		ClearAnswerList ();
		LoadQuestion (questionTypes[UnityEngine.Random.Range(0,questionTypes.Length)]);
		PopulateAnswerHolder (gameObject, inputPrefab, answerContent);
		SelectionInit ();

	}
		
	public void TweenCallBack ()
	{
		TweenLogic.TweenTextScale (gpText.transform, Vector3.one, 1.0f);
		gpText.GetComponent<Text> ().text = " ";
	}

	public void OnFinishQuestion ()
	{
		TweenCallBack ();
		hasSkippedQuestion = false;
		QuestionController.Instance.Stoptimer = true;
		ClearAnswerList ();
		answerIndex = 1;
		currentRound += 1;
		NextQuestion ();
		QuestionController.Instance.Returner (delegate {
			QuestionController.Instance.onFinishQuestion = true;
		}, currentRound, correctAnswers);
	}


	public void SelectionInit ()
	{
		answerGameObject.Clear ();


		int numberOfLetters = questionAnswer.Length;
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		List <int> randomList = new List<int>();
		int whileindex = 0;
		for (int i = 0; i < selectionButtons.Length; i++) {
			int randomnum = UnityEngine.Random.Range (0, selectionButtons.Length); 
			while (randomList.Contains (randomnum)) {
				randomnum = UnityEngine.Random.Range (0, selectionButtons.Length);
				whileindex++;
				if (whileindex > 100) {
					break;
				}
			}
			randomList.Add (randomnum);
			selectionButtons [randomnum].GetComponentInChildren<Text>().text = i < questionAnswer.Length ?
				""+questionAnswer [i] : ""+alphabet [UnityEngine.Random.Range (1, 26)];
		}
	}


}

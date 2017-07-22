using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class SelectLetter : MonoBehaviour, ISelection
{
	public GameObject[] selectionButtons = new GameObject[12];
	private string questionAnswer;
	public void OnSelect(){
		QuestionSystemController.Instance.partAnswerController.SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
		EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text> ().text = "";
	}

	public void DeploySelectionType(string questionAnswer){
		gameObject.SetActive (true);
		this.questionAnswer = questionAnswer;
	}
	public void ShuffleSelection ()
	{
		int numberOfLetters = questionAnswer.Length;
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		List <int> randomList = new List<int> ();
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
			gameObject.transform.GetChild (randomnum).transform.GetComponentInChildren<Text> ().text =
				i < questionAnswer.Length ?
				"" + questionAnswer [i].ToString().ToUpper() : "" + alphabet [UnityEngine.Random.Range (1, 26)];
		}
	}

}

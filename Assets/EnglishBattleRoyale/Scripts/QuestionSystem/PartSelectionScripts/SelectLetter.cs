using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class SelectLetter : MonoBehaviour
{
	public GameObject[] selectionButtons = new GameObject[12];

	public void OnSelect(){
		QuestionSystemController.Instance.answerController.SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
		EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text> ().text = "";
	}

	public void SelectLetterShuffle (string questionAnswer)
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

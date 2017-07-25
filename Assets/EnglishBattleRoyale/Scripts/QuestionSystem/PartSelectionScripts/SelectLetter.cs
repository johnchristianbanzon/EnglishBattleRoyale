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
		QuestionSystemController.Instance.partAnswer.SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
		EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text> ().text = "";
	}

	public void DeploySelectionType(string questionAnswer){
		gameObject.SetActive (true);
		ShuffleSelection ();
		this.questionAnswer = questionAnswer;
	}


	public void RemoveSelection(int hintIndex){

	}


	/// <summary>
	/// Shuffles the Selection by placing each letters in questionAnswer to random selection location 
	/// then populates the others with random letters inside alphabet
	/// </summary>
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
			if(i<questionAnswer.Length){
				selectionButtons [randomnum].GetComponentInChildren<Text> ().text = questionAnswer [i].ToString ().ToUpper ();}
			else{
				selectionButtons [randomnum].GetComponentInChildren<Text> ().text = alphabet [UnityEngine.Random.Range (1, 26)].ToString();
			}
		}
	}

}

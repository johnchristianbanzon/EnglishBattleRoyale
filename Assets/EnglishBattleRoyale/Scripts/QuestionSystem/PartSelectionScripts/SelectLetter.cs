using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class SelectLetter : MonoBehaviour, ISelection
{
	public GameObject[] selectionButtons = new GameObject[12];
	private string questionAnswer;

	public void OnSelect(){
		QuestionSystemController.Instance.partAnswer.fillAnswer.SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
		EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text> ().text = "";
//		EventSystem.current.currentSelectedGameObject.SetActive (false);
	}

	public void HideSelectionType(){
		gameObject.SetActive (false);
	}

	public void ShowCorrectAnswer(){
		
	}

	public void ShowSelectionType (string questionAnswer,Action<List<GameObject>> onSelectCallBack){
		gameObject.SetActive (true);
		this.questionAnswer = questionAnswer;
		ShuffleSelection ();

	}



	public void ShowSelectionHint(int hintIndex){

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

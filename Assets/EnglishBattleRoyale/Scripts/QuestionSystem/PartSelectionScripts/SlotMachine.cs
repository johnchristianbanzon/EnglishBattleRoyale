using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class SlotMachine : MonoBehaviour{

	public GameObject gpText;
	public GameObject[] roulletes = new GameObject[12];
	public Text questionText;
	private List<GameObject> roulleteText = new List<GameObject> ();
	private bool gotAnswer = false;
	private List<Color> previousSlotColor = new List<Color>();
	private List<GameObject> correctButtons = new List<GameObject> ();
	public SlotMachineOnChange[]  slotController = new SlotMachineOnChange[6];
	public string correctAnswer;

	public void findSlotMachines(string questionAnswer){
		gotAnswer = false;
		roulleteText.Clear ();
		GameObject content;
		for (int i = 0; i < questionAnswer.Length; i++) {
			content = roulletes [i];
			for (int j = 0; j < 3; j++) {
				roulleteText.Add (content.transform.GetChild(j).gameObject);
		 	}
		}
		for (int i = 0; i < questionAnswer.Length; i++) {
			roulletes [i].transform.parent.parent.parent.parent.gameObject.SetActive (true);
		}
		for(int i = 6 ; i > questionAnswer.Length ;i--){
			roulletes[i-1].transform.parent.parent.parent.parent.gameObject.SetActive(false);
		}
		correctAnswer = questionAnswer;
	}

	void Update(){
		GetSlotAnswers ();
	}

	public void GetSlotAnswers(){
		string writtenAnswer = "";
		for (int i = 0; i < slotController.Length - (slotController.Length - correctAnswer.Length); i++) {
			writtenAnswer += slotController [i].GetSlots ().GetComponentInChildren<Text> ().text;
		}
		if (writtenAnswer.Equals(correctAnswer)) {
			if (!gotAnswer) {
				gotAnswer = true;
				QuestionSystemController.Instance.CheckAnswer (true);
			}
		}
	}

	public void ShuffleAlgo (string questionAnswer)
	{
		correctButtons.Clear ();
		findSlotMachines (questionAnswer);
		string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int letterIndex = 0;
		int letterStartIndex = 0;
		int letterEndIndex = 3;
		int randomnum = UnityEngine.Random.Range (letterStartIndex+1, letterEndIndex);
		for (int i = 0; i < roulleteText.Count; i++) {
			roulleteText [i].transform.GetChild (0).GetComponent<Text> ().text = (i%randomnum)==0 ?
				questionAnswer [letterIndex].ToString ().ToUpper ():
				Letters [UnityEngine.Random.Range (0, Letters.Length)].ToString ().ToUpper ();
			if ((i % randomnum) == 0) {
				letterIndex += 1;
				letterStartIndex = letterEndIndex;
				letterEndIndex = letterEndIndex + 3;
				randomnum = UnityEngine.Random.Range (letterStartIndex, letterEndIndex);
				correctButtons.Add (roulleteText [i]);
				previousSlotColor.Add (roulleteText [i].GetComponent<Image> ().color);
			}
		}
		QuestionSystemController.Instance.correctAnswerButtons = correctButtons;
	}

}

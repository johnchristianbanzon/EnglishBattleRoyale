using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SlotMachine : MonoBehaviour,ISelection
{
	public SlotMachineEvent[] slots = new SlotMachineEvent[6];
	private List<Color> previousSlotColor = new List<Color> ();
	private string questionAnswer = "";

	public void ShowCorrectAnswer ()
	{
		
	}

	public void HideSelectionType ()
	{
		gameObject.SetActive (false);
	}

	public void HideSelectionHint(){

	}

	public void ShowSelectionType (string questionAnswer, Action<List<GameObject>> onSelectCallBack)
	{
		gameObject.SetActive (true);
		this.questionAnswer = questionAnswer;
		for (int i = 0; i < slots.Length; i++) {
			if (i >= questionAnswer.Length) {
				slots [i].gameObject.SetActive (false);
			}
		}
		InitSlots ();
	}

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{
		
	}

	public void CheckAnswer(){
		string answerWritten = "";
		List<GameObject> correctAnswerSlots = new List<GameObject> ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			answerWritten += slots[i].GetSelectedSlot ().GetComponentInChildren<Text>().text;
			correctAnswerSlots.Add (slots [i].correctLetterAnswer);
		}
		QuestionSystemController.Instance.correctAnswerButtons = correctAnswerSlots;
		if (answerWritten == questionAnswer) {
			QuestionSystemController.Instance.CheckAnswer (true);
		}
	}

	public void InitSlots ()
	{
		for (int i = 0; i < questionAnswer.Length; i++) {
			slots [i].Init (questionAnswer[i]);
		}
	}

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class SlotMachine : MonoBehaviour,ISelection
{
	public SlotMachineEvent[] slots = new SlotMachineEvent[6];
	private string questionAnswer = "";
	List<GameObject> correctAnswerSlots = new List<GameObject>();

	public void ShowCorrectAnswer (bool isAnswerCorrect)
	{
		//TO BE IMPLEMENTED
	}

	public void HideSelectionType ()
	{
		gameObject.SetActive (false);
	}

	public void HideSelectionHint ()
	{
		if (slots [0].getOverAllHintLeft() > 1) {
			int randomSlot = UnityEngine.Random.Range (0, slots.Length-(slots.Length-correctAnswerSlots.Count));

			while (slots [randomSlot].hintContainersLeft <= 0) {
				randomSlot = UnityEngine.Random.Range (0, slots.Length-(slots.Length-correctAnswerSlots.Count));
			}
			slots [randomSlot].HideHintContainer ();
		}
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
		ShowAnswer showAnswer = QuestionSystemController.Instance.partAnswer.showAnswer;
		List<int> selectionIndex = new List<int>();
		for (int i = 0; i < showAnswer.hintContainers.Count; i++) {
			if(showAnswer.hintContainers[i].GetComponent<Button>().interactable){
				selectionIndex.Add (i);
			}
		}
		selectionIndex = ListShuffleUtility.Shuffle (selectionIndex);
		showAnswer.hintContainers[selectionIndex[0]].GetComponentInChildren<Text> ().text = questionAnswer[selectionIndex[0]].ToString();
		showAnswer.hintContainers [selectionIndex [0]].GetComponent<Button> ().interactable = false;
	}

	private string GetAnswer(){
		string answerWritten = "";
		correctAnswerSlots.Clear ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			answerWritten += slots[i].GetSelectedSlot ().GetComponentInChildren<Text>().text;
			correctAnswerSlots.Add (slots [i].correctLetterAnswer);
		}
		QuestionSystemController.Instance.correctAnswerButtons = correctAnswerSlots;
		return answerWritten;
	}

	public void CheckAnswer(){
		if (GetAnswer () == questionAnswer) {
			QuestionSystemController.Instance.CheckAnswer (true);
		}
	}

	public void InitSlots ()
	{
		for (int i = 0; i < questionAnswer.Length; i++) {
			slots [i].Init (questionAnswer[i]);
		}
		GetAnswer ();
	}

}

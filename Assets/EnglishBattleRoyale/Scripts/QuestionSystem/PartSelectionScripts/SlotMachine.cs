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

	public void ShowCorrectAnswer ()
	{

	}

	public void HideSelectionType ()
	{
		gameObject.SetActive (false);
	}
		
	public void HideSelectionHint ()
	{
		if (slots [0].getOverAllHintLeft() > 0) {
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
		correctAnswerContainer.GetComponentInChildren<Text> ().enabled = true;
		TweenFacade.TweenMoveTo (gameObject.transform, new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y - 40f), 0.3f);
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

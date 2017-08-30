using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.EventSystems;
 public class SlotMachine : MonoBehaviour,ISelection
{
	public SlotMachineEvent[] slots = new SlotMachineEvent[6];
	private string questionAnswer = "";
	List<GameObject> correctAnswerSlots = new List<GameObject>();

	public void ShowCorrectAnswer (bool isAnswerCorrect)
	{
		Color answerColor = new Color();
		if (isAnswerCorrect) {
			answerColor = new Color32 (255, 223, 0, 255);	
		} else {
			answerColor = new Color32 (255, 100, 100, 255);
		}
		for (int i = 0; i < correctAnswerSlots.Count; i++) {
			correctAnswerSlots [i].GetComponent<Image> ().color = answerColor;
			slots [i].isDraggable = false;
		}

	}
	List<GameObject> popUpSelectionList= new List<GameObject> ();
	public GameObject ShowSelectionPopUp(){
		GameObject selectionPopUp = SystemResourceController.Instance.LoadPrefab ("PopUpSlotMachine", SystemPopupController.Instance.popUp);
		for (int i = 0; i < selectionPopUp.transform.GetChild(0).childCount; i++) {
			popUpSelectionList.Add(selectionPopUp.transform.GetChild(0).GetChild(i).gameObject);
		}
		if (popUpSelectionList.Count > 0) {
			InvokeRepeating ("PopUpMoveSelection", 0, 0.7f);
		}
		return selectionPopUp;
	}

	int popUpSelectionCounter = 0;
	private void PopUpMoveSelection(){
		if (popUpSelectionCounter < 3) {
			popUpSelectionList [0].GetComponent<SlotMachineEvent> ().OnClickDownButton ();
			popUpSelectionList [1].GetComponent<SlotMachineEvent> ().OnClickUpButton ();
			popUpSelectionList [2].GetComponent<SlotMachineEvent> ().OnClickDownButton ();
			popUpSelectionList [3].GetComponent<SlotMachineEvent> ().OnClickUpButton ();
		} else {
			CancelInvoke ();
		}
		popUpSelectionCounter++;
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
		QuestionSystemController.Instance.questionRoundHasStarted = true;
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
		if (QuestionSystemController.Instance.questionHint.hasHintAvailable) {
			ShowAnswer showAnswer = QuestionSystemController.Instance.partAnswer.showAnswer;
			List<int> selectionIndex = new List<int> ();
			for (int i = 0; i < showAnswer.hintContainers.Count; i++) {
				if (showAnswer.hintContainers [i].GetComponent<Button> ().interactable) {
					selectionIndex.Add (i);
				}
			}
//		selectionIndex = ListShuffleUtility.Shuffle (selectionIndex);
			showAnswer.hintContainers [selectionIndex [0]].GetComponentInChildren<Text> ().text = questionAnswer [selectionIndex [0]].ToString ();
			showAnswer.hintContainers [selectionIndex [0]].GetComponent<Button> ().interactable = false;
		}
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
		Debug.Log (QuestionSystemController.Instance.questionRoundHasStarted);
		if (GetAnswer () == questionAnswer) {
			ShowCorrectAnswer (true);
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

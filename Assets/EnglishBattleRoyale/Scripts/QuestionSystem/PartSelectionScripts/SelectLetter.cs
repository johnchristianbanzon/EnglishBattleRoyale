using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;

public class SelectLetter : MonoBehaviour, ISelection
{
	public SelectLetterEvent[] selectionButtons = new SelectLetterEvent[12];
	public FillAnswerType fillAnswer;
	public string questionAnswer;
	List<SelectLetterEvent> correctContainers = new List<SelectLetterEvent> ();

	public void OnSelect ()
	{
		QuestionSystemController.Instance.partAnswer.fillAnswer.GetAnswerWritten ();
		if (questionAnswer.Length > QuestionSystemController.Instance.partAnswer.fillAnswer.GetAnswerWritten ().Length) {
			QuestionSystemController.Instance.partAnswer.fillAnswer.SelectionLetterGot (EventSystem.current.currentSelectedGameObject);
			EventSystem.current.currentSelectedGameObject.SetActive (false);
		} 
	}

	public void HideSelectionType ()
	{
		gameObject.SetActive (false);
	}

	public void ShowCorrectAnswer ()
	{

	}

	private bool initHideHint = false;
	private List<int> hideSelectionIndex = new List<int> ();

	private List<int> InitHideHint ()
	{
		hideSelectionIndex.AddRange (Enumerable.Range (0, selectionButtons.Length));
		for (int i = 0; i < hideSelectionIndex.Count; i++) {
			if (questionAnswer.Contains (selectionButtons [hideSelectionIndex [i]].GetComponentInChildren<Text> ().text)) {
				hideSelectionIndex.RemoveAt (i);
				i--;
			}
		}
		return hideSelectionIndex;
	}

	public void HideSelectionHint ()
	{
		if (MyConst.ALLOW_REMOVE_SELECTLETTER.Equals (1)) {
			if (!initHideHint) {
				InitHideHint ();
				initHideHint = true;
			}
			if (hideSelectionIndex.Count > 0) {
				int randomHintIndex = UnityEngine.Random.Range (0, hideSelectionIndex.Count);
				selectionButtons [hideSelectionIndex [randomHintIndex]].GetComponent<EventTrigger> ().enabled = false;
				selectionButtons [hideSelectionIndex [randomHintIndex]].GetComponent<Button> ().interactable = false;

				hideSelectionIndex.RemoveAt (randomHintIndex);
			}
		}
	}

	public void ShowSelectionType (string questionAnswer, Action<List<GameObject>> onSelectCallBack)
	{
		gameObject.SetActive (true);
		this.questionAnswer = questionAnswer;
		ShuffleSelection ();
	}

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{
		if (MyConst.ALLOW_SHOW_SELECTLETTER.Equals (1)) {
			List<int> correctContainerIndexList = new List<int> ();
			for (int i = 0; i < correctContainers.Count; i++) {
				if (fillAnswer.answerContainers [i].transform.childCount.Equals (0)) {
					correctContainerIndexList.Add (i);
				} else {
					if (!questionAnswer[i].ToString().Equals(fillAnswer.answerContainers [i].GetComponentInChildren<SelectLetterEvent> ().letter.text)) {
						correctContainerIndexList.Add (i);
					}
				}
			}
			correctContainerIndexList = ListShuffleUtility.Shuffle (correctContainerIndexList);
			int firstContainerIndex = correctContainerIndexList[0];
			if (fillAnswer.answerContainers [firstContainerIndex].GetComponentInChildren<SelectLetterEvent> () != null) {
				fillAnswer.answerContainers [firstContainerIndex].transform.GetChild(0).transform.SetParent (transform);
			}
			correctContainers [firstContainerIndex].transform.SetParent (fillAnswer.answerContainers [correctContainers [firstContainerIndex].correctAnswerIndex].transform);
			correctContainers [firstContainerIndex].GetComponent<EventTrigger> ().enabled = false;
			correctContainers [firstContainerIndex].GetComponent<Button> ().interactable = false;
			correctContainerIndexList.Remove(firstContainerIndex);
		}
	}

	/// <summary>
	/// Shuffles the Selection by placing each letters in questionAnswer to random selection location 
	/// then populates the others with random letters inside alphabet
	/// </summary>
	public void ShuffleSelection ()
	{
		List<int> randomizedIndexList = new List<int> ();
		correctContainers.Clear ();
		randomizedIndexList.AddRange (Enumerable.Range (0, selectionButtons.Length));
		ListShuffleUtility.Shuffle (randomizedIndexList);
		for (int i = 0; i < selectionButtons.Length; i++) {
			bool isCorrect = false;
			if (i < questionAnswer.Length) {
				isCorrect = true;
			}
			selectionButtons [randomizedIndexList [0]].Init (isCorrect, i);
			if (isCorrect) {
				correctContainers.Add (selectionButtons [randomizedIndexList [0]]);
			}
			randomizedIndexList.RemoveAt (0);
		}
	}

}

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
//		if (QuestionSystemConst.ALLOW_REMOVE_SELECTLETTER.Equals (1)) {
//			if (!initHideHint) {
//				InitHideHint ();
//				initHideHint = true;
//			}
//			if (hideSelectionIndex.Count > 0) {
//				int randomHintIndex = UnityEngine.Random.Range (0, hideSelectionIndex.Count);
//				selectionButtons [hideSelectionIndex [randomHintIndex]].gameObject.SetActive (false);
//				hideSelectionIndex.RemoveAt (randomHintIndex);
//			}
//		}
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
			correctContainers [0].OnSelectLetter (correctContainers [0].gameObject);
			correctContainers [0].GetComponent<Button> ().interactable = false;
			correctContainers [0].GetComponent<Image> ().raycastTarget = false;
			correctContainers.RemoveAt (0);
		}
	}

	/// <summary>
	/// Shuffles the Selection by placing each letters in questionAnswer to random selection location 
	/// then populates the others with random letters inside alphabet
	/// </summary>
	public void ShuffleSelection ()
	{
		List<int> randomizedIndexList = new List<int> ();

		randomizedIndexList.AddRange (Enumerable.Range (0, selectionButtons.Length));
		ListShuffleUtility.Shuffle (randomizedIndexList);
		for (int i = 0; i < selectionButtons.Length; i++) {
			bool isCorrect = false;
			if (i < questionAnswer.Length) {
				isCorrect = true;
			}
			selectionButtons [randomizedIndexList[0]].Init (isCorrect,i);
			if (isCorrect) {
				correctContainers.Add (selectionButtons [randomizedIndexList [0]]);
			}
			randomizedIndexList.RemoveAt (0);
		}
	}

}

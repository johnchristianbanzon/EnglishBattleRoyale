﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;

public class SelectLetter : MonoBehaviour, ISelection
{
	public GameObject[] selectionButtons = new GameObject[12];
	private string questionAnswer;


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
		if (QuestionSystemConst.ALLOW_REMOVE_SELECTLETTER.Equals (1)) {
			if (!initHideHint) {
				InitHideHint ();
				initHideHint = true;
			}

			if (hideSelectionIndex.Count > 0) {
				int randomHintIndex = UnityEngine.Random.Range (0, hideSelectionIndex.Count);
				selectionButtons [hideSelectionIndex [randomHintIndex]].SetActive (false);
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
		if (QuestionSystemConst.ALLOW_SHOW_SELECTLETTER.Equals (1)) {
			GameObject letterHint = null;
			GameObject[] selectionArray = QuestionSystemController.Instance.partSelection.selectLetter.selectionButtons;
			for (int i = 0; i < selectionArray.Length; i++) {
				if (selectionArray [i].GetComponentInChildren<Text> ().text.Equals (questionAnswer [hintIndex].ToString ())
				    && selectionArray [i].activeInHierarchy) {
					letterHint = selectionArray [i];
					QuestionSystemController.Instance.partAnswer.fillAnswer.selectionIdentifier [hintIndex] = letterHint;
					letterHint.SetActive (false);
					break;
				}
			}
			if (!string.IsNullOrEmpty (correctAnswerContainer.GetComponentInChildren<Text> ().text)) {
				QuestionSystemController.Instance.partAnswer.fillAnswer.OnAnswerClick (correctAnswerContainer.GetComponent<Button> ());
			} 
			correctAnswerContainer.GetComponent<Button> ().enabled = false;
			correctAnswerContainer.GetComponentInChildren<Text> ().text = questionAnswer [hintIndex].ToString ();
			TweenFacade.TweenScaleToLarge (correctAnswerContainer.transform, Vector3.one, 0.3f);
			correctAnswerContainer.GetComponent<Image> ().color = new Color (255 / 255, 102 / 255f, 51 / 255f);

		}
	}

	/// <summary>
	/// Shuffles the Selection by placing each letters in questionAnswer to random selection location 
	/// then populates the others with random letters inside alphabet
	/// </summary>
	public void ShuffleSelection ()
	{
		initHideHint = false;
		int numberOfLetters = questionAnswer.Length;
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		List <int> randomList = new List<int> ();
		int whileindex = 0;
		for (int i = 0; i < selectionButtons.Length; i++) {
			selectionButtons [i].SetActive (true);
			selectionButtons [i].GetComponent<Button> ().enabled = true;
			int randomnum = UnityEngine.Random.Range (0, selectionButtons.Length); 
			while (randomList.Contains (randomnum)) {
				randomnum = UnityEngine.Random.Range (0, selectionButtons.Length);
				whileindex++;
				if (whileindex > 100) {
					break;
				}
			}
			randomList.Add (randomnum);
			if (i < questionAnswer.Length) {
				selectionButtons [randomnum].GetComponentInChildren<Text> ().text = questionAnswer [i].ToString ().ToUpper ();
			} else {
				selectionButtons [randomnum].GetComponentInChildren<Text> ().text = alphabet [UnityEngine.Random.Range (1, 26)].ToString ();
			}
		}
	}

}

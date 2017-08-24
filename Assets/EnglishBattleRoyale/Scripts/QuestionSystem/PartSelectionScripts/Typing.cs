﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Linq;

public class Typing : MonoBehaviour, ISelection
{
	private string questionAnswer;
	public GameObject[] selectionButtons = new GameObject[26];

	public void ShowSelectionType (string questionAnswer, Action<List<GameObject>> onSelectCallBack)
	{
		this.questionAnswer = questionAnswer;
		initHideHint = false;
		gameObject.SetActive (true);
		for (int i = 0; i < selectionButtons.Length; i++) {
			selectionButtons [i].GetComponent<Button> ().interactable = true;
		}
	}

	public void ShowSelectionPopUp(GameObject selectionPopUp){

	}

	public void ShowCorrectAnswer (bool isAnswerCorrect)
	{
		Color answerColor = new Color();
		if (isAnswerCorrect) {
			answerColor = new Color32 (255, 223, 0, 255);
		} else {
			answerColor = new Color32 (255, 100, 100, 255);
		}
		/*
		List<GameObject> answerContainers = QuestionSystemController.Instance.partAnswer.fillAnswer.answerContainers;
		for (int i = 0; i < answerContainers.Count; i++) {
			if (answerContainers [i].transform.childCount > 0) {
				answerContainers [i].transform.GetComponentInChildren<Image> ().color = answerColor;
			} else {
				answerContainers [i].transform.GetComponent<Image> ().color = answerColor;
			}
		}*/
	}

	private bool initHideHint = false;
	private List<int> hideSelectionIndex = new List<int> ();

	private List<int> InitHideHint ()
	{
		hideSelectionIndex.Clear ();
		hideSelectionIndex.AddRange (Enumerable.Range (0, selectionButtons.Length));
		for (int i = 0; i < hideSelectionIndex.Count; i++) {
			if (questionAnswer.Contains (selectionButtons [hideSelectionIndex [i]].transform.GetChild (0).GetComponentInChildren<Text> ().text)) {
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
				selectionButtons [hideSelectionIndex [randomHintIndex]].GetComponent<Button> ().interactable = false;
				hideSelectionIndex.RemoveAt (randomHintIndex);
			}
		}
	}

	public void HideSelectionType ()
	{
		gameObject.SetActive (false);
	}

	FillAnswerType fillAnswer;

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{
		List<int> randomizedIndexList = new List<int> ();
		if (MyConst.ALLOW_SHOW_SELECTLETTER.Equals (1)) {
			fillAnswer = QuestionSystemController.Instance.partAnswer.fillAnswer;
			for (int i = 0; i < fillAnswer.answerContainers.Count; i++) {
				if (fillAnswer.answerContainers [i].transform.childCount.Equals (0)) {
					randomizedIndexList.Add (i);

				} else {
					if (!questionAnswer [i].ToString ().ToUpper ().Equals (fillAnswer.answerContainers [i].GetComponentInChildren<Text> ().text.ToUpper ())) {
						randomizedIndexList.Add (i);
					}
				}
			}
			ListShuffleUtility.Shuffle (randomizedIndexList);
			GameObject answerContainer;
			if (fillAnswer.answerContainers [randomizedIndexList [0]].transform.childCount.Equals (0)) {
				answerContainer = SystemResourceController.Instance.LoadPrefab ("Input-UI", fillAnswer.answerContainers [randomizedIndexList [0]].gameObject);
			} else {
				answerContainer = fillAnswer.answerContainers [randomizedIndexList [0]].transform.GetChild(0).gameObject;
			}
			answerContainer.GetComponentInChildren<Text> ().text = questionAnswer [randomizedIndexList [0]].ToString ();
			answerContainer.GetComponentInChildren<Button> ().interactable = false;
		}
	}

	public void OnSelect ()
	{
		QuestionSystemController.Instance.partAnswer.fillAnswer.
		SelectionLetterGot (EventSystem.current.currentSelectedGameObject);
	}
}

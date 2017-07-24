using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ChangeOrder : MonoBehaviour,ISelection
{
	public GameObject[] selectionContainers = new GameObject[5];
	public GameObject inputContent;
	private int slotLimit = 5;
	private GameObject currentSelectedLetter;
	private bool isDragging = false;
	private int selectedIndex;
	public string questionAnswer;

	public void OnBeginDrag (GameObject selectedButton)
	{
		selectedButton.transform.SetParent (selectedButton.transform.parent.parent);
	}

	public void OnEndDrag (GameObject selectedButton)
	{
		selectedButton.transform.SetParent (inputContent.transform);
		selectedButton.GetComponent<Image> ().raycastTarget = true;
		selectedButton.transform.SetSiblingIndex (selectedIndex);
		QuestionSystemController.Instance.partAnswer.noAnswerController.CheckAnswerFromSelection (GetSelectedAnswer (), questionAnswer);
		isDragging = false;
	}

	public string GetSelectedAnswer ()
	{
		string selectedAnswer = "";
		foreach (Transform child in inputContent.transform) {
			if (child.gameObject.activeInHierarchy) {
				selectedAnswer += child.GetComponentInChildren<Text> ().text;
			}
		}
		return selectedAnswer;
	}

	public void OnSelectionDrag (Button selectedButton)
	{
		isDragging = true;
		Vector2 pos = new Vector2 (0, 0);
		Canvas myCanvas = SystemGlobalDataController.Instance.gameCanvas;
		selectedButton.GetComponent<Image> ().raycastTarget = false;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
		selectedButton.transform.position = myCanvas.transform.TransformPoint (pos);
		//selectedButton.transform.position = Input.mousePosition;
	}

	public void OnDetectDraggedLetter (GameObject touchedObject)
	{
		if (isDragging) {
			selectedIndex = touchedObject.transform.GetSiblingIndex ();
			currentSelectedLetter = touchedObject.gameObject;
		}
	}

	public void ShuffleSelection ()
	{
		for (int i = 0; i < selectionContainers.Length; i++) {
			selectionContainers [i].transform.SetSiblingIndex (UnityEngine.Random.Range (0, selectionContainers.Length));
		}
		if (GetSelectedAnswer ().Equals (questionAnswer)) {
			ShuffleSelection ();
		}
	}
	public void RemoveSelection(){

	}
	public void ResetLetterSelection ()
	{
		foreach (GameObject letter in selectionContainers) {
			letter.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
			letter.SetActive (true);
		}
	}
	/// <summary>
	/// Deploies the type of the selection.
	/// Resets the selections first,
	/// Couples letters to the button if questionAnswer is greater than 5
	/// Places couples inside the list and then replacin the buttons text with the list, respectably
	/// </summary>
	/// <param name="answer">Answer.</param>
	public void DeploySelectionType (string answer)
	{
		gameObject.SetActive (true);
		ResetLetterSelection ();
		QuestionSystemController.Instance.partAnswer
			.noAnswerController.correctAnswerContainer.SetActive (false);
		QuestionSystemController.Instance.correctAnswerButtons = new List<GameObject> (selectionContainers);
		questionAnswer = answer;
		if (answer.Length <= slotLimit) {
			for (int i = 0; i < selectionContainers.Length; i++) {
				if (i < answer.Length) {
					selectionContainers [i].GetComponentInChildren<Text> ().text = answer [i].ToString ();
				} else {
					selectionContainers [i].SetActive (false);
				}
			}
		} else {
			List<string> questioningList = new List<string> ();

			int orderCoupling = questionAnswer.Length % 5;
			for (int j = 0; j < questionAnswer.Length; j++) {
				questioningList.Add (questionAnswer [j].ToString ());
			}
			for (int j = 0; j < orderCoupling; j++) {
				int randomizedIndex = UnityEngine.Random.Range (0, questionAnswer.Length - 1);
				questioningList [randomizedIndex] += questioningList [randomizedIndex + 1];
				questioningList.Remove (questioningList [randomizedIndex + 1].ToString ());
				Debug.Log (questioningList [randomizedIndex]);
			}
			for (int j = 0; j < selectionContainers.Length; j++) {
				selectionContainers [j].GetComponentInChildren<Text> ().text = questioningList [j];
			}

		}
		ShuffleSelection ();
	}

}

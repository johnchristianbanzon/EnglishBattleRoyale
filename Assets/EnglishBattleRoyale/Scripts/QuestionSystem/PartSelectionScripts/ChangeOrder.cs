using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOrder : MonoBehaviour,ISelection
{
	public GameObject[] selectionContainers = new GameObject[5];
	public GameObject selectionContent;
	private bool isDragging = false;
	private int selectedIndex;
	public string questionAnswer;
	private GameObject selectedButton;

	public void OnBeginDrag (GameObject selectedButton)
	{
		this.selectedButton = selectedButton;
		selectedButton.transform.SetParent (this.transform);

	}

	public void OnEndDrag ()
	{
		selectedButton.transform.SetParent (selectionContent.transform);
		selectedButton.GetComponent<Image> ().raycastTarget = true;
		selectedButton.transform.SetSiblingIndex (selectedIndex);
		QuestionSystemController.Instance.partAnswer.noAnswer.CheckAnswerFromSelection (GetSelectedAnswer (), questionAnswer);
		isDragging = false;
	}

	public string GetSelectedAnswer ()
	{
		string selectedAnswer = "";
		foreach (Transform child in selectionContent.transform) {
			if (child.gameObject.activeInHierarchy) {
				selectedAnswer += child.GetComponentInChildren<Text> ().text;
			}
		}
		return selectedAnswer;
	}

	public void OnSelectionDrag ()
	{
		isDragging = true;
		Vector2 pos = new Vector2 (0, 0);
		Canvas myCanvas = SystemGlobalDataController.Instance.gameCanvas;
		selectedButton.GetComponent<Image> ().raycastTarget = false;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
		selectedButton.transform.position = myCanvas.transform.TransformPoint (pos);
	}

	public void OnDetectDraggedLetter (GameObject touchedObject)
	{
		if (isDragging) {
			selectedIndex = touchedObject.transform.GetSiblingIndex ();
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
		QuestionSystemController.Instance.correctAnswerButtons = new List<GameObject> (selectionContainers);
	}

	public void RemoveSelectionHint (int hintIndex)
	{
		//Change order hint 
	}

	public void ClearLetterSelection ()
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
		ClearLetterSelection ();
		questionAnswer = answer;
		if (answer.Length <= selectionContainers.Length) {
			for (int i = 0; i < selectionContainers.Length; i++) {
				if (i < answer.Length) {
					selectionContainers [i].GetComponentInChildren<Text> ().text = answer [i].ToString ();
				} else {
					selectionContainers [i].SetActive (false);
				}
			}
		} else {
			List<string> questioningList = new List<string> ();
			for (int j = 0; j < questionAnswer.Length; j++) {
				questioningList.Add (questionAnswer [j].ToString ());
			}
			OrderCoupling (questioningList);
		}
		ShuffleSelection ();
	}

	/// <summary>
	/// Couples letters if greater than 5
	/// </summary>
	/// <param name="questioningList">Questioning list.</param>
	public void OrderCoupling (List<string> questioningList)
	{
		int orderCouplingCount = questionAnswer.Length % 5;
		List<int> couplingRandomizeList = new List<int> ();
		while (orderCouplingCount > 0) {
			int randomizedIndex = UnityEngine.Random.Range (0, questionAnswer.Length - 1);
			if (!couplingRandomizeList.Contains (randomizedIndex)) {
				questioningList [randomizedIndex] += questioningList [randomizedIndex + 1];
				questioningList.Remove (questioningList [randomizedIndex + 1].ToString ());
				orderCouplingCount--;
			}
		}
		for (int j = 0; j < selectionContainers.Length; j++) {
			selectionContainers [j].GetComponentInChildren<Text> ().text = questioningList [j];
		}

	}


}

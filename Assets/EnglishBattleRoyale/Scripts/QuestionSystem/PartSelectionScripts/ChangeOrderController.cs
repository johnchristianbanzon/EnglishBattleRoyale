using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOrderController : MonoBehaviour, ISelection
{
	public string questionAnswer;
	public GameObject[] selectionContainers = new GameObject[5];
	public GameObject selectionViewContent;

	public void DeploySelectionType (string answer)
	{
		gameObject.SetActive (true);
		PopulateSelectionContainers (answer);
	}

	/// <summary>
	/// Getting the answer from the selection by adding the letters on its children position, not the index on array
	/// </summary>
	/// <returns>The selected answer.</returns>
	public string GetSelectedAnswer ()
	{
		string selectedAnswer = "";
		foreach (Transform container in selectionViewContent.transform) {
			selectedAnswer += container.GetComponentInChildren<Text> ().text;
		}
		return selectedAnswer;
	}

	/// <summary>
	/// This Method is to be activated by ChangeOrderEvent: OnSelectionEndDrag
	/// Sends the written answer and correct answer to the NoAnswer-AnswerType for checking
	/// PartAnswer -> NoAnswer AnswerType: The Answer Part is hidden with this AnswerType
	/// </summary>
	public void CheckAnswer ()
	{
		QuestionSystemController.Instance.partAnswer.noAnswer.CheckAnswerFromSelection (GetSelectedAnswer (), questionAnswer);
	}

	/// <summary>
	/// Shuffles selection by shuffling it with its siblings
	/// Shuffles again if the questionAnswer is spelled out
	/// Sends the Selection GameObjects to the QuestionSystemController for special effects purposes
	/// </summary>
	public void ShuffleSelection ()
	{
		for (int i = 0; i < selectionContainers.Length; i++) {
			selectionContainers [i].transform.SetSiblingIndex (Random.Range (0, selectionContainers.Length));
		}
		if (GetSelectedAnswer ().Equals (questionAnswer)) {
			ShuffleSelection ();
		}
		QuestionSystemController.Instance.correctAnswerButtons = new List<GameObject> (selectionContainers);
	}

	public void RemoveSelectionHint (int hintIndex)
	{
		// For future use
		// Change order hint OnClick
	}

	/// <summary>
	/// Populates Selection Containers
	/// Activates OrderCoupling if the length of questionAnswer is greater that 5
	/// Activates the shuffler after the population
	/// </summary>
	/// <param name="answer">Answer.</param>
	public void PopulateSelectionContainers (string answer)
	{
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
			List<string> couplingList = new List<string> ();
			for (int j = 0; j < questionAnswer.Length; j++) {
				couplingList.Add (questionAnswer [j].ToString ());
			}
			OrderCoupling (couplingList);
		}
		ShuffleSelection ();
		
	}

	public void ClearLetterSelection ()
	{
		foreach (GameObject letter in selectionContainers) {
			letter.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
			letter.SetActive (true);
		}
	}

	/// <summary>
	/// Couples letters into the container if greater than 5
	/// Gets the remainder of length of the container and the length of the answer
	/// Number of Couples depends on number of remainder
	/// if the randomized index has couple, generate another random
	/// Couples letters by adding the letter after the randomized index and removes it after
	/// </summary>
	/// <param name="questioningList">Questioning list.</param>
	public void OrderCoupling (List<string> questioningList)
	{
		int orderCouplingCount = questionAnswer.Length % selectionContainers.Length;
		List<int> couplingRandomizeList = new List<int> ();
		while (orderCouplingCount > 0) {
			int randomizedIndex = Random.Range (0, questionAnswer.Length - 1);
			if (!couplingRandomizeList.Contains (randomizedIndex)) {
				//couple the letters and remove the coupled letter
				questioningList [randomizedIndex] += questioningList [randomizedIndex + 1];
				questioningList.Remove (questioningList [randomizedIndex + 1].ToString ());
				couplingRandomizeList.Add (randomizedIndex);
				orderCouplingCount--;
			}
		}
		for (int j = 0; j < selectionContainers.Length; j++) {
			selectionContainers [j].GetComponentInChildren<Text> ().text = questioningList [j];
		}
	}
}

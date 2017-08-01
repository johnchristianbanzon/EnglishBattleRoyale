using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChangeOrderController : MonoBehaviour, ISelection
{
	public string questionAnswer;
	public ChangeOrderEvent[] selectionContainers = new ChangeOrderEvent[5];
	public GameObject selectionViewContent;
	public Action<List<GameObject>> onSelectCallBack;
	public string[] letterArray = new string[5];

	//ShowSelecton(string answer)
	/// <summary>
	/// Populates Selection Containers
	/// Activates OrderCoupling if the length of questionAnswer is greater that 5
	/// Activates the shuffler after the population
	/// </summary>
	/// <param name="answer">Answer.</param>
	public void ShowSelectionType (string answer,Action<List<GameObject>> onSelectCallBack)
	{
		this.onSelectCallBack = onSelectCallBack;
		gameObject.SetActive (true);
		InitSelectionContainers ();
		questionAnswer = answer;
		if (questionAnswer.Length > selectionContainers.Length) {
			letterArray = OrderCoupling ();
		}
		else{
			for (int i = 0; i < questionAnswer.Length; i++) {
				letterArray [i] = questionAnswer [i].ToString();
			}
		}
		List<GameObject> selectionsList = new List<GameObject> ();
		for (int i = 0; i < selectionContainers.Length; i++) { 
			if (i < answer.Length) {
				selectionContainers [i].Init(letterArray[i]);
			}  else {
				selectionContainers [i].gameObject.SetActive (false);
			}
			selectionsList.Add (selectionContainers [i].gameObject);
		}
		QuestionSystemController.Instance.correctAnswerButtons = selectionsList;
		ShuffleSelection ();
	}

	// Hide()
	public void HideSelectionType(){
		gameObject.SetActive (false);
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
	//	public void OnChangeOrder
	public void OnChangeOrder ()
	{
		if (GetSelectedAnswer ().Equals (questionAnswer)) {
			selectionViewContent.GetComponent<HorizontalLayoutGroup> ().enabled = false;
			GameObject showAnswerPrefab = SystemResourceController.Instance.LoadPrefab ("CluePrefab", selectionViewContent);
			showAnswerPrefab.transform.position = Vector2.zero;
			showAnswerPrefab.GetComponentInChildren<Text> ().text = questionAnswer;
			TweenFacade.TweenScaleToLarge (showAnswerPrefab.transform, Vector3.one, 0.3f);
			for (int i = 0; i < selectionContainers.Length; i++) {
				TweenFacade.TweenMoveTo (selectionContainers [i].transform,  showAnswerPrefab.transform.localPosition, 1.0f);
			}
			QuestionSystemController.Instance.partAnswer.showAnswer.CheckAnswerFromSelection (GetSelectedAnswer (), questionAnswer);
		}
	}

	/// <summary>
	/// Shuffles selection by shuffling it with its siblings
	/// Shuffles again if the questionAnswer is spelled out
	/// Sends the Selection GameObjects to the QuestionSystemController for special effects purposes
	/// </summary>
	public void ShuffleSelection ()
	{
		for (int i = 0; i < selectionContainers.Length; i++) {
			selectionContainers [i].transform.SetSiblingIndex (UnityEngine.Random.Range (0, selectionContainers.Length));
		}
		if (GetSelectedAnswer ().Equals (questionAnswer)) {
			ShuffleSelection ();
		}
	}

	public void ShowCorrectAnswer(){

	}

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{
		TweenFacade.TweenMoveTo(transform,new Vector2(transform.parent.position.x,transform.parent.position.y - 80f),0.4f);
		TweenFacade.TweenScaleToLarge (correctAnswerContainer.transform,Vector3.one,0.3f);
		correctAnswerContainer.GetComponentInChildren<Text> ().enabled = true;
	}

	public void HideSelectionHint(){

	}

	public void InitSelectionContainers ()
	{
		for(int i =0;i<selectionContainers.Length;i++){
			selectionContainers[i].Init ("");
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
	private string[] OrderCoupling ()
	{
		int orderCouplingCount = questionAnswer.Length % selectionContainers.Length;
		List<int> couplingRandomizeList = new List<int> ();
		int letterIndex = 0;
		int couplingCounter = 0;
		while (letterIndex != selectionContainers.Length) {
			int randomizedIndex = UnityEngine.Random.Range (0, selectionContainers.Length);
			if (!couplingRandomizeList.Contains (randomizedIndex)) {
				if (orderCouplingCount > 0) {
					letterArray [letterIndex] = questionAnswer [couplingCounter].ToString () + questionAnswer [couplingCounter+1].ToString ();
					couplingCounter += 2;
				} else {
					letterArray [letterIndex] = questionAnswer [couplingCounter].ToString ();
					couplingCounter++;
				}
				couplingRandomizeList.Add (randomizedIndex);
				letterIndex ++;
				orderCouplingCount--;
			}
		}

		return letterArray;

	}
}


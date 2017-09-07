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
		selectionViewContent.GetComponent<HorizontalLayoutGroup> ().enabled = true;
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
				selectionsList.Add (selectionContainers [i].gameObject);
			}  else {
				selectionContainers [i].gameObject.SetActive (false);
			}

		}
		QuestionSystemController.Instance.correctAnswerButtons = selectionsList;
		ShuffleSelection ();
	}
		
	public GameObject ShowSelectionPopUp(){
		SystemSoundController.Instance.PlaySFX ("SFX_ChangeOrder");
		GameObject selectionPopUp = SystemResourceController.Instance.LoadPrefab ("PopUpChangeOrder", SystemPopupController.Instance.popUp);
		List<GameObject> popUpSelectionList = new List<GameObject> ();
		for (int i = 0; i < selectionPopUp.transform.childCount; i++) {
			popUpSelectionList.Add(selectionPopUp.transform.GetChild(i).gameObject);
		}
		if(popUpSelectionList.Count>1){
			int randomSelection = UnityEngine.Random.Range (1, popUpSelectionList.Count);
			GameObject selectionToBeSwitched1 = popUpSelectionList[randomSelection-1];
			GameObject selectionToBeSwitched2 = popUpSelectionList[randomSelection];
			Vector3 selectionPosition = selectionToBeSwitched1.transform.localPosition;
			selectionToBeSwitched1.transform.localPosition = selectionToBeSwitched2.transform.localPosition;
			selectionToBeSwitched2.transform.localPosition = selectionPosition;
			TweenFacade.TweenMoveTo (selectionToBeSwitched1.transform,
				selectionToBeSwitched2.transform.localPosition, 0.5f);
			TweenFacade.TweenJumpTo (selectionToBeSwitched2.transform
				,selectionToBeSwitched1.transform.localPosition,180f,1,0.5f,0);
		}
		return selectionPopUp;
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

	private GameObject showAnswerPrefab = null;
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
		    showAnswerPrefab = SystemResourceController.Instance.LoadPrefab ("CluePrefab", selectionViewContent);
			showAnswerPrefab.transform.position = transform.position;
			showAnswerPrefab.GetComponent<RectTransform> ().sizeDelta = new Vector2 (600,125);
			showAnswerPrefab.GetComponentInChildren<Text> ().text = questionAnswer;
			TweenFacade.TweenScaleToLarge (showAnswerPrefab.transform, Vector3.one, 0.3f);
			for (int i = 0; i < selectionContainers.Length; i++) {
				TweenFacade.TweenMoveTo (selectionContainers [i].transform,  showAnswerPrefab.transform.localPosition, 1.0f);
			}
			QuestionSystemController.Instance.CheckAnswer (true);
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
		if (GetSelectedAnswer ().Equals (questionAnswer.ToUpper())) {
			ShuffleSelection ();
		}
	}

	public void ShowCorrectAnswer(bool isAnswerCorrect){
		for (int i = 0; i < selectionContainers.Length; i++) {
			selectionContainers [i].transform.SetSiblingIndex (selectionContainers[i].containerIndex);
			selectionContainers [i].GetComponent<Image> ().color = new Color32 (255, 100, 100, 255);
		}
	}

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{
		ShowAnswer showAnswer = QuestionSystemController.Instance.partAnswer.showAnswer;
		List<int> selectionIndex = new List<int>();
		for (int i = 0; i < showAnswer.hintContainers.Count; i++) {
			if(showAnswer.hintContainers[i].GetComponent<Button>().interactable){
				selectionIndex.Add (i);
			}
		}
		TweenFacade.TweenScaleToLarge (showAnswer.hintContainers[selectionIndex[0]].transform,Vector3.one,0.3f);
		showAnswer.hintContainers[selectionIndex[0]].GetComponent<Button> ().interactable = false;
		showAnswer.hintContainers [selectionIndex [0]].GetComponentInChildren<Text> ().text = letterArray [selectionIndex [0]].ToString();
	}

	public void HideSelectionHint(){

	}

	public void InitSelectionContainers ()
	{
		Destroy (showAnswerPrefab);
		selectionViewContent.GetComponent<HorizontalLayoutGroup> ().enabled = enabled;
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


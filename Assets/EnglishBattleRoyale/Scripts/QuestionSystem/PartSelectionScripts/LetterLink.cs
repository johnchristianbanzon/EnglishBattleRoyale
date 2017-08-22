using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LetterLink : MonoBehaviour ,ISelection
{
	public LetterLinkEvent[] connectLetterButtons = new LetterLinkEvent[9];
	public string questionAnswer;


	public void ShowCorrectAnswer (bool isAnswerCorrect)
	{
		foreach (LetterLinkEvent container in connectLetterButtons) {
			container.ShowCorrectAnswer (isAnswerCorrect);
		}
	}

	public void HideSelectionType ()
	{
		gameObject.SetActive (false);
	}

	private List<GameObject> popUpSelectionList = new List<GameObject>();
	private GameObject handCursor;
	public void ShowSelectionPopUp(GameObject selectionPopUp){
		popUpSelectionList.Clear ();
		popUpSelectIndex = 0;
		for (int i = 0; i < selectionPopUp.transform.childCount; i++) {
			if(selectionPopUp.transform.GetChild(i).childCount>0){
			popUpSelectionList.Add(selectionPopUp.transform.GetChild(i).gameObject);
			}
		}
		if (popUpSelectionList.Count > 0) {
			float popUpDelay = 1.8f;
			handCursor = SystemResourceController.Instance.LoadPrefab ("HandCursor", SystemPopupController.Instance.popUp);
			handCursor.transform.localPosition = new Vector2(popUpSelectionList [0].transform.localPosition.x,popUpSelectionList [0].transform.localPosition.y - 100f);
			TweenFacade.TweenMoveTo (handCursor.transform, new Vector2(popUpSelectionList [popUpSelectionList.Count-1].transform.localPosition.x
				,handCursor.transform.localPosition.y), popUpDelay);
			Destroy (handCursor, popUpDelay+0.1f);
			InvokeRepeating ("PopUpSelect", 0, (popUpDelay/4));

		}
	}
	private int popUpSelectIndex = 0;
	public void PopUpSelect(){
		if (popUpSelectIndex.Equals (popUpSelectionList.Count)) {
			for (int i = 0; i < popUpSelectionList.Count; i++) {
				popUpSelectionList [i].GetComponent<Image> ().color = new Color32 (255, 223, 0, 255);
			}
			CancelInvoke ();
		} else {
			popUpSelectionList [popUpSelectIndex].GetComponent<Image> ().color = new Color (36f / 255, 189f / 255f, 88f / 255f);
		}
			popUpSelectIndex++;
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
		selectionIndex = ListShuffleUtility.Shuffle (selectionIndex);
		showAnswer.hintContainers[selectionIndex[0]].GetComponentInChildren<Text>().text = questionAnswer[selectionIndex[0]].ToString();
		showAnswer.hintContainers [selectionIndex [0]].GetComponent<Image> ().color = new Color32 (255,255,255,255);
		showAnswer.hintContainers[selectionIndex[0]].GetComponent<Button> ().interactable = false;
	}

	public void HideSelectionHint(){
		
	}

	public void ShowSelectionType (string questionAnswer, Action<List<GameObject>> onSelectCallBack)
	{
		this.questionAnswer = questionAnswer;
		ShuffleSelection ();
		QuestionSystemController.Instance.partAnswer.showAnswer.InitHints ();
		gameObject.SetActive (true);
	}

	/// <summary>
	/// Shuffles The Selection in the Letter Link, Randomizes Starting point first and then goes to the next viable 
	/// letter index in the selectableIndex List depending on the number. 
	/// While loop is used to ensure to repeating index is selected. 
	/// </summary>
	public void ShuffleSelection ()
	{
		Debug.Log (questionAnswer);
//		ClearSelection ();
		List<GameObject> correctAnswerButtons = new List<GameObject> ();
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	
		/*
		List<List<int>> selectableIndex = new List<List<int>> {
			new List<int>{ 0 }, new List<int>{ 2, 4 }, new List<int>{ 1, 3, 5 }, new List<int>{ 2, 6 },
			new List<int>{ 1, 5, 7 }, new List<int>{ 2, 4, 6, 8 }, new List<int>{ 3, 5, 9 },
			new List<int>{ 4, 8 }, new List<int>{ 7, 5, 9 }, new List<int>{ 8, 6 }
		};*/
		List<List<int>> selectableIndex = new List<List<int>> {
			new List<int>{ 0 }, new List<int>{ 2, 4, 5 }, new List<int>{ 1, 3,4, 5,6 }, new List<int>{ 2,5, 6 },
			new List<int>{ 1,2, 5, 7,8 }, new List<int>{ 1,2,3,4,6,7,8,9 }, new List<int>{ 2,3, 5,8, 9 },
			new List<int>{ 4,5, 8 }, new List<int>{ 4,5, 6,7, 9 }, new List<int>{ 5,6,8 }
		};



		int selectionIndex = UnityEngine.Random.Range (1, connectLetterButtons.Length + 1);
		List<int> numbersDone = new List<int> ();
		for (int i = 0; i < connectLetterButtons.Length; i++) {
			int randomizeAlphabetLetter = UnityEngine.Random.Range (0, alphabet.Length);
			while (questionAnswer.Contains (alphabet [randomizeAlphabetLetter].ToString ())) {
				randomizeAlphabetLetter = UnityEngine.Random.Range (0, alphabet.Length);
			}
			connectLetterButtons [i].gameObject.GetComponentInChildren<Text> ().text = alphabet [randomizeAlphabetLetter].ToString ();
			connectLetterButtons [i].GetComponent<Image> ().color = Color.white;
		}
		List<int> linkOrder = new List<int> ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			/*
			int randomizedSelection = UnityEngine.Random.Range (0, selectableIndex [selectionIndex].Count);
			connectLetterButtons [selectionIndex - 1].gameObject.GetComponentInChildren<Text> ().text = questionAnswer [i].ToString ();

			while (numbersDone.Contains (selectableIndex [selectionIndex] [randomizedSelection])) {
				randomizedSelection = UnityEngine.Random.Range (0, selectableIndex [selectionIndex].Count);
				if (whileindex > 100) {
					break;
				}
				whileindex++;
			}
			numbersDone.Add (selectionIndex);

			for(int j =0;j<selectableIndex.Count;j++){
				selectableIndex [j].Remove (selectionIndex);
			}
			selectionIndex = selectableIndex [selectionIndex] [randomizedSelection];

		*/
			int randomizedSelection = UnityEngine.Random.Range (0, selectableIndex [selectionIndex].Count);
			linkOrder.Add (selectionIndex);
			Debug.Log (linkOrder [i]);
			correctAnswerButtons.Add (connectLetterButtons [selectionIndex - 1].gameObject);
			for(int j =0;j<selectableIndex.Count;j++){
				selectableIndex [j].Remove (selectionIndex);
			}
			selectionIndex = selectableIndex [selectionIndex] [randomizedSelection];
		}

		for (int i = 0; i < questionAnswer.Length; i++) {
			connectLetterButtons[linkOrder[i]].GetComponentInChildren<Text> ().text = questionAnswer [i].ToString ();
		}
		QuestionSystemController.Instance.correctAnswerButtons = correctAnswerButtons;
	}

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StackSwipeController : MonoBehaviour, ISelection
{
	public StackSwipeEvent[] stackSwipeContainers = new StackSwipeEvent[4];
	public GameObject stackSwipeContent;
	public bool isPointerInside;
	public string questionAnswer;
	private int numberOfAnswers = 1;

	public void OnPointerExit ()
	{
		isPointerInside = false;
	}

	public void OnPointerEnter ()
	{
		isPointerInside = true;
	}

	public void ShowSelectionType (string answer, Action<List<GameObject>> onSelectCallBack)
	{
		questionAnswer = answer;
		gameObject.SetActive (true);
		List<GameObject> correctAnswer = new List<GameObject> ();
		for (int i = 0; i < stackSwipeContainers.Length; i++) {
			if (numberOfAnswers > i) {
				stackSwipeContainers [i].Init (true);

			} else {
				stackSwipeContainers [i].Init (false);
				correctAnswer.Add (stackSwipeContainers [i].gameObject);
			}
		}
		QuestionSystemController.Instance.correctAnswerButtons = correctAnswer;
	}

	private GameObject popUp;
	private int popUpCounter = 0;

	public GameObject ShowSelectionPopUp ()
	{
		GameObject selectionPopUp = SystemResourceController.Instance.LoadPrefab ("PopUpStackSwipe", SystemPopupController.Instance.popUp);
		popUp = selectionPopUp;
		popUpCounter = 0;
		InvokeRepeating ("RemoveSelectionPopUp", 0, 0.3f);
		return selectionPopUp;
	}

	public void RemoveSelectionPopUp ()
	{
		if (popUpCounter >= popUp.transform.childCount) {
			CancelInvoke ();
		} else {	
			if (popUpCounter != 2) {
				float direction = 700;
				if (popUpCounter % 3 == 0) {
					direction = -700;
				}
				TweenFacade.TweenMoveTo (popUp.transform.GetChild (popUpCounter), new Vector2 (direction, popUp.transform.GetChild (popUpCounter).transform.localPosition.y), 0.3f);
			}
			popUpCounter++;
		}
	}

	public void ResetSelections ()
	{
		for (int i = 0; i < stackSwipeContainers.Length; i++) {
			
			stackSwipeContainers [i].gameObject.SetActive (true);
		}
		
	}

	public void HideSelectionType ()
	{
		gameObject.SetActive (false);
	}

	public void ShowCorrectAnswer (bool isAnswerCorrect)
	{
		Color selectionColor = new Color ();
		if (isAnswerCorrect) {
			selectionColor = new Color32 (255, 223, 0, 255);
		} else {
			selectionColor = new Color32 (255, 100, 100, 255);
		}
		for (int i = 0; i < stackSwipeContainers.Length; i++) {
			if (stackSwipeContainers [i].isCorrect) {
				stackSwipeContainers [i].gameObject.SetActive (true);
				stackSwipeContainers [i].GetComponent<Image> ().color = selectionColor;
			} else {
				stackSwipeContainers [i].GetComponent<Image> ().color = Color.white;
			}
		}

	}

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{
		//CHANGES HERE FOR FUTURE
		//ONLY SELECTION CHANGE
		List<GameObject> hintableContainer = new List<GameObject>(); 
		for(int i =0;i<stackSwipeContainers.Length;i++){
			if (!stackSwipeContainers [i].isCorrect && 
				(stackSwipeContainers[i].GetComponent<Image>().color == Color.white) && 
				stackSwipeContainers[i].gameObject.activeInHierarchy) {
				hintableContainer.Add (stackSwipeContainers [i].gameObject);
			}
		}
		if (hintableContainer.Count > 0) {
			hintableContainer [0].GetComponent<Image> ().color = new Color32 (255, 100, 100, 255);
		}

	}

	public void HideSelectionHint ()
	{
		
	}
}

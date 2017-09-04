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
				correctAnswer.Add (stackSwipeContainers [i].gameObject);
			} else {
				stackSwipeContainers [i].Init (false);
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

	}

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{

	}

	public void HideSelectionHint ()
	{

	}


}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StackSwipeController : MonoBehaviour, ISelection {
	public StackSwipeEvent[] stackSwipeContainers = new StackSwipeEvent[4];
	public GameObject stackSwipeContent;
	public bool isPointerInside;

	public void OnPointerExit(){
		isPointerInside = false;
	}

	public void OnPointerEnter(){
		isPointerInside = true;
	}


	public void ShowSelectionType (string answer,Action<List<GameObject>> onSelectCallBack)
	{
		
	}

	public GameObject ShowSelectionPopUp(){
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

	public void HideSelectionType(){
		gameObject.SetActive (false);
	}

	public void ShowCorrectAnswer(bool isAnswerCorrect){

	}

	public void ShowSelectionHint (int hintIndex, GameObject correctAnswerContainer)
	{

	}

	public void HideSelectionHint(){

	}


}

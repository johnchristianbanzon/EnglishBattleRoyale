using UnityEngine;
using System;
using System.Collections.Generic;
public class PartSelectionController : MonoBehaviour
{
	public GameObject[] inputSelections = new GameObject[4];
	public SelectLetter selectLetter;
	public Typing typing;
	public ChangeOrderController changeOrder;
	public WordChoice wordChoice;
	public SlotMachine slotMachine;
	public LetterLink letterLink;
	public StackSwipeController stackSwipe;

	public void DeploySelectionType(ISelection selectionType, string questionAnswer,Action<List<GameObject>> onSelectCallBack){
		selectionType.ShowSelectionType (questionAnswer,onSelectCallBack);
	}

	public void HideSelectionType(ISelection selectionType){
		selectionType.HideSelectionType ();
	}

}

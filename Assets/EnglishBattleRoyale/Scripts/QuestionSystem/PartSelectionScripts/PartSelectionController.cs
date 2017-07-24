using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PartSelectionController : MonoBehaviour
{
	public GameObject[] inputSelections = new GameObject[4];
	public SelectLetter selectLetterController;
	public Typing typingController;
	public ChangeOrder changeOrderController;
	public WordChoice wordChoiceController;
	public SlotMachine slotMachineController;
	public LetterLink letterLink;

	public void DeploySelectionType(ISelection selectionType, string questionAnswer){
		selectionType.DeploySelectionType (questionAnswer);
		selectionType.ShuffleSelection ();
	}


}

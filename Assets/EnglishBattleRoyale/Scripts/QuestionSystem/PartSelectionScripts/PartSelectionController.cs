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

	public void DeploySelectionType(QuestionSystemEnums.SelectionType selectionType, string questionAnswer){
		switch(selectionType){
		case QuestionSystemEnums.SelectionType.Typing:
			typingController.gameObject.SetActive(true);
			break;
		case QuestionSystemEnums.SelectionType.SelectLetter:
			selectLetterController.SelectLetterShuffle (questionAnswer);
			selectLetterController.gameObject.SetActive (true);
			break;
		case QuestionSystemEnums.SelectionType.ChangeOrder:
			//QuestionSystemObjects.Instance.get
			changeOrderController.PopulateLetterSelection (questionAnswer);
			changeOrderController.gameObject.SetActive (true);
			break;
		case QuestionSystemEnums.SelectionType.WordChoice:
			wordChoiceController.WordChoiceShuffle (questionAnswer);
			wordChoiceController.gameObject.SetActive (true);
			break;
		case QuestionSystemEnums.SelectionType.SlotMachine:
			slotMachineController.ShuffleAlgo(questionAnswer);
			slotMachineController.gameObject.SetActive (true);
			break;
		case QuestionSystemEnums.SelectionType.LetterLink:
			letterLink.gameObject.SetActive (true);
			letterLink.ConnectLetterShuffle (questionAnswer);
			break;
		}
	}


}

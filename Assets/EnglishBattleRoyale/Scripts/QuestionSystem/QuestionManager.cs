using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestionManager: SingletonMonoBehaviour<QuestionManager>
{
	public GameObject[] questionTypeModals;
	public GameObject questionUI;
	public void QuestionHide(){
		for (int i = 0; i < questionTypeModals.Length; i++) {
			questionTypeModals [i].SetActive (false);
		}
	}
	void Start(){
		QuestionBuilder.PopulateQuestion ("QuestionSystemCsv");
		//SelectLetterIcon typingicon = FindObjectOfType<SelectLetterIcon>();
		//QuestionRoundController.Instance.SetQuestion (typingicon, 15, null);
	}
	public void SetQuestionEntry(int questionType, int questionTime, Action<int, int> onResult){
		questionTypeModals[questionType].SetActive (true);

		switch (questionType) {
		case 0:
			SelectLetter selectletterIcon = questionTypeModals[0].GetComponent<SelectLetter>();
			//questionTypeModals[0].SetActive (true);
		//	QuestionRoundController.Instance.SetQuestion (selectletterIcon, questionTime, onResult);
			break;
		case 1:
			//TypingIcon typingicon = FindObjectOfType<TypingIcon>();
			//questionTypeModals[1].SetActive (true);
			//QuestionRoundController.Instance.SetQuestion (typingicon, questionTime, onResult);
		
			break;
		case 2:
			//questionTypeModals[2].SetActive (true);
			//ChangeOrderIcon changeOrderIcon = FindObjectOfType<ChangeOrderIcon>();
			//QuestionRoundController.Instance.SetQuestion (changeOrderIcon, questionTime, onResult);
		
			break;
		case 3:
			//questionTypeModals[2].SetActive (true);
//			WordChoiceIcon wordchoiceIcon = FindObjectOfType<WordChoiceIcon>();
//			QuestionRoundController.Instance.SetQuestion (wordchoiceIcon, questionTime, onResult);

			break;
		case 4:
			//questionTypeModals[2].SetActive (true);
			//SlotMachineIcon slotMachineIcon = questionTypeModals[4].GetComponent<SlotMachineIcon>();
			//QuestionRoundController.Instance.SetQuestion (slotMachineIcon, questionTime, onResult);
			break;
		}

			
			
	}


}


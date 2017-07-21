using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartQuestionController: MonoBehaviour
{

	// JOCHRIS START HERE   ---------------------------------------------------------------------------------------------
	public GameObject questionSelect;

	public GameObject[] questionTypeModals;

	public void QuestionHide ()
	{
		for (int i = 0; i < questionTypeModals.Length; i++) {
			//Debug.Log (questionTypeModals[i].name);
			questionTypeModals [i].SetActive (false);
		}
	}

	void Start ()
	{
		QuestionBuilder.PopulateQuestion ("QuestionSystemCsv");
		SelectLetterIcon typingicon = FindObjectOfType<SelectLetterIcon> ();
		QuestionController.Instance.SetQuestion (typingicon, null);
	}

	public void SetQuestionEntry (int questionType, Action<int, int> onResult)
	{
		questionTypeModals [questionType].SetActive (true);


		switch (questionType) {
		case 0:
			SelectLetterIcon selectletterIcon = questionTypeModals [0].GetComponent<SelectLetterIcon> ();
			//questionTypeModals[0].SetActive (true);
			QuestionController.Instance.SetQuestion (selectletterIcon, onResult);


			break;
		case 1:
			TypingIcon typingicon = FindObjectOfType<TypingIcon> ();
			//questionTypeModals[1].SetActive (true);
			QuestionController.Instance.SetQuestion (typingicon, onResult);
		
			break;
		case 2:
			//questionTypeModals[2].SetActive (true);
			ChangeOrderIcon changeOrderIcon = FindObjectOfType<ChangeOrderIcon> ();
			QuestionController.Instance.SetQuestion (changeOrderIcon, onResult);
		
			break;
		case 3:
			//questionTypeModals[2].SetActive (true);
			WordChoiceIcon wordchoiceIcon = FindObjectOfType<WordChoiceIcon> ();
			QuestionController.Instance.SetQuestion (wordchoiceIcon, onResult);

			break;
		case 4:
			//questionTypeModals[2].SetActive (true);
			SlotMachineIcon slotMachineIcon = questionTypeModals [4].GetComponent<SlotMachineIcon> ();
			QuestionController.Instance.SetQuestion (slotMachineIcon, onResult);
			break;
		}
	}
	// JOCHRIS END HERE   ---------------------------------------------------------------------------------------------

	private void OnEndQuestion(int gp, int qtimeLeft){
		QuestionStart (gp, qtimeLeft);
	}

	private void OnEndSelectQuestionTime(){
		HideUI ();
		SetQuestionEntry (UnityEngine.Random.Range (0, 2), OnEndQuestion);
	}



	public void OnStartPhase ()
	{
		ScreenBattleController.Instance.partSkill.ShowAutoActivateButtons (true);
		Debug.Log ("Starting Answer Phase");
		RPCDicObserver.AddObserver (PartAnswerIndicatorController.Instance);
		GameTimeManager.HasAnswered(false);

		GameTimeManager.StartSelectQuestionTimer (OnEndSelectQuestionTime);
		questionSelect.SetActive (true);

	}

	public void OnEndPhase ()
	{
		RPCDicObserver.RemoveObserver (PartAnswerIndicatorController.Instance);
		if (questionSelect.activeInHierarchy) {
			questionSelect.SetActive (false);
		}
	}



	public void OnQuestionSelect (int questionNumber)
	{
		GameTimeManager.StopTimer ();
		GameTimeManager.HasAnswered (true);
		questionSelect.SetActive (false);
		//call question callback here
		SetQuestionEntry (questionNumber, OnEndQuestion);

	}

	private void QuestionStart (int gp, int qtimeLeft)
	{
		Debug.Log (gp);
		Debug.Log (SystemGlobalDataController.Instance.gpEarned);

		SystemGlobalDataController.Instance.gpEarned = gp;
		ScreenBattleController.Instance.PlayerGP += gp;
		SystemFirebaseDBController.Instance.AnswerPhase (qtimeLeft, gp);

		//for mode 3
		ScreenBattleController.Instance.partSkill.CheckSkillActivate ();

		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
			if (SystemGlobalDataController.Instance.skillChosenCost <= ScreenBattleController.Instance.PlayerGP) {
				if (SystemGlobalDataController.Instance.playerSkillChosen != null) {
					SystemGlobalDataController.Instance.playerSkillChosen ();
				}
			} else {
				Debug.Log ("LESS GP CANNOT ACTIVATE SKILL");
			}
		} 
		HideUI ();
	}

	private void HideUI ()
	{
		questionSelect.SetActive (false);
		GameTimeManager.ToggleTimer (false);

	}

}


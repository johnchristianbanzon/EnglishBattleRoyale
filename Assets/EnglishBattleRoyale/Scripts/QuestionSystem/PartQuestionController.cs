using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartQuestionController: MonoBehaviour
{
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
		QuestionController.Instance.SetQuestion (typingicon, 15, null);
	}

	public void SetQuestionEntry (int questionType, int questionTime, Action<int, int> onResult)
	{
		questionTypeModals [questionType].SetActive (true);


		switch (questionType) {
		case 0:
			SelectLetterIcon selectletterIcon = questionTypeModals [0].GetComponent<SelectLetterIcon> ();
			//questionTypeModals[0].SetActive (true);
			QuestionController.Instance.SetQuestion (selectletterIcon, questionTime, onResult);


			break;
		case 1:
			TypingIcon typingicon = FindObjectOfType<TypingIcon> ();
			//questionTypeModals[1].SetActive (true);
			QuestionController.Instance.SetQuestion (typingicon, questionTime, onResult);
		
			break;
		case 2:
			//questionTypeModals[2].SetActive (true);
			ChangeOrderIcon changeOrderIcon = FindObjectOfType<ChangeOrderIcon> ();
			QuestionController.Instance.SetQuestion (changeOrderIcon, questionTime, onResult);
		
			break;
		case 3:
			//questionTypeModals[2].SetActive (true);
			WordChoiceIcon wordchoiceIcon = FindObjectOfType<WordChoiceIcon> ();
			QuestionController.Instance.SetQuestion (wordchoiceIcon, questionTime, onResult);

			break;
		case 4:
			//questionTypeModals[2].SetActive (true);
			SlotMachineIcon slotMachineIcon = questionTypeModals [4].GetComponent<SlotMachineIcon> ();
			QuestionController.Instance.SetQuestion (slotMachineIcon, questionTime, onResult);
			break;
		}
	}


	public void OnStartPhase ()
	{
		ScreenBattleController.Instance.partSkill.ShowAutoActivateButtons (true);
		Debug.Log ("Starting Answer Phase");
		RPCDicObserver.AddObserver (PartAnswerIndicatorController.Instance);
		ScreenBattleController.Instance.partState.gameTimer.hasAnswered = false;

		ScreenBattleController.Instance.partState.gameTimer.SelectQuestionTimer (delegate() {
			HideUI ();
			SetQuestionEntry (UnityEngine.Random.Range (0, 2), SystemGlobalDataController.Instance.answerQuestionTime, delegate(int gp, int qtimeLeft) {

				QuestionStart (gp, qtimeLeft);
			});
		});
		questionSelect.SetActive (true);

	}

	public void OnEndPhase ()
	{
		RPCDicObserver.RemoveObserver (PartAnswerIndicatorController.Instance);
		if (questionSelect.activeInHierarchy) {
			questionSelect.SetActive (false);
		}
		ScreenBattleController.Instance.partState.gameTimer.CancelInvoke ("StartQuestionTimer");
	}

	public void OnQuestionSelect (int questionNumber)
	{
		ScreenBattleController.Instance.partState.gameTimer.StopTimer ();
		ScreenBattleController.Instance.partState.gameTimer.hasAnswered = true;
		questionSelect.SetActive (false);
		//call question callback here
		SetQuestionEntry (questionNumber, SystemGlobalDataController.Instance.answerQuestionTime, delegate(int gp, int qtimeLeft) {
			QuestionStart (gp, qtimeLeft);
		});

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
		ScreenBattleController.Instance.partState.gameTimer.ToggleTimer (false);

	}

}


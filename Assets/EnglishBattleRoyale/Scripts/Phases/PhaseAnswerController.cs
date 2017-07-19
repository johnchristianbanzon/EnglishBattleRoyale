using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class PhaseAnswerController : BasePhase
{
	public GameObject questionSelect;
	private bool hasAnswered = false;


	public override void OnStartPhase ()
	{
		FindObjectOfType<PhaseSkillController> ().ShowAutoActivateButtons (true);
		Debug.Log ("Starting Answer Phase");
		RPCDicObserver.AddObserver(AnswerIndicatorController.Instance);
		hasAnswered = false;

		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);
		questionSelect.SetActive (true);

	}

	public override void OnEndPhase ()
	{
		RPCDicObserver.RemoveObserver(AnswerIndicatorController.Instance);
		if (questionSelect.activeInHierarchy) {
			questionSelect.SetActive (false);
		}
		CancelInvoke ("StartTimer");
	}

	public void OnQuestionSelect (int questionNumber)
	{
		stoptimer = false;
		hasAnswered = true;
		questionSelect.SetActive (false);
		//call question callback here
		PartQuestionController.Instance.SetQuestionEntry (questionNumber, SystemGlobalDataController.Instance.answerQuestionTime, delegate(int gp, int qtimeLeft) {
			QuestionStart (gp, qtimeLeft);
		});

	}


	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimerController.Instance.ToggleTimer (true);
			if (timeLeft > 0 && hasAnswered == false) {
				GameTimerController.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 

			HideUI ();
			PartQuestionController.Instance.SetQuestionEntry (UnityEngine.Random.Range (0, 2), SystemGlobalDataController.Instance.answerQuestionTime, delegate(int gp, int qtimeLeft) {
				
				QuestionStart (gp, qtimeLeft);
			});
				
			GameTimerController.Instance.ToggleTimer (false);
			stoptimer = false;
		
		}
	}

	private void QuestionStart (int gp, int qtimeLeft)
	{
		Debug.Log (gp);
		Debug.Log (SystemGlobalDataController.Instance.gpEarned);

		SystemGlobalDataController.Instance.gpEarned = gp;
		ScreenBattleController.Instance.PlayerGP += gp;
		SystemFirebaseDBController.Instance.AnswerPhase (qtimeLeft, gp);

		//for mode 3
		FindObjectOfType<PhaseSkillController> ().CheckSkillActivate ();

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
		GameTimerController.Instance.ToggleTimer (false);

	}

}

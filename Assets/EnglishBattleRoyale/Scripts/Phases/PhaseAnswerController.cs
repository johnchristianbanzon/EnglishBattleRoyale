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

	public void OnQuestionSelect ()
	{
		stoptimer = false;
		string questionSelectedName = EventSystem.current.currentSelectedGameObject.name;
		int questionNumber = Int32.Parse (questionSelectedName [questionSelectedName.Length - 1].ToString ()) - 1;	
		hasAnswered = true;
		questionSelect.SetActive (false);
		//call question callback here
		QuestionManager.Instance.SetQuestionEntry (questionNumber, GameData.Instance.answerQuestionTime, delegate(int gp, int qtimeLeft) {
			QuestionStart (gp, qtimeLeft);
		});

	}


	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimerView.Instance.ToggleTimer (true);
			if (timeLeft > 0 && hasAnswered == false) {
				GameTimerView.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 

			HideUI ();
			QuestionManager.Instance.SetQuestionEntry (UnityEngine.Random.Range (0, 2), GameData.Instance.answerQuestionTime, delegate(int gp, int qtimeLeft) {
				
				QuestionStart (gp, qtimeLeft);
			});
				
			GameTimerView.Instance.ToggleTimer (false);
			stoptimer = false;
		
		}
	}

	private void QuestionStart (int gp, int qtimeLeft)
	{
		Debug.Log (gp);
		Debug.Log (GameData.Instance.gpEarned);

		GameData.Instance.gpEarned = gp;
		BattleController.Instance.PlayerGP += gp;
		SystemFirebaseDBController.Instance.AnswerPhase (qtimeLeft, gp);

		//for mode 3
		FindObjectOfType<PhaseSkillController> ().CheckSkillActivate ();

		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			if (GameData.Instance.skillChosenCost <= BattleController.Instance.PlayerGP) {
				if (GameData.Instance.playerSkillChosen != null) {
					GameData.Instance.playerSkillChosen ();
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
		GameTimerView.Instance.ToggleTimer (false);

	}

}

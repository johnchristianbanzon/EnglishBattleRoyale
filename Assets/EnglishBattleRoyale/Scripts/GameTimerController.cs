using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

public class GameTimerController: MonoBehaviour
{

	public Text gameTimerText;

	public void ToggleTimer (bool toggleFlag)
	{
		gameTimerText.enabled = toggleFlag;
	}

	#region PrebattleTimer

	public void PreBattleTimer ()
	{
		StopTimer ();
		StartCoroutine (PreBattleStartTimer ());
	}

	IEnumerator PreBattleStartTimer ()
	{
		ToggleTimer (true);
		int timeLeft = 3;

		while (timeLeft > 0) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);
		
		}
			
		ToggleTimer (false);
		PhaseManager.StartPhase1 ();
	}

	#endregion

	#region SkillTimer

	public void SkillTimer (Action action)
	{
		StopTimer ();
		StartCoroutine (StartSkillTimer (action));
	}


	IEnumerator StartSkillTimer (Action action)
	{
		ToggleTimer (true);
		int timeLeft = 5;

		while (timeLeft > 0) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);
		}

		action ();
		ToggleTimer (false);
		SystemFirebaseDBController.Instance.SkillPhase ();
	}

	#endregion

	#region SelectQuestionTimer

	public void SelectQuestionTimer (Action action)
	{
		StopTimer ();
		StartCoroutine (StartSelectQuestionTimer (action));
	}

	public bool hasAnswered = false;

	IEnumerator StartSelectQuestionTimer (Action action)
	{
		ToggleTimer (true);
		int timeLeft = 5;

		while (timeLeft > 0 && hasAnswered == false) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);
		}

		action ();
		ToggleTimer (false);
	}

	#endregion

	#region QuestionTimer

	public void QuestionTimer (Action action, int timer)
	{
		StopTimer ();
		StartCoroutine (StartQuestionTimer (action, timer));
	}

	IEnumerator StartQuestionTimer (Action action, int timer)
	{
		ToggleTimer (true);
		int timeLeft = timer;

		while (timeLeft > 0 && hasAnswered == false) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);
		}

		action ();
		ToggleTimer (false);
	}

	#endregion

	public void StopTimer ()
	{
		StopAllCoroutines ();
	}

}
